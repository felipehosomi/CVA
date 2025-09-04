using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;

namespace CVA.Core.Alessi.VIEW
{
    public class f141 : BaseForm
    {
        #region Constructor
        public f141()
        {
            FormCount++;
        }

        public f141(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f141(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f141(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            this.IsSystemForm = true;

            if (!ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_CLICK)
            {
                if (ItemEventInfo.ItemUID == "bt_CTe")
                {
                    if (Form.Mode == BoFormMode.fm_ADD_MODE)
                    {
                        SBOApp.Application.SetStatusBarMessage("Por favor adicione o documento antes de informar as CT-e's referenciadas");
                        return true;
                    }
                    if (Form.Mode != BoFormMode.fm_FIND_MODE)
                    {
                        DBDataSource dt_OPCH = Form.DataSources.DBDataSources.Item("OPCH");
                        int docEntry = Convert.ToInt32(dt_OPCH.GetValue("DocEntry", dt_OPCH.Offset));
                        int docNum = Convert.ToInt32(dt_OPCH.GetValue("DocNum", dt_OPCH.Offset));

                        new f2000003001().Show(docEntry, docNum);
                    }
                }
            }

            return base.ItemEvent();
        }
    }
}
