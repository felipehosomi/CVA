using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.StockTransfer.BLL;
using CVA.View.StockTransfer.MODEL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace CVA.View.StockTransfer.VIEW
{
    /// <summary>
    /// Nota Fiscal de Saída
    /// </summary>
    public class f133 : BaseForm
    {
        public static string ErrorMessage = String.Empty;
        private static int StockTransferId = 0;
        
        #region Constructor
        public f133()
        {
            FormCount++;
        }

        public f133(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f133(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f133(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
            {
                SBOApp.Application.StatusBarEvent += Application_StatusBarEvent;
            }
            if (ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_FORM_CLOSE)
            {
                SBOApp.Application.StatusBarEvent -= Application_StatusBarEvent;
            }
            return true;
        }

        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction)
            {
               if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    if (BusinessObjectInfo.ActionSuccess)
                    {
                        DBDataSource ds_OINV = Form.DataSources.DBDataSources.Item("OINV");
                        string canceled = ds_OINV.GetValue("CANCELED", ds_OINV.Offset);
                        if (canceled == "C")
                        {
                            int docEntry = Convert.ToInt32(ds_OINV.GetValue("DocEntry", ds_OINV.Offset));
                            StockTransferRequestBLL stockTransferRequestBLL = new StockTransferRequestBLL();

                            string msg = stockTransferRequestBLL.RevertStockTransferRequest(docEntry, BoObjectTypes.oInvoices, ref StockTransferId);
                            if (!String.IsNullOrEmpty(msg))
                            {
                                SBOApp.Application.MessageBox("Erro ao gerar solicitação de transferência: " + msg);
                            }
                            else
                            {
                                if (StockTransferId > 0)
                                {
                                    StockTransferId = 0;
                                    SBOApp.Application.MessageBox("Solicitação de transferência gerada com sucesso!");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    DBDataSource ds_Lines = Form.DataSources.DBDataSources.Item("INV1");
                    List<int> baseDocsPO = new List<int>();
                    List<int> baseDocsDelivery = new List<int>();

                    for (int i = 0; i < ds_Lines.Size; i++)
                    {

                        int baseEntry;
                        int baseType;

                        Int32.TryParse(ds_Lines.GetValue("BaseEntry", ds_Lines.Offset), out baseEntry);
                        Int32.TryParse(ds_Lines.GetValue("BaseType", ds_Lines.Offset), out baseType);

                        if (baseType == 17)
                        {
                            if (baseEntry != 0 && !baseDocsPO.Contains(baseEntry))
                            {
                                baseDocsPO.Add(baseEntry);
                            }
                        }
                        else if (baseType == 15)
                        {
                            if (baseEntry != 0 && !baseDocsDelivery.Contains(baseEntry))
                            {
                                baseDocsDelivery.Add(baseEntry);
                            }
                        }
                    }

                    StockTransferBLL stockTranferBLL = new StockTransferBLL();
                    ErrorMessage = stockTranferBLL.Validate(baseDocsPO, baseDocsDelivery);
                    if (!String.IsNullOrEmpty(ErrorMessage))
                    {
                        return false;
                    }

                    DBDataSource ds_OINV = Form.DataSources.DBDataSources.Item("OINV");
                    string canceled = ds_OINV.GetValue("CANCELED", ds_OINV.Offset);
                    if (canceled == "C")
                    {
                        StockTransferModel model = new StockTransferModel();
                        model.CardCode = ds_OINV.GetValue("CardCode", ds_OINV.Offset);
                        model.BaseDocEntry = Convert.ToInt32(ds_OINV.GetValue("DocNum", ds_OINV.Offset));
                        model.DocType = BoObjectTypes.oInvoices;
                        model.Items = new List<StockTransferItemModel>();

                        for (int i = 0; i < ds_Lines.Size; i++)
                        {
                            StockTransferItemModel item = new StockTransferItemModel();
                            item.ItemCode = ds_Lines.GetValue("ItemCode", i);
                            item.Quantity = Convert.ToDouble(ds_Lines.GetValue("Quantity", i).Replace(".", ","));
                            item.Usage = ds_Lines.GetValue("Usage", i);
                            model.Items.Add(item);
                        }

                        StockTransferRequestBLL stockTransferRequestBLL = new StockTransferRequestBLL();
                        string msg = stockTransferRequestBLL.GenerateStockTransferRequest(model, ref StockTransferId, false);
                        if (!String.IsNullOrEmpty(msg))
                        {
                            ErrorMessage = "Erro ao gerar solicitação de transferência: " + msg;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void Application_StatusBarEvent(string Text, BoStatusBarMessageType messageType)
        {
            if (Text.Contains("UI_API -7780") && !String.IsNullOrEmpty(ErrorMessage))
            {
                SBOApp.Application.StatusBar.SetText(ErrorMessage, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }
    }
}
