using CopyDocuments.Model;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CopyDocuments.DAL
{
    public class MarketingDocumentDAO
    {
        private B1Connection ConnectionFrom;
        private B1Connection ConnectionTo;
        List<UtilizacaoModel> UtilizacaoFromList = new List<UtilizacaoModel>();
        List<UtilizacaoModel> UtilizacaoToList = new List<UtilizacaoModel>();
        List<CondicaoPagamentoModel> CondicaoFromList = new List<CondicaoPagamentoModel>();
        List<CondicaoPagamentoModel> CondicaoToList = new List<CondicaoPagamentoModel>();

        public MarketingDocumentDAO(B1Connection connectionFrom, B1Connection connectionTo)
        {
            this.ConnectionFrom = connectionFrom;
            this.ConnectionTo = connectionTo;
            this.GetFromTo();
        }

        #region GetFromTo
        private void GetFromTo()
        {
            Recordset rstFrom = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset rstTo = (Recordset)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            Console.WriteLine("Buscando utilizações");
            rstFrom.DoQuery("SELECT ID, Usage FROM OUSG");
            while (!rstFrom.EoF)
            {
                UtilizacaoModel model = new UtilizacaoModel();
                model.ID = Convert.ToInt32(rstFrom.Fields.Item(0).Value);
                model.Desc = rstFrom.Fields.Item(1).Value.ToString();
                UtilizacaoFromList.Add(model);

                rstFrom.MoveNext();
            }


            rstTo.DoQuery("SELECT ID, Usage FROM OUSG");
            while (!rstTo.EoF)
            {
                UtilizacaoModel model = new UtilizacaoModel();
                model.ID = Convert.ToInt32(rstTo.Fields.Item(0).Value);
                model.Desc = rstTo.Fields.Item(1).Value.ToString();
                UtilizacaoToList.Add(model);

                rstTo.MoveNext();
            }

            Console.WriteLine("Buscando condições de pagamento");
            rstFrom.DoQuery("SELECT GroupNum, PymntGroup FROM OCTG");
            while (!rstFrom.EoF)
            {
                CondicaoPagamentoModel model = new CondicaoPagamentoModel();
                model.ID = Convert.ToInt32(rstFrom.Fields.Item(0).Value);
                model.Desc = rstFrom.Fields.Item(1).Value.ToString();
                CondicaoFromList.Add(model);

                rstFrom.MoveNext();
            }

            rstTo.DoQuery("SELECT GroupNum, PymntGroup FROM OCTG");
            while (!rstTo.EoF)
            {
                CondicaoPagamentoModel model = new CondicaoPagamentoModel();
                model.ID = Convert.ToInt32(rstTo.Fields.Item(0).Value);
                model.Desc = rstTo.Fields.Item(1).Value.ToString();
                CondicaoToList.Add(model);

                rstTo.MoveNext();
            }
        }
        #endregion

        public void DoCopy(BoObjectTypes objectType, string tableName, DocTypeEnum docType)
        {
            Recordset rstDocFrom = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset rstUpdate = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            Console.WriteLine();
            Console.WriteLine("Buscando dados da tabela " + tableName);
            rstDocFrom.DoQuery($"SELECT DocEntry FROM {tableName} WHERE ISNULL(U_CVA_Imported, 0) = 0");

            Console.WriteLine("Registros encontrados: " + rstDocFrom.RecordCount);

            while (!rstDocFrom.EoF)
            {
                Documents docFrom = (Documents)ConnectionFrom.oCompany.GetBusinessObject(objectType);
                Documents docTo = (Documents)ConnectionTo.oCompany.GetBusinessObject(objectType);
                try
                {
                    string msg = String.Empty;

                    docFrom.GetByKey((int)rstDocFrom.Fields.Item(0).Value);

                    //string xml = docFrom.GetAsXML();
                    //var xmlFile = $"{Path.GetTempPath()}\\Documento.xml";
                    //File.WriteAllText(xmlFile, xml);

                    docTo.BPL_IDAssignedToInvoice = docFrom.BPL_IDAssignedToInvoice;
                    docTo.CardCode = docFrom.CardCode;
                    if (docFrom.Cancelled == BoYesNoEnum.tNO)
                    {
                        docTo.DocDate = docFrom.DocDate;
                        docTo.DocDueDate = docFrom.DocDueDate;
                        docTo.TaxDate = docFrom.TaxDate;
                    }
                    else
                    {
                        docTo.DocDate = DateTime.Today;
                        docTo.DocDueDate = DateTime.Today;
                        docTo.TaxDate = DateTime.Today;
                    }
                        
                    docTo.PaymentMethod = docFrom.PaymentMethod;
                    docTo.TaxExtension.Incoterms = docFrom.TaxExtension.Incoterms;

                    docTo.Series = docFrom.Series;
                    docTo.SeriesString = docFrom.SeriesString;
                    //docTo.TransportationCode = docFrom.TransportationCode;
                    //docTo.Comments = docFrom.Comments;
                    //docTo.OpeningRemarks = docFrom.OpeningRemarks;
                    //docTo.ClosingRemarks = docFrom.ClosingRemarks;
                    docTo.DocTotal = docFrom.DocTotal;

                    CondicaoPagamentoModel condicaoFrom = CondicaoFromList.FirstOrDefault(c => c.ID == docFrom.GroupNumber);
                    if (condicaoFrom != null)
                    {
                        CondicaoPagamentoModel condicaoTo = CondicaoToList.FirstOrDefault(c => c.Desc.ToLower().Trim() == condicaoFrom.Desc.ToLower().Trim());
                        if (condicaoTo != null)
                        {
                            docTo.GroupNumber = condicaoTo.ID;
                        }
                        else
                        {
                            Console.WriteLine($"{tableName}: {docFrom.DocEntry} - Condição de pagamento {condicaoFrom.ID} - {condicaoFrom.Desc} não encontrada na base destino");
                            continue;
                        }
                    }

                    for (int i = 0; i < docFrom.Lines.Count; i++)
                    {
                        docFrom.Lines.SetCurrentLine(i);
                        if (!String.IsNullOrEmpty(docTo.Lines.ItemCode))
                        {
                            docTo.Lines.Add();
                        }
                        docTo.Lines.ItemCode = docFrom.Lines.ItemCode;
                        docTo.Lines.Quantity = docFrom.Lines.Quantity;
                        docTo.Lines.UnitPrice = docFrom.Lines.UnitPrice;
                        docTo.Lines.Price = docFrom.Lines.Price;
                        //docTo.Lines.UserFields.Fields.Item("U_CVA_DocEntryFrom").Value = docFrom.DocEntry;

                        //docTo.Lines.WarehouseCode = docFrom.Lines.WarehouseCode;

                        UtilizacaoModel utilizacaoFrom = UtilizacaoFromList.FirstOrDefault(u => u.ID.ToString() == docFrom.Lines.Usage);
                        if (utilizacaoFrom != null)
                        {
                            UtilizacaoModel utilizacaoTo = UtilizacaoToList.FirstOrDefault(u => u.Desc.ToLower().Trim() == utilizacaoFrom.Desc.ToLower().Trim());
                            if (utilizacaoTo != null)
                            {
                                docTo.Lines.Usage = utilizacaoTo.ID.ToString();
                            }
                            else
                            {
                                Console.WriteLine($"{tableName}: {docFrom.DocEntry} - Utilização {utilizacaoFrom.ID} - {utilizacaoFrom.Desc} não encontrada na base destino");
                                continue;
                            }
                        }
                        docTo.Lines.TaxCode = docFrom.Lines.TaxCode;

                        if (docFrom.Lines.BatchNumbers.Quantity > 0)
                        {
                            for (int j = 0; j < docFrom.Lines.BatchNumbers.Count; j++)
                            {
                                if (!String.IsNullOrEmpty(docTo.Lines.BatchNumbers.BatchNumber))
                                {
                                    docTo.Lines.BatchNumbers.Add();
                                }

                                docFrom.Lines.BatchNumbers.SetCurrentLine(j);
                                docTo.Lines.BatchNumbers.BatchNumber = docFrom.Lines.BatchNumbers.BatchNumber;
                                docTo.Lines.BatchNumbers.Quantity = docFrom.Lines.BatchNumbers.Quantity;
                            }
                        }

                        if (docFrom.Lines.SerialNumbers.Quantity > 0)
                        {
                            for (int j = 0; j < docFrom.Lines.SerialNumbers.Count; j++)
                            {
                                if (!String.IsNullOrEmpty(docTo.Lines.SerialNumbers.InternalSerialNumber))
                                {
                                    docTo.Lines.SerialNumbers.Add();
                                }

                                docFrom.Lines.SerialNumbers.SetCurrentLine(j);
                                docTo.Lines.SerialNumbers.InternalSerialNumber = docFrom.Lines.SerialNumbers.InternalSerialNumber;
                                //docTo.Lines.SerialNumbers.ManufacturerSerialNumber = docFrom.Lines.SerialNumbers.ManufacturerSerialNumber;
                                //docTo.Lines.SerialNumbers.SystemSerialNumber = docFrom.Lines.SerialNumbers.SystemSerialNumber;
                                docTo.Lines.SerialNumbers.Quantity = docFrom.Lines.SerialNumbers.Quantity;
                            }
                        }
                    }

                    ConnectionTo.oCompany.StartTransaction();
                    if (docTo.Add() != 0)
                    {
                        Console.WriteLine($"{tableName}: {docFrom.DocEntry} - {ConnectionTo.oCompany.GetLastErrorDescription()}");
                    }
                    else
                    {
                        int docEntryTo = Convert.ToInt32(ConnectionTo.oCompany.GetNewObjectKey());

                        msg = this.GeneratePayment(docFrom.DocEntry, docEntryTo, objectType, docType);
                        if (String.IsNullOrEmpty(msg))
                        {
                            docTo.GetByKey(docEntryTo);
                            //if (docFrom.DocumentStatus == BoStatus.bost_Close && docTo.DocumentStatus != BoStatus.bost_Close)
                            //{
                            //    if (docTo.Close() != 0)
                            //    {
                            //        Console.WriteLine($"{tableName}: {docFrom.DocEntry} - Erro ao fechar documento: {ConnectionTo.oCompany.GetLastErrorDescription()}");
                            //    }
                            //}
                            //if (docFrom.Cancelled == BoYesNoEnum.tYES)
                            //{
                            //    docTo.GetByKey(docEntryTo);
                            //    if (docTo.Cancel() != 0)
                            //    {
                            //        Console.WriteLine($"{tableName}: {docFrom.DocEntry} - Erro ao cancelar documento: {ConnectionTo.oCompany.GetLastErrorDescription()}");
                            //    }
                            //}

                            if (ConnectionTo.oCompany.InTransaction)
                            {
                                ConnectionTo.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            }
                            rstUpdate.DoQuery($"UPDATE {tableName} SET U_CVA_Imported = 1 WHERE DocEntry = " + docFrom.DocEntry);
                        }
                        else
                        {
                            if (ConnectionTo.oCompany.InTransaction)
                            {
                                ConnectionTo.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            Console.WriteLine($"{tableName}: {docFrom.DocEntry} - {msg}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{tableName}: {docFrom.DocEntry} - Erro geral: {ex.Message}");
                }
                finally
                {
                    Marshal.ReleaseComObject(docTo);
                    docTo = null;

                    Marshal.ReleaseComObject(docFrom);
                    docFrom = null;

                    rstDocFrom.MoveNext();
                }
            }
        }

        private string GeneratePayment(int docEntryFrom, int docEntryTo, BoObjectTypes objectType, DocTypeEnum docType)
        {
            Recordset rst = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            string table = String.Empty;
            if (docType == DocTypeEnum.In)
            {
                table = "VPM2";
            }
            else
            {
                table = "RCT2";
            }

            rst.DoQuery($"SELECT DocNum FROM {table} WHERE DocEntry = {docEntryFrom} AND InvType = {(int)objectType}");
            if (rst.RecordCount > 0)
            {
                Payments paymentFrom;
                Payments paymentTo;
                if (docType == DocTypeEnum.Out)
                {
                    paymentFrom = (Payments)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.oIncomingPayments);
                    paymentTo = (Payments)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.oIncomingPayments);
                }
                else
                {
                    paymentFrom = (Payments)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.oVendorPayments);
                    paymentTo = (Payments)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.oVendorPayments);
                }

                paymentFrom.GetByKey((int)rst.Fields.Item(0).Value);

                paymentTo.CardCode = paymentFrom.CardCode;
                paymentTo.DocType = paymentFrom.DocType;
                paymentTo.DocObjectCode = paymentFrom.DocObjectCode;
                paymentTo.BPLID = paymentFrom.BPLID;
                paymentTo.DocDate = paymentFrom.DocDate;
                paymentTo.TaxDate = paymentFrom.TaxDate;
                paymentTo.DueDate = paymentFrom.DueDate;

                paymentTo.BoeAccount = paymentFrom.BoeAccount;

                paymentTo.CashSum = paymentFrom.CashSum;
                paymentTo.BillOfExchangeAmount = paymentFrom.BillOfExchangeAmount;
                paymentTo.BillOfExchange.BillOfExchangeDueDate = paymentFrom.BillOfExchange.BillOfExchangeDueDate;
                paymentTo.BillOfExchange.PaymentMethodCode = paymentFrom.BillOfExchange.PaymentMethodCode;

                paymentTo.TransferSum = paymentFrom.TransferSum;
                paymentTo.TransferDate = paymentFrom.TransferDate;
                paymentTo.TransferAccount = paymentFrom.TransferAccount;

                for (int i = 0; i < paymentFrom.Invoices.Count; i++)
                {
                    paymentFrom.Invoices.SetCurrentLine(i);
                    if (paymentFrom.Invoices.DocEntry == docEntryFrom)
                    {
                        paymentTo.Invoices.DocEntry = docEntryTo;
                        paymentTo.Invoices.InstallmentId = paymentFrom.Invoices.InstallmentId;
                        paymentTo.Invoices.InvoiceType = paymentFrom.Invoices.InvoiceType;
                        paymentTo.Invoices.SumApplied = paymentFrom.Invoices.SumApplied;
                    }
                }

                if (paymentTo.Add() != 0)
                {
                    return "Erro ao gerar pagamento: " + ConnectionTo.oCompany.GetLastErrorDescription();
                }

                Marshal.ReleaseComObject(paymentTo);
                paymentTo = null;

                Marshal.ReleaseComObject(paymentFrom);
                paymentFrom = null;
            }
            return String.Empty;
        }
    }
}
