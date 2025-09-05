using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using SAPbouiCOM;
using System;
using System.IO;

namespace CVA.Core.Escoteiros.View
{
    //[CVA.AddOn.Common.Attributes.Form(3047)]
   public class f1001 : BaseForm
    {
        Form Form;
        public static string Path;

        #region Constructor
        public f1001()
        {
            FormCount++;
        }

        public f1001(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f1001(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f1001(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_DOUBLE_CLICK)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;

                    ((EditText)Form.Items.Item("tx_XML").Specific).DoubleClickAfter += F1001_DoubleClickAfter;
                }
            }
            return true;
        }

        private void F1001_DoubleClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                EditText et_File = ((EditText)Form.Items.Item("tx_XML").Specific);
                if (string.IsNullOrEmpty(et_File.Value))
                {
                    DialogUtil dialog = new DialogUtil();
                    string file_From = dialog.OpenFileDialog();

                    FileInfo oFileInfo = new FileInfo(file_From);

                    string Arq = oFileInfo.Name;
                    string File_To = Arq;

                    et_File.Value = File_To;

                }
                else
                {
                    System.Diagnostics.Process.Start(et_File.Value);
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message.ToString(), BoMessageTime.bmt_Short, false);
            }
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    EditText et_Code = (EditText)Form.Items.Item("et_Code").Specific;
                    et_Code.Value = CrudController.GetNextCode("@CVA_ParGerais");

                }
            }

            if (!BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
            {
                Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                ComboBox cb_Ante = (ComboBox)Form.Items.Item("cb_P_Ante").Specific;
                ComboBox cb_Fat = (ComboBox)Form.Items.Item("cb_P_Fat").Specific;
                EditText et_xml = (EditText)Form.Items.Item("tx_XML").Specific;               
            }
            return true;
        }

        private void EnableMenus(SAPbouiCOM.Form oForm, bool enable)
        {
            oForm.EnableMenu("1281", enable); //Find Record
            oForm.EnableMenu("1282", enable); //Add New Record
            oForm.EnableMenu("1288", enable); //Next Record
            oForm.EnableMenu("1289", enable); //Previous Record
            oForm.EnableMenu("1290", enable); //Fist Record
            oForm.EnableMenu("1291", enable); //Last Record
        }

    }
}
