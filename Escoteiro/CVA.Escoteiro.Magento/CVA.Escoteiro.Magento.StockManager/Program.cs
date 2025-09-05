using CVA.Escoteiro.Magento.BLL.BLL;
using CVA.Escoteiro.Magento.StockManager.Client;
using Escoteiro.Magento.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.StockManager
{
    class Program
    {
        public static string Token;
        public static string Api = ConfigurationManager.AppSettings["ApiMagento"];
        public static string User = ConfigurationManager.AppSettings["ApiMagentoUser"];
        public static string Password = ConfigurationManager.AppSettings["ApiMagentoPassWord"];
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            // Realiza a sincronização do estoque do SAP Business One com o do Magento
            workSynchronizeStock();
        }

        private static void workSynchronizeStock()
        {
            try
            {
                logger.Trace("Iniciando integração de estoque entre o SAP Business One e o do Magento");

                // Obtém os itens a terem a quantidade em estoque atualizada na tabela @CVA_STOCK_MAGENTO
                var stockQtyList = GetOutdatedStockQuantity();

                // Obtém o token de acesso
                Token = GetToken(User, Password);

                foreach (var stockQty in stockQtyList)
                {
                    // Atualiza a tabela @CVA_STOCK_MAGENTO com as quantidades em estoque atuais dos itens
                    callSaveUpdatedStockQuantity(stockQty).Wait();

                    // Atualiza no Magento a quantidade em estoque dos itens que sofreram alteração
                    callUpdateMagentoStockQuantity(stockQty);
                }

                logger.Trace("Finalizando integração de estoque entre o SAP Business One e o do Magento");
            }
            catch (Exception ex)
            {
                logger.Error($"Exceção na integração de estoque entre o SAP Business One e o do Magento: {ex.InnerException}");
            }
        }

        private static List<CVA_MAGENTO_STOCK_Model> GetOutdatedStockQuantity()
        {
            var stock_BLL = new CVA_MAGENTO_STOCK_BLL();
            return stock_BLL.GetOutdatedStockQuantity();
        }

        private static async Task callSaveUpdatedStockQuantity(CVA_MAGENTO_STOCK_Model stockQty)
        {
            var stock_BLL = new CVA_MAGENTO_STOCK_BLL();
            await stock_BLL.SaveUpdatedStockQuantity(stockQty);
        }

        static string GetToken(string userName, string password)
        {
            var client = new ClientMagento(Api);
            return client.GetAdminToken(userName, password);
        }

        private static void callUpdateMagentoStockQuantity(CVA_MAGENTO_STOCK_Model stockQty)
        {
            var magento = new ClientMagento(Api, Token);
            logger.Trace(magento.SetProductStockQuantity(Token, stockQty.U_BarCode, stockQty.U_WhsQty));
        }
    }
}
