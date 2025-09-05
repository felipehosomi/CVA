using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Nota fiscal entrada
    /// </summary>
    public class f141 : BaseForm
    {
        #region Constructor
        public f141()
        {
            FormCount++;
        }

        public f141(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.IsSystemForm = true;
            this.ItemEventInfo = itemEvent;

            //  SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(MenuEvent);

        }

        public f141(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f141(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        #endregion

        public override bool ItemEvent()
        {
            base.ItemEvent();
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    Form.Items.Item("bt_Desp").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("bt_Desp").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("bt_Desp").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Desp")
                    {
                        DBDataSource dt_OPCH = Form.DataSources.DBDataSources.Item("OPCH");
                        var modelo = dt_OPCH.GetValue("Model", dt_OPCH.Offset).ToString();
                        var docNum = dt_OPCH.GetValue("DocNum", dt_OPCH.Offset).ToString();

                        if (modelo.Equals("44"))
                        {
                            SBOApp.Application.MessageBox($"Nota modelo Fiscal CTe: {docNum} não pode ser de despesas de importação");
                            Form.Items.Item("bt_Desp").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                            return false;

                        }

                        new f2000003042().Show(((EditText)Form.Items.Item("8").Specific).Value);
                    }
                }
            }

            return true;
        }

    }
}
