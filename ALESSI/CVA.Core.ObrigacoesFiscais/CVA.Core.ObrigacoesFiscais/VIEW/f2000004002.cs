using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.Core.ObrigacoesFiscais.DAO.Resources;
using SAPbouiCOM;
using System;

namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    /// <summary>
    /// Exclusão de modelos
    /// </summary>
    public class f2000004002 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000004002()
        {
            FormCount++;
        }

        public f2000004002(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000004002(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000004002(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            ComboBox cb_Model = (ComboBox)Form.Items.Item("cb_Model").Specific;
            cb_Model.AddValuesFromQuery(Query.NFModel_Get);

            return Form;
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_MODEL_EX").PadLeft(4, '0');
                }
            }
            
            return true;
        }
    }
}
