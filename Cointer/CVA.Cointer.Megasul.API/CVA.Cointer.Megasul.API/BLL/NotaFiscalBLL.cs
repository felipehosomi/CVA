using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using CVA.Cointer.Megasul.API.Models.ServiceLayer;
using SBO.Hub.SBOHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CVA.Cointer.Megasul.API.BLL
{
    public class NotaFiscalBLL
    {
        public string Create(NotaFiscalModel model)
        {
            ServiceLayer serviceLayer = new ServiceLayer();
            try
            {
                HanaDAO hanaDAO = new HanaDAO();
                ContasReceberBLL contasReceberBLL = new ContasReceberBLL();
                DocumentoMarketingModel doc = new DocumentoMarketingModel();

                object filial = hanaDAO.ExecuteScalar(String.Format(Hana.Filial_Get, model.cnpj_filial));
                if (filial == null)
                {
                    return $"Filial {model.cnpj_filial} não encontrada";
                }
                doc.BPL_IDAssignedToInvoice = Convert.ToInt32(filial);
                doc.CardCode = model.cliente;

                DateTime date;
                if (!DateTime.TryParseExact(model.identificador.data_hora, "dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                {
                    return $"Campo data_hora deve estar no formato dd/MM/yyyy HH:mm:ss";
                }

                if (model.identificador.ccf.ToString().Length > 3)
                {
                    model.identificador.ccf = Convert.ToInt32(model.identificador.ccf.ToString().Substring(0, 3));
                }

                object docEntry = hanaDAO.ExecuteScalar(String.Format(Hana.NotaFiscal_Exists, model.identificador.coo, model.identificador.ccf, filial.ToString()));
                if (docEntry != null)
                {
                    object crExists = hanaDAO.ExecuteScalar(String.Format(Hana.ContasReceber_Exists, docEntry));
                    if (crExists == null)
                    {
                        contasReceberBLL.AddIncomingPayment(model, Convert.ToInt32(docEntry), date, doc.BPL_IDAssignedToInvoice);
                    }
                    else
                    {
                        throw new Exception("Cupom fiscal já incluído");
                    }
                }
                else
                {
                    doc.DocDate = date;
                    doc.SequenceCode = -1;
                    doc.SequenceSerial = model.identificador.coo;
                    doc.SequenceModel = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ModeloNF"]);
                    doc.SeriesString = model.identificador.ccf.ToString();
                    doc.TaxExtension = new TaxExtension();
                    doc.TaxExtension.MainUsage = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UtilizacaoID"]);
                    if (!String.IsNullOrEmpty(model.vendedor))
                    {
                        object slpCode = hanaDAO.ExecuteScalar(String.Format(Hana.Vendedor_GetByName, model.vendedor.ToUpper()));
                        if (slpCode != null)
                        {
                            doc.SalesPersonCode = Convert.ToInt32(slpCode);
                        }
                        else
                        {
                            throw new Exception($"Vendedor {model.vendedor} não encontrado");
                        }
                    }

                    //if (model.pagamentos != null && model.pagamentos.Length > 0)
                    //{
                    //    doc.PaymentMethod = model.pagamentos[0].codigo_sap;
                    //}

                    if (model.valor_desconto != 0)
                    {
                        doc.DiscountPercent = model.valor_desconto / (model.valor_total + model.valor_desconto) * 100;
                        //doc.DocTotal = model.valor_total;
                    }
                    doc.U_CVA_CF_EF = model.identificador.ef;
                    doc.U_CVA_CF_SI = model.identificador.si;
                    doc.U_CVA_CF_TR = model.identificador.tr;

                    doc.DocumentLines = new List<Documentline>();
                    int i = 0;

                    string erroLote = String.Empty;
                    foreach (var produto in model.produtos)
                    {
                        if (produto.lotes != null)
                        {
                            if (!String.IsNullOrEmpty(produto.lotes.identificador))
                            {
                                object filialLote = hanaDAO.ExecuteScalar(String.Format(Hana.Lote_GetFilial, produto.codigo_sap, produto.lotes.identificador, filial));

                                if (filialLote == null)
                                {
                                    erroLote += $"Item {produto.codigo_sap} - Lote {produto.lotes.identificador} não se encontra na filial informada (BPLId {filial}){Environment.NewLine}";
                                }
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(erroLote))
                    {
                        throw new Exception(erroLote);
                    }

                    foreach (var produto in model.produtos)
                    {
                        if (!produto.cancelado)
                        {
                            Documentline line = new Documentline();
                            line.ItemCode = produto.codigo_sap;
                            line.Usage = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UtilizacaoID"]);
                            line.Quantity = produto.quantidade;
                            line.UnitPrice = produto.preco;
                            //line.LineTotal = (produto.quantidade * produto.preco) - produto.desconto;
                            line.LineTotal = produto.quantidade * produto.preco;
                            //line.DiscountPercent = (produto.desconto / (produto.quantidade * produto.preco)) * 100;

                            if (model.pedido_cancelado == 1)
                            {
                                line.WarehouseCode = System.Configuration.ConfigurationManager.AppSettings["DepositoEnvioDireto"];
                            }

                            if (produto.lotes != null)
                            {
                                if (!String.IsNullOrEmpty(produto.lotes.identificador))
                                {
                                    line.BatchNumbers = new List<Batchnumber>();
                                    Batchnumber batchnumber = new Batchnumber();
                                    batchnumber.BaseLineNumber = i;
                                    batchnumber.BatchNumber = produto.lotes.identificador;
                                    batchnumber.Quantity = produto.quantidade;
                                    line.BatchNumbers.Add(batchnumber);
                                }
                                else
                                {
                                    line.SerialNumbers = new List<Serialnumber>();
                                    Serialnumber serialnumber = new Serialnumber();

                                    object systemNumber = hanaDAO.ExecuteScalar(String.Format(Hana.Serie_GetSystemNumber, produto.codigo_sap, produto.lotes.nro_serie));
                                    if (systemNumber != null)
                                    {
                                        serialnumber.BaseLineNumber = i;
                                        serialnumber.SystemSerialNumber = Convert.ToInt32(systemNumber);
                                        serialnumber.Quantity = 1;
                                        line.SerialNumbers.Add(serialnumber);
                                    }
                                    else
                                    {
                                        throw new Exception($"Número de série {produto.lotes.nro_serie} não encontrado para o item {produto.codigo_sap}");
                                    }
                                }
                            }
                            i++;
                            doc.DocumentLines.Add(line);
                        }
                    }

                    if (model.valor_acrescimo > 0)
                    {
                        doc.DocumentAdditionalExpenses = new List<DocumentAdditionalExpense>();
                        doc.DocumentAdditionalExpenses.Add(new DocumentAdditionalExpense() { ExpenseCode = 1, LineTotal = model.valor_acrescimo });
                    }

                    if (model.pagamentos != null)
                    {
                        foreach (var pagamento in model.pagamentos.Where(m => m.codigo_sap == "VOU"))
                        {
                            if (pagamento.vouchers != null)
                            {
                                foreach (var voucher in pagamento.vouchers)
                                {
                                    if (!String.IsNullOrEmpty(voucher.codigo) && voucher.valor > 0)
                                    {
                                        if (doc.DownPaymentsToDraw == null)
                                        {
                                            doc.DownPaymentsToDraw = new List<DownPaymentsToDraw>();
                                        }
                                        object docEntryAdiantamento = hanaDAO.ExecuteScalar(String.Format(Hana.Adiantamento_Get, voucher.codigo));
                                        if (docEntryAdiantamento == null)
                                        {
                                            throw new Exception($"Adiantamento {voucher.codigo} não encontrado!");
                                        }
                                        doc.DownPaymentsToDraw.Add(new DownPaymentsToDraw() { DocEntry = Convert.ToInt32(docEntryAdiantamento), AmountToDraw = voucher.valor });
                                    }
                                }
                            }
                        }
                    }

                    doc = serviceLayer.PostAndGetAdded<DocumentoMarketingModel>("Invoices", "DocEntry", doc);

                    contasReceberBLL.AddIncomingPayment(model, doc.DocEntry.Value, date, doc.BPL_IDAssignedToInvoice);
                }
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(serviceLayer.LastError))
                {
                    return serviceLayer.LastError;
                }
                else
                {
                    return ex.Message;
                }
            }
            return String.Empty;
        }

        public string Cancel(NotaFiscalCancelamentoModel model)
        {
            ServiceLayer serviceLayer = new ServiceLayer();
            try
            {
                HanaDAO hanaDAO = new HanaDAO();

                object filial = hanaDAO.ExecuteScalar(String.Format(Hana.Filial_Get, model.cnpj_filial));
                if (filial == null)
                {
                    return $"Filial {model.cnpj_filial} não encontrada";
                }

                if (model.identificador.ccf.ToString().Length > 3)
                {
                    model.identificador.ccf = Convert.ToInt32(model.identificador.ccf.ToString().Substring(0, 3));
                }

                object docEntry = hanaDAO.ExecuteScalar(String.Format(Hana.NotaFiscal_Exists, model.identificador.coo, model.identificador.ccf, filial.ToString()));
                string error = String.Empty;

                if (docEntry != null)
                {
                    DocumentoMarketingModel invoice = serviceLayer.GetByID<DocumentoMarketingModel>("Invoices", Convert.ToInt32(docEntry));
                    object crExists = hanaDAO.ExecuteScalar(String.Format(Hana.ContasReceber_Exists, docEntry));
                    if (crExists != null)
                    {
                        error = serviceLayer.Cancel("IncomingPayments", Convert.ToInt32(crExists));
                        if (!String.IsNullOrEmpty(error))
                        {
                            throw new Exception(error);
                        }
                    }
                    error = serviceLayer.Cancel("Invoices", Convert.ToInt32(docEntry));

                    //if (invoice.DocDate < DateTime.Today.AddDays(-7))
                    //{
                    //    object crExists = hanaDAO.ExecuteScalar(String.Format(Hana.ContasReceber_Exists, docEntry));
                    //    if (crExists != null)
                    //    {
                    //        error = serviceLayer.Cancel("IncomingPayments", Convert.ToInt32(crExists));
                    //        if (!String.IsNullOrEmpty(error))
                    //        {
                    //            throw new Exception(error);
                    //        }
                    //    }
                    //    error = serviceLayer.Cancel("Invoices", Convert.ToInt32(docEntry));
                    //}
                    //else
                    //{
                    //    DevolucaoBLL devolucaoBLL = new DevolucaoBLL();
                    //    error = devolucaoBLL.Create(model, invoice);
                    //}
                }
                else
                {
                    throw new Exception("Cupom fiscal não encontrado");
                }
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(serviceLayer.LastError))
                {
                    return serviceLayer.LastError;
                }
                else
                {
                    return ex.Message;
                }
            }
            return String.Empty;
        }

        public string CupomNFe(CupomFiscalNFeModel model)
        {
            ServiceLayer serviceLayer = new ServiceLayer();
            try
            {
                HanaDAO hanaDAO = new HanaDAO();

                object filial = hanaDAO.ExecuteScalar(String.Format(Hana.Filial_Get, model.cnpj_filial));
                if (filial == null)
                {
                    return $"Filial {model.cnpj_filial} não encontrada";
                }

                object cupomNFe = hanaDAO.ExecuteScalar(String.Format(Hana.NotaFiscal_GetByCupomNFe, model.identificador.ef, model.identificador.si, model.identificador.tr, model.nf.numero, model.nf.serie, filial.ToString()));
                if (cupomNFe != null)
                {
                    throw new Exception("NF-e já vinculada");
                }

                object docEntry = hanaDAO.ExecuteScalar(String.Format(Hana.NotaFiscal_GetByCupom, model.identificador.ef, model.identificador.si, model.identificador.tr, filial.ToString()));
                string error = String.Empty;

                if (docEntry != null)
                {
                    DocumentoMarketingModel invoice = serviceLayer.GetByID<DocumentoMarketingModel>("Invoices", Convert.ToInt32(docEntry));
                    invoice.DocumentStatus = null;
                    invoice.DocEntry = null;
                    invoice.DocNum = null;

                    invoice.U_ChaveAcesso = model.nf.chave_acesso;

                    if (!String.IsNullOrEmpty(model.nf.cliente))
                    {
                        invoice.CardCode = model.nf.cliente;
                    }

                    invoice.SequenceModel = 39;
                    invoice.SequenceSerial = model.nf.numero;
                    invoice.SeriesString = model.nf.serie.ToString();

                    invoice.TaxExtension.MainUsage = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UtilizacaoNFeID"]);

                    invoice.DownPaymentsToDraw = null;

                    if (!String.IsNullOrEmpty(model.nf.vendedor))
                    {
                        object slpCode = hanaDAO.ExecuteScalar(String.Format(Hana.Vendedor_GetByName, model.nf.vendedor.ToUpper()));
                        if (slpCode != null)
                        {
                            invoice.SalesPersonCode = Convert.ToInt32(slpCode);
                        }
                        else
                        {
                            throw new Exception($"Vendedor {model.nf.vendedor} não encontrado");
                        }
                    }

                    foreach (var line in invoice.DocumentLines)
                    {
                        line.Usage = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UtilizacaoNFeID"]);
                        line.WarehouseCode = System.Configuration.ConfigurationManager.AppSettings["DepositoEnvioDireto"];
                        line.SerialNumbers = null;
                        line.BatchNumbers = null;
                    }

                    //invoice.DocumentReferences = new List<Documentreference>();
                    //invoice.DocumentReferences.Add(new Documentreference() { RefDocEntr = Convert.ToInt32(docEntry) });

                    invoice = serviceLayer.PostAndGetAdded<DocumentoMarketingModel>("Invoices", "DocEntry", invoice);

                }
                else
                {
                    throw new Exception("Cupom fiscal não encontrado ou cancelado");
                }
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(serviceLayer.LastError))
                {
                    return serviceLayer.LastError;
                }
                else
                {
                    return ex.Message;
                }
            }
            return String.Empty;
        }
    }
}