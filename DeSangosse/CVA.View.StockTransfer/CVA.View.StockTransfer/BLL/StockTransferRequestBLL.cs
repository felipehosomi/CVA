using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.StockTransfer.DAO;
using CVA.View.StockTransfer.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CVA.View.StockTransfer.BLL
{
    public class StockTransferRequestBLL
    {
        public bool Exists(int baseDocEntry, int baseType)
        {
            object exists = CrudController.ExecuteScalar(String.Format(Query.StockTransferRequest_GetByBaseDoc, baseDocEntry, baseType));
            return exists != null;
        }

        public List<OrderModel> GetListOrdersLines(int docEntrty)
        {
            CrudController crudController = new CrudController();
            List<OrderModel> listOrderLines = crudController.FillModelListAccordingToSql<OrderModel>(String.Format(Query.Document_GetOrderLines, docEntrty));
          
            return listOrderLines;
        }

        #region GenerateStockTransferRequest
        public string GenerateStockTransferRequest(StockTransferModel model, ref int stockTransferId, bool commit = true)
        {
            SAPbobsCOM.StockTransfer stockTransfer = (SAPbobsCOM.StockTransfer)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryTransferRequest);
            CrudController crudController = new CrudController();

            try
            {
                bool validateTransfer = false;

                string bpWarehouse = CrudController.ExecuteScalar(String.Format(Query.BusinessPartner_GetWarehouse, model.CardCode)).ToString();
                string outWarehouse = CrudController.ExecuteScalar(Query.Warehouse_GetWhsOut).ToString();
                
                string fromWarehouse;
                string toWarehouse;

                if (model.DocType == BoObjectTypes.oOrders)
                {
                    fromWarehouse = outWarehouse;
                    toWarehouse = bpWarehouse;
                }
                else
                {
                    fromWarehouse = bpWarehouse;
                    toWarehouse = outWarehouse;
                }

                stockTransfer.CardCode = model.CardCode;
                stockTransfer.FromWarehouse = fromWarehouse;
                stockTransfer.ToWarehouse = toWarehouse;

                stockTransfer.UserFields.Fields.Item("U_CVA_Base_DocEntry").Value = model.BaseDocEntry;
                stockTransfer.UserFields.Fields.Item("U_CVA_Base_Type").Value = (int)model.DocType;

                foreach (var item in model.Items)
                {
                    if (!String.IsNullOrEmpty(item.Usage) && UsageBLL.ValidateTransfer(item.Usage))
                    {
                        validateTransfer = true;
                    }
                    else
                    {
                        continue;
                    }

                    if (!String.IsNullOrEmpty(stockTransfer.Lines.ItemCode))
                    {
                        stockTransfer.Lines.Add();
                    }
                   
                    stockTransfer.Lines.ItemCode = item.ItemCode;
                    stockTransfer.Lines.Quantity = item.Quantity;
                    stockTransfer.Lines.FromWarehouseCode = fromWarehouse;

                    if (model.DocType == BoObjectTypes.oOrders)
                    {
                        if (CrudController.ExecuteScalar(String.Format(Query.Item_GetBatchControl, item.ItemCode)).ToString() == "Y")
                        {
                            string sql = String.Format(Query.Batch_Get, item.ItemCode, outWarehouse);
                            List<BatchModel> batchList = crudController.FillModelListAccordingToSql<BatchModel>(sql);
                            double quantity = item.Quantity;
                            foreach (var batch in batchList)
                            {
                                if (!String.IsNullOrEmpty(stockTransfer.Lines.BatchNumbers.BatchNumber))
                                {
                                    stockTransfer.Lines.BatchNumbers.Add();
                                }
                                stockTransfer.Lines.BatchNumbers.BatchNumber = batch.BatchNum;
                                if (batch.Quantity >= quantity)
                                {
                                    stockTransfer.Lines.BatchNumbers.Quantity = quantity;
                                    quantity = 0;
                                    break;
                                }
                                else
                                {
                                    stockTransfer.Lines.BatchNumbers.Quantity = batch.Quantity;
                                    quantity -= batch.Quantity;
                                }
                            }
                            if (quantity > 0)
                            {
                                SBOApp.Application.MessageBox($"Solicitação Transf.: Não existem lotes suficientes para o item {item.ItemCode} no depósito {outWarehouse}");
                            }
                        }
                    }
                }

                if (validateTransfer)
                {
                    if (String.IsNullOrEmpty(bpWarehouse))
                    {
                        return "Depósito do cliente não configurado";
                    }
                    if (String.IsNullOrEmpty(outWarehouse))
                    {
                        return "Depósito de saída não configurado";
                    }
                    if (!commit)
                    {
                        SBOApp.Company.StartTransaction();
                    }

                    if (stockTransfer.Add() != 0)
                    {
                        return SBOApp.Company.GetLastErrorDescription();
                    }
                    else
                    {
                        stockTransferId = Convert.ToInt32(SBOApp.Company.GetNewObjectKey());
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro geral: " + ex.Message;
            }
            finally
            {
                // Transação é só pra saber se o documento vai ser adicionado corretamente
                if (SBOApp.Company.InTransaction)
                {
                    SBOApp.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }

                Marshal.ReleaseComObject(stockTransfer);
                stockTransfer = null;
            }
            return String.Empty;
        }
        #endregion

        #region SetStockTransferBaseDocument
        public string SetStockTransferBaseDocument(BoObjectTypes baseType, int baseDocEntry, int stockTransferId)
        {
            try
            {
                if (stockTransferId == 0)
                {
                    return String.Empty;
                }

                int docNum = Convert.ToInt32(CrudController.ExecuteScalar(String.Format(Query.Document_GetDocNum, "ORDR", baseDocEntry)));

                string docTypeDesc = String.Empty;
                switch (baseType)
                {
                    case BoObjectTypes.oOrders:
                        docTypeDesc = "Pedido de Venda: " + docNum;
                        break;
                    case BoObjectTypes.oInvoices:
                        docTypeDesc = "Cancelamento Nota Fisca Saída: " + docNum;
                        break;
                    case BoObjectTypes.oCreditNotes:
                        docTypeDesc = "Dev. Nota Fisca Saída: " + docNum;
                        break;
                }

                string update = String.Format(Query.StockTransferRequest_UpdateBaseDoc, stockTransferId, docTypeDesc, baseDocEntry, (int)baseType);
                CrudController.ExecuteNonQuery(update);
            }
            catch (Exception ex)
            {
                return "Erro geral: " + ex.Message;
            }
            return String.Empty;
        }
        #endregion

        #region RevertStockTransferRequest
        public string RevertStockTransferRequest(int baseDocEntry, BoObjectTypes baseType, ref  int stockTransferId)
        {
            Documents baseDoc = (Documents)SBOApp.Company.GetBusinessObject(baseType);
            SAPbobsCOM.StockTransfer stockTransfer = (SAPbobsCOM.StockTransfer)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryTransferRequest);
            CrudController crudController = new CrudController();

            try
            {
                baseDoc.GetByKey(baseDocEntry);
                bool validateTransfer = false;

                string bpWarehouse = CrudController.ExecuteScalar(String.Format(Query.BusinessPartner_GetWarehouse, baseDoc.CardCode)).ToString();
                string outWarehouse = CrudController.ExecuteScalar(Query.Warehouse_GetWhsOut).ToString();

                stockTransfer.CardCode = baseDoc.CardCode;
                stockTransfer.FromWarehouse = bpWarehouse;
                stockTransfer.ToWarehouse = outWarehouse;

                int docNum;
                if (baseType == BoObjectTypes.oInvoices)
                {
                    docNum = Convert.ToInt32(CrudController.ExecuteScalar(String.Format(Query.Document_GetDocNum, "OINV", baseDocEntry)));
                    stockTransfer.UserFields.Fields.Item("U_CVA_Base_Doc").Value = "Cancelamento Nota Fisca Saída: " + docNum;
                }
                else if (baseType == BoObjectTypes.oCreditNotes)
                {
                    docNum = Convert.ToInt32(CrudController.ExecuteScalar(String.Format(Query.Document_GetDocNum, "ORIN", baseDocEntry)));
                    stockTransfer.UserFields.Fields.Item("U_CVA_Base_Doc").Value = "Dev. Nota Fisca Saída: " + docNum;
                }

                stockTransfer.UserFields.Fields.Item("U_CVA_Base_DocEntry").Value = baseDocEntry;
                stockTransfer.UserFields.Fields.Item("U_CVA_Base_Type").Value = (int)baseType;

                for (int i = 0; i < baseDoc.Lines.Count; i++)
                {
                    baseDoc.Lines.SetCurrentLine(i);
                    if (!String.IsNullOrEmpty(baseDoc.Lines.Usage) && UsageBLL.ValidateTransfer(baseDoc.Lines.Usage))
                    {
                        validateTransfer = true;
                    }
                    else
                    {
                        continue;
                    }

                    if (!String.IsNullOrEmpty(stockTransfer.Lines.ItemCode))
                    {
                        stockTransfer.Lines.Add();
                    }
                    
                    stockTransfer.Lines.ItemCode = baseDoc.Lines.ItemCode;
                    stockTransfer.Lines.Quantity = baseDoc.Lines.Quantity;
                    stockTransfer.Lines.FromWarehouseCode = bpWarehouse;

                    for (int j = 0; j < baseDoc.Lines.BatchNumbers.Count; j++)
                    {
                        baseDoc.Lines.BatchNumbers.SetCurrentLine(j);
                        if (j > 0)
                        {
                            stockTransfer.Lines.BatchNumbers.Add();
                        }

                        stockTransfer.Lines.BatchNumbers.BatchNumber = baseDoc.Lines.BatchNumbers.BatchNumber;
                        stockTransfer.Lines.BatchNumbers.Quantity = baseDoc.Lines.BatchNumbers.Quantity;

                        //stockTransfer.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
                        //stockTransfer.Lines.BinAllocations.BaseLineNumber = po.Lines.LineNum;
                        //stockTransfer.Lines.BinAllocations.BinAbsEntry = po.Lines.BinAllocations.BinAbsEntry;

                    }
                }
                if (validateTransfer)
                {
                    if (stockTransfer.Add() != 0)
                    {
                        return SBOApp.Company.GetLastErrorDescription();
                    }
                    else
                    {
                        stockTransferId = Convert.ToInt32(SBOApp.Company.GetNewObjectKey());
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro geral: " + ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(baseDoc);
                Marshal.ReleaseComObject(stockTransfer);

                baseDoc = null;
                stockTransfer = null;
            }
            return String.Empty;
        }
        #endregion
    }
}
