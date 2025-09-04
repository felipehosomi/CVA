using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;

namespace CVA.View.Boleto.VIEW
{
    /// <summary>
    /// Forma de pagamento
    /// </summary>
    public class f505 : BaseForm
    {
        #region Constructor
        public f505()
        {
            FormCount++;
        }

        public f505(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f505(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f505(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            this.IsSystemForm = true;
            return base.ItemEvent();
        }
    }
}
