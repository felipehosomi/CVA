using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Grupo Fornecedor
    /// </summary>
    public class f247 : BaseFormView
    {
        #region Constructor
        public f247()
        {
            FormCount++;
        }

        public f247(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f247(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f247(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = true;
            ObjectType = MODEL.CVAObjectEnum.GrupoParceiroNegocio;
            TableName = "OCRG";
            CodeColumn = "GroupCode";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = true;
            ObjectType = MODEL.CVAObjectEnum.GrupoParceiroNegocio;
            TableName = "OCRG";
            CodeColumn = "GroupCode";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.FormDataEvent();
        }
        #endregion
    }
}
