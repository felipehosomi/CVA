using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.View.EmailAtividade.BLL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAtividade.VIEW
{
    public class f651 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f651()
        {
            FormCount++;
        }

        public f651(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f651(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f651(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
            {
                string clgCode = DocumentXmlController.GetXmlField(BusinessObjectInfo, "ContactCode");
                EmailActivityBLL emailActivityBLL = new EmailActivityBLL();
                string msg = emailActivityBLL.SendEmail(Convert.ToInt32(clgCode));
                if (!String.IsNullOrEmpty(msg))
                {
                    SBOApp.Application.MessageBox(msg);
                }
            }
            return true;
        }
    }
}
