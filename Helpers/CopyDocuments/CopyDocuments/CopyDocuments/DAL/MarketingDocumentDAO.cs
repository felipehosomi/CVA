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
            Recordset rstWhs = (Recordset)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            Program.Logger.Info("Buscando dados da tabela " + tableName);
            //string sql = @"SELECT Origem.DocEntry FROM SBO_TestePRD.dbo.OPCH Origem
            //                WHERE NOT EXISTS
            //                (
            //                 SELECT TOP 1 1 FROM SBO_CIANET_Dev.dbo.OPCH Destino
            //                 WHERE Destino.DocDate = Origem.DocDate
            //                 AND Destino.DocTotal = Origem.DocTotal
            //                 AND Destino.CardCode = Origem.CardCode
            //                )
            //                AND Origem.DocDate > CAST('20170101' AS DATETIME) 
            //                AND Origem.DocDate < CAST('20170430' AS DATETIME)
            //                AND  DocType = 'I' ";
            //rstDocFrom.DoQuery(sql);
            rstDocFrom.DoQuery($"SELECT DocEntry FROM {tableName} WHERE ISNULL(U_CVA_Imported, 0) = 0 AND DocStatus <> 'C' AND DocDate >= CAST('20170101' AS DATETIME) AND DocDate <= CAST('20170531' AS DATETIME)");
            //rstDocFrom.DoQuery($"SELECT DocEntry FROM {tableName} WHERE ISNULL(U_CVA_Imported, 0) = 0 AND DocEntry = 11594");

            Program.Logger.Info("Registros encontrados: " + rstDocFrom.RecordCount);
            int updated = 0;
            int error = 0;

            while (!rstDocFrom.EoF)
            {
                Documents docFrom = (Documents)ConnectionFrom.oCompany.GetBusinessObject(objectType);
                Documents docTo;

                docTo = (Documents)ConnectionTo.oCompany.GetBusinessObject(objectType);

                //if (tableName != "ORDN" && tableName != "ORIN")
                //{
                //    docTo = (Documents)ConnectionTo.oCompany.GetBusinessObject(objectType);
                //}
                //else
                //{
                //docTo = (Documents)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                //docTo.DocObjectCode = objectType;
                //}

                string msg = String.Empty;
                try
                {
                    //Program.Logger.Info("DocEntry: " + (int)rstDocFrom.Fields.Item(0).Value);

                    docFrom.GetByKey((int)rstDocFrom.Fields.Item(0).Value);

                    //string xml = docFrom.GetAsXML();
                    //var xmlFile = $"{Path.GetTempPath()}\\Documento.xml";
                    //File.WriteAllText(xmlFile, xml);

                    docTo.BPL_IDAssignedToInvoice = docFrom.BPL_IDAssignedToInvoice;
                    docTo.DocType = docFrom.DocType;
                    docTo.SalesPersonCode = docFrom.SalesPersonCode;
                    //object[] oParam = new object[1];
                    //oParam[0] = docFrom.CardCode;

                    //typeof(Documents).InvokeMember("CardCode", System.Reflection.BindingFlags.SetProperty, null, docTo, oParam);

                    docTo.CardCode = docFrom.CardCode;
                    docTo.DocDate = docFrom.DocDate;
                    docTo.DocDueDate = docFrom.DocDueDate;
                    docTo.TaxDate = docFrom.TaxDate;
                    docTo.DocRate = docFrom.DocRate;
                    docTo.DocCurrency = docFrom.DocCurrency;
                    docTo.DocRate = docFrom.DocRate;

                    docTo.PaymentMethod = docFrom.PaymentMethod;
                    docTo.TaxExtension.Incoterms = docFrom.TaxExtension.Incoterms;

                    if (docType == DocTypeEnum.Out)
                    {
                        docTo.SequenceCode = -1;
                    }
                    else
                    {
                        docTo.SequenceCode = docFrom.SequenceCode;
                    }
                    docTo.SequenceSerial = docFrom.SequenceSerial;
                    docTo.Series = docFrom.Series;
                    docTo.SeriesString = docFrom.SeriesString;
                    //docTo.TransportationCode = docFrom.TransportationCode;
                    //docTo.Comments = docFrom.Commen'ts;
                    //docTo.OpeningRemarks = docFrom.OpeningRemarks;
                    //docTo.ClosingRemarks = docFrom.ClosingRemarks;
                    
                    docTo.DocTotal = docFrom.DocTotal;

                    if (docFrom.PaymentGroupCode != 0)
                    {
                        CondicaoPagamentoModel condicaoFrom = CondicaoFromList.FirstOrDefault(c => c.ID == docFrom.PaymentGroupCode);
                        if (condicaoFrom != null)
                        {
                            CondicaoPagamentoModel condicaoTo = CondicaoToList.FirstOrDefault(c => c.Desc.ToLower().Trim() == condicaoFrom.Desc.ToLower().Trim());
                            if (condicaoTo != null)
                            {
                                docTo.PaymentGroupCode = condicaoTo.ID;
                            }
                            else
                            {
                                Program.Logger.Info($"{tableName}: {docFrom.DocEntry} - Condição de pagamento {condicaoFrom.ID} - {condicaoFrom.Desc} não encontrada na base destino");
                                continue;
                            }
                        }
                        else
                        {
                            Program.Logger.Info("Condição origem não encontrada!");
                            continue;
                        }
                    }

                    for (int i = 0; i < docFrom.Lines.Count; i++)
                    {
                        docFrom.Lines.SetCurrentLine(i);
                        if (!String.IsNullOrEmpty(docTo.Lines.ItemCode) || !String.IsNullOrEmpty(docTo.Lines.ItemDescription))
                        {
                            docTo.Lines.Add();
                        }
                        //Program.Logger.Info("Item: " + docFrom.Lines.ItemCode);

                        if (!String.IsNullOrEmpty(docFrom.Lines.ItemCode))
                        {
                            //oParam[0] = docFrom.CardCode;
                            //typeof(Document_Lines).InvokeMember("ItemCode", System.Reflection.BindingFlags.SetProperty, null, docTo.Lines, oParam);

                            docTo.Lines.ItemCode = docFrom.Lines.ItemCode;

                            //rstWhs.DoQuery(String.Format("select OITM.DfltWH from OITM WHERE ItemCode = '{0}'", docFrom.Lines.ItemCode));
                            //if (!String.IsNullOrEmpty(rstWhs.Fields.Item(0).Value.ToString()))
                            //{
                            //    docTo.Lines.WarehouseCode = rstWhs.Fields.Item(0).Value.ToString();
                            //}
                        }
                        docTo.Lines.ItemDescription = docFrom.Lines.ItemDescription;
                        
                        docTo.Lines.Currency = docFrom.Lines.Currency;
                        if (docFrom.DocType == BoDocumentTypes.dDocument_Items)
                        {
                            docTo.Lines.Quantity = docFrom.Lines.Quantity;
                            docTo.Lines.UnitPrice = docFrom.Lines.UnitPrice;
                            docTo.Lines.Price = docFrom.Lines.Price;
                        }
                        else
                        {
                            if (tableName == "OPCH")
                            {
                                docTo.Lines.AccountCode = "5.1.01.03.001";
                            }
                        }

                        if (tableName == "OINV" && docFrom.Lines.ItemCode == "9.119.0020")
                        {
                            docTo.Lines.AccountCode = "3.1.01.03.001";
                        }

                        //Program.Logger.Info(docFrom.Lines.ItemCode);
                        //Program.Logger.Info(docTo.Lines.AccountCode);

                        docTo.Lines.LineTotal = docFrom.Lines.LineTotal;

                        //docTo.Lines.UserFields.Fields.Item("U_CVA_DocEntryFrom").Value = docFrom.DocEntry;

                        docTo.Lines.WarehouseCode = docFrom.Lines.WarehouseCode;

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
                                Program.Logger.Info($"{tableName}: {docFrom.DocEntry} - Utilização {utilizacaoFrom.ID} - {utilizacaoFrom.Desc} não encontrada na base destino");
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

                        //Program.Logger.Info($"Linha {docTo.Lines.LineNum} - Item: {docTo.Lines.ItemCode} - Qtde: {docTo.Lines.Quantity}");

                        int serieQty = 0;
                        //if (docFrom.Lines.SerialNumbers.Quantity > 0)
                        //{
                        //    Program.Logger.Info("Série: " + docFrom.Lines.SerialNumbers.ManufacturerSerialNumber);
                        //    for (int j = 0; j < docFrom.Lines.SerialNumbers.Count; j++)
                        //    {
                        //        //Program.Logger.Info("Nr série: " + docFrom.Lines.SerialNumbers.ManufacturerSerialNumber);

                        //        if (!String.IsNullOrEmpty(docTo.Lines.SerialNumbers.ManufacturerSerialNumber))
                        //        {
                        //            docTo.Lines.SerialNumbers.Add();
                        //        }

                        //        docFrom.Lines.SerialNumbers.SetCurrentLine(j);
                        //        docTo.Lines.SerialNumbers.ManufacturerSerialNumber = docFrom.Lines.SerialNumbers.ManufacturerSerialNumber;
                        //        docTo.Lines.SerialNumbers.Quantity = docFrom.Lines.SerialNumbers.Quantity;
                        //        serieQty++;
                        //    }
                        //}

                        //Program.Logger.Info($"Qtde série: " + serieQty);
                    }

                    try
                    {
                        for (int i = 0; i < docFrom.Installments.Count; i++)
                        {
                            docFrom.Installments.SetCurrentLine(i);
                            if (docFrom.Installments.Total > 0)
                            {
                                if (docTo.Installments.Total != 0)
                                {
                                    docTo.Installments.Add();
                                }

                                docTo.Installments.DueDate = docFrom.Installments.DueDate;
                                docTo.Installments.Total = docFrom.Installments.Total;
                            }
                        }
                    }
                    catch { }

                    ConnectionTo.oCompany.StartTransaction();
                    if (docTo.Add() != 0)
                    {
                        msg = ConnectionTo.oCompany.GetLastErrorDescription();
                        Program.Logger.Info($"{tableName}: {docFrom.DocEntry} - {msg}");
                    }
                    else
                    {
                        int docEntryTo = Convert.ToInt32(ConnectionTo.oCompany.GetNewObjectKey());

                        msg = this.GeneratePayment(docFrom.DocEntry, docEntryTo, objectType, docType);
                        if (String.IsNullOrEmpty(msg))
                        {
                            if (ConnectionTo.oCompany.InTransaction)
                            {
                                ConnectionTo.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                                rstUpdate.DoQuery($"UPDATE {tableName} SET U_CVA_Imported = 1 WHERE DocEntry = " + docFrom.DocEntry);
                                updated++;
                            }
                        }
                        else
                        {
                            if (ConnectionTo.oCompany.InTransaction)
                            {
                                ConnectionTo.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            Program.Logger.Info($"{tableName}: {docFrom.DocEntry} - {msg}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    Program.Logger.Info($"{tableName}: {docFrom.DocEntry} - Erro geral: {msg}");
                }
                finally
                {
                    Marshal.ReleaseComObject(docTo);
                    docTo = null;

                    Marshal.ReleaseComObject(docFrom);
                    docFrom = null;

                    if (!String.IsNullOrEmpty(msg))
                    {
                        error++;
                    }
                    msg = String.Empty;
                    rstDocFrom.MoveNext();
                }
            }
            Program.Logger.Info($"Registros inseridos: " + updated);
            Program.Logger.Info($"Registros com erro: " + error);
        }

        private string GeneratePayment(int docEntryFrom, int docEntryTo, BoObjectTypes objectType, DocTypeEnum docType)
        {
            Recordset rst = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            string table = String.Empty;
            if (docType == DocTypeEnum.In)
            {
                table = "VPM";
            }
            else
            {
                table = "RCT";
            }

            string sql = $@"SELECT {table}2.DocNum FROM {table}2
	                            INNER JOIN O{table}
		                            ON O{table}.DocNum = {table}2.DocNum
		                            AND Canceled <> 'Y'
                            WHERE {table}2.DocEntry = {docEntryFrom}
                            AND {table}2.InvType = {(int)objectType}";

            rst.DoQuery(sql);
            while (!rst.EoF)
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
                paymentTo.VatDate = paymentFrom.VatDate;

                if (!String.IsNullOrEmpty(paymentFrom.BoeAccount))
                {
                    return String.Empty;
                }

                paymentTo.BoeAccount = this.GetAccount(paymentFrom.BoeAccount);

                paymentTo.CashSum = paymentFrom.CashSum;
                paymentTo.BillOfExchangeAmount = paymentFrom.BillOfExchangeAmount;
                paymentTo.BillOfExchange.BillOfExchangeDueDate = paymentFrom.BillOfExchange.BillOfExchangeDueDate;
                paymentTo.BillOfExchange.PaymentMethodCode = paymentFrom.BillOfExchange.PaymentMethodCode;

                paymentTo.TransferSum = paymentFrom.TransferSum;
                paymentTo.TransferDate = paymentFrom.TransferDate;

                paymentTo.TransferAccount = this.GetAccount(paymentFrom.TransferAccount);

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

                rst.MoveNext();

            }
            return String.Empty;
        }

        private string GetAccount(string account)
        {
            account = account.Replace("1.1.1", "1.1.01");
            account = account.Replace("1.1.2", "1.1.02");

            switch (account)
            {
                case "1.1.01.02.007":
                    account = "1.1.01.02.049";
                    break;
                case "1.1.01.02.001":
                case "1.1.01.02.002":
                    account = "1.1.01.02.011";
                    break;
                case "1.1.01.02.009":
                    account = "1.1.01.02.025";
                    break;
                case "1.1.01.02.032":
                    account = "1.1.01.02.037";
                    break;
                case "1.1.01.02.033":
                    account = "1.1.01.02.079";
                    break;
                case "1.1.01.02.040":
                    account = "1.1.01.02.036";
                    break;
                case "1.1.02.01.004":
                    account = "1.1.01.01.004";
                    break;
            }
            return account;
        }
    }
}
