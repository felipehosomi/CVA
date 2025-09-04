using SAPbouiCOM.Framework;
using SAPbouiCOM;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.ProteinTypes", "Views/ProteinTypes.b1f")]
    class ProteinTypes : UserFormBase
    {
        public ProteinTypes()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.etName = ((SAPbouiCOM.EditText)(this.GetItem("etName").Specific));
            this.etFmlName = ((SAPbouiCOM.EditText)(this.GetItem("etFmlName").Specific));
            this.etFmlName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etFmlName_ChooseFromListAfter);
            this.etSFmlName = ((SAPbouiCOM.EditText)(this.GetItem("etSFmlName").Specific));
            this.etSFmlName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etSFmlName_ChooseFromListAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Item_0").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataAddBefore += new DataAddBeforeHandler(this.Form_DataAddBefore);

        }

        private void OnCustomInitialize()
        {

        }

        private void etFmlName_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPROTEINA");

            dbDataSource.SetValue("U_CVA_FAMILIA", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }

        private void etSFmlName_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPROTEINA");

            dbDataSource.SetValue("U_CVA_SUB_FAMILIA", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }

        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPROTEINA");
            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");

            dtQuery.Clear();
            dtQuery.ExecuteQuery(@"select coalesce(max(""DocEntry""), 0) + 1 as ""DocEntry"" from ""@CVA_TIPOPROTEINA""");

            dataSource.SetValue("Code", dataSource.Offset, dtQuery.GetValue("DocEntry", 0).ToString());
        }

        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText etName;
        private SAPbouiCOM.EditText etFmlName;
        private SAPbouiCOM.EditText etSFmlName;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private Matrix Matrix0;
    }
}
