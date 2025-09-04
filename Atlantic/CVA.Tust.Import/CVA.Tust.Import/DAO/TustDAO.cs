using CVA.Tust.Import.ConciliatorDAO;
using CVA.Tust.Import.PortalDAO;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CVA.Tust.Import.DAO
{
    public class TustDAO
    {
        private ConciliatorEDM conciliatorEDM;
        private readonly PortalEDM portalEDM;

        #region Constructor
        public TustDAO()
        {
            conciliatorEDM = new ConciliatorEDM();
            portalEDM = new PortalEDM();
        }
        #endregion

        #region Import

        public string Execute()
        {
            var msg = string.Empty;

            TustLogModel logModel = new TustLogModel();

            List<TustParamModel> paramList = portalEDM.CVA_PAR_TUST.ToList();
            List<TustModel> tustList = portalEDM.CVA_IMP_TUST.Where(t => t.STATUS == 0).ToList();
            if (tustList.Count == 0)
            {
                return String.Empty;
            }

            Recordset rst = null;
            int tmpLine = 2;
            
            var errorsCount = 0;
            string cardCode = String.Empty;
            string cardName = String.Empty;

            double totalPerBase = 0;
            TustParamModel paramModel = new TustParamModel();
            string cnpjFilial = String.Empty;
            string cnpjFornecedor = String.Empty;

            try
            {
                int totalImported = 0;
                foreach (var model in tustList)
                {
                    try
                    {
                        logModel = new TustLogModel();
                        logModel.INS = DateTime.Now;
                        logModel.LINE = model.LINE;
                        logModel.FILE = model.FILE;
                        
                        totalImported++;

                        cnpjFilial = Convert.ToUInt64(model.CNPJ).ToString(@"00\.000\.000\/0000\-00");
                        if (paramModel.CNPJ != cnpjFilial)
                        {
                            string companyName = paramModel.BASE_NAME;
                            paramModel = paramList.FirstOrDefault(p => p.CNPJ == cnpjFilial);
                            if (paramModel == null)
                            {
                                paramModel = new TustParamModel();
                                logModel.DSCR = "Parâmetros não encontrados para o CNPJ " + cnpjFilial;
                                logModel.LOG_TYPE = "Erro";
                                errorsCount++;
                                continue;
                            }

                            if (companyName != paramModel.BASE_NAME)
                            {
                                if (totalPerBase > 0)
                                {
                                    TustLogModel totalLogModel = new TustLogModel();
                                    totalLogModel.INS = DateTime.Now;
                                    totalLogModel.LINE = 0;
                                    totalLogModel.FILE = model.FILE;
                                    totalLogModel.DSCR = $"Valor Total {paramModel.BASE_NAME}";
                                    totalLogModel.VALUE = (decimal)totalPerBase;
                                    totalLogModel.LOG_TYPE = "Sucesso";

                                    portalEDM.CVA_LOG_TUST.Add(totalLogModel);
                                    portalEDM.SaveChanges();
                                    totalPerBase = 0;
                                }
                            }

                            if (paramModel != null)
                            {
                                var connMsg = SBOConnectionDao.ConnectToCompany(paramModel.BASE_ID, paramModel.BASE_NAME);
                                if (!string.IsNullOrEmpty(connMsg))
                                {
                                    logModel.DSCR = "Erro ao conectar no banco de dados: " + connMsg;
                                    logModel.LOG_TYPE = "Erro";
                                    errorsCount++;
                                    continue;
                                }
                                rst = (Recordset)SBOConnectionDao._Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                            }
                            else
                            {
                                logModel.DSCR = "CNPJ não parametrizado: " + cnpjFilial;
                                logModel.LOG_TYPE = "Erro";
                                errorsCount++;
                                continue;
                            }
                        }

                        if (String.IsNullOrEmpty(cardCode) || (model.CNPJ_SUPPLIER != cnpjFornecedor))
                        {
                            var cnpjComprador = Convert.ToUInt64(model.CNPJ_SUPPLIER).ToString(@"00\.000\.000\/0000\-00");
                            rst.DoQuery(string.Format(SQL.BusinessPartner_GetByCnpj, cnpjComprador));
                            if (rst.RecordCount == 0)
                            {
                                logModel.DSCR = "CNPJ Fornecedor não encontrado: " + cnpjComprador;
                                logModel.LOG_TYPE = "Erro";
                                errorsCount++;
                                continue;
                            }
                            cardCode = rst.Fields.Item("CardCode").Value.ToString();
                            cardName = rst.Fields.Item("CardName").Value.ToString();
                            cnpjFornecedor = model.CNPJ_SUPPLIER;
                        }

                        string error = String.Empty;

                        logModel.SUPPLIER = cardCode + " - " + cardName;
                        logModel.PAY_METHOD = !String.IsNullOrEmpty(model.BOE_NUM) ? "Boleto" : "Transferência";
                        logModel.VALUE = (decimal)model.VALUE;

                        int docEntry = 0;
                        if (model.DOC_TYPE.ToUpper() == "RECIBO")
                        {
                            logModel.IMP_TYPE= "LCM";
                            error = this.GenerateLCM(paramModel, model, cardCode, cardName, tmpLine, ref docEntry);
                        }
                        else
                        {
                            logModel.IMP_TYPE = "NF";
                            error = this.GenerateInvoice(paramModel, model, cardCode, cardName, tmpLine, ref docEntry);
                        }
                        if (!String.IsNullOrEmpty(error))
                        {
                            logModel.DSCR = error;
                            logModel.LOG_TYPE = "Erro";
                            errorsCount++;
                        }
                        else
                        {
                            totalPerBase += model.VALUE;
                            logModel.TRANSID = docEntry;
                            logModel.DSCR = "Importado com sucesso!";
                            logModel.LOG_TYPE = "Sucesso";
                        }
                    }
                    catch (Exception ex)
                    {
                        logModel.DSCR = "Erro geral: " + ex.Message;
                        logModel.LOG_TYPE = "Erro";
                        errorsCount++;
                    }
                    finally
                    {
                        model.STATUS = 1;
                        portalEDM.CVA_LOG_TUST.Add(logModel);
                        portalEDM.SaveChanges();
                        
                    }
                }

                if (totalPerBase > 0)
                {
                    TustLogModel totalLogModel = new TustLogModel();
                    totalLogModel.INS = DateTime.Now;
                    totalLogModel.LINE = 0;
                    totalLogModel.FILE = tustList[0].FILE;
                    totalLogModel.DSCR = $"Valor Total {paramModel.BASE_NAME}";
                    totalLogModel.VALUE = (decimal)totalPerBase;
                    totalLogModel.LOG_TYPE = "Sucesso";

                    portalEDM.CVA_LOG_TUST.Add(totalLogModel);
                    portalEDM.SaveChanges();
                }

                logModel = new TustLogModel();
                logModel.INS = DateTime.Now;
                logModel.LINE = 0;
                logModel.FILE = tustList[0].FILE;

                if (errorsCount == 0)
                {
                    logModel.DSCR = $"Importação finalizada: {totalImported} linhas importadas com sucesso!";
                    logModel.LOG_TYPE = "Sucesso";
                }
                else
                {
                    logModel.DSCR =
                        $"Importação finalizada: {totalImported - errorsCount} linhas com sucesso. {errorsCount} linhas com erro.";
                    logModel.LOG_TYPE = "Alerta";
                }
                
                portalEDM.CVA_LOG_TUST.Add(logModel);
                portalEDM.SaveChanges();

                if (rst != null)
                {
                    Marshal.ReleaseComObject(rst);
                    rst = null;
                }
            }
            catch (Exception ex)
            {
                logModel = new TustLogModel();
                logModel.INS = DateTime.Now;
                logModel.DSCR = "Erro geral: " + ex.Message;
                logModel.LOG_TYPE = "Erro";
                logModel.LINE = -1;
                logModel.FILE = tustList[0].FILE;
                portalEDM.CVA_LOG_TUST.Add(logModel);
                portalEDM.SaveChanges();

                msg = "Erro geral: " + ex.Message;
                

                if (SBOConnectionDao._Company != null && SBOConnectionDao._Company.InTransaction)
                {
                    SBOConnectionDao._Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
            }

            return msg;
        }

        #endregion

        #region GenerateLCM
        private string GenerateLCM(TustParamModel paramModel, TustModel model, string cardCode, string cardName, int tmpLine, ref int docEntry)
        {
            string msg = String.Empty;
            JournalEntries lcm = (JournalEntries)SBOConnectionDao._Company.GetBusinessObject(BoObjectTypes.oJournalEntries);

            try
            {
                lcm.ReferenceDate = model.DATE;
                lcm.TaxDate = model.DOC_DATE;
                lcm.DueDate = model.DOC_DUE;
                lcm.UserFields.Fields.Item("U_CVA_Integracao").Value = "Y";

                string comments = $"TUST - {model.DATE.ToString("MM/yyyy")} - {cardName}";
                if (comments.Length > 50)
                {
                    comments = comments.Substring(0, 50);
                }

                lcm.Memo = comments;

                lcm.Lines.BPLID = paramModel.BPLID;

                lcm.Lines.ProjectCode = paramModel.PROJECT;
                lcm.Lines.ShortName = cardCode;
                if (!String.IsNullOrEmpty(paramModel.ACC_CONTROL))
                {
                    lcm.Lines.ControlAccount = paramModel.ACC_CONTROL;
                }

                lcm.Lines.Reference2 = model.NR_DOC.ToString();
                lcm.Lines.DueDate = model.DOC_DUE;
                lcm.Lines.Credit = model.VALUE;

                lcm.Lines.Add();
                lcm.Lines.BPLID = paramModel.BPLID;
                lcm.Lines.AccountCode = paramModel.ACC;
                lcm.Lines.ProjectCode = paramModel.PROJECT;
                lcm.Lines.Reference2 = model.NR_DOC.ToString();
                lcm.Lines.DueDate = model.DOC_DUE;
                lcm.Lines.Debit = model.VALUE;
                if (!String.IsNullOrEmpty(paramModel.COST_CENTER))
                {
                    lcm.Lines.CostingCode = paramModel.COST_CENTER;
                }

                if (!String.IsNullOrEmpty(model.BANK) || !String.IsNullOrEmpty(model.BOE_NUM))
                {
                    SBOConnectionDao._Company.StartTransaction();
                }

                if (lcm.Add() != 0)
                {
                    return SBOConnectionDao._Company.GetLastErrorDescription();
                }
                else
                {
                    docEntry = Convert.ToInt32(SBOConnectionDao._Company.GetNewObjectKey());
                    if (!String.IsNullOrEmpty(model.BOE_NUM))
                    {
                        msg = GenerateOutGoingPaymentBillet(BoRcptInvTypes.it_JournalEntry, docEntry, cardCode, paramModel, model);

                    }
                    else if (!String.IsNullOrEmpty(model.BANK))
                    {
                        msg = GenerateOutGoingPaymentTransfer(BoRcptInvTypes.it_JournalEntry, docEntry, cardCode, paramModel, model);
                    }

                    if (String.IsNullOrEmpty(msg))
                    {
                        if (SBOConnectionDao._Company.InTransaction)
                        {
                            SBOConnectionDao._Company.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                    }
                    else
                    {
                        msg = "Erro ao gerar contas a pagar: " + msg;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (SBOConnectionDao._Company.InTransaction)
                {
                    SBOConnectionDao._Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                Marshal.ReleaseComObject(lcm);
                lcm = null;
            }

            return msg;
        }
        #endregion

        #region GenerateInvoice
        private string GenerateInvoice(TustParamModel paramModel, TustModel model, string cardCode, string cardName, int tmpLine, ref int docEntry)
        {
            string msg = String.Empty;
            Documents doc = (Documents)SBOConnectionDao._Company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
            try
            {
                doc.BPL_IDAssignedToInvoice = paramModel.BPLID;
                doc.CardCode = cardCode;
                doc.UserFields.Fields.Item("U_chaveacesso").Value = model.XML;
                doc.DocDate = model.DATE;
                doc.TaxDate = model.DOC_DATE;
                doc.DocDueDate = model.DOC_DUE;
                doc.Comments = $"TUST - {model.DATE.ToString("MM/yyyy")} - {cardName}";
                if (doc.Comments.Length > 254)
                {
                    doc.Comments = doc.Comments.Substring(0, 254);
                }
                doc.UserFields.Fields.Item("U_CVA_Integracao").Value = "Y";


                doc.SequenceCode = -2;
                doc.SequenceSerial = model.NR_DOC;
                doc.SequenceModel = paramModel.NF_MODEL;

                if (!String.IsNullOrEmpty(model.XML) && model.XML.Length > 25)
                {
                    doc.SeriesString = model.XML.Substring(22, 3);
                }

                //doc.TransportationCode = paramModel.TranspCode;

                doc.TaxExtension.Incoterms = paramModel.INCOTERMS;

                if (!String.IsNullOrEmpty(paramModel.ACC_CONTROL))
                {
                    doc.ControlAccount = paramModel.ACC_CONTROL;
                }

                doc.Lines.ItemCode = paramModel.ITEMCODE;
                doc.Lines.Usage = paramModel.USAGE.ToString();
                doc.Lines.Price = model.VALUE;
                doc.Lines.Quantity = 1;
                doc.Lines.ProjectCode = paramModel.PROJECT;
                doc.Lines.CostingCode = paramModel.COST_CENTER;

                if (paramModel.ALIQIPI.HasValue)
                {
                    doc.Lines.UserFields.Fields.Item("U_SX_AliqIPI").Value = Convert.ToInt32(paramModel.ALIQIPI);
                }
                doc.Lines.UserFields.Fields.Item("U_SX_Aplicacao").Value = paramModel.APLIC;
                doc.Lines.UserFields.Fields.Item("U_RegPC").Value = paramModel.REG_PC;
                doc.Lines.UserFields.Fields.Item("U_IND_OPER").Value = paramModel.IND_OPER;
                doc.Lines.UserFields.Fields.Item("U_IND_ORIG_CRED").Value = paramModel.ORIG_CRED;
                doc.Lines.UserFields.Fields.Item("U_DESC_DOC_OPER").Value = paramModel.DESC_DOC;

                if (!String.IsNullOrEmpty(model.BANK) || !String.IsNullOrEmpty(model.BOE_NUM))
                {
                    SBOConnectionDao._Company.StartTransaction();
                }
                if (doc.Add() != 0)
                {
                    return SBOConnectionDao._Company.GetLastErrorDescription();
                }
                else
                {
                    docEntry = Convert.ToInt32(SBOConnectionDao._Company.GetNewObjectKey());

                    if (!String.IsNullOrEmpty(model.BOE_NUM))
                    {
                        msg = GenerateOutGoingPaymentBillet(BoRcptInvTypes.it_PurchaseInvoice, docEntry, cardCode, paramModel, model);
                    }
                    else if (!String.IsNullOrEmpty(model.BANK))
                    {
                        msg = GenerateOutGoingPaymentTransfer(BoRcptInvTypes.it_PurchaseInvoice, docEntry, cardCode, paramModel, model);
                    }

                    if (String.IsNullOrEmpty(msg))
                    {
                        msg = this.InsertItemAdditionalInformationSkillTable(paramModel, docEntry);
                    }
                    else
                    {
                        if (SBOConnectionDao._Company.InTransaction)
                        {
                            SBOConnectionDao._Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                        msg = "Erro ao gerar contas a pagar: " + msg;
                        return msg;
                    }

                    if (String.IsNullOrEmpty(msg))
                    {
                        if (SBOConnectionDao._Company.InTransaction)
                        {
                            SBOConnectionDao._Company.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                    }
                    else
                    {
                        if (SBOConnectionDao._Company.InTransaction)
                        {
                            SBOConnectionDao._Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                        msg = "Erro ao inserir dados na tabela de informações adicionais do item [@SKILL_INF_ITEM_DOC]: " + msg;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro geral: " + ex.Message;
            }
            finally
            {
                if (SBOConnectionDao._Company.InTransaction)
                {
                    SBOConnectionDao._Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
            }

            return msg;
        }
        #endregion

        #region GenerateOutGoingPaymentTransfer

        public string GenerateOutGoingPaymentTransfer(BoRcptInvTypes docType, int id, string cardCode, TustParamModel paramModel, TustModel model)
        {
            Recordset rst = (Recordset)SBOConnectionDao._Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            Payments payment = (Payments)SBOConnectionDao._Company.GetBusinessObject(BoObjectTypes.oVendorPayments);

            try
            {
                payment.CardCode = cardCode;
                payment.DocType = BoRcptTypes.rSupplier;
                payment.DocObjectCode = BoPaymentsObjectType.bopot_OutgoingPayments;
                payment.BPLID = paramModel.BPLID;

                payment.DocDate = model.DATE;
                payment.TaxDate = model.DOC_DATE;
                payment.DueDate = model.DOC_DUE;

                payment.CashSum = 0;
                payment.TransferSum = model.VALUE;
                payment.TransferDate = model.DOC_DUE;

                payment.Invoices.DocEntry = id;
                payment.Invoices.InvoiceType = docType;
                payment.Invoices.SumApplied = model.VALUE;

                payment.TransferAccount = paramModel.ACC_TRANS_COMP;

                rst.DoQuery(String.Format(SQL.PayMethod_Validation, paramModel.PAY_METH_TRANS, cardCode));
                if (rst.RecordCount > 0)
                {
                    payment.UserFields.Fields.Item("U_PayMthod").Value = paramModel.PAY_METH_TRANS;
                }
                else
                {
                    payment.UserFields.Fields.Item("U_PayMthod").Value = paramModel.PAY_METH_TED;
                }

                payment.UserFields.Fields.Item("U_IsAuto").Value = 1;
                payment.UserFields.Fields.Item("U_IsBSPmnt").Value = 1;
                payment.UserFields.Fields.Item("U_TrnfsNum").Value = "1";
                payment.UserFields.Fields.Item("U_SIEBS_TrnsfCmp").Value = paramModel.ACC_TRANS;

                payment.UserFields.Fields.Item("U_TrnfsNum").Value = this.InsertBSTransferTable(model.DATE);

                if (payment.Add() != 0)
                    return SBOConnectionDao._Company.GetLastErrorDescription();
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(payment);
                payment = null;

                Marshal.ReleaseComObject(rst);
                rst = null;
            }
        }

        #endregion

        #region GenerateOutGoingPaymentBillet

        public string GenerateOutGoingPaymentBillet(BoRcptInvTypes docType, int docEntry, string cardCode, TustParamModel paramModel, TustModel model)
        {
            Payments payment = (Payments)SBOConnectionDao._Company.GetBusinessObject(BoObjectTypes.oVendorPayments);
            Recordset rst = (Recordset)SBOConnectionDao._Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            try
            {
                rst.DoQuery(SQL.Boe_GetNext);

                payment.CardCode = cardCode;
                payment.DocType = BoRcptTypes.rSupplier;
                payment.DocObjectCode = BoPaymentsObjectType.bopot_OutgoingPayments;
                payment.BPLID = paramModel.BPLID;
                payment.UserFields.Fields.Item("U_CVA_BarCode").Value = model.BOE_NUM;

                payment.DocDate = model.DATE;
                payment.TaxDate = model.DOC_DATE;
                payment.DueDate = model.DOC_DUE;

                payment.BoeAccount = paramModel.ACC;

                payment.Invoices.DocEntry = docEntry;
                payment.Invoices.InvoiceType = docType;
                payment.Invoices.SumApplied = model.VALUE;

                payment.CashSum = 0;
                payment.BillOfExchangeAmount = model.VALUE;

                payment.BillOfExchange.BillOfExchangeNo = rst.Fields.Item(0).Value.ToString();
                payment.BillOfExchange.BillOfExchangeDueDate = model.DOC_DUE;
                //if (model.LinhaDigitavel.Trim().StartsWith("033"))
                //{
                //    payment.BillOfExchange.PaymentMethodCode = "P-SANT-BOLETO";
                //}
                //else
                //{
                //    payment.BillOfExchange.PaymentMethodCode = "P-SANT-BOL-OUT";
                //}
                payment.BillOfExchange.PaymentMethodCode = paramModel.PAY_METH;

                payment.BillOfExchange.UserFields.Fields.Item("U_CVA_BarCode").Value = model.BOE_NUM;

                if (payment.Add() != 0)
                    return SBOConnectionDao._Company.GetLastErrorDescription();
                else
                {
                    //Recordset rst = (Recordset)SBOConnectionDao._Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                    //rst.DoQuery("UPDATE OBOE SET BarcodeRep = U_CVA_BarCode WHERE ISNULL(BarcodeRep, '') = '' AND ISNULL(U_CVA_BarCode, '') <> '' ");

                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(payment);
                payment = null;
            }
        }

        #endregion

        #region InsertBSTransferTable

        private string InsertBSTransferTable(DateTime docDate)
        {
            Recordset rst = (Recordset)SBOConnectionDao._Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            UserTable table = SBOConnectionDao._Company.UserTables.Item("SIEBS_OTRF");

            try
            {
                rst.DoQuery(SQL.BankSyncTransfer_GetMaxCode);
                string nextCode = (Convert.ToInt32(rst.Fields.Item(0).Value) + 1).ToString();

                table.Code = nextCode;
                table.Name = "O" + table.Code;
                table.UserFields.Fields.Item("U_GenDate").Value = docDate;
                table.UserFields.Fields.Item("U_BSStatus").Value = 0;
                table.UserFields.Fields.Item("U_PmtTransId").Value = 0;
                table.UserFields.Fields.Item("U_ContaTransit").Value = "Y";
                if (table.Add() != 0)
                {
                    throw new Exception(SBOConnectionDao._Company.GetLastErrorDescription());
                }
                return nextCode;
            }
            finally
            {
                Marshal.ReleaseComObject(rst);
                rst = null;

                Marshal.ReleaseComObject(table);
                table = null;
            }
        }

        #endregion


        private string InsertItemAdditionalInformationSkillTable(TustParamModel paramModel, int docEntry)
        {
            Recordset rst = (Recordset)SBOConnectionDao._Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            UserTable table = SBOConnectionDao._Company.UserTables.Item("SKILL_INF_ITEM_DOC");

            try
            {
                rst.DoQuery(SQL.SkillItemAdditionalInformation_GetMaxCode);
                string nextCode = (Convert.ToInt32(rst.Fields.Item(0).Value) + 1).ToString();

                table.Code = nextCode;
                table.Name = nextCode;
                table.UserFields.Fields.Item("U_SKILL_DOCUMENTO").Value = "OPCH";
                table.UserFields.Fields.Item("U_SKILL_DOCENTRY").Value = docEntry;
                table.UserFields.Fields.Item("U_SKILL_LINHA").Value = 0;
                table.UserFields.Fields.Item("U_SKILL_ITEMCODE").Value = paramModel.ITEMCODE;
                table.UserFields.Fields.Item("U_SKILL_NAT_BC_CRED").Value = paramModel.NAT_BC;
                table.UserFields.Fields.Item("U_RegPC").Value = paramModel.REG_PC;
                table.UserFields.Fields.Item("U_IND_OPER").Value = paramModel.IND_OPER;
                table.UserFields.Fields.Item("U_IND_ORIG_CRED").Value = paramModel.ORIG_CRED;
                table.UserFields.Fields.Item("U_DESC_DOC_OPER").Value = paramModel.DESC_DOC;
                if (table.Add() != 0)
                {
                    return SBOConnectionDao._Company.GetLastErrorDescription();
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(rst);
                rst = null;

                Marshal.ReleaseComObject(table);
                table = null;
            }
        }
    }
}
