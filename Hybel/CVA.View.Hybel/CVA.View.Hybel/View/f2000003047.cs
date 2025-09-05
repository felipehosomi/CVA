using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using SAPbouiCOM;
using System;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Configuração E-mail
    /// </summary>
    public class f2000003047 : BaseForm
    {
        Form Form;
        public static string Path;

        #region Constructor
        public f2000003047()
        {
            FormCount++;
        }

        public f2000003047(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003047(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003047(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    EditText et_Code = (EditText)Form.Items.Item("et_Code").Specific;
                    et_Code.Value = CrudController.GetNextCode("@CVA_CONFIG_EMAIL");

                    EditText et_Password = (EditText)Form.Items.Item("et_Pass").Specific;
                    EditText et_Email = (EditText)Form.Items.Item("et_Email").Specific;
                    if (!String.IsNullOrEmpty(et_Password.Value) && !String.IsNullOrEmpty(et_Email.Value))
                    {
                        et_Password.Value = EncryptController.Encrypt(et_Password.Value, et_Email.Value);
                    }
                    else
                    {
                        SBOApp.Application.SetStatusBarMessage("E-mail e senha devem estar preenchidos");
                        return false;
                    }
                }
            }
            if (!BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
            {
                Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                EditText et_Password = (EditText)Form.Items.Item("et_Pass").Specific;
                EditText et_Email = (EditText)Form.Items.Item("et_Email").Specific;
                if (!String.IsNullOrEmpty(et_Password.Value) && !String.IsNullOrEmpty(et_Email.Value))
                {
                    et_Password.Value = EncryptController.Decrypt(et_Password.Value, et_Email.Value);
                }
            }

            return true;
        }
    }
}
