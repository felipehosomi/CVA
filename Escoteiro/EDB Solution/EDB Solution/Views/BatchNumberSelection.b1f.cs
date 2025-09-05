using SAPbouiCOM.Framework;

namespace EDB_Solution.Views
{
    [Form("42", "Views/BatchNumberSelection.b1f")]
    class BatchNumberSelection : SystemFormBase
    {
        public BatchNumberSelection()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.btGlbSel = ((SAPbouiCOM.Button)(this.GetItem("btGlbSel").Specific));
            this.btGlbSel.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
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

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                var matrix3 = (SAPbouiCOM.Matrix)UIAPIRawForm.Items.Item("3").Specific;
                var matrix4 = (SAPbouiCOM.Matrix)UIAPIRawForm.Items.Item("4").Specific;
                var button1 = (SAPbouiCOM.Button)UIAPIRawForm.Items.Item("1").Specific;
                var button11 = (SAPbouiCOM.Button)UIAPIRawForm.Items.Item("11").Specific;
                var button16 = (SAPbouiCOM.Button)UIAPIRawForm.Items.Item("16").Specific;

                for (var i = 0; i < matrix3.RowCount; i++)
                {
                    matrix3.Columns.Item(1).Cells.Item(i + 1).Click(SAPbouiCOM.BoCellClickType.ct_Regular);

                    if (matrix4.RowCount == 0)
                    {
                        button11.Item.Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                        continue;
                    }

                    button16.Item.Click(SAPbouiCOM.BoCellClickType.ct_Regular);

                    if (button1.Caption == "OK") continue;

                    button1.Item.Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                }

                Application.SBO_Application.StatusBar.SetText("Seleção automática dos lotes realizada.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            }
            catch
            {

            }
        }

        private SAPbouiCOM.Button btGlbSel;
    }
}
