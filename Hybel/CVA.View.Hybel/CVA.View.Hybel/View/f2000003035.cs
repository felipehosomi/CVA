using CVA.AddOn.Common.Forms;
using SAPbouiCOM;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Função
    /// </summary>
    public class f2000003035 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000003035()
        {
            FormCount++;
        }

        public f2000003035(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003035(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003035(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
