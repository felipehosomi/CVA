namespace CVA.View.ObservacoesFiscais.VIEW
{
    /// <summary>
    /// Pedido Venda
    /// </summary>
    public class f139 : DocumentBaseView
    {
        #region Constructor
        public f139()
        {
            FormCount++;
        }

        public f139(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f139(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f139(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
