using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// DE-PARA Montadora X Itens
    /// </summary>
    public class f2000003036 : BaseForm
    {
        #region Constructor
        public f2000003036()
        {
            FormCount++;
        }

        public f2000003036(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003036(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003036(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD)
            {
                ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_MONT_ITEM").PadLeft(8, '0');
            }
            return true;
        }
    }
}
