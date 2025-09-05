namespace CVA.View.ObservacoesFiscais.VIEW
{
    /// <summary>
    /// Recebimento de Mercadoria
    /// </summary>
    public class f143 : DocumentBaseView
    {
        #region Constructor
        public f143()
        {
            FormCount++;
        }

        public f143(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f143(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f143(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
