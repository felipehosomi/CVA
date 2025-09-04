using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.Romaneio.DAO.Resources;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Romaneio.VIEW
{
    /// <summary>
    /// Veículo
    /// </summary>
    public class f2000002005 : BaseForm
    {
        Form Form;

        #region Constructor
        public f2000002005()
        {
            FormCount++;
        }

        public f2000002005(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000002005(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000002005(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public object Show()
        {
            Form = (Form)base.Show();
                        
            ComboBox cb_VehType = (ComboBox)Form.Items.Item("cb_VehType").Specific;
            cb_VehType.AddValuesFromQuery(Query.VehicleType_Get, "Code", "Name");

            return Form;
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;
                    
                    ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_VEHICLE").PadLeft(8, '0');
                }
            }
            return true;
        }
    }
}
