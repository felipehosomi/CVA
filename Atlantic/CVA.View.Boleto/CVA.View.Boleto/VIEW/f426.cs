using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Boleto.BLL;
using SAPbouiCOM;


namespace CVA.View.Boleto.VIEW
{
    /// <summary>
    /// Contas a pagar
    /// </summary>
    public class f426 : BaseForm
    {
        #region Constructor
        public f426()
        {
            FormCount++;
        }

        public f426(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f426(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f426(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
