using MenuConsolidator.Extensions;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace MenuConsolidator.Views
{
    [FormAttribute("MenuConsolidator.Views.StorageCategory", "Views/StorageCategory.b1f")]
    class StorageCategory : UserFormBase
    {
        private string RowsDataSource = "@CVA_SCT1";
        private static int _menuRow;

        public StorageCategory()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtCat = ((SAPbouiCOM.Matrix)(this.GetItem("mtCat").Specific));
            this.mtCat.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtCat_ValidateAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataAddBefore += new SAPbouiCOM.Framework.FormBase.DataAddBeforeHandler(this.Form_DataAddBefore);
            this.DataAddAfter += new SAPbouiCOM.Framework.FormBase.DataAddAfterHandler(this.Form_DataAddAfter);
            this.DataUpdateBefore += new SAPbouiCOM.Framework.FormBase.DataUpdateBeforeHandler(this.Form_DataUpdateBefore);
            this.DataUpdateAfter += new SAPbouiCOM.Framework.FormBase.DataUpdateAfterHandler(this.Form_DataUpdateAfter);
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);
            this.RightClickBefore += new RightClickBeforeHandler(this.Form_RightClickBefore);
            this.RightClickAfter += new RightClickAfterHandler(this.Form_RightClickAfter);
        }

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;

            UIAPIRawForm.EnableMenu("1281", false);
            UIAPIRawForm.EnableMenu("1282", false);

            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
            dtQuery.ExecuteQuery(@"select * from ""@CVA_OSCT""");

            if (!dtQuery.IsEmpty)
            {
                UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;

                dtQuery.ExecuteQuery(@"select max(""DocEntry"") as ""DocEntry"" from ""@CVA_OSCT""");

                var osct = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OSCT");
                var sct1 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_SCT1");
                var conditions = new SAPbouiCOM.Conditions();
                var condition = conditions.Add();
                condition.Alias = "Code";
                condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                condition.CondVal = dtQuery.GetValue("DocEntry", 0).ToString();
                osct.Query(conditions);
                sct1.Query(conditions);
            }

            InsertNewRow();
        }

        #region [ Menu Events ]
        private void MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.MenuUID != "RemoveRow") return;
            if (UIAPIRawForm == null) return;
            if (UIAPIRawForm.UniqueID != SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.UniqueID) return;

            if (!pVal.BeforeAction)
            {
                switch (pVal.MenuUID)
                {
                    case "RemoveRow":
                        RemoveRow();
                        break;
                }
            }
        }
        #endregion

        #region [ Item Events ]
        private void mtCat_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

            if (pVal.ItemChanged && pVal.Row - 1 == dataSource.Offset)
            {
                mtCat.FlushToDataSource();
                InsertNewRow();
            }
        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            mtCat.AutoResizeColumns();
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtCat")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtCat")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = false;
                _menuRow = -1;
            }
        }
        #endregion

        #region [ Form Data Events ]
        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var osct = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OSCT");
            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");

            dtQuery.Clear();
            dtQuery.ExecuteQuery(@"select coalesce(max(""DocEntry""), 0) + 1 as ""DocEntry"" from ""@CVA_OSCT""");
            osct.SetValue("Code", osct.Offset, dtQuery.GetValue("DocEntry", 0).ToString());

            RemoveLastRow();
        }

        private void Form_DataAddAfter(ref BusinessObjectInfo pVal)
        {
            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
            dtQuery.ExecuteQuery(@"select max(""DocEntry"") as ""DocEntry"" from ""@CVA_OSCT""");

            var osct = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OSCT");
            var sct1 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_SCT1");
            var conditions = new SAPbouiCOM.Conditions();
            var condition = conditions.Add();
            condition.Alias = "Code";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            condition.CondVal = dtQuery.GetValue("DocEntry", 0).ToString();
            osct.Query(conditions);
            sct1.Query(conditions);

            InsertNewRow();
        }

        private void Form_DataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            RemoveLastRow();
        }

        private void Form_DataUpdateAfter(ref BusinessObjectInfo pVal)
        {
            InsertNewRow();
        }

        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            InsertNewRow();
        }
        #endregion

        #region [ Auxiliar Methods ]
        private void InsertNewRow()
        {
            UIAPIRawForm.Freeze(true);

            mtCat.LoadFromDataSourceEx(false);

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            dataSource.InsertRow(mtCat.RowCount == 0);

            mtCat.LoadFromDataSourceEx(false);
            mtCat.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        private void RemoveRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

            mtCat.FlushToDataSource();
            dataSource.RemoveRecord(_menuRow);
            mtCat.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private void RemoveLastRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            dataSource.RemoveRecord(dataSource.Offset);
        }
        #endregion

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Matrix mtCat;
    }
}
