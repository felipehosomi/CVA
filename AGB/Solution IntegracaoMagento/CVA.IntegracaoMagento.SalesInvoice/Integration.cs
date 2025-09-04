using CVA.IntegracaoMagento.SalesInvoice.Models.SAP;
using Flurl.Http;
using Newtonsoft.Json;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.SalesInvoice
{
    public class Integration
    {
        internal static SLConnection SLConnection;
        private static readonly string ServiceLayerURL = ConfigurationManager.AppSettings["ServiceLayerURL"];
        internal static readonly string Database = ConfigurationManager.AppSettings["Database"];
        private static readonly string B1User = ConfigurationManager.AppSettings["B1User"];
        private static readonly string B1Password = ConfigurationManager.AppSettings["B1Password"];

        public Integration()
        {

        }

        public async Task SetIntegration()
        {
            DateTime dataAtual = DateTime.Now.AddSeconds(1);
            string sCaminho = String.Format(@"{0}Log", System.AppDomain.CurrentDomain.BaseDirectory.ToString());

            if (!(System.IO.Directory.Exists(sCaminho)))
                System.IO.Directory.CreateDirectory(sCaminho);

            sCaminho = String.Format(@"{0}\\Log_{1}.txt", sCaminho, String.Format(@"{0}{1}{2}", dataAtual.Year.ToString(), dataAtual.Month.ToString().PadLeft(2, '0'), dataAtual.Day.ToString().PadLeft(2, '0')));

            if (!(System.IO.File.Exists(sCaminho)))
                System.IO.File.Create(sCaminho).Close();

            Util.GravarLog(sCaminho, "[PROCESSO] - Iniciando o processo.");

            try
            {
                SLConnection = new SLConnection(ServiceLayerURL, Database, B1User, B1Password, 29);
                
                var deliveryNote = new DeliveryNotes.DeliveryNote();
                var order = new Orders.Order();
                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão HANA (OK)");
                var invoiceMD = await SLConnection.GetAsync<List<Metadata_Invoices.Invoice>>("sml.svc/CVAMAGENTOINVOICE");
                Util.GravarLog(sCaminho, "[PROCESSO] - Quantidade de notas fiscais: " + invoiceMD.Count);
                if (invoiceMD.Count > 0)
                {
                    foreach (var item in invoiceMD)
                    {
                        var invoice = new Invoices();

                        try
                        {
                            //Util.GravarLog(sCaminho, "[PROCESSO] - U_CVA_Vcto: " + item.U_CVA_Vcto.Year.ToString());

                            if (item.U_CVA_Vcto.Year.ToString().Equals("1"))
                                throw new Exception(String.Format(@"Campo 'U_CVA_Vcto' inválido."));

                            if (item.ObjType == 15)
                            {
                                if (item.U_Adiant.Equals("S") && (item.DocEntryAD == 0 || item.DocEntryCR == 0))
                                    throw new Exception(String.Format(@"Para gerar a NF automática, o Adiantamento e o CR precisam ser criados. Entrega nº {0}", item.DocNum));

                                deliveryNote = await SLConnection.GetAsync<DeliveryNotes.DeliveryNote>($"DeliveryNotes(" + item.DocEntry + ")");
                                invoice.CardCode = item.CardCode;
                                invoice.DocType = "dDocument_Items";
                                invoice.DocCurrency = "R$";
                                invoice.DocRate = 1;
                                invoice.BPL_IDAssignedToInvoice = item.BPLId;
                                invoice.DocDueDate = item.U_CVA_Vcto;
                                invoice.U_CVA_SourceChannel = item.U_CVA_SourceChannel;
                                invoice.U_nfe_lib_env = "Y";

                                if (item.U_CVA_SourceChannel.Equals("B"))
                                    invoice.U_nfe_indPres = 1;

                                invoice.DocumentLines = new List<Invoices.DocumentLine>();
                                foreach (var itemDelivery in deliveryNote.DocumentLines)
                                {
                                    invoice.DocumentLines.Add(new Invoices.DocumentLine
                                    {
                                        BaseType = item.ObjType,
                                        BaseEntry = item.DocEntry,
                                        BaseLine = itemDelivery.LineNum,
                                        ItemCode = itemDelivery.ItemCode,
                                        Quantity = itemDelivery.Quantity,
                                        UnitPrice = itemDelivery.UnitPrice
                                    });
                                }

                                //-- Se tem adiantamento, é necessário vincular na NF
                                if (item.U_Adiant.Equals("S"))
                                {
                                    invoice.DownPaymentsToDraws = new List<Invoices.DownPaymentsToDraw>();
                                    invoice.DownPaymentsToDraws.Add(new Invoices.DownPaymentsToDraw
                                    {
                                        DocEntry = item.DocEntryAD,
                                        AmountToDraw = deliveryNote.DocTotal,
                                        DownPaymentType = "dptInvoice"
                                    });
                                }

                                var jsonInvoice = JsonConvert.SerializeObject(invoice);
                                Util.GravarLog(sCaminho, String.Format(@"[JSON] - Invoice: {0}", jsonInvoice));

                                try
                                {
                                    await SLConnection.PostAsync($"Invoices", invoice);
                                    Util.GravarLog(sCaminho, "[PROCESSO] - Nota Fiscal criada: " + invoice.CardCode + " Entrega número: " + item.DocNum + " Tipo objeto: " + item.ObjType);
                                }
                                catch (Exception ex)
                                {
                                    Util.GravarLog(sCaminho, String.Format(@"[ERRO] - DocNum: {0} | Tipo objeto: {1} | Detalhes: {2}", item.DocNum, item.ObjType, ex.Message));
                                }
                            }
                            if (item.ObjType == 17)
                            {
                                if (item.U_Adiant.Equals("S") && (item.DocEntryAD == 0 || item.DocEntryCR == 0))
                                    throw new Exception(String.Format(@"Para gerar a NF automática, o Adiantamento e o CR precisam ser criados. Pedido nº {0}", item.DocNum));

                                order = await SLConnection.GetAsync<Orders.Order>($"Orders(" + item.DocEntry + ")");
                                invoice.CardCode = item.CardCode;
                                invoice.DocType = "dDocument_Items";
                                invoice.DocCurrency = "R$";
                                invoice.DocRate = 1;
                                invoice.BPL_IDAssignedToInvoice = item.BPLId;
                                invoice.DocDueDate = item.U_CVA_Vcto;
                                invoice.U_CVA_SourceChannel = item.U_CVA_SourceChannel;
                                invoice.U_nfe_lib_env = "Y";

                                if (item.U_CVA_SourceChannel.Equals("B"))
                                    invoice.U_nfe_indPres = 1;

                                invoice.DocumentLines = new List<Invoices.DocumentLine>();
                                foreach (var itemOrder in order.DocumentLines)
                                {
                                    invoice.DocumentLines.Add(new Invoices.DocumentLine
                                    {
                                        BaseType = item.ObjType,
                                        BaseEntry = item.DocEntry,
                                        BaseLine = itemOrder.LineNum,
                                        ItemCode = itemOrder.ItemCode,
                                        Quantity = itemOrder.Quantity,
                                        UnitPrice = itemOrder.UnitPrice
                                    });
                                }

                                //-- Se tem adiantamento, é necessário vincular na NF
                                if (item.U_Adiant.Equals("S"))
                                {
                                    invoice.DownPaymentsToDraws = new List<Invoices.DownPaymentsToDraw>();
                                    invoice.DownPaymentsToDraws.Add(new Invoices.DownPaymentsToDraw
                                    {
                                        DocEntry = item.DocEntryAD,
                                        AmountToDraw = order.DocTotal,
                                        DownPaymentType = "dptInvoice"
                                    });
                                }

                                var jsonInvoice = JsonConvert.SerializeObject(invoice);
                                Util.GravarLog(sCaminho, String.Format(@"[JSON] - Invoice: {0}", jsonInvoice));

                                try
                                {
                                    await SLConnection.PostAsync($"Invoices", invoice);
                                    Util.GravarLog(sCaminho, "[PROCESSO] - Nota Fiscal criada: " + invoice.CardCode + " Pedido número: " + item.DocNum + " Tipo objeto: " + item.ObjType);
                                }
                                catch (Exception ex)
                                {
                                    Util.GravarLog(sCaminho, String.Format(@"[ERRO] - DocNum: {0} | Tipo objeto: {1} | Detalhes: {2}", item.DocNum, item.ObjType, ex.Message));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Util.GravarLog(sCaminho, String.Format(@"[ERRO] - Detalhes: {0}", ex.Message));
                        }
                    }
                }
            }
            catch (FlurlHttpException ex)
            {
                var responseString = await ex.Call.Response.GetStringAsync();
                Util.GravarLog(sCaminho, "[PROCESSO] - (Erro): " + responseString);
            }
            catch (Exception ex)
            {
                Util.GravarLog(sCaminho, "[PROCESSO] - (Erro): " + ex.Message);
            }
        }
    }
}
