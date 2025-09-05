using CVA.Escoteiro.Magento.BLL;
using CVA.Escoteiro.Magento.Client;
using CVA.Escoteiro.Magento.Models.Magento;
using Escoteiro.Magento.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.Service
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                // Insere/Atualiza no Magento produtos cadastrados no SAP Business One
                workCreateProducts();
            }
            catch (Exception ex)
            {
                logger.Error($"Error Integração Magento {ex.ToString()} | {ex.StackTrace}");
            }

            try
            {
                // Insere novos pedidos de venda no SAP Business One
                workCreateOrders();
            }
            catch (Exception ex)
            {
                logger.Error($"Error Integração Magento {ex.ToString()} | {ex.StackTrace}");
            }

            try
            {
                // Atualiza os pedidos de venda já inseridos no SAP Business One
                workUpdateOrders();
            }
            catch (Exception ex)
            {
                logger.Error($"Error Integração Magento {ex.ToString()} | {ex.StackTrace}");
            }

            try
            {
                // Cancela no Magento os pedidos de venda que foram cancelados no SAP Business One
                workCancellationOrders();
            }
            catch (Exception ex)
            {
                logger.Error($"Error Integração Magento {ex.ToString()} | {ex.StackTrace}");
            }

            try
            {
                workSetOrdersStatusMessages();
            }
            catch (Exception ex)
            {
                logger.Error($"Error Integração Magento {ex.ToString()} | {ex.StackTrace}");
            }
        }

        private static void workCreateProducts()
        {
            // Obtém os itens do SAP Business One a serem integrados
            foreach (var itemCode in GetItemsToIntegrate())
            {
                try
                {
                    logger.Trace($"Iniciando integração do item {itemCode}");

                    // Insere/atualiza os itens no Magento
                    var product = SetProduct(callGetItemData(itemCode).Result);

                    // Atualiza no SAP o cadastro do item com o id do produto no Magento e indicar que ele foi integrado
                    callSetItemIntegratedStatus(itemCode, product.id.ToString()).Wait();
                }
                catch (Exception ex)
                {
                    logger.Error($"Error Integração Magento {ex.Message}");
                }
            }
        }

        static private void workCreateOrders()
        {
            int pageSize = 15;

            // Obtém a data da última integração realizada, a qual será utilizada para filtrar
            // a consulta de novos pedidos de venda inseridos no Magento
            var dataCreateLast = callGetLastDateCreate("Orders").Result;

            // Obtém no máximo 15 pedidos de vendas inseridos no Magento, de acordo com a data 
            // da última integração
            var orderList = GetListOrdersCreate(1, pageSize, dataCreateLast);

            // Total de páginas (pedidos) obtidos na consulta
            int totalPages = (int)Math.Ceiling((double)orderList.total_count / pageSize);

            if (totalPages > 0)
            {
                // Realiza a integração de cada pedido de venda obtido
                for (int i = 1; i <= totalPages; i++)
                {
                    if (orderList == null)
                    {
                        orderList = GetListOrdersCreate(i, pageSize, dataCreateLast);
                    }

                    if (orderList == null) continue;

                    callOrdersCreateSL(orderList).Wait();
                    callSaveDateCreate(orderList).Wait();

                    orderList = null;
                }
            }
        }

        static private void workUpdateOrders()
        {
            int pageSize = 15;

            var dataCreateLast = callGetLastDateUpdate("Orders").Result;
            OrdersListModel orderList = GetListOrdersUpdate(1, pageSize, dataCreateLast);

            int totalPages = (int)Math.Ceiling((double)orderList.total_count / pageSize);

            if (totalPages > 0)
            {
                for (int i = 1; i <= totalPages; i++)
                {
                    if (orderList == null)
                    {
                        orderList = GetListOrdersUpdate(i, pageSize, dataCreateLast);
                    }

                    if (orderList == null) continue;

                    callOrdersUpdateSL(orderList).Wait();
                    callSaveDateUpdate(orderList).Wait();

                    orderList = null;
                }
            }
        }

        static private void workCancellationOrders()
        {
            OrdersBLL orderBLL = new OrdersBLL();

            foreach (var order in orderBLL.GetCancelledOrders())
            {
                var magento = new ClientMagento();

                // Realiza o cancelamento do pedido no Magento
                if (magento.CancelOrder(order.Value))
                {
                    // Atualiza o campo ORDR.U_CVA_IntegratedCancellation para Y (Sim)
                    callOrdersIntegratedCancellationUpdate(order.Key).Wait();
                }
            }
        }

        static private void workSetOrdersStatusMessages()
        {
            OrdersBLL orderBLL = new OrdersBLL();

            foreach (var order in orderBLL.GetOrdersStatusMessages())
            {
                var magento = new ClientMagento();

                if (order.Status == "emissaoNF")
                {
                    magento.SendShipping(order.EntityId, order.Message, order.Status);
                }

                magento.SendOrderComments(order.EntityId, order.Message, order.Status);

                callOrdersStatusMessagesIntegratedUpdate(order.Code).Wait();
            }
        }

        static private List<string> GetItemsToIntegrate()
        {
            var itemMasterDataBLL = new ItemMasterDataBLL();
            return itemMasterDataBLL.GetItemsToIntegrate();
        }

        static private async Task<ProductModel> callGetItemData(string itemCode)
        {
            var itemMasterDataBLL = new ItemMasterDataBLL();
            return await itemMasterDataBLL.GetItemData(itemCode);
        }

        static private async Task callSetItemIntegratedStatus(string itemCode, string productId)
        {
            var itemMasterDataBLL = new ItemMasterDataBLL();
            await itemMasterDataBLL.SetItemIntegratedStatus(itemCode, productId);
        }

        static private async Task callSaveDateCreate(OrdersListModel orderList)
        {
            CVA_MAGENTO_DT_LAST_BLL BLL = new CVA_MAGENTO_DT_LAST_BLL();
            var data = DateTime.Parse(orderList.items.OrderByDescending(i => i.created_at).FirstOrDefault().created_at);
            await BLL.SaveOrdersLastCreateDate(data, "Orders");
        }

        static private async Task callSaveDateUpdate(OrdersListModel orderList)
        {
            CVA_MAGENTO_DT_LAST_BLL BLL = new CVA_MAGENTO_DT_LAST_BLL();
            var data = DateTime.Parse(orderList.items.OrderByDescending(i => i.updated_at).FirstOrDefault().updated_at);
            await BLL.SaveOrdersLastUpdateDate(data, "Orders");
        }

        static private async Task<DateTime> callGetLastDateCreate(string endPoint)
        {
            CVA_MAGENTO_DT_LAST_BLL dataLastCreate_BLL = new CVA_MAGENTO_DT_LAST_BLL();
            CVA_MAGENTO_LAST_DT_Model dataLastCreateModel = await dataLastCreate_BLL.GetLastCreateDate(endPoint);
            DateTime data = DateTime.Parse(dataLastCreateModel.U_DataCreate.ToString());
            TimeSpan timeSpan = TimeSpan.Parse(dataLastCreateModel.U_HoraCreate.ToString());
            int totalMinutes = (int)timeSpan.TotalMinutes;
            data = data.AddMinutes(totalMinutes);
            data = data.AddSeconds(dataLastCreateModel.U_SegundoCreate);
            return data;
        }

        static private async Task<DateTime> callGetLastDateUpdate(string endPoint)
        {
            CVA_MAGENTO_DT_LAST_BLL dataLastCreate_BLL = new CVA_MAGENTO_DT_LAST_BLL();
            CVA_MAGENTO_LAST_DT_Model dataLastCreateModel = await dataLastCreate_BLL.GetLastCreateDate(endPoint);
            DateTime data = DateTime.Parse(dataLastCreateModel.U_DataUpdate.ToString());
            TimeSpan timeSpan = TimeSpan.Parse(dataLastCreateModel.U_HoraUpdate.ToString());
            int totalMinutes = (int)timeSpan.TotalMinutes;
            data = data.AddMinutes(totalMinutes);
            data = data.AddSeconds(dataLastCreateModel.U_SegundoUpdate);
            return data;
        }

        static private async Task callOrdersCreateSL(OrdersListModel orderList)
        {
            OrdersBLL orderBLL = new OrdersBLL();
            await orderBLL.Create(orderList);
        }

        static private async Task callOrdersUpdateSL(OrdersListModel orderList)
        {
            OrdersBLL orderBLL = new OrdersBLL();
            await orderBLL.Update(orderList);
        }

        static private async Task callOrdersIntegratedCancellationUpdate(int docEntry)
        {
            OrdersBLL orderBLL = new OrdersBLL();
            await orderBLL.SetIntegratedCancellation(docEntry);
        }

        static private async Task callOrdersStatusMessagesIntegratedUpdate(int code)
        {
            OrdersBLL orderBLL = new OrdersBLL();
            await orderBLL.SetIntegratedStatusMessage(code);
        }

        static Product SetProduct(ProductModel product)
        {
            var magento = new ClientMagento();
            return magento.SetProduct(product);
        }

        static OrdersListModel GetListOrdersCreate(int currentPage, int pageSize, DateTime data)
        {
            var magento = new ClientMagento();
            return magento.GetListOrdersCreate(currentPage, pageSize, data);
        }

        static OrdersListModel GetListOrdersUpdate(int currentPage, int pageSize, DateTime data)
        {
            var magento = new ClientMagento();
            return magento.GetListOrdersUpdate(currentPage, pageSize, data);
        }

        static OrdersListModel GetListPendingOrdersCreate(string pendingOrders)
        {
            var magento = new ClientMagento();
            return magento.GetListPendingOrdersCreate(pendingOrders);
        }
    }
}
