using DAL.Connection;
using DAL.Data;
using DAL.DataInterface;
using MODEL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NotaFiscalEntradaBlo
    {
        public Arquivo Generate(Arquivo arquivo, SAPbouiCOM.DataTable dt_Doc, SAPbouiCOM.Form form, Application pApplication)
        {
            try
            {
                IEnumerable<IGrouping<string, ArquivoLinha>> groupedByBase = arquivo.LINHAS.GroupBy(l => l.BASE);

                foreach (var itemByBase in groupedByBase)
                {
                    BaseModel baseModel = DatabaseConfigDao.GetBaseByName(itemByBase.Key);
                    ConnectionDao.ConnectExternal(baseModel);

                    foreach (var linha in itemByBase)
                    {
                        Documents invoice = ConnectionDao.ExternalCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices) as Documents;
                        Documents po = ConnectionDao.ExternalCompany.GetBusinessObject(BoObjectTypes.oPurchaseOrders) as Documents;

                        try
                        {
                            po.GetByKey(linha.NUMEROPEDIDOSAP.Value);

                            invoice.BPL_IDAssignedToInvoice = po.BPL_IDAssignedToInvoice;
                            invoice.CardCode = po.CardCode;
                            invoice.DocDate = DateTime.Today;
                            invoice.DocDueDate = po.DocDueDate;
                            if (po.SequenceCode != 0)
                            {
                                invoice.SequenceCode = po.SequenceCode;
                            }
                            if (po.SequenceSerial != 0)
                            {
                                invoice.SequenceSerial = po.SequenceSerial;
                            }
                            invoice.SeriesString = po.SeriesString;

                            if (po.PaymentGroupCode != 0)
                            {
                                invoice.PaymentGroupCode = po.PaymentGroupCode;
                            }
                            invoice.PaymentMethod = po.PaymentMethod;

                            invoice.Comments = po.Comments;

                            if (po.AttachmentEntry != 0)
                            {
                                invoice.AttachmentEntry = po.AttachmentEntry;
                            }

                            invoice.Lines.BaseEntry = po.DocEntry;
                            invoice.Lines.BaseLine = po.Lines.LineNum;
                            invoice.Lines.BaseType = (int)BoObjectTypes.oPurchaseOrders;

                            invoice.Lines.Quantity = po.Lines.Quantity;
                            invoice.Lines.UnitPrice = po.Lines.UnitPrice;
                            invoice.Lines.Usage = po.Lines.Usage;
                            invoice.Lines.ProjectCode = po.Lines.ProjectCode;
                            invoice.Lines.CostingCode = po.Lines.CostingCode;
                            invoice.Lines.CostingCode2 = po.Lines.CostingCode2;
                            invoice.Lines.CostingCode3 = po.Lines.CostingCode3;
                            invoice.Lines.CostingCode4 = po.Lines.CostingCode4;
                            invoice.Lines.CostingCode5 = po.Lines.CostingCode5;

                            for (int i = 0; i < po.Lines.WithholdingTaxLines.Count; i++)
                            {
                                po.Lines.WithholdingTaxLines.SetCurrentLine(i);
                                if (!String.IsNullOrEmpty(po.Lines.WithholdingTaxLines.WTCode))
                                {
                                    if (!String.IsNullOrEmpty(invoice.Lines.WithholdingTaxLines.WTCode))
                                    {
                                        invoice.Lines.WithholdingTaxLines.Add();
                                    }
                                    invoice.Lines.WithholdingTaxLines.WTCode = po.Lines.WithholdingTaxLines.WTCode;
                                }
                            }

                            if (invoice.Add() != 0)
                            {
                                linha.STATUSLINHA = 4;
                                linha.MENSAGEMSTATUS = $"{ConnectionDao.ExternalCompany.GetLastErrorDescription()}";
                                pApplication.SetStatusBarMessage(string.Format("Erro ao Gerar NF! Empresa: {0} - Base {1} - Server {2} - Erro {3}", ConnectionDao.ExternalCompany.CompanyName, ConnectionDao.ExternalCompany.CompanyDB, ConnectionDao.ExternalCompany.Server, linha.MENSAGEMSTATUS), BoMessageTime.bmt_Medium, true);
                            }
                            else
                            {
                                po.GetByKey(linha.NUMEROPEDIDOSAP.Value);
                                if (po.DocumentStatus != BoStatus.bost_Close)
                                {
                                    if (po.Close() != 0)
                                    {
                                        linha.STATUSLINHA = 4;
                                        linha.MENSAGEMSTATUS = $"Erro ao fechar pedido de compra: {ConnectionDao.ExternalCompany.GetLastErrorDescription()}";
                                    }
                                    else
                                    {
                                        linha.STATUSLINHA = 3;
                                        linha.MENSAGEMSTATUS = $"NF gerada com sucesso!";
                                    }
                                }
                                else
                                {
                                    linha.STATUSLINHA = 3;
                                    linha.MENSAGEMSTATUS = $"NF gerada com sucesso!";
                                }

                                if (linha.STATUSLINHA == 3)
                                {
                                    pApplication.SetStatusBarMessage(string.Format("NF gerada com sucesso! Empresa: {0} - Base {1} - Server {2}", ConnectionDao.ExternalCompany.CompanyName, ConnectionDao.ExternalCompany.CompanyDB, ConnectionDao.ExternalCompany.Server), BoMessageTime.bmt_Medium, false);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            linha.STATUSLINHA = 4;
                            linha.MENSAGEMSTATUS = $"Erro geral: {ex.Message}";
                        }
                        finally
                        {
                            form.Freeze(true);

                            dt_Doc.SetValue("Status", linha.LINHAGRID - 1, linha.STATUSDESC);
                            dt_Doc.SetValue("Log", linha.LINHAGRID - 1, linha.MENSAGEMSTATUS);

                            form.Freeze(false);

                            Marshal.ReleaseComObject(invoice);
                            invoice = null;

                            Marshal.ReleaseComObject(po);
                            po = null;
                        }
                    }
                }

                ArquivoDao dao = new ArquivoDao();
                dao.Update(arquivo);
            }
            catch (Exception ex)
            {
                arquivo.MENSAGEMSTATUS = ex.Message;
            }
            return arquivo;
        }

        public Arquivo Cancel(Arquivo arquivo, SAPbouiCOM.DataTable dt_Doc, SAPbouiCOM.Form form)
        {
            try
            {
                IEnumerable<IGrouping<string, ArquivoLinha>> groupedByBase = arquivo.LINHAS.GroupBy(l => l.BASE);

                foreach (var itemByBase in groupedByBase)
                {
                    BaseModel baseModel = DatabaseConfigDao.GetBaseByName(itemByBase.Key);
                    ConnectionDao.ConnectExternal(baseModel);

                    foreach (var linha in itemByBase)
                    {
                        Documents invoice = ConnectionDao.ExternalCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices) as Documents;
                        Documents po = ConnectionDao.ExternalCompany.GetBusinessObject(BoObjectTypes.oPurchaseOrders) as Documents;
                        Documents cancel = null;

                        try
                        {
                            po.GetByKey(linha.NUMEROPEDIDOSAP.Value);
                            invoice.GetByKey(Convert.ToInt32(po.Lines.POTargetEntry));
                            cancel = invoice.CreateCancellationDocument();
                            if (cancel.Add() != 0)
                            {
                                linha.STATUSLINHA = 8;
                                linha.MENSAGEMSTATUS = $"{ConnectionDao.ExternalCompany.GetLastErrorDescription()}";
                            }
                            else
                            {
                                linha.STATUSLINHA = 7;
                                linha.MENSAGEMSTATUS = $"NF cancelada";
                            }
                        }
                        catch (Exception ex)
                        {
                            linha.STATUSLINHA = 8;
                            linha.MENSAGEMSTATUS = $"Erro geral: {ex.Message}";
                        }
                        finally
                        {
                            form.Freeze(true);

                            dt_Doc.SetValue("Status", linha.LINHAGRID - 1, linha.STATUSDESC);
                            dt_Doc.SetValue("Log", linha.LINHAGRID - 1, linha.MENSAGEMSTATUS);

                            form.Freeze(false);

                            Marshal.ReleaseComObject(invoice);
                            invoice = null;

                            Marshal.ReleaseComObject(po);
                            po = null;

                            if (cancel != null)
                            {
                                Marshal.ReleaseComObject(cancel);
                                cancel = null;
                            }
                        }
                    }
                }

                ArquivoDao dao = new ArquivoDao();
                dao.Update(arquivo);
            }
            catch (Exception ex)
            {
                arquivo.MENSAGEMSTATUS = ex.Message;
            }
            return arquivo;
        }
    }
}
