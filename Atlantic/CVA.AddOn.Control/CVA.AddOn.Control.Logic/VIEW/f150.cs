using SAPbouiCOM;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Item
    /// </summary>
    public class f150 : BaseFormView
    {
        #region Constructor
        public f150()
        {
            FormCount++;
        }

        public f150(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f150(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f150(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.Item;
            TableName = "OITM";
            CodeColumn = "ItemCode";

            CodeField = "5";
            FocusField = "7";

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.Item;
            TableName = "OITM";
            CodeColumn = "ItemCode";

            CodeField = "5";
            FocusField = "7";

            return base.FormDataEvent();
        }
        #endregion
    }
}
