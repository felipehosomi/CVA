using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using SAPbouiCOM;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.MealTypes", "Views/MealTypes.b1f")]
    class MealTypes : UserFormBase
    {
        public MealTypes()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.ckProtein = ((SAPbouiCOM.CheckBox)(this.GetItem("ckProtein").Specific));
            this.etCode = ((SAPbouiCOM.EditText)(this.GetItem("etCode").Specific));
            this.etName = ((SAPbouiCOM.EditText)(this.GetItem("etName").Specific));
            this.etFmlCode = ((SAPbouiCOM.EditText)(this.GetItem("etFmlCode").Specific));
            this.etFmlCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etFmlCode_ChooseFromListAfter);
            this.etSFmlCode = ((SAPbouiCOM.EditText)(this.GetItem("etSFmlCode").Specific));
            this.etSFmlCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etSFmlCode_ChooseFromListAfter);
            this.etFmlName = ((SAPbouiCOM.EditText)(this.GetItem("etFmlName").Specific));
            this.etFmlName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etFmlName_ChooseFromListAfter);
            this.etSFmlName = ((SAPbouiCOM.EditText)(this.GetItem("etSFmlName").Specific));
            this.etSFmlName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etSFmlName_ChooseFromListAfter);
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

        }

        private void etFmlCode_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");

            dbDataSource.SetValue("U_CVA_D_FAMILIA", dbDataSource.Offset, dataTable.GetValue("Name", 0).ToString());
        }

        private void etFmlName_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");

            dbDataSource.SetValue("U_CVA_FAMILIA", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }

        private void etSFmlCode_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");

            dbDataSource.SetValue("U_CVA_D_SUB_FAMILIA", dbDataSource.Offset, dataTable.GetValue("Name", 0).ToString());
        }

        private void etSFmlName_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");

            dbDataSource.SetValue("U_CVA_SUB_FAMILIA", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }

        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.CheckBox ckProtein;
        private SAPbouiCOM.EditText etCode;
        private SAPbouiCOM.EditText etName;
        private SAPbouiCOM.EditText etFmlCode;
        private SAPbouiCOM.EditText etSFmlCode;
        private SAPbouiCOM.EditText etFmlName;
        private SAPbouiCOM.EditText etSFmlName;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
    }
}
