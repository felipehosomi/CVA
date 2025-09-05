using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.EDoc.DAO;
using SAPbouiCOM;

namespace CVA.View.EDoc.View
{
    [CVA.AddOn.Common.Attributes.Form(6011)]
    public class FrmContribuinte : BaseForm
    {
        Form Form;

        #region Constructor
        public FrmContribuinte()
        {
            FormCount++;
        }

        public FrmContribuinte(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public FrmContribuinte(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public FrmContribuinte(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show(string srfPath)
        {
            Form = (Form)base.Show(srfPath);
            ((ComboBox)Form.Items.Item("cb_Filial").Specific).AddValuesFromQuery(SQL.Filial_Get);

            return Form;
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
            {
                Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                ((EditText)Form.Items.Item("et_Code").Specific).Value = CrudController.GetNextCode("@CVA_EDOC_COMPLEM");
            }
            return true;
        }
    }
}
