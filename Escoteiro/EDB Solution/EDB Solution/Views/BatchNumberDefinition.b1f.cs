using SAPbouiCOM.Framework;

namespace EDB_Solution.Views
{
    [Form("41", "Views/BatchNumberDefinition.b1f")]
    class BatchNumberDefinition : SystemFormBase
    {
        public BatchNumberDefinition()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.ComboBox234000002 = ((SAPbouiCOM.ComboBox)(this.GetItem("234000002").Specific));
            this.ComboBox234000002.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.ComboBox234000002_ComboSelectAfter);

            ComboBox234000002.ValidValues.Add("30", "Criação automática global");
            this.OnCustomInitialize();
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {

        }

        private void OnCustomInitialize()
        {

        }

        private void ComboBox234000002_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (ComboBox234000002.Value == "30")
            {
                try
                {
                    var matrix35 = (SAPbouiCOM.Matrix)UIAPIRawForm.Items.Item("35").Specific;
                    var matrix3 = (SAPbouiCOM.Matrix)UIAPIRawForm.Items.Item("3").Specific;
                    var button1 = (SAPbouiCOM.Button)UIAPIRawForm.Items.Item("1").Specific;

                    for (var i = 0; i < matrix35.RowCount; i++)
                    {
                        matrix35.Columns.Item(1).Cells.Item(i + 1).Click(SAPbouiCOM.BoCellClickType.ct_Regular);

                        ((SAPbouiCOM.EditText)(matrix3.Columns.Item(2).Cells.Item(1).Specific)).Value = "1";

                        if (button1.Caption == "OK") continue;

                        button1.Item.Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                    }

                    Application.SBO_Application.StatusBar.SetText("Criação automática dos lotes realizada.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                }
                catch
                {

                }
            }
        }

        private SAPbouiCOM.ComboBox ComboBox234000002;
    }
}
