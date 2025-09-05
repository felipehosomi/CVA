namespace CVA.View.ObservacoesFiscais.VIEW
{
    /// <summary>
    /// NF Recebimento Futuro
    /// </summary>
    public class f60092 : DocumentBaseView
    {
        #region Constructor
        public f60092()
        {
            FormCount++;
        }

        public f60092(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f60092(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f60092(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
