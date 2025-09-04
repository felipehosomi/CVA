using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using SAPbouiCOM;

namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    /// <summary>
    /// Cadastro de Layout de Arquivo
    /// </summary>
    public class f2000004000 : BaseForm
    {
        #region Constructor
        public f2000004000()
        {
            FormCount++;
        }

        public f2000004000(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000004000(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000004000(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_CLICK)
            {
                if (ItemEventInfo.ItemUID == "bt_Dir")
                {
                    EditText et_Dir = Form.Items.Item("et_Dir").Specific as EditText;

                    DialogUtil dialogUtil = new DialogUtil();
                    et_Dir.Value = dialogUtil.FolderBrowserDialog(et_Dir.Value);
                }
            }
            return true;
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
            {
                ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_LAYOUT").PadLeft(4, '0');
            }
            return true;
        }
    }
}
