namespace CVA.View.ObservacoesFiscais.VIEW
{
    /// <summary>
    /// Pedido de compra
    /// </summary>
    public class f142 : DocumentBaseView
    {
        #region Constructor
        public f142()
        {
            FormCount++;
        }

        public f142(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f142(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f142(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
