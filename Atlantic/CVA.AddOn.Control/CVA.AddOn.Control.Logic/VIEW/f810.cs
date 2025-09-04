using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Centro de Custo
    /// </summary>
    public class f810 : BaseFormView
    {
        #region Constructor
        public f810()
        {
            FormCount++;
        }

        public f810(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f810(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f810(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.CentroCusto;
            TableName = "OPRC";
            CodeColumn = "PrcCode";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.CentroCusto;
            TableName = "OPRC";
            CodeColumn = "PrcCode";

            CodeField = System.String.Empty;
            FocusField = System.String.Empty;

            return base.FormDataEvent();
        }
        #endregion
    }
}
