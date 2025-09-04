using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.Dctf.BLL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Dctf.VIEW
{
    public class f2000005050 : BaseForm
    {
        private Form Form { get; set; }

        #region Constructor
        public f2000005050()
        {
            FormCount++;
        }

        public f2000005050(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000005050(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000005050(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            Form.PaneLevel = 1;
            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    if (ItemEventInfo.ItemUID == "bt_Dir")
                    {
                        EditText et_Dir = (EditText)Form.Items.Item("et_Dir").Specific;
                        DialogUtil dialog = new DialogUtil();
                        et_Dir.Value = dialog.FolderBrowserDialog();
                    }
                    if (ItemEventInfo.ItemUID == "bt_Gen")
                    {
                        if (Form.Mode == BoFormMode.fm_ADD_MODE || Form.Mode == BoFormMode.fm_UPDATE_MODE)
                        {
                            SBOApp.Application.SetStatusBarMessage("Por favor, salve os dados antes de continuar!");
                        }
                        else
                        {
                            DctfFileBLL dctfFileBLL = new DctfFileBLL();
                            string error = dctfFileBLL.GenerateFile(Form.DataSources.DBDataSources.Item("@CVA_DCTF"));
                            if (!String.IsNullOrEmpty(error))
                            {
                                SBOApp.Application.SetStatusBarMessage(error);
                            }
                            else
                            {
                                SBOApp.Application.SetStatusBarMessage("Arquivo gerado com sucesso!", BoMessageTime.bmt_Medium, false);
                            }
                        }
                    }
                }
            }
            return true;
        }

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_DCTF").PadLeft(4, '0');
                }
            }
            return true;
        }
        #endregion
    }
}
