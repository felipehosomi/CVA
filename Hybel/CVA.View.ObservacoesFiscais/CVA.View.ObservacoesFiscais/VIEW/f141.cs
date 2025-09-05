namespace CVA.View.ObservacoesFiscais.VIEW
{
    /// <summary>
    /// Nota Fiscal Entrada
    /// </summary>
    public class f141 : DocumentBaseView
    {
        #region Constructor
        public f141()
        {
            FormCount++;
        }

        public f141(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f141(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f141(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
