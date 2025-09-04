using PackIndicator.Extensions;
using SAPbouiCOM.Framework;
using System;

namespace PackIndicator.Views
{
    [FormAttribute("PackIndicator.Views.CustomerPrioritization", "Views/CustomerPrioritization.b1f")]
    class CustomerPrioritization : UserFormBase
    {
        private string HeaderDataSource = "@CVA_OBPP";
        private string RowsDataSource = "@CVA_BPP1";
        private static int _menuRow;

        public CustomerPrioritization()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtPriority = ((SAPbouiCOM.Matrix)(this.GetItem("mtPriority").Specific));
            this.mtPriority.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtPriority_ChooseFromListAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
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

        }

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;

            UIAPIRawForm.EnableMenu("1281", false);
            UIAPIRawForm.EnableMenu("1282", false);

            var cflCardCode = UIAPIRawForm.ChooseFromLists.Item("cflCardCode");
            var cflCardName = UIAPIRawForm.ChooseFromLists.Item("cflCardName");

            var conditions = new SAPbouiCOM.Conditions();
            var condition = conditions.Add();
            condition.Alias = "CardType";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            condition.CondVal = "C";

            cflCardCode.SetConditions(conditions);
            cflCardName.SetConditions(conditions);

            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
            dtQuery.ExecuteQuery($@"select * from ""{HeaderDataSource}""");

            if (!dtQuery.IsEmpty)
            {
                UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;

                dtQuery.ExecuteQuery($@"select max(""DocEntry"") as ""DocEntry"" from ""{HeaderDataSource}""");

                var header = UIAPIRawForm.DataSources.DBDataSources.Item(HeaderDataSource);
                var rows = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
                conditions = new SAPbouiCOM.Conditions();
                condition = conditions.Add();
                condition.Alias = "Code";
                condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                condition.CondVal = dtQuery.GetValue("DocEntry", 0).ToString();
                header.Query(conditions);
                rows.Query(conditions);
            }

            mtPriority.AutoResizeColumns();
            InsertNewRow();
        }

        #region [ Menu Events ]
        private void MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
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
        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            mtPriority.AutoResizeColumns();
        }

        private void mtPriority_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                var chooseFromListEvent = (SAPbouiCOM.SBOChooseFromListEventArg)pVal;
                var dataTable = chooseFromListEvent.SelectedObjects;

                if (dataTable == null) return;

                var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

                mtPriority.FlushToDataSource();

                switch (pVal.ColUID)
                {
                    case "WhsCode":
                        dbDataSource.SetValue("U_WhsCode", pVal.Row - 1, dataTable.GetValue("WhsCode", 0).ToString());
                        break;

                    case "CardCode":
                    case "CardName":
                        dbDataSource.SetValue("U_CardCode", pVal.Row - 1, dataTable.GetValue("CardCode", 0).ToString());
                        dbDataSource.SetValue("U_CardName", pVal.Row - 1, dataTable.GetValue("CardName", 0).ToString());
                        dbDataSource.SetValue("U_Address", pVal.Row - 1, dataTable.GetValue("ShipToDef", 0).ToString());
                        break;
                }

                if (pVal.Row == dbDataSource.Size)
                {
                    dbDataSource.InsertRow();
                }

                mtPriority.LoadFromDataSourceEx();
                mtPriority.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message);
                throw;
            }
        }

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE) return;

            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
            dtQuery.ExecuteQuery($@"select max(""DocEntry"") as ""DocEntry"" from ""{HeaderDataSource}""");

            var header = UIAPIRawForm.DataSources.DBDataSources.Item(HeaderDataSource);
            var rows = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            var conditions = new SAPbouiCOM.Conditions();
            var condition = conditions.Add();
            condition.Alias = "Code";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            condition.CondVal = dtQuery.GetValue("DocEntry", 0).ToString();
            header.Query(conditions);
            rows.Query(conditions);

            InsertNewRow();

            UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
            UIAPIRawForm.Freeze(false);
        }
        #endregion

        #region [ Form Data Events ]
        private void Form_DataAddBefore(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var header = UIAPIRawForm.DataSources.DBDataSources.Item(HeaderDataSource);
            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");

            dtQuery.Clear();
            dtQuery.ExecuteQuery($@"alter sequence ""U_CVA_OBPP_S"" restart with 1");
            dtQuery.ExecuteQuery($@"select coalesce(max(""DocEntry""), 0) + 1 as ""DocEntry"" from ""{HeaderDataSource}""");
            header.SetValue("Code", header.Offset, dtQuery.GetValue("DocEntry", 0).ToString());

            RemoveLastRow();

            UIAPIRawForm.Freeze(true);
        }

        private void Form_DataAddAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            
        }

        private void Form_DataUpdateBefore(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            RemoveLastRow();
        }

        private void Form_DataUpdateAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            InsertNewRow();
        }

        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            InsertNewRow();
        }
        #endregion

        #region [ Auxiliar Methods ]
        private void InsertNewRow()
        {
            UIAPIRawForm.Freeze(true);

            mtPriority.LoadFromDataSourceEx(false);

            var rows = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            rows.InsertRow(mtPriority.RowCount == 0);

            mtPriority.LoadFromDataSourceEx(false);
            mtPriority.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        private void RemoveRow()
        {
            var rows = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

            mtPriority.FlushToDataSource();
            rows.RemoveRecord(_menuRow);
            mtPriority.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
        }

        private void RemoveLastRow()
        {
            var rows = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            rows.RemoveRecord(rows.Offset);
        }
        #endregion

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;

        private SAPbouiCOM.Matrix mtPriority;
    }
}
