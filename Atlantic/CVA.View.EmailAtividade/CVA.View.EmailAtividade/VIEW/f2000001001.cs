using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.EmailAtividade.BLL;
using SAPbouiCOM;
using System;

namespace CVA.View.EmailAtividade.VIEW
{
    public class f2000001001 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000001001()
        {
            FormCount++;
        }

        public f2000001001(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000001001(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000001001(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        //public override object Show()
        //{
        //    Form = (Form)base.Show();

        //    ComboBox cb_BPlId = (ComboBox)Form.Items.Item("cb_BPlId").Specific;
        //    BranchBLL branchBLL = new BranchBLL();
        //    List<BranchModel> branchList = branchBLL.GetBranches();
        //    foreach (var item in branchList)
        //    {
        //        cb_BPlId.ValidValues.Add(item.BPlId.ToString(), item.BPlName);
        //    }

        //    return Form;
        //}

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_CLICK)
            {
                if (ItemEventInfo.ItemUID == "bt_Help")
                {
                    f2000001002 fr_Tags = new f2000001002();
                    fr_Tags.Show();
                }
            }

            return base.ItemEvent();
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;
                    EditText et_Code = (EditText)Form.Items.Item("et_Code").Specific;
                    et_Code.Value = EmailActivityBLL.GetNextCode();

                    EditText et_Password = (EditText)Form.Items.Item("et_Pass").Specific;
                    EditText et_Email = (EditText)Form.Items.Item("et_Email").Specific;
                    if (!String.IsNullOrEmpty(et_Password.Value) && !String.IsNullOrEmpty(et_Email.Value))
                    {
                        et_Password.Value = EncryptBLL.Encrypt(et_Password.Value, et_Email.Value);
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
                Form = SBOApp.Application.Forms.ActiveForm;
                EditText et_Password = (EditText)Form.Items.Item("et_Pass").Specific;
                EditText et_Email = (EditText)Form.Items.Item("et_Email").Specific;
                if (!String.IsNullOrEmpty(et_Password.Value) && !String.IsNullOrEmpty(et_Email.Value))
                {
                    et_Password.Value = EncryptBLL.Decrypt(et_Password.Value, et_Email.Value);
                }
            }

            return true;
        }
    }
}
