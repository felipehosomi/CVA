using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class f150 : BaseForm
    {
        private Form form = null;

        #region Constructor
        public f150()
        {
            FormCount++;
        }

        public f150(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f150(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f150(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Show()

        public object Show()
        {
            return null;
        }

        public object Show(string[] args)
        {
            form = SBOApp.Application.OpenForm(BoFormObjectEnum.fo_Items, "", args[0]);
            return form;
        }
        #endregion

        public override bool ItemEvent()
        {
            this.IsSystemForm = true;
            base.ItemEvent();
            if (!ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
            {
                SAPbouiCOM.IChooseFromListEvent oCFLEvento = ((SAPbouiCOM.IChooseFromListEvent)(ItemEventInfo));
                SAPbouiCOM.DataTable oDataTable = oCFLEvento.SelectedObjects;

                if (ItemEventInfo.ItemUID == "et_CtCod")
                {
                    if (oDataTable != null)
                    {
                        if (oDataTable.Rows.Count > 0)
                        {
                            if (Form.Mode != SAPbouiCOM.BoFormMode.fm_FIND_MODE)
                            {
                                EditText et_CodCentroCusto = this.Form.Items.Item("et_CtCod").Specific as EditText;
                                EditText et_DescCentroCusto = this.Form.Items.Item("et_CtDesc").Specific as EditText;

                                string code = oDataTable.GetValue("PrcCode", 0).ToString();
                                string desc = oDataTable.GetValue("PrcName", 0).ToString();

                                try
                                {
                                    et_CodCentroCusto.Value = code;
                                }
                                catch { }
                                try
                                {
                                    et_DescCentroCusto.Value = desc;
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
