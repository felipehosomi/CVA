using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Código de Imposto
    /// </summary>
    public class f82105 : BaseFormView
    {
        #region Constructor
        public f82105()
        {
            FormCount++;
        }

        public f82105(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f82105(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f82105(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.CodigoImposto;
            TableName = "OSTC";
            CodeColumn = "Code";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.CodigoImposto;
            TableName = "OSTC";
            CodeColumn = "Code";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.FormDataEvent();
        }
        #endregion
    }
}
