namespace CVA.View.ObservacoesFiscais.VIEW
{
    /// <summary>
    /// Devolução
    /// </summary>
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
