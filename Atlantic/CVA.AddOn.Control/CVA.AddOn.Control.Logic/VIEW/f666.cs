using CVA.AddOn.Control.Logic.MODEL;
using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Vendedores/Compradores
    /// </summary>
    public class f666 : BaseFormView
    {
        #region Constructor
        public f666()
        {
            FormCount++;
        }

        public f666(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f666(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f666(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = true;
            ObjectType = CVAObjectEnum.VendedoresCompradores;
            TableName = "OSLP";
            CodeColumn = "SlpCOde";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = true;
            ObjectType = CVAObjectEnum.VendedoresCompradores;
            TableName = "OSLP";
            CodeColumn = "SlpCOde";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.FormDataEvent();
        }
        #endregion
    }
}
