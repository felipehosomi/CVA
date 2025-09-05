namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class f140 : DocumentBaseView
    {
        #region Constructor
        public f140()
        {
            FormCount++;
        }

        public f140(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f140(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f140(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
