using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Romaneio.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Romaneio.VIEW
{
    /// <summary>
    /// Romaneio - Wizard - Selecionar Ação
    /// </summary>
    public class f2000002001 : BaseForm
    {
        private Form Form;
        
        #region Constructor
        public f2000002001()
        {
            FormCount++;
        }

        public f2000002001(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000002001(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000002001(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion


        public object Show()
        {
            Form = (Form)base.Show();

            ((OptionBtn)Form.Items.Item("op_Edit").Specific).GroupWith("op_Add");
            ((OptionBtn)Form.Items.Item("op_Cancel").Specific).GroupWith("op_Add");
            ((OptionBtn)Form.Items.Item("op_Search").Specific).GroupWith("op_Add");

            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "bt_Next")
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    UserDataSource ud_Option = Form.DataSources.UserDataSources.Item("ud_Option");
                    if (!String.IsNullOrEmpty(ud_Option.Value))
                    {
                        WaybillActionEnum waybillActionEnum = (WaybillActionEnum)Convert.ToInt32(ud_Option.Value);

                        if (waybillActionEnum == WaybillActionEnum.Add)
                        {
                            new f2000002002().Show();
                        }
                        else
                        {
                            new f2000002003().Show(waybillActionEnum);
                        }
                        Form.Close();
                    }
                }
            }
            return true;
        }
    }
}
