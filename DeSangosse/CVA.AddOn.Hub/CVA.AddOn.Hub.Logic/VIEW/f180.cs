namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class f180 : DocumentBaseView
    {
        #region Constructor
        public f180()
        {
            FormCount++;
        }

        public f180(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f180(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f180(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
