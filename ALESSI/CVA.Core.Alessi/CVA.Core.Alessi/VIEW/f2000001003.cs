using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.VIEW
{
    /// <summary>
    /// Valores default
    /// </summary>
    public class f2000001003 : BaseForm
    {
        #region Constructor
        public f2000001003()
        {
            FormCount++;
        }

        public f2000001003(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000001003(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000001003(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    //Form = SBOApp.Application.Forms.ActiveForm;
                    ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_IMP_DEFAULT").PadLeft(4, '0');
                }
            }
            
            return true;
        }
    }
}
