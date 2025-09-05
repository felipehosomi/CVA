using CVA.Escoteiro.Magento.Client;
using CVA.Escoteiro.Magento.DAO.Resources;
using CVA.Escoteiro.Magento.DAO.Util;
using CVA.Escoteiro.Magento.Models.Magento;
using CVA.Escoteiro.Magento.Models.SAP;
using Escoteiro.Magento.Models;
using Newtonsoft.Json;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.BLL
{
    public class OrdersBLL : BaseBLL
    {
        public string DataBase;
        CVA_MAGENTO_PARAM_BLL parametrosBLL;
        CVA_MAGENTO_PARAM_Model parametrosModel;

        ServiceLayerUtil sl;

        public OrdersBLL()
        {
            DataBase = ConfigurationManager.AppSettings["Database"];
            sl = new ServiceLayerUtil();
        }

        public async Task Update(OrdersListModel orderList)
        {
            //await Task.WhenAll(orderList.items.Select(i => UpdateDocs(i)));

            foreach (var item in orderList.items)
            {
                await UpdateDocs(item);
            }
        }

        public async Task Create(OrdersListModel orderList)
        {
            //await Task.WhenAll(orderList.items.Select(i => CreateAllDocuments(i)));

            foreach (var item in orderList.items)
            {
                await CreateAllDocuments(item);
            }
        }

        public async Task UpdateDocs(Item model)
        {
            await VerifyStateOrder(model);
        }

        public async Task VerifyStateOrder(Item model)
        {
            string json = string.Empty;

            try
            {
                json = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                DocumentoMarketingModel documentMarketingModel;
                DocumentoMarketingModel downPaymentModel;
                BusinessPartnersModel businessPartner;

                parametrosBLL = new CVA_MAGENTO_PARAM_BLL();
                parametrosModel = await parametrosBLL.GetById("1");

                if (model.state == "processing" || model.state == "new")
                {
                    var docentry = DAO.ExecuteScalar(String.Format(HanaCommands.DownPayment_Get, DataBase, model.entity_id));

                    if (docentry != null)
                    {
                        downPaymentModel = await sl.GetByIDAsync<DocumentoMarketingModel>("DownPayments", (int)docentry);

                        if (model.state == "processing" && downPaymentModel.PaidToDate == 0.0)
                        {
                            await CreatePayment(json, model, downPaymentModel);
                            SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, "Pagamento realizado com sucesso.", "OK", downPaymentModel.DocEntry, downPaymentModel.ObjectType, "IncomingPayments").Wait();
                        }
                    }
                    else
                    {
                        docentry = DAO.ExecuteScalar(String.Format(HanaCommands.Orders_Get, DataBase, model.entity_id));

                        if (docentry == null)
                        {
                            var bussinesPartnerModel = await CreatePn(model);
                            documentMarketingModel = await CreateOrders(json, model, bussinesPartnerModel);
                            downPaymentModel = await CreateDownPayments(json, model, bussinesPartnerModel, documentMarketingModel);

                            if (model.state == "processing" && downPaymentModel.PaidToDate == 0.0)
                            {
                                await CreatePayment(json, model, downPaymentModel);
                                SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, "Pagamento realizado com sucesso.", "OK", downPaymentModel.DocEntry, downPaymentModel.ObjectType, "IncomingPayments").Wait();
                            }
                        }
                        else
                        {
                            documentMarketingModel = await sl.GetByIDAsync<DocumentoMarketingModel>("Orders", (int)docentry);
                            businessPartner = await sl.GetByIDAsync<BusinessPartnersModel>("BusinessPartners", documentMarketingModel.CardCode);
                            downPaymentModel = await CreateDownPayments(json, model, businessPartner, documentMarketingModel);

                            if (model.state == "processing" && downPaymentModel.PaidToDate == 0.0)
                            {
                                await CreatePayment(json, model, downPaymentModel);
                                SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, "Pagamento realizado com sucesso.", "OK", downPaymentModel.DocEntry, downPaymentModel.ObjectType, "IncomingPayments").Wait();
                            }
                        }
                    }
                }
                else if (model.state == "canceled")
                {
                    int docentry = await CancelOrders(model);
                    SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, "", "OK", docentry, "17", "Orders").Wait();
                }
            }
            catch (AggregateException err)
            {
                if (err.Message.Contains("Unexpected character encountered while parsing value: <. Path '', line 0, position 0.")) return;

                // Altera o status do pedido para Em Espera
                var magento = new ClientMagento();
                magento.HoldOrder(model.entity_id);

                foreach (var errInner in err.InnerExceptions)
                {
                    SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, String.Concat("Pedido ", model.increment_id, ": ", errInner.Message), "NOK", null, "", "Orders").Wait();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Unexpected character encountered while parsing value: <. Path '', line 0, position 0.")) return;

                // Altera o status do pedido para Em Espera
                var magento = new ClientMagento();
                magento.HoldOrder(model.entity_id);

                SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, String.Concat("Pedido ", model.increment_id, ": ", ex.Message), "NOK", null, "", "Orders").Wait();
            }
        }

        public async Task CreateAllDocuments(Item model)
        {
            var json = string.Empty;

            try
            {
                json = JsonConvert.SerializeObject(model, Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                await sl.Login(DataBase);

                parametrosBLL = new CVA_MAGENTO_PARAM_BLL();
                parametrosModel = await parametrosBLL.GetById("1");

                var bussinesPartnerModel = await CreatePn(model);
                var orderModel = await CreateOrders(json, model, bussinesPartnerModel);
                var downPaymentModel = await CreateDownPayments(json, model, bussinesPartnerModel, orderModel);

                // Caso o State do pedido não esteja como processado, então ainda não houve o pagamento
                if (model.state == "processing" && downPaymentModel.PaidToDate == 0.0)
                {
                    await CreatePayment(json, model, downPaymentModel);
                }
            }
            catch (AggregateException err)
            {
                // Altera o status do pedido para Em Espera
                var magento = new ClientMagento();
                magento.HoldOrder(model.entity_id);

                foreach (var errInner in err.InnerExceptions)
                {
                    SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, String.Concat("Pedido ", model.increment_id, ": ", errInner.Message), "NOK", null, "", "Orders").Wait();
                }
            }
            catch (Exception ex)
            {
                // Altera o status do pedido para Em Espera
                var magento = new ClientMagento();
                magento.HoldOrder(model.entity_id);

                SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, String.Concat("Pedido ", model.increment_id, ": ", ex.Message), "NOK", null, "", "Orders").Wait();
            }
        }

        public async Task<DocumentoMarketingModel> CreateDownPayments(string json, Item model, BusinessPartnersModel bussinesPartnerModel, DocumentoMarketingModel orderModel)
        {
            var sql = String.Format(HanaCommands.DownPayment_Get, DataBase, model.entity_id);
            var docentry = DAO.ExecuteScalar(sql);

            if (docentry != null)
            {
                return await sl.GetByIDAsync<DocumentoMarketingModel>("DownPayments", (int)docentry);
            }

            var parametrosBLL = new CVA_MAGENTO_PARAM_BLL();
            var parametrosModel = await parametrosBLL.GetById("1");
            var returnList = new List<string>();
            var downPayment = new DocumentoMarketingModel();
            var lineNum = 0;

            downPayment.DownPaymentType = "dptInvoice";
            downPayment.CardCode = bussinesPartnerModel.CardCode;
            downPayment.DocDate = orderModel.DocDate;
            downPayment.TaxDate = orderModel.DocDate;
            downPayment.DocDueDate = orderModel.DocDueDate;
            downPayment.U_CVA_EntityId = (int)model.entity_id;
            downPayment.U_CVA_Increment_id = Convert.ToInt32(model.increment_id);
            downPayment.BPL_IDAssignedToInvoice = parametrosModel.U_BplID;
            downPayment.DocumentLines = new List<Documentline>();

            foreach (var item in orderModel.DocumentLines)
            {
                var lineModel = new Documentline();

                lineModel.ItemCode = item.ItemCode;
                lineModel.UnitPrice = item.UnitPrice;
                lineModel.DiscountPercent = item.DiscountPercent;
                lineModel.Quantity = item.Quantity;
                lineModel.Usage = item.Usage;
                lineModel.WarehouseCode = item.WarehouseCode;
                lineModel.BaseEntry = orderModel.DocEntry;
                lineModel.BaseType = 17;
                lineModel.BaseLine = lineNum;
                downPayment.DocumentLines.Add(lineModel);

                lineNum++;
            }

            if (orderModel.DocumentAdditionalExpenses != null && orderModel.DocumentAdditionalExpenses.Sum(x => x.LineTotal) > 0)
            {
                downPayment.DocTotal = orderModel.DocumentLines.Sum(x => (x.UnitPrice - (x.UnitPrice * x.DiscountPercent / 100)) * x.Quantity) + orderModel.DocumentAdditionalExpenses.Sum(x => x.LineTotal);
            }

            returnList = await sl.PostAsyncReturnList("DownPayments", downPayment);

            if (returnList[0] == "NOK")
            {
                throw new Exception($"Erro ao gerar adiantamento: {returnList[1]}");
            }

            downPayment = JsonConvert.DeserializeObject<DocumentoMarketingModel>(returnList[1]);

            SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, "Adiantamento de cliente inserido com sucesso.", "OK", downPayment.DocEntry, "203", "DownPayments").Wait();

            return downPayment;
        }

        public async Task<int> CancelOrders(Item model)
        {
            var returnList = new List<string>();
            var docentryOrders = DAO.ExecuteScalar(String.Format(HanaCommands.Orders_Get, DataBase, model.entity_id));

            if (docentryOrders != null)
            {
                if (await sl.PostFuncAsync("Orders", docentryOrders.ToString(), "Cancel") != "")
                {
                    throw new Exception("Erro ao gerar cancelamento do pedido de venda: " + returnList[1]);
                }
            }

            var docentryDown = DAO.ExecuteScalar(String.Format(HanaCommands.DownPayment_Get, DataBase, model.entity_id));

            if (docentryDown != null)
            {
                var dowmPayment = await sl.GetByIDAsync<DocumentoMarketingModel>("DownPayments", (int)docentryDown);
                var creditNotesModel = new DocumentoMarketingModel();

                creditNotesModel.CardCode = dowmPayment.CardCode;
                creditNotesModel.DocDate = DateTime.Today.ToString("yyyy-MM-dd");
                creditNotesModel.TaxDate = DateTime.Today.ToString("yyyy-MM-dd");
                creditNotesModel.DocDueDate = DateTime.Today.ToString("yyyy-MM-dd");
                creditNotesModel.BPL_IDAssignedToInvoice = dowmPayment.BPL_IDAssignedToInvoice;
                creditNotesModel.SequenceCode = -2;
                creditNotesModel.SequenceSerial = int.Parse(model.increment_id);

                creditNotesModel.DocumentLines = new List<Documentline>();

                foreach (var item in dowmPayment.DocumentLines)
                {
                    var lineModel = new Documentline();

                    lineModel.ItemCode = item.ItemCode;
                    lineModel.BaseEntry = dowmPayment.DocEntry;
                    lineModel.BaseLine = item.LineNum;
                    lineModel.BaseType = 203;
                    lineModel.Quantity = item.Quantity;
                    lineModel.Usage = item.Usage;
                    lineModel.UnitPrice = item.UnitPrice;
                    lineModel.WarehouseCode = item.WarehouseCode;

                    creditNotesModel.DocumentLines.Add(lineModel);
                }

                creditNotesModel.DocTotal = dowmPayment.DocTotal;

                returnList = await sl.PostAsyncReturnList<DocumentoMarketingModel>("CreditNotes", creditNotesModel);

                if (returnList[0] == "NOK")
                {
                    throw new Exception($"Erro ao gerar devolução para o adiantamento de cliente: {returnList[1]}");
                }
            }

            if (docentryOrders == null)
            {
                docentryOrders = 0;
            }

            return Convert.ToInt32(docentryOrders);
        }

        public async Task<DocumentoMarketingModel> CreateOrders(string json, Item model, BusinessPartnersModel bussinesPartnerModel)
        {
            // Verifica se o pedido de venda já foi inserido no SAP Business One através do Entity ID do pedido no Magento
            var docentry = DAO.ExecuteScalar(String.Format(HanaCommands.Orders_Get, DataBase, model.entity_id));

            // Caso o pedido de venda já esteja inserido no SAP Business One, retorna os dados do pedido de venda
            if (docentry != null)
            {
                return await sl.GetByIDAsync<DocumentoMarketingModel>("Orders", (int)docentry);
            }

            var parametrosBLL = new CVA_MAGENTO_PARAM_BLL();
            // Obtém os parâmetros de valores padrões a serem utilizados na inserção dos pedidos de venda
            var parametrosModel = await parametrosBLL.GetById("1");
            var returnList = new List<string>();
            var orderModel = new DocumentoMarketingModel();

            orderModel.CardCode = bussinesPartnerModel.CardCode;
            orderModel.DocDate = DateTime.ParseExact(model.created_at, "yyyy-MM-dd HH:mm:ss", null).AddHours(-3).ToString("yyyy-MM-dd HH:mm:ss");
            orderModel.DocDueDate = DateTime.ParseExact(model.created_at, "yyyy-MM-dd HH:mm:ss", null).AddHours(-3).ToString("yyyy-MM-dd HH:mm:ss");
            orderModel.U_CVA_EntityId = (int)model.entity_id;
            orderModel.U_CVA_Increment_id = Convert.ToInt32(model.increment_id);
            orderModel.BPL_IDAssignedToInvoice = parametrosModel.U_BplID;
            orderModel.U_OrigemPedido = "1";
            orderModel.ShipToCode = String.Concat("ENTREGA_", model.extension_attributes.shipping_assignments.First().shipping.address.customer_address_id.ToString());
            orderModel.PayToCode = String.Concat("COBRANCA_", model.billing_address.customer_address_id.ToString());
            orderModel.DiscountPercent = 0;
            orderModel.U_nfe_tipoEnv = model.extension_attributes.shipping_assignments.FirstOrDefault().shipping.total.base_shipping_incl_tax > 0 ? "1" : "0";
            orderModel.PaymentMethod = model.payment.method.Contains("creditcard") ? "R_CARTAO" : "R_BOLETO";

            orderModel.DocumentLines = new List<Documentline>();

            float totalWeight = 0;

            //fazer requisição pegar os itens do pedido
            //devdocs.magento.com/redoc/2.3/admin-rest-api.html#tag/ordersid
            foreach (var item in model.items.Where(x => x.price > 0.0))
            {
                var itemCode = DAO.ExecuteScalar(String.Format(HanaCommands.Item_Get, DataBase, item.sku));

                if (itemCode == null)
                {
                    throw new Exception($"Item não encontrado para o sku {item.sku}");
                }

                var lineModel = new Documentline();

                lineModel.ItemCode = itemCode.ToString();
                lineModel.UnitPrice = bussinesPartnerModel.GroupCode != 104 ? item.price : null;
                lineModel.Quantity = item.qty_ordered;
                lineModel.DiscountPercent = item.discount_percent;

                lineModel.CostingCode = "07"; // LEN
                lineModel.CostingCode2 = "0701"; // Vendas
                lineModel.CostingCode3 = "070101"; // Revenda de Mercadorias
                lineModel.U_CVA_CCusto = "070101";

                if (Convert.ToInt32(parametrosModel.U_Utilizacao) > 0) lineModel.Usage = Convert.ToInt32(parametrosModel.U_Utilizacao);
                if (!String.IsNullOrEmpty(parametrosModel.U_Deposito)) lineModel.WarehouseCode = parametrosModel.U_Deposito;

                var manBtchNum = DAO.ExecuteScalar(String.Format(HanaCommands.ItemManageBatchNum_Get, DataBase, itemCode)).ToString();

                if (manBtchNum == "Y")
                {
                    lineModel.BatchNumbers = new List<Batchnumber>();
                    lineModel.BatchNumbers.Add(new Batchnumber
                    {
                        BaseLineNumber = orderModel.DocumentLines.Count(),
                        BatchNumber = "1",
                        Quantity = item.qty_ordered
                    });
                }

                orderModel.DocumentLines.Add(lineModel);

                totalWeight += item.weight * item.qty_ordered;
            }

            orderModel.TaxExtension = new Taxextension();
            orderModel.TaxExtension.GrossWeight = totalWeight;
            orderModel.TaxExtension.NetWeight = totalWeight;
            orderModel.TaxExtension.PackDescription = "Caixa";
            orderModel.TaxExtension.Incoterms = model.extension_attributes.shipping_assignments.FirstOrDefault().shipping.total.base_shipping_incl_tax > 0 ? "1" : "0";

            var shippingDescription = model.shipping_description.Replace(" - ", "-").Split('-');

            if (shippingDescription.Count() >= 2)
            {
                var transpName = shippingDescription[0].ToUpper();
                var incoterms = orderModel.TaxExtension.Incoterms == "1" ? "POR CONTA DESTINATÁRIO" : "POR CONTA REMETENTE";
                var shipType = Convert.ToInt16(DAO.ExecuteScalar(String.Format(HanaCommands.TipoTransporte_Get, DataBase, $"{transpName} {incoterms}").ToString()));
                var carrier = DAO.ExecuteScalar(String.Format(HanaCommands.Carrier_Get, DataBase, shippingDescription[1].ToUpper()));

                if (shipType == 0)
                {
                    throw new Exception($"Tipo de envio não encontrado: {transpName} {incoterms}");
                }

                if (carrier == null)
                {
                    throw new Exception($"Transportadora não encontrada: {shippingDescription[1].ToUpper()}");
                }

                orderModel.TransportationCode = (int)shipType;
                orderModel.TaxExtension.Carrier = carrier.ToString();
            }

            orderModel.DocumentAdditionalExpenses = new List<DocumentAdditionalExpenses>();

            var docExpenses = new DocumentAdditionalExpenses();

            docExpenses.ExpenseCode = 1;
            docExpenses.LineTotal = model.extension_attributes.shipping_assignments.FirstOrDefault().shipping.total.base_shipping_incl_tax;

            if (parametrosModel.U_TaxExpsCode != null) docExpenses.TaxCode = parametrosModel.U_TaxExpsCode;
            orderModel.DocumentAdditionalExpenses.Add(docExpenses);

            returnList = await sl.PostAsyncReturnList("Orders", orderModel);

            if (returnList[0] == "NOK")
            {
                throw new Exception($"Erro ao gerar o pedido de venda: {returnList[1]}");
            }

            orderModel = JsonConvert.DeserializeObject<DocumentoMarketingModel>(returnList[1]);
            SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, "Pedido de venda integrado com sucesso.", "OK", orderModel.DocEntry, "17", "Orders").Wait();

            return orderModel;
        }

        public async Task<BusinessPartnersModel> CreatePn(Item model)
        {
            object cardCode = null;
            object taxId = null;

            using (HanaDataReader dr = DAO.ExecuteReader(String.Format(HanaCommands.BP_Get, DataBase, model.customer_taxvat)))
            {
                if (dr.HasRows)
                {
                    var dt = new DataTable();
                    dt.Load(dr);

                    foreach (DataRow row in dt.Rows)
                    {
                        if (String.IsNullOrEmpty(row["TaxId"].ToString())) continue;

                        cardCode = row["CardCode"].ToString();
                        taxId = row["TaxId"].ToString();
                    }
                }
            }

            if (cardCode == null)
            {
                throw new Exception($"Cliente portador do documento {model.customer_taxvat} não cadastrado no SAP Business One.");
            }
            else
            {
                // Obtém os dados do PN através do CardCode
                var businessPartner = await sl.GetByIDAsync<BusinessPartnersModel>("BusinessPartners", cardCode.ToString());

                // Verifica se o PN já tem as formas de pagamento vinculadas, caso contrário fica inserindo novos registros na CRD2
                if (!businessPartner.BPPaymentMethods.Any(x => x.PaymentMethodCode == (model.payment.method.Contains("creditcard") ? "R_CARTAO" : "R_BOLETO")))
                {
                    var bussinesPartnerModel = new BusinessPartnersModel();
                    bussinesPartnerModel.BPPaymentMethods = new List<BPPaymentMethods>();
                    bussinesPartnerModel.BPPaymentMethods.Add(new BPPaymentMethods { PaymentMethodCode = model.payment.method.Contains("creditcard") ? "R_CARTAO" : "R_BOLETO" });

                    var errorMessage = await sl.PatchAsync<BusinessPartnersModel>("BusinessPartners", "'" + cardCode.ToString() + "'", bussinesPartnerModel);

                    if (!String.IsNullOrEmpty(errorMessage))
                    {
                        throw new Exception(errorMessage);
                    }
                }

                SetBillingAddress(model, cardCode.ToString(), taxId.ToString()).Wait();
                SetShippingAddress(model, cardCode.ToString(), taxId.ToString()).Wait();

                return businessPartner;
            }
        }

        private async Task SetBillingAddress(Item model, string cardCode, string taxId)
        {
            if (DAO.ExecuteScalar(String.Format(HanaCommands.Address_Get, DataBase, cardCode, "COBRANCA_" + model.billing_address.customer_address_id.ToString())) != null) return;

            var bussinesPartnerModel = new BusinessPartnersModel();
            var regex = new Regex(@"^(RUA|R.|R|AVENIDA|AV.|AV|TRAVESSA|TRAV.|TRAV|ALAMEDA|AL|AL.|RODOVIA|ROD.|ROD|TREVO|TRV.|TRV)", RegexOptions.IgnoreCase);
            var bpAdress = new Bpaddress();
            bussinesPartnerModel.BPAddresses = new List<Bpaddress>();
            bussinesPartnerModel.BPFiscalTaxIDCollection = new List<Bpfiscaltaxidcollection>();
            bpAdress.AddressName = "COBRANCA_" + model.billing_address.customer_address_id.ToString();
            bpAdress.AddressType = "bo_BillTo";
            bpAdress.AddressName2 = model.billing_address_id;
            bpAdress.ZipCode = model.billing_address.postcode;
            bpAdress.Country = model.billing_address.country_id;
            bpAdress.State = model.billing_address.region_code;

            var billingCountyCode = DAO.ExecuteScalar(String.Format(HanaCommands.CountyAbsId_Get, DataBase, model.billing_address.city.TrimStart().TrimEnd().Replace("\'", ""), model.billing_address.region_code));

            if (billingCountyCode == null)
            {
                throw new Exception($"Cidade {model.billing_address.city} informada no endereço de cobrança não encontrada na lista de cidades do SAP Business One.");
            }

            bpAdress.County = billingCountyCode.ToString();
            bpAdress.City = model.billing_address.city;

            for (int i = 0; i < model.billing_address.street.Count(); i++)
            {
                switch (i)
                {
                    case 0:
                        var street = regex.Match(model.billing_address.street[i].ToUpper());

                        if (street.Success)
                        {
                            bpAdress.TypeOfAddress = street.Value.First().ToString().ToUpper() + String.Join("", street.Value.ToLower().Skip(1));
                            bpAdress.Street = Regex.Replace(model.billing_address.street[i], street.Value, "", RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            bpAdress.TypeOfAddress = "Rua";
                            bpAdress.Street = model.billing_address.street[i];
                        }

                        break;
                    case 1:
                        bpAdress.StreetNo = model.billing_address.street[i];
                        break;
                    case 2:
                        if (model.billing_address.street.Count() == 3)
                        {
                            bpAdress.Block = model.billing_address.street[i];
                        }
                        else
                        {
                            bpAdress.BuildingFloorRoom = model.billing_address.street[i];
                        }
                        break;
                    case 3:
                        bpAdress.Block = model.billing_address.street[i];
                        break;
                }
            }

            bussinesPartnerModel.BPAddresses.Add(bpAdress);

            var bpFiscal = new Bpfiscaltaxidcollection();
            bpFiscal.Address = "COBRANCA_" + model.billing_address.customer_address_id.ToString();
            bpFiscal.AddrType = "bo_BillTo";
            switch (Regex.Replace(model.customer_taxvat, @"[^\d]", "").Length)
            {
                case 11:
                    bpFiscal.TaxId4 = taxId.ToString(); // model.customer_taxvat;
                    break;

                case 14:
                    bpFiscal.TaxId0 = taxId.ToString(); // model.customer_taxvat;
                    break;
            }
            bussinesPartnerModel.BPFiscalTaxIDCollection.Add(bpFiscal);

            var errorMessage = await sl.PatchAsync<BusinessPartnersModel>("BusinessPartners", "'" + cardCode.ToString() + "'", bussinesPartnerModel);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                throw new Exception(errorMessage);
            }
        }

        private async Task SetShippingAddress(Item model, string cardCode, string taxId)
        {
            var bussinesPartnerModel = new BusinessPartnersModel();
            var regex = new Regex(@"^(RUA|R.|R|AVENIDA|AV.|AV|TRAVESSA|TRAV.|TRAV|ALAMEDA|AL|AL.|RODOVIA|ROD.|ROD|TREVO|TRV.|TRV)", RegexOptions.IgnoreCase);
            var bpAdress = new Bpaddress();
            var AddressName = string.Empty;

            bussinesPartnerModel = new BusinessPartnersModel();
            bussinesPartnerModel.BPAddresses = new List<Bpaddress>();
            bussinesPartnerModel.BPFiscalTaxIDCollection = new List<Bpfiscaltaxidcollection>();
            var bp = await sl.GetByIDAsync<BusinessPartnersModel>("BusinessPartners", cardCode);
            bussinesPartnerModel.BPAddresses = bp.BPAddresses;

            foreach (var item in model.extension_attributes.shipping_assignments)
            {
                bpAdress = new Bpaddress();
                var id_address = "ENTREGA_" + item.shipping.address.customer_address_id;

                
                var exists = bussinesPartnerModel.BPAddresses.Exists(x => x.AddressName == id_address);
                if (exists)
                {
                    foreach (var bpAddress in bussinesPartnerModel.BPAddresses.Where(x => x.AddressName == id_address))
                    {
                        bpAdress = bpAddress;
                    }
                }
                else
                {
                    AddressName = "ENTREGA_" + item.shipping.address.customer_address_id;
                    bpAdress.AddressName = AddressName;
                }
               
               
               
                bpAdress.AddressType = "bo_ShipTo";
                bpAdress.Country = item.shipping.address.country_id;
                bpAdress.AddressName2 = item.shipping.address.customer_address_id;
                bpAdress.ZipCode = item.shipping.address.postcode;
                bpAdress.Country = item.shipping.address.country_id;
                bpAdress.State = item.shipping.address.region_code;

                var shippingCountyCode = DAO.ExecuteScalar(String.Format(HanaCommands.CountyAbsId_Get, DataBase, item.shipping.address.city.TrimStart().TrimEnd().Replace("\'", ""), item.shipping.address.region_code));

                if (shippingCountyCode == null)
                {
                    throw new Exception($"Cidade {item.shipping.address.city} informada no endereço de entrega não encontrada na lista de cidades do SAP Business One.");
                }

                bpAdress.County = shippingCountyCode.ToString();
                bpAdress.City = item.shipping.address.city;

                for (int i = 0; i < item.shipping.address.street.Count(); i++)
                {
                    switch (i)
                    {
                        case 0:
                            var street = regex.Match(item.shipping.address.street[i].ToUpper());

                            if (street.Success)
                            {
                                bpAdress.TypeOfAddress = street.Value.First().ToString().ToUpper() + String.Join("", street.Value.ToLower().Skip(1));
                                bpAdress.Street = Regex.Replace(item.shipping.address.street[i], street.Value, "", RegexOptions.IgnoreCase);
                            }
                            else
                            {
                                bpAdress.TypeOfAddress = "Rua";
                                bpAdress.Street = item.shipping.address.street[i];
                            }
                            break;
                        case 1:
                            bpAdress.StreetNo = item.shipping.address.street[i];
                            break;
                        case 2:
                            if (item.shipping.address.street.Count() == 3)
                            {
                                bpAdress.Block = item.shipping.address.street[i];
                            }
                            else
                            {
                                bpAdress.BuildingFloorRoom = item.shipping.address.street[i];
                            }
                            break;
                        case 3:
                            bpAdress.Block = item.shipping.address.street[i];
                            break;
                    }
                }
                //var errorMessage = await sl.PatchAsync<Bpaddress>("Bpaddress", "'" + cardCode.ToString() + "'", bussinesPartnerModel);
                if (!exists)
                {
                    bussinesPartnerModel.BPAddresses.Add(bpAdress);

                    var bpFiscal = new Bpfiscaltaxidcollection();
                    bpFiscal.Address = AddressName;
                    bpFiscal.AddrType = "bo_ShipTo";
                    switch (Regex.Replace(model.customer_taxvat, @"[^\d]", "").Length)
                    {
                        case 11:
                            bpFiscal.TaxId4 = taxId.ToString(); // model.customer_taxvat;
                            break;

                        case 14:
                            bpFiscal.TaxId0 = taxId.ToString(); // model.customer_taxvat;
                            break;
                    }
                    bussinesPartnerModel.BPFiscalTaxIDCollection.Add(bpFiscal);
                }
                
            }


            var errorMessage = await sl.PatchAsync<BusinessPartnersModel>("BusinessPartners", "'" + cardCode.ToString() + "'", bussinesPartnerModel);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                throw new Exception(errorMessage);
            }
        }

        public async Task<BusinessPartnersModel> GetPn(Item model)
        {
            var bussinesPartnerModel = new BusinessPartnersModel();
            var cardCode = DAO.ExecuteScalar(String.Format(HanaCommands.BP_Get, DataBase, model.customer_id));

            if (cardCode == null)
            {
                throw new Exception($"Não encontrado o cliente cadastrado com o código: {model.customer_id}");
            }

            return await sl.GetByIDAsync<BusinessPartnersModel>("BusinessPartners", model.customer_id.ToString());
        }

        public async Task CreatePayment(string json, Item model, DocumentoMarketingModel dowModel)
        {
            var docentry = DAO.ExecuteScalar(String.Format(HanaCommands.IncomingPayments_Get, DataBase, model.entity_id));

            if (docentry != null)
            {
                return;
            }

            // Realiza a baixa do adiantamento de acordo com o tipo de pagamento (a combinar (dinheiro), boleto (dinheiro) e cartão de crédito)
            switch (model.payment.method)
            {
                case "mundipagg_creditcard":
                    {
                        var returnList = new List<string>();
                        var operadora = parametrosModel.U_CreditCardOp.ToUpper();
                        var bandeira = model.payment.cc_type.ToUpper();
                        var creditCard = DAO.ExecuteScalar(String.Format(HanaCommands.CreditCard_Get, DataBase, String.Concat(operadora, "_", bandeira)));

                        if (creditCard == null)
                        {
                            throw new Exception($"Não encontrado a configuração do cartão {String.Concat(operadora, "_", bandeira)} no sap.");
                        }

                        var creditCardTypeCode = int.Parse(DAO.ExecuteScalar(String.Format(HanaCommands.CreditCardTypeCode_Get, DataBase, Convert.ToInt32(model.payment.additional_information[model.payment.additional_information.Count() - 2]) == 1 ? "CREDITO" : "PARCELADO")).ToString());
                        var magento = new ClientMagento();
                        var voucher = magento.GetCreditCardVoucher(model.entity_id);
                        var ownerIdNum = magento.GetOwnerIdNum(model.entity_id);
                        var ano = model.payment.cc_exp_year;
                        var mes = Convert.ToInt32(model.payment.cc_exp_month).ToString("D2");
                        var cardDateUntil = new DateTime(Convert.ToInt32(ano), Convert.ToInt32(mes), 1);
                        var paymentModel = new IncomingPaymentsModel();

                        paymentModel.DocType = "rCustomer";
                        paymentModel.DocDate = DateTime.Today.ToString("yyyy-MM-dd");
                        paymentModel.DueDate = DateTime.Today.ToString("yyyy-MM-dd");
                        paymentModel.TaxDate = DateTime.Today.ToString("yyyy-MM-dd");
                        paymentModel.CardCode = dowModel.CardCode;
                        paymentModel.BPLID = dowModel.BPL_IDAssignedToInvoice;
                        paymentModel.U_CVA_EntityId = model.entity_id;
                        paymentModel.U_CVA_Increment_id = Convert.ToInt32(model.increment_id);
                        paymentModel.PaymentInvoices = new List<Paymentinvoice>();

                        var paymentInvoiceModel = new Paymentinvoice();

                        paymentInvoiceModel.DocEntry = (int)dowModel.DocEntry;
                        paymentInvoiceModel.InstallmentId = 1;
                        paymentInvoiceModel.InvoiceType = "it_DownPayment";
                        paymentInvoiceModel.SumApplied = (float)dowModel.DocTotal;
                        paymentModel.PaymentInvoices.Add(paymentInvoiceModel);
                        paymentModel.PaymentCreditCards = new List<Paymentcreditcard>();

                        var paymentCreditModel = new Paymentcreditcard();

                        paymentCreditModel.CardValidUntil = cardDateUntil.ToString("yyyy-MM-dd");
                        paymentCreditModel.CreditCard = Convert.ToInt32(creditCard);
                        paymentCreditModel.VoucherNum = voucher;
                        paymentCreditModel.OwnerIdNum = ownerIdNum;
                        paymentCreditModel.PaymentMethodCode = creditCardTypeCode;

                        var creditCardNumber = model.payment.cc_last4;

                        paymentCreditModel.CreditCardNumber = creditCardNumber.ToString().Substring(creditCardNumber.ToString().Length - 4, 4);
                        paymentCreditModel.NumOfPayments = Convert.ToInt32(model.payment.additional_information[model.payment.additional_information.Count() - 2]);
                        paymentCreditModel.CreditSum = paymentInvoiceModel.SumApplied;

                        if (paymentCreditModel.NumOfPayments > 1)
                        {
                            paymentCreditModel.FirstPaymentSum = (float)Math.Round(paymentInvoiceModel.SumApplied / paymentCreditModel.NumOfPayments, 2);
                            paymentCreditModel.AdditionalPaymentSum = (float)Math.Round(paymentInvoiceModel.SumApplied / paymentCreditModel.NumOfPayments, 2);
                            paymentCreditModel.SplitPayments = "N";

                            var t = Math.Round(paymentCreditModel.AdditionalPaymentSum * (paymentCreditModel.NumOfPayments - 1), 2);
                            var u = Math.Round(paymentCreditModel.CreditSum, 2);
                            double dif = Math.Round(u - t, 2);

                            if (dif != 0)
                            {
                                paymentCreditModel.FirstPaymentSum = (float)dif;
                            }
                        }

                        if (creditCardTypeCode != 0) paymentCreditModel.PaymentMethodCode = Convert.ToInt32(creditCardTypeCode);
                        paymentModel.PaymentCreditCards.Add(paymentCreditModel);

                        returnList = await sl.PostAsyncReturnList("IncomingPayments", paymentModel);

                        if (returnList[0] == "NOK")
                        {
                            throw new Exception($"Erro ao gerar o pagamento do adiantamento de cliente: {returnList[1]}");
                        }

                        paymentModel = JsonConvert.DeserializeObject<IncomingPaymentsModel>(returnList[1]);
                        SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, "Contas a receber gerado com sucesso.", "OK", paymentModel.DocEntry, "24", "IncomingPayments").Wait();
                    }
                    break;

                case "braspag_pagador_creditcard":
                    {
                        var returnList = new List<string>();
                        var operadora = parametrosModel.U_CreditCardOp.ToUpper();
                        var bandeira = "";
                        if (String.IsNullOrEmpty(model.payment.cc_type) && 
                            model.payment.cc_type.IndexOf("-") != 0)
                        {
                             bandeira = model.payment.cc_type.ToUpper().Substring(model.payment.cc_type.IndexOf("-") + 1, model.payment.cc_type.Length - model.payment.cc_type.IndexOf("-") - 1);
                        }

                       
                        var creditCard = DAO.ExecuteScalar(String.Format(HanaCommands.CreditCard_Get, DataBase, String.Concat(operadora, "_", bandeira)));

                        if (creditCard == null)
                        {
                            throw new Exception($"Não encontrado a configuração do cartão {String.Concat(operadora, "_", bandeira)} no sap.");
                        }

                        var creditCardTypeCode = int.Parse(DAO.ExecuteScalar(String.Format(HanaCommands.CreditCardTypeCode_Get, DataBase, Convert.ToInt32(model.extension_attributes.payment_additional_info.Where(x => x.key == "cc_installments").First().value) == 1 ? "CREDITO" : "PARCELADO")).ToString());
                        var magento = new ClientMagento();
                        //var ownerIdNum = magento.GetOwnerIdNum(model.entity_id);
                        var ano = model.payment.cc_exp_year;
                        var mes = Convert.ToInt32(model.payment.cc_exp_month).ToString("D2");
                        var cardDateUntil = new DateTime(Convert.ToInt32(ano), Convert.ToInt32(mes), 1);
                        var paymentModel = new IncomingPaymentsModel();

                        paymentModel.DocType = "rCustomer";
                        paymentModel.DocDate = DateTime.Today.ToString("yyyy-MM-dd");
                        paymentModel.DueDate = DateTime.Today.ToString("yyyy-MM-dd");
                        paymentModel.TaxDate = DateTime.Today.ToString("yyyy-MM-dd");
                        paymentModel.CardCode = dowModel.CardCode;
                        paymentModel.BPLID = dowModel.BPL_IDAssignedToInvoice;
                        paymentModel.U_CVA_EntityId = model.entity_id;
                        paymentModel.U_CVA_Increment_id = Convert.ToInt32(model.increment_id);
                        paymentModel.PaymentInvoices = new List<Paymentinvoice>();

                        var paymentInvoiceModel = new Paymentinvoice();

                        paymentInvoiceModel.DocEntry = (int)dowModel.DocEntry;
                        paymentInvoiceModel.InstallmentId = 1;
                        paymentInvoiceModel.InvoiceType = "it_DownPayment";
                        paymentInvoiceModel.SumApplied = (float)dowModel.DocTotal;
                        paymentModel.PaymentInvoices.Add(paymentInvoiceModel);
                        paymentModel.PaymentCreditCards = new List<Paymentcreditcard>();

                        var paymentCreditModel = new Paymentcreditcard();

                        paymentCreditModel.CardValidUntil = cardDateUntil.ToString("yyyy-MM-dd");
                        paymentCreditModel.CreditCard = Convert.ToInt32(creditCard);
                        paymentCreditModel.VoucherNum = model.extension_attributes.payment_additional_info.Where(x => x.key == "proof_of_sale").First().value;
                        paymentCreditModel.PaymentMethodCode = creditCardTypeCode;

                        var creditCardNumber = model.payment.cc_last4;

                        paymentCreditModel.CreditCardNumber = creditCardNumber.ToString().Substring(creditCardNumber.ToString().Length - 4, 4);
                        paymentCreditModel.NumOfPayments = Convert.ToInt32(model.extension_attributes.payment_additional_info.Where(x => x.key == "cc_installments").First().value);
                        paymentCreditModel.CreditSum = paymentInvoiceModel.SumApplied;

                        if (paymentCreditModel.NumOfPayments > 1)
                        {
                            paymentCreditModel.FirstPaymentSum = (float)Math.Round(paymentInvoiceModel.SumApplied / paymentCreditModel.NumOfPayments, 2);
                            paymentCreditModel.AdditionalPaymentSum = (float)Math.Round(paymentInvoiceModel.SumApplied / paymentCreditModel.NumOfPayments, 2);
                            paymentCreditModel.SplitPayments = "Y";

                            var t = Math.Round(paymentCreditModel.AdditionalPaymentSum * (paymentCreditModel.NumOfPayments - 1), 2);
                            var u = Math.Round(paymentCreditModel.CreditSum, 2);
                            double dif = Math.Round(u - t, 2);

                            if (dif != 0)
                            {
                                paymentCreditModel.FirstPaymentSum = (float)dif;
                            }
                        }

                        if (creditCardTypeCode != 0) paymentCreditModel.PaymentMethodCode = Convert.ToInt32(creditCardTypeCode);
                        paymentModel.PaymentCreditCards.Add(paymentCreditModel);

                        returnList = await sl.PostAsyncReturnList("IncomingPayments", paymentModel);

                        if (returnList[0] == "NOK")
                        {
                            throw new Exception($"Erro ao gerar o pagamento do adiantamento de cliente: {returnList[1]}");
                        }

                        paymentModel = JsonConvert.DeserializeObject<IncomingPaymentsModel>(returnList[1]);
                        SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, "Contas a receber gerado com sucesso.", "OK", paymentModel.DocEntry, "24", "IncomingPayments").Wait();
                    }
                    break;

                default:
                    {
                        var returnList = new List<string>();
                        var paymentModel = new IncomingPaymentsModel();

                        paymentModel.DocType = "rCustomer";
                        paymentModel.DocDate = DateTime.Today.ToString("yyyy-MM-dd");
                        paymentModel.DueDate = DateTime.Today.ToString("yyyy-MM-dd");
                        paymentModel.TaxDate = DateTime.Today.ToString("yyyy-MM-dd");
                        paymentModel.CardCode = dowModel.CardCode;
                        paymentModel.BPLID = dowModel.BPL_IDAssignedToInvoice;
                        paymentModel.U_CVA_EntityId = model.entity_id;
                        paymentModel.U_CVA_Increment_id = Convert.ToInt32(model.increment_id);
                        paymentModel.PaymentInvoices = new List<Paymentinvoice>();

                        var paymentInvoiceModel = new Paymentinvoice();

                        paymentInvoiceModel.DocEntry = (int)dowModel.DocEntry;
                        paymentInvoiceModel.InstallmentId = 1;
                        paymentInvoiceModel.InvoiceType = "it_DownPayment";
                        paymentInvoiceModel.SumApplied = (float)dowModel.DocTotal;

                        paymentModel.PaymentInvoices.Add(paymentInvoiceModel);
                        paymentModel.CashAccount = parametrosModel.U_CashAccount;
                        paymentModel.CashSum = paymentInvoiceModel.SumApplied;

                        returnList = await sl.PostAsyncReturnList<IncomingPaymentsModel>("IncomingPayments", paymentModel);

                        if (returnList[0] == "NOK")
                        {
                            throw new Exception($"Erro ao gerar o pagamento do adiantamento de cliente: {returnList[1]}");
                        }

                        paymentModel = JsonConvert.DeserializeObject<IncomingPaymentsModel>(returnList[1]);
                        SaveOrdersLog(json, (int)model.entity_id, model.state, model.status, "Contas a receber gerado com sucesso.", "OK", paymentModel.DocEntry, "24", "IncomingPayments").Wait();
                    }
                    break;
            }
        }

        private async Task<string> SaveOrdersLastCreateDate(DateTime data, string endpoint)
        {
            var sl = new ServiceLayerUtil();

            await sl.Login(DataBase);

            var magentoSaveDateLast = new CVA_MAGENTO_LAST_DT_Model();
            var returnList = new List<string>();
            var retorno = string.Empty;
            var code = DAO.ExecuteScalar(String.Format(HanaCommands.LastDate_GetCode, DataBase, endpoint));

            magentoSaveDateLast.U_DataCreate = data.ToString("yyyy-MM-dd");
            magentoSaveDateLast.U_HoraCreate = data.ToString("HH:mm");
            magentoSaveDateLast.U_EndPoint = endpoint;

            if (code == null)
            {
                magentoSaveDateLast.Code = (int)code;
                returnList = await sl.PostAsyncReturnList("U_CVA_MAGENTO_DT", magentoSaveDateLast);
            }
            else
            {
                returnList = await sl.PatchAsyncReturnList("U_CVA_MAGENTO_DT", (int)code, magentoSaveDateLast);
            }

            if (returnList[0] == "NOK")
            {
                retorno = "Erro ao gravar data last create magento na tabela CVA_MAGENTO_DT: " + returnList[1];
            }

            return retorno;
        }

        private async Task<string> SaveOrdersLastUpdateDate(DateTime data, string endpoint)
        {
            var sl = new ServiceLayerUtil();

            await sl.Login(DataBase);

            var returnList = new List<string>();
            var retorno = string.Empty;
            var code = DAO.ExecuteScalar(String.Format(HanaCommands.LastDate_GetCode, DataBase, endpoint));
            var magentoSaveDateLast = new CVA_MAGENTO_LAST_DT_Model();

            magentoSaveDateLast.U_DataUpdate = data.ToString("yyyy-MM-dd");
            magentoSaveDateLast.U_HoraUpdate = data.ToString("HH:mm");
            magentoSaveDateLast.U_EndPoint = endpoint;

            if (code == null)
            {
                magentoSaveDateLast.Code = (int)code;
                returnList = await sl.PostAsyncReturnList("U_CVA_MAGENTO_DT", magentoSaveDateLast);
            }
            else
            {
                returnList = await sl.PatchAsyncReturnList("U_CVA_MAGENTO_DT", (int)code, magentoSaveDateLast);
            }

            if (returnList[0] == "NOK")
            {
                retorno = "Erro ao gravar data last create magento na tabela CVA_MAGENTO_DT: " + returnList[1];
            }

            return retorno;
        }

        private async Task<string> SaveOrdersLog(string json, int entity_id, string state, string status, string Message, string StatusProc, int? docEntry, string objType, string method = "")
        {
            var sl = new ServiceLayerUtil();

            await sl.Login(DataBase);

            var returnList = new List<string>();
            var retorno = string.Empty;
            var orderMagento = new CVA_ORDERS_MAGENTO_Model();

            orderMagento.Name = method;
            orderMagento.U_Data = DateTime.Today.ToString("yyyy-MM-dd");
            orderMagento.U_Hora = DateTime.Now.ToString("HH:mm");
            orderMagento.U_State = state;
            orderMagento.U_Status = status;
            orderMagento.U_EntityId = entity_id;
            orderMagento.U_Mensagem = Message;
            orderMagento.U_StatusProc = StatusProc;
            orderMagento.U_DataProc = DateTime.Today.ToString("yyyy-MM-dd");
            orderMagento.U_HoraProc = DateTime.Now.ToString("HH:mm");
            orderMagento.U_DocEntry = docEntry;
            orderMagento.U_ObjType = objType;
            orderMagento.U_JSON = json;
            orderMagento.U_IntegrateStatus = "N";

            returnList = await sl.PostAsyncReturnList("U_CVA_ORDERS_MAGENTO", orderMagento);

            while (returnList[1].Contains("-2035"))
            {
                DAO.ExecuteScalar(String.Format(HanaCommands.OrdersNextVal_Set, DataBase));
                returnList = await sl.PostAsyncReturnList("U_CVA_ORDERS_MAGENTO", orderMagento);
            }

            if (returnList[0] == "NOK")
            {
                throw new Exception($"Erro ao gravar pedido magento na tabela CVA_ORDERS_MAGENTO: {returnList[1]} para o pedido {entity_id}");
            }

            return retorno;
        }

        public Dictionary<int, int> GetCancelledOrders()
        {
            var cancelledOrders = new Dictionary<int, int>();
            var serviceLayer = new ServiceLayerUtil();
            var sql = String.Format(HanaCommands.CancelledOrders_Get, DataBase);

            using (HanaDataReader dr = DAO.ExecuteReader(sql))
            {
                if (dr.HasRows)
                {
                    var dt = new DataTable();
                    dt.Load(dr);

                    foreach (DataRow row in dt.Rows)
                    {
                        cancelledOrders.Add(int.Parse(row["DocEntry"].ToString()), int.Parse(row["U_CVA_EntityId"].ToString()));
                    }
                }
            }

            return cancelledOrders;
        }

        public List<OrderstatusMessages> GetOrdersStatusMessages()
        {
            //// Realiza a inserção de registros para alterar o status do pedido para 'transportadora'
            //DAO.ExecuteNonQuery(String.Format(HanaCommands.OrderStatusMessages_SetShipped, DataBase));

            var orderStatusMessages = new List<OrderstatusMessages>();
            var serviceLayer = new ServiceLayerUtil();
            var sql = String.Format(HanaCommands.OrderStatusMessages_Get, DataBase);

            using (HanaDataReader dr = DAO.ExecuteReader(sql))
            {
                var dt = new DataTable();
                dt.Load(dr);

                foreach (DataRow row in dt.Rows)
                {
                    orderStatusMessages.Add(new OrderstatusMessages
                    {
                        Code = int.Parse(row["Code"].ToString()),
                        EntityId = int.Parse(row["U_EntityId"].ToString()),
                        Message = row["U_Mensagem"].ToString(),
                        Status = row["U_Status"].ToString()
                    });
                }
            }

            return orderStatusMessages;
        }

        public async Task<string> SetIntegratedCancellation(int docEntry)
        {
            var retorno = string.Empty;

            try
            {
                var orderModel = new DocumentoMarketingModel();
                var returnList = new List<string>();

                await sl.Login(DataBase);

                orderModel = await sl.GetByIDAsync<DocumentoMarketingModel>("Orders", docEntry);
                orderModel.U_CVA_IntegratedCancellation = "Y";

                returnList = await sl.PatchAsyncReturnList<DocumentoMarketingModel>("Orders", docEntry, orderModel);

                if (returnList[0] == "NOK")
                {
                    retorno = $"Erro ao atualizar o campo U_CVA_IntegratedCancellation no pedido de venda {docEntry} : {returnList[1]}";
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return retorno;
        }

        public async Task<string> SetIntegratedStatusMessage(int code)
        {
            var orderModel = new CVA_ORDERS_MAGENTO_Model();
            var returnList = new List<string>();
            var retorno = string.Empty;

            await sl.Login(DataBase);

            orderModel = await sl.GetByIDAsync<CVA_ORDERS_MAGENTO_Model>("U_CVA_ORDERS_MAGENTO", code);
            orderModel.U_IntegrateStatus = "N";

            returnList = await sl.PatchAsyncReturnList<CVA_ORDERS_MAGENTO_Model>("U_CVA_ORDERS_MAGENTO", code, orderModel);

            if (returnList[0] == "NOK")
            {
                throw new Exception("Erro ao gravar pedido magento na tabela CVA_ORDERS_MAGENTO: " + returnList[1]);
            }

            return retorno;
        }

        public async Task CancelPayment()
        {
            await sl.Login(DataBase);

            using (StreamReader sr = new StreamReader("c:/orders.csv"))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    using (HanaDataReader dr = DAO.ExecuteReader($@"select ""DocEntry"" from ""{DataBase}"".ORCT where ""U_CVA_Increment_id"" = {currentLine} and ""Canceled"" = 'N'"))
                    {
                        if (!dr.HasRows) continue;

                        Console.WriteLine($@"{DateTime.Now} - Cancelando pagamento - {currentLine.ToString()}");

                        var dt = new DataTable();
                        dt.Load(dr);

                        foreach (DataRow row in dt.Rows)
                        {
                            await sl.PostFuncAsync("IncomingPayments", row["DocEntry"].ToString(), "Cancel");
                        }
                    }
                }
            }
        }

        public async Task CancelDownPayment()
        {
            await sl.Login(DataBase);
            string currentLine;

            try
            {
                using (StreamReader sr = new StreamReader("c:/orders.csv"))
                {
                    // currentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        using (HanaDataReader dr = DAO.ExecuteReader($@"select ""DocEntry"" from ""{DataBase}"".ODPI where ""U_CVA_Increment_id"" = {currentLine} and ""DocStatus"" <> 'C' and not exists (select * from ""{DataBase}"".DPI1 where DPI1.""DocEntry"" = ODPI.""DocEntry"" and DPI1.""TargetType"" in (14))"))
                        {
                            if (!dr.HasRows) continue;

                            Console.WriteLine($@"{DateTime.Now} - Cancelando adiantamento - {currentLine.ToString()}");

                            var dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                var dowmPayment = await sl.GetByIDAsync<DocumentoMarketingModel>("DownPayments", int.Parse(row["DocEntry"].ToString()));
                                var creditNotesModel = new DocumentoMarketingModel();

                                creditNotesModel.CardCode = dowmPayment.CardCode;
                                creditNotesModel.DocDate = DateTime.Today.ToString("yyyy-MM-dd");
                                creditNotesModel.TaxDate = DateTime.Today.ToString("yyyy-MM-dd");
                                creditNotesModel.DocDueDate = DateTime.Today.ToString("yyyy-MM-dd");
                                creditNotesModel.BPL_IDAssignedToInvoice = dowmPayment.BPL_IDAssignedToInvoice;
                                creditNotesModel.SequenceCode = -2;
                                creditNotesModel.SequenceSerial = dowmPayment.U_CVA_Increment_id;
                                creditNotesModel.DocumentLines = new List<Documentline>();

                                foreach (var item in dowmPayment.DocumentLines)
                                {
                                    var lineModel = new Documentline();

                                    lineModel.ItemCode = item.ItemCode;
                                    lineModel.BaseEntry = dowmPayment.DocEntry;
                                    lineModel.BaseLine = item.LineNum;
                                    lineModel.BaseType = 203;
                                    lineModel.Quantity = item.Quantity;
                                    lineModel.Usage = item.Usage;
                                    lineModel.UnitPrice = item.UnitPrice;
                                    lineModel.WarehouseCode = item.WarehouseCode;

                                    creditNotesModel.DocumentLines.Add(lineModel);
                                }

                                creditNotesModel.DocTotal = dowmPayment.DocTotal;


                                var returnList = await sl.PostAsyncReturnList<DocumentoMarketingModel>("CreditNotes", creditNotesModel);

                                if (returnList[0] == "NOK")
                                {
                                    throw new Exception($"Erro ao gerar devolução para o adiantamento de cliente: {returnList[1]}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        public async Task CancelOrders()
        {
            await sl.Login(DataBase);

            using (StreamReader sr = new StreamReader("c:/orders.csv"))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    Console.WriteLine($@"{DateTime.Now} - Cancelando pedido - {currentLine.ToString()}");

                    await sl.PostFuncAsync("Orders", currentLine, "Cancel");

                    //using (HanaDataReader dr = DAO.ExecuteReader($@"select ""DocEntry"" from ""{DataBase}"".ORDR where ""U_CVA_Increment_id"" = {currentLine} and ""CANCELED"" = 'N'"))
                    //{
                    //    if (!dr.HasRows) continue;

                    //    Console.WriteLine($@"{DateTime.Now} - Cancelando pedido - {currentLine.ToString()}");

                    //    var dt = new DataTable();
                    //    dt.Load(dr);

                    //    foreach (DataRow row in dt.Rows)
                    //    {
                    //        await sl.PostFuncAsync("Orders", row["DocEntry"].ToString(), "Cancel");
                    //    }
                    //}
                }
            }
        }

        public async Task CreateSpecificOrders()
        {
            try
            {
                using (StreamReader sr = new StreamReader("c:/orders.csv"))
                {
                    string currentLine;
                    // currentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        Console.WriteLine($@"{DateTime.Now} - Recriando pedido - {currentLine.ToString()}");

                        var magento = new ClientMagento();

                        var model = magento.GetOrder(int.Parse(currentLine));

                        if (model.items.First().state == "holded")
                        {
                            magento.UnholdOrder(model.items.First().entity_id);
                            model = magento.GetOrder(int.Parse(currentLine));
                        }

                        await Create(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"{DateTime.Now} - {ex.Message}");
            }
        }

        public async Task UpdateAddress()
        {
            await sl.Login(DataBase);

            using (HanaDataReader dr = DAO.ExecuteReader($@"with Addresses as
                                                            (
	                                                            select ""CardCode"", ""AdresType"", ""Address"", replace(replace(""Address"", 'COBRANCA_', ''), 'ENTREGA_', '') as ID 
	                                                              from ""{DataBase}"".CRD1 
	                                                             where ""Block"" is null
                                                            )	
                                                            select ORDR.""U_CVA_EntityId"", Addresses.*
                                                              from Addresses
                                                             inner join ""{DataBase}"".ORDR on ORDR.""CardCode"" = Addresses.""CardCode""
                                                               and ORDR.""U_CVA_EntityId"" is not null
                                                             where ORDR.""U_CVA_EntityId"" = 85035"))
            {
                var dt = new DataTable();
                dt.Load(dr);

                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        var magento = new ClientMagento();
                        var model = magento.GetOrder(int.Parse(row["U_CVA_EntityId"].ToString()));

                        if (model == null) continue;

                        var item = model.items.First();
                        var addresssType = row["AdresType"].ToString();
                        var regex = new Regex(@"^(RUA|R.|R|AVENIDA|AV.|AV|TRAVESSA|TRAV.|TRAV|ALAMEDA|AL|AL.|RODOVIA|ROD.|ROD|TREVO|TRV.|TRV)", RegexOptions.IgnoreCase);
                        var cardCode = row["CardCode"].ToString();
                        var bp = await sl.GetByIDAsync<BusinessPartnersModel>("BusinessPartners", cardCode);
                        var bussinesPartnerModel = new BusinessPartnersModel();
                        var AddressName = string.Empty;

                        bussinesPartnerModel = new BusinessPartnersModel();
                        bussinesPartnerModel.BPAddresses = bp.BPAddresses;

                        AddressName = (row["AdresType"].ToString() == "S" ? "ENTREGA_" : "COBRANCA_") + (addresssType == "B" ? item.billing_address_id : item.extension_attributes.shipping_assignments.First().shipping.address.customer_address_id);

                        var exists = bussinesPartnerModel.BPAddresses.Exists(x => x.AddressName == AddressName);

                        if (exists)
                        {
                            foreach (var bpAddress in bussinesPartnerModel.BPAddresses.Where(x => x.AddressName == AddressName))
                            {
                                if (addresssType == "B")
                                {
                                    bpAddress.AddressName = AddressName;
                                    bpAddress.RowNum = int.Parse(DAO.ExecuteScalar(String.Format(HanaCommands.Address_Get, DataBase, cardCode, AddressName)).ToString());
                                    bpAddress.AddressType = (row["AdresType"].ToString() == "S" ? "bo_ShipTo" : "bo_BillTo");
                                    bpAddress.Country = item.billing_address.country_id;
                                    bpAddress.AddressName2 = item.billing_address_id;
                                    bpAddress.ZipCode = item.billing_address.postcode;
                                    bpAddress.Country = item.billing_address.country_id;
                                    bpAddress.State = item.billing_address.region_code;

                                    var shippingCountyCode = DAO.ExecuteScalar(String.Format(HanaCommands.CountyAbsId_Get, DataBase, item.billing_address.city.TrimStart().TrimEnd().Replace("\'", ""), item.billing_address.region_code));

                                    if (shippingCountyCode == null)
                                    {
                                        throw new Exception($"Cidade {item.billing_address.city} informada no endereço de entrega não encontrada na lista de cidades do SAP Business One.");
                                    }

                                    bpAddress.County = shippingCountyCode.ToString();
                                    bpAddress.City = item.billing_address.city;

                                    for (int i = 0; i < item.billing_address.street.Count(); i++)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                var street = regex.Match(item.billing_address.street[i].ToUpper());

                                                if (street.Success)
                                                {
                                                    bpAddress.TypeOfAddress = street.Value.First().ToString().ToUpper() + String.Join("", street.Value.ToLower().Skip(1));
                                                    bpAddress.Street = Regex.Replace(item.billing_address.street[i], street.Value, "", RegexOptions.IgnoreCase);
                                                }
                                                else
                                                {
                                                    bpAddress.TypeOfAddress = "Rua";
                                                    bpAddress.Street = item.billing_address.street[i];
                                                }
                                                break;
                                            case 1:
                                                bpAddress.StreetNo = item.billing_address.street[i];
                                                break;
                                            case 2:
                                                if (item.billing_address.street.Count() == 3)
                                                {
                                                    bpAddress.Block = item.billing_address.street[i];
                                                }
                                                else
                                                {
                                                    bpAddress.BuildingFloorRoom = item.billing_address.street[i];
                                                }
                                                break;
                                            case 3:
                                                bpAddress.Block = item.billing_address.street[i];
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    bpAddress.AddressName = AddressName;
                                    bpAddress.RowNum = int.Parse(DAO.ExecuteScalar(String.Format(HanaCommands.Address_Get, DataBase, cardCode, AddressName)).ToString());
                                    bpAddress.AddressType = (row["AdresType"].ToString() == "S" ? "bo_ShipTo" : "bo_BillTo");
                                    bpAddress.Country = item.extension_attributes.shipping_assignments.First().shipping.address.country_id;
                                    bpAddress.AddressName2 = item.extension_attributes.shipping_assignments.First().shipping.address.customer_address_id;
                                    bpAddress.ZipCode = item.extension_attributes.shipping_assignments.First().shipping.address.postcode;
                                    bpAddress.Country = item.extension_attributes.shipping_assignments.First().shipping.address.country_id;
                                    bpAddress.State = item.extension_attributes.shipping_assignments.First().shipping.address.region_code;

                                    var shippingCountyCode = DAO.ExecuteScalar(String.Format(HanaCommands.CountyAbsId_Get, DataBase, item.extension_attributes.shipping_assignments.First().shipping.address.city.TrimStart().TrimEnd().Replace("\'", ""), item.extension_attributes.shipping_assignments.First().shipping.address.region_code));

                                    if (shippingCountyCode == null)
                                    {
                                        throw new Exception($"Cidade {item.extension_attributes.shipping_assignments.First().shipping.address.city} informada no endereço de entrega não encontrada na lista de cidades do SAP Business One.");
                                    }

                                    bpAddress.County = shippingCountyCode.ToString();
                                    bpAddress.City = item.extension_attributes.shipping_assignments.First().shipping.address.city;

                                    for (int i = 0; i < item.extension_attributes.shipping_assignments.First().shipping.address.street.Count(); i++)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                var street = regex.Match(item.extension_attributes.shipping_assignments.First().shipping.address.street[i].ToUpper());

                                                if (street.Success)
                                                {
                                                    bpAddress.TypeOfAddress = street.Value.First().ToString().ToUpper() + String.Join("", street.Value.ToLower().Skip(1));
                                                    bpAddress.Street = Regex.Replace(item.extension_attributes.shipping_assignments.First().shipping.address.street[i], street.Value, "", RegexOptions.IgnoreCase);
                                                }
                                                else
                                                {
                                                    bpAddress.TypeOfAddress = "Rua";
                                                    bpAddress.Street = item.extension_attributes.shipping_assignments.First().shipping.address.street[i];
                                                }
                                                break;
                                            case 1:
                                                bpAddress.StreetNo = item.extension_attributes.shipping_assignments.First().shipping.address.street[i];
                                                break;
                                            case 2:
                                                if (item.extension_attributes.shipping_assignments.First().shipping.address.street.Count() == 3)
                                                {
                                                    bpAddress.Block = item.extension_attributes.shipping_assignments.First().shipping.address.street[i];
                                                }
                                                else
                                                {
                                                    bpAddress.BuildingFloorRoom = item.extension_attributes.shipping_assignments.First().shipping.address.street[i];
                                                }
                                                break;
                                            case 3:
                                                bpAddress.Block = item.extension_attributes.shipping_assignments.First().shipping.address.street[i];
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var bpAddress = new Bpaddress();

                            if (addresssType == "B")
                            {
                                bpAddress.AddressName = AddressName;
                                //bpAddress.RowNum = int.Parse(DAO.ExecuteScalar(String.Format(HanaCommands.Address_Get, DataBase, cardCode, AddressName)).ToString());
                                bpAddress.AddressType = (row["AdresType"].ToString() == "S" ? "bo_ShipTo" : "bo_BillTo");
                                bpAddress.Country = item.billing_address.country_id;
                                bpAddress.AddressName2 = item.billing_address_id;
                                bpAddress.ZipCode = item.billing_address.postcode;
                                bpAddress.Country = item.billing_address.country_id;
                                bpAddress.State = item.billing_address.region_code;

                                var shippingCountyCode = DAO.ExecuteScalar(String.Format(HanaCommands.CountyAbsId_Get, DataBase, item.billing_address.city.TrimStart().TrimEnd().Replace("\'", ""), item.billing_address.region_code));

                                if (shippingCountyCode == null)
                                {
                                    throw new Exception($"Cidade {item.billing_address.city} informada no endereço de entrega não encontrada na lista de cidades do SAP Business One.");
                                }

                                bpAddress.County = shippingCountyCode.ToString();
                                bpAddress.City = item.billing_address.city;

                                for (int i = 0; i < item.billing_address.street.Count(); i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            var street = regex.Match(item.billing_address.street[i].ToUpper());

                                            if (street.Success)
                                            {
                                                bpAddress.TypeOfAddress = street.Value.First().ToString().ToUpper() + String.Join("", street.Value.ToLower().Skip(1));
                                                bpAddress.Street = Regex.Replace(item.billing_address.street[i], street.Value, "", RegexOptions.IgnoreCase);
                                            }
                                            else
                                            {
                                                bpAddress.TypeOfAddress = "Rua";
                                                bpAddress.Street = item.billing_address.street[i];
                                            }
                                            break;
                                        case 1:
                                            bpAddress.StreetNo = item.billing_address.street[i];
                                            break;
                                        case 2:
                                            if (item.billing_address.street.Count() == 3)
                                            {
                                                bpAddress.Block = item.billing_address.street[i];
                                            }
                                            else
                                            {
                                                bpAddress.BuildingFloorRoom = item.billing_address.street[i];
                                            }
                                            break;
                                        case 3:
                                            bpAddress.Block = item.billing_address.street[i];
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                bpAddress.AddressName = AddressName;
                                //bpAddress.RowNum = int.Parse(DAO.ExecuteScalar(String.Format(HanaCommands.Address_Get, DataBase, cardCode, AddressName)).ToString());
                                bpAddress.AddressType = (row["AdresType"].ToString() == "S" ? "bo_ShipTo" : "bo_BillTo");
                                bpAddress.Country = item.extension_attributes.shipping_assignments.First().shipping.address.country_id;
                                bpAddress.AddressName2 = item.extension_attributes.shipping_assignments.First().shipping.address.customer_address_id;
                                bpAddress.ZipCode = item.extension_attributes.shipping_assignments.First().shipping.address.postcode;
                                bpAddress.Country = item.extension_attributes.shipping_assignments.First().shipping.address.country_id;
                                bpAddress.State = item.extension_attributes.shipping_assignments.First().shipping.address.region_code;

                                var shippingCountyCode = DAO.ExecuteScalar(String.Format(HanaCommands.CountyAbsId_Get, DataBase, item.extension_attributes.shipping_assignments.First().shipping.address.city.TrimStart().TrimEnd().Replace("\'", ""), item.extension_attributes.shipping_assignments.First().shipping.address.region_code));

                                if (shippingCountyCode == null)
                                {
                                    throw new Exception($"Cidade {item.extension_attributes.shipping_assignments.First().shipping.address.city} informada no endereço de entrega não encontrada na lista de cidades do SAP Business One.");
                                }

                                bpAddress.County = shippingCountyCode.ToString();
                                bpAddress.City = item.extension_attributes.shipping_assignments.First().shipping.address.city;

                                for (int i = 0; i < item.extension_attributes.shipping_assignments.First().shipping.address.street.Count(); i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            var street = regex.Match(item.extension_attributes.shipping_assignments.First().shipping.address.street[i].ToUpper());

                                            if (street.Success)
                                            {
                                                bpAddress.TypeOfAddress = street.Value.First().ToString().ToUpper() + String.Join("", street.Value.ToLower().Skip(1));
                                                bpAddress.Street = Regex.Replace(item.extension_attributes.shipping_assignments.First().shipping.address.street[i], street.Value, "", RegexOptions.IgnoreCase);
                                            }
                                            else
                                            {
                                                bpAddress.TypeOfAddress = "Rua";
                                                bpAddress.Street = item.extension_attributes.shipping_assignments.First().shipping.address.street[i];
                                            }
                                            break;
                                        case 1:
                                            bpAddress.StreetNo = item.extension_attributes.shipping_assignments.First().shipping.address.street[i];
                                            break;
                                        case 2:
                                            if (item.extension_attributes.shipping_assignments.First().shipping.address.street.Count() == 3)
                                            {
                                                bpAddress.Block = item.extension_attributes.shipping_assignments.First().shipping.address.street[i];
                                            }
                                            else
                                            {
                                                bpAddress.BuildingFloorRoom = item.extension_attributes.shipping_assignments.First().shipping.address.street[i];
                                            }
                                            break;
                                        case 3:
                                            bpAddress.Block = item.extension_attributes.shipping_assignments.First().shipping.address.street[i];
                                            break;
                                    }
                                }
                            }

                            bussinesPartnerModel.BPAddresses.Add(bpAddress);
                        }

                        var errorMessage = await sl.PatchAsyncWithReplaceCollectionsOnPatch<BusinessPartnersModel>("BusinessPartners", "'" + cardCode.ToString() + "'", bussinesPartnerModel);

                        if (!String.IsNullOrEmpty(errorMessage))
                        {
                            throw new Exception(errorMessage);
                        }

                    }
                    catch (Exception ex)
                    {
                        var a = ex.Message;
                    }
                }
            }
        }
    }
}
