using System;
using SAPbouiCOM.Framework;
using MenuPlanner.Controllers;
using SAPbouiCOM;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.Service", "Views/Service.b1f")]
    class Service : UserFormBase
    {
        public Service()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.CheckBox0 = ((SAPbouiCOM.CheckBox)(this.GetItem("ckActive").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("etCode").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("etName").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {
            if (!String.IsNullOrEmpty(CommonController.FormFatherType))
            {
                var fatherForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(CommonController.FormFatherType, CommonController.FormFatherCount);
                var plannerData = fatherForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                var srv = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_SERVICO_PLAN");

                var conditions = new Conditions();
                var condition = conditions.Add();
                condition.Alias = "Code";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = plannerData.GetValue("U_CVA_ID_SERVICO", plannerData.Offset);
                srv.Query(conditions);

                UIAPIRawForm.Mode = BoFormMode.fm_OK_MODE;

                CommonController.FormFatherType = String.Empty;
                CommonController.FormFatherCount = -1;
            }
        }

        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.CheckBox CheckBox0;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
    }
}
