using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Condição de Pagamento
    /// </summary>
    public class f177 : BaseFormView
    {
        #region Constructor
        public f177()
        {
            FormCount++;
        }

        public f177(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f177(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f177(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.CondicaoPagamento;
            TableName = "OCTG";
            CodeColumn = "GroupNum";

            CodeField = "3";
            FocusField = "39";

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.CondicaoPagamento;
            TableName = "OCTG";
            CodeColumn = "GroupNum";

            CodeField = "3";
            FocusField = "39";

            return base.FormDataEvent();
        }
        #endregion
    }
}
