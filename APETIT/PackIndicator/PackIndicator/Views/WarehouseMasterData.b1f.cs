using SAPbouiCOM.Framework;

namespace PackIndicator.Views
{
    [FormAttribute("62", "Views/WarehouseMasterData.b1f")]
    class WarehouseMasterData : SystemFormBase
    {
        private bool LazyProcess = false;

        public WarehouseMasterData()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.cmUMCntrl = ((SAPbouiCOM.CheckBox)(this.GetItem("cmUMCntrl").Specific));
            this.cmUMCntrl.PressedAfter += new SAPbouiCOM._ICheckBoxEvents_PressedAfterEventHandler(this.cmUMCntrl_PressedAfter);
            this.etDueLimit = ((SAPbouiCOM.EditText)(this.GetItem("etDueLimit").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.ActivateAfter += new ActivateAfterHandler(this.Form_ActivateAfter);

        }

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;

            cmUMCntrl.DataBind.SetBound(true, "OWHS", "U_CVA_UomControl");
            etDueLimit.DataBind.SetBound(true, "OWHS", "U_CVA_DueDateLimit");
            etDueLimit.Item.Enabled = false;
        }

        #region [ Menu Events ]
        private void MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.MenuUID != "1282") return;
            if (UIAPIRawForm == null) return;
            if (UIAPIRawForm.UniqueID != SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.UniqueID) return;

            if (!pVal.BeforeAction)
            {
                switch (pVal.MenuUID)
                {
                    case "1282":
                        etDueLimit.Item.Enabled = false;
                        break;
                }
            }
        }
        #endregion

        #region [ Item Events ]
        private void cmUMCntrl_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("OWHS");
            etDueLimit.Item.Enabled = dataSource.GetValue("U_CVA_UomControl", dataSource.Offset) == "Y";
        }

        private void Form_ActivateAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (!LazyProcess) return;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("OWHS");
            etDueLimit.Item.Enabled = dataSource.GetValue("U_CVA_UomControl", dataSource.Offset) == "Y";
            LazyProcess = false;
        }
        #endregion

        #region [ Form Data Events ]
        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            try
            {
                var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("OWHS");
                etDueLimit.Item.Enabled = dataSource.GetValue("U_CVA_UomControl", dataSource.Offset) == "Y";
            }
            catch
            {
                LazyProcess = true;
            }
        }
        #endregion

        private SAPbouiCOM.CheckBox cmUMCntrl;
        private SAPbouiCOM.EditText etDueLimit;
    }
}
