using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Romaneio.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Romaneio.VIEW
{
    /// <summary>
    /// Tipo Veículo
    /// </summary>
    public class f2000002004 : BaseForm
    {
        #region Constructor
        public f2000002004()
        {
            FormCount++;
        }

        public f2000002004(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000002004(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000002004(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion
    }
}
