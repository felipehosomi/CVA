using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.View
{
    //[CVA.AddOn.Common.Attributes.Form(3048)]
    public class f1002 : BaseForm
    {
        Form Form;
        Form _Form;
        public static string Path;
        
        #region Constructor
        public f1002()
        {
            FormCount++;
        }

        public f1002(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f1002(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f1002(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            EnableMenus(Form, true);

            ((ComboBox)Form.Items.Item("cb_Tp_WS").Specific).AddValuesFromQuery(DAO.Query.GetValues);
            

            return Form;
        }



        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                Form = SBOApp.Application.Forms.GetForm(ItemEventInfo.FormTypeEx, ItemEventInfo.FormTypeCount);                

                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST && ItemEventInfo.ItemUID == "tx_Transp")
                {                    
                     this.ChooseFromListBPName();
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS && ItemEventInfo.ItemUID == "tx_Transp")
                {

                    EditText et_Razao = (EditText)Form.Items.Item("tx_pn").Specific;
                    et_Razao.Value = Form.DataSources.UserDataSources.Item("ud_Desc").Value;
                }
            }
            return true;
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    EditText et_Code = (EditText)Form.Items.Item("et_code").Specific;
                    et_Code.Value = CrudController.GetNextCode("@CVA_CfgDespacho");



                }
            }

            if (!BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
            {
                Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);

            }
            return true;
        }

        private void EnableMenus(SAPbouiCOM.Form oForm, bool enable)
        {
            oForm.EnableMenu("1281", enable); //Find Record
            oForm.EnableMenu("1282", enable); //Add New Record
            oForm.EnableMenu("1288", enable); //Next Record
            oForm.EnableMenu("1289", enable); //Previous Record
            oForm.EnableMenu("1290", enable); //Fist Record
            oForm.EnableMenu("1291", enable); //Last Record
        }

        private void ChooseFromListBPName()
        {
            

            IChooseFromListEvent oCFLEvento = ((IChooseFromListEvent)(ItemEventInfo));
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvento.ChooseFromListUID);
            DataTable oDataTable = oCFLEvento.SelectedObjects;

            if (oDataTable != null && oDataTable.Rows.Count > 0)
            {
                // Tranpostadora
                string cardCode = oDataTable.GetValue("CardCode", 0).ToString();
                Form.DataSources.UserDataSources.Item("ud_Cod").Value = cardCode;

                // Razão Social
                string cardCardName = oDataTable.GetValue("CardName", 0).ToString();
                Form.DataSources.UserDataSources.Item("ud_Desc").Value = cardCardName;
            }

        }

    }
}
