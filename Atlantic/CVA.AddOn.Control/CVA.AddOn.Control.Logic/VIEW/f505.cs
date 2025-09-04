using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Forma de Pagamento
    /// </summary>
    public class f505 : BaseFormView
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

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.FormaPagamento;
            TableName = "OPYM";
            CodeColumn = "PayMethCod";

            CodeField = "5";
            FocusField = "6";

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.FormaPagamento;
            TableName = "OPYM";
            CodeColumn = "PayMethCod";

            CodeField = "5";
            FocusField = "6";

            if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
            {
                return true;
            }
            
            return base.FormDataEvent();
        }
        #endregion
    }
}
