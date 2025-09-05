using SAPbouiCOM;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Nota fiscal de saída
    /// </summary>
    public class f133 : DocumentBaseView
    {
        #region Constructor
        public f133()
        {
            FormCount++;
        }

        public f133(ItemEvent itemEvent)
        {
            this.IsSystemForm = true;
            this.ItemEventInfo = itemEvent;
        }

        public f133(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f133(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}

