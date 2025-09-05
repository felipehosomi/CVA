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
    /// Dev. Nota Fiscal Saída
    /// </summary>
    public class f179 : BaseForm
    {
        public static string ErrorMessage = String.Empty;
        #region Constructor
        public f179()
        {
            FormCount++;
        }

        public f179(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f179(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f179(ContextMenuInfo contextMenuInfo)
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

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.ActionSuccess && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    DBDataSource ds_ORIN = Form.DataSources.DBDataSources.Item("ORIN");
                    string docEntry = ds_ORIN.GetValue("DocEntry", ds_ORIN.Offset);
                    string canceled = ds_ORIN.GetValue("CANCELED", ds_ORIN.Offset);
                    if (canceled != "C")
                    {
                        int stockTransferId = 0;

                        StockTransferRequestBLL stockTransferRequestBLL = new StockTransferRequestBLL();
                        string msg = stockTransferRequestBLL.RevertStockTransferRequest(Convert.ToInt32(docEntry), BoObjectTypes.oCreditNotes, ref stockTransferId);
                        if (!String.IsNullOrEmpty(msg))
                        {
                            SBOApp.Application.MessageBox("Erro ao gerar solicitação de transferência: " + msg);
                        }
                        else
                        {
                            if (stockTransferId > 0)
                            {
                                SBOApp.Application.MessageBox("Solicitação de transferência gerada com sucesso!");
                            }
                        }
                    }
                }
            }
            //else
            //{
            //    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
            //    {
            //        DBDataSource ds_ORIN = Form.DataSources.DBDataSources.Item("ORIN");
            //        DBDataSource ds_Lines = Form.DataSources.DBDataSources.Item("RIN1");
            //        StockTransferModel model = new StockTransferModel();
            //        //model.BaseDocEntry = Convert.ToInt32(ds_ORIN.GetValue("DocEntry", ds_ORIN.Offset));
            //        model.CardCode = ds_ORIN.GetValue("CardCode", ds_ORIN.Offset);
            //        model.DocType = BoObjectTypes.oCreditNotes;
            //        model.Items = new List<StockTransferItemModel>();

            //        for (int i = 0; i < ds_Lines.Size; i++)
            //        {
            //            StockTransferItemModel item = new StockTransferItemModel();
            //            item.ItemCode = ds_Lines.GetValue("ItemCode", i);
            //            item.Quantity = Convert.ToDouble(ds_Lines.GetValue("Quantity", i).Replace(".", ","));
            //            item.Usage = ds_Lines.GetValue("Usage", i);
            //            model.Items.Add(item);
            //        }

            //        StockTransferRequestBLL stockTransferRequestBLL = new StockTransferRequestBLL();
            //        int stockTransferId = 0;
            //        string msg = stockTransferRequestBLL.GenerateStockTransferRequest(model, ref stockTransferId, false);
            //        if (!String.IsNullOrEmpty(msg))
            //        {
            //            ErrorMessage = "Erro ao gerar solicitação de transferência: " + msg;
            //            return false;
            //        }
            //    }
            //}

            return true;
        }
        #endregion

        private void Application_StatusBarEvent(string Text, BoStatusBarMessageType messageType)
        {
            if (Text.Contains("UI_API -7780") && !String.IsNullOrEmpty(ErrorMessage))
            {
                SBOApp.Application.StatusBar.SetText(ErrorMessage, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }
    }
}
