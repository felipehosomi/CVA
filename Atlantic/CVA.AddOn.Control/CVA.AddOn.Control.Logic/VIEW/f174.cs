using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Grupo Cliente
    /// </summary>
    public class f174 : BaseFormView
    {
        #region Constructor
        public f174()
        {
            FormCount++;
        }

        public f174(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f174(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f174(ContextMenuInfo contextMenuInfo)
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
