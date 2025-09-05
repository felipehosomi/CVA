using CVA.AddOn.Common;
using CVA.Hub.BLL;
using SAPbouiCOM;
using System;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class f139 : DocumentBaseView
    {

        public SAPbouiCOM.EditText et_DocEntry { get; set; }
        public SAPbouiCOM.Button btn_procurar { get; set; }
        public SAPbouiCOM.EditText et_Comments { get; set; }


        #region Constructor
        public f139()
        {
            FormCount++;
        }

        public f139(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f139(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f139(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool FormDataEvent()
        {

            bool isOK = base.FormDataEvent();
            if (isOK)
            {
                if (!BusinessObjectInfo.BeforeAction)
                {
                    if (!IsCanceling)
                    {
                        if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                        {
                            DBDataSource dtsDoc = this.Form.DataSources.DBDataSources.Item(0);
                            if (!String.IsNullOrEmpty(dtsDoc.GetValue("DocEntry", dtsDoc.Offset)))
                            {
                                int docEntry = Convert.ToInt32(dtsDoc.GetValue("DocEntry", dtsDoc.Offset));
                                int objType = Convert.ToInt32(dtsDoc.GetValue("ObjType", dtsDoc.Offset));
                                string cancelado = dtsDoc.GetValue("CANCELED", dtsDoc.Offset);
                                if (cancelado != "Y")
                                {
                                    try
                                    {
                                        DocumentoBLL documentoBLL = new DocumentoBLL();
                                        documentoBLL.UpdateObsPN(docEntry);
                                    }
                                    catch (Exception ex)
                                    {
                                        SBOApp.Application.MessageBox(ex.Message);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return isOK;
        }   
    }
}


