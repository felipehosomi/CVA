namespace CVA.View.ObservacoesFiscais.VIEW
{
    /// <summary>
    /// Solicitação de compra
    /// </summary>
    class f1470000200 : DocumentBaseView
    {
        #region Constructor
        public f1470000200()
        {
            FormCount++;
        }

        public f1470000200(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f1470000200(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f1470000200(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
