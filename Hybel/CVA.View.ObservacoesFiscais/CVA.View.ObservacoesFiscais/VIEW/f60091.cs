namespace CVA.View.ObservacoesFiscais.VIEW
{
    /// <summary>
    /// NF Entrega Futura
    /// </summary>
    public class f60091 : DocumentBaseView
    {
        #region Constructor
        public f60091()
        {
            FormCount++;
        }

        public f60091(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f60091(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f60091(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
