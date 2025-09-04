using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.VIEW
{
    public class f2000001006 : BaseForm
    {
        private Form Form;

        #region Constructor

        public f2000001006()
        {
            FormCount++;

        }

        public f2000001006(ItemEvent itemEvent)
        {

            this.ItemEventInfo = itemEvent;
        }

        public f2000001006(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000001006(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {

                    Form form = SBOApp.Application.Forms.ActiveForm;
                }
            }
            return true;
        }
    }
}

