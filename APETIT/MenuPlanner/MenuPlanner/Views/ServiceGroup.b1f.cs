using SAPbouiCOM.Framework;
using SAPbouiCOM;
using System;
using MenuPlanner.Extensions;
using MenuPlanner.Controllers;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.ServiceGroup", "Views/ServiceGroup.b1f")]
    class ServiceGroup : UserFormBase
    {
        private static int _menuRow;

        public ServiceGroup()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.etSrvGrp = ((SAPbouiCOM.EditText)(this.GetItem("etName").Specific));
            this.etCardCode = ((SAPbouiCOM.EditText)(this.GetItem("etCardCode").Specific));
            this.etCardCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardCode_ChooseFromListAfter);
            this.etCardName = ((SAPbouiCOM.EditText)(this.GetItem("etCardName").Specific));
            this.etCardName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardName_ChooseFromListAfter);
            this.etAgrNum = ((SAPbouiCOM.EditText)(this.GetItem("etAgrNum").Specific));
            this.etAgrNum.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etAgrNum_ChooseFromListAfter);
            this.etMinQty = ((SAPbouiCOM.EditText)(this.GetItem("etMinQty").Specific));
            this.mtServices = ((SAPbouiCOM.Matrix)(this.GetItem("mtServices").Specific));
            this.mtServices.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtServices_ChooseFromListAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.etAbsID = ((SAPbouiCOM.EditText)(this.GetItem("etAbsID").Specific));
            this.LinkedButton0 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_7").Specific));
            this.LinkedButton0.PressedBefore += new SAPbouiCOM._ILinkedButtonEvents_PressedBeforeEventHandler(this.LinkedButton0_PressedBefore);
            this.LinkedButton1 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_6").Specific));
            this.etSrv = ((SAPbouiCOM.EditText)(this.GetItem("etSrv").Specific));
            this.etSrv.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etSrv_ChooseFromListAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.DataAddBefore += new SAPbouiCOM.Framework.FormBase.DataAddBeforeHandler(this.Form_DataAddBefore);
            this.DataUpdateBefore += new SAPbouiCOM.Framework.FormBase.DataUpdateBeforeHandler(this.Form_DataUpdateBefore);
            this.DataUpdateAfter += new SAPbouiCOM.Framework.FormBase.DataUpdateAfterHandler(this.Form_DataUpdateAfter);
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
            this.RightClickBefore += new SAPbouiCOM.Framework.FormBase.RightClickBeforeHandler(this.Form_RightClickBefore);
            this.RightClickAfter += new SAPbouiCOM.Framework.FormBase.RightClickAfterHandler(this.Form_RightClickAfter);

        }

        private void OnCustomInitialize()
        {
            try
            {
                Conditions conditions;
                Condition condition;

                if (!String.IsNullOrEmpty(CommonController.FormFatherType))
                {
                    var fatherForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(CommonController.FormFatherType, CommonController.FormFatherCount);
                    var groupSrvID = String.Empty;

                    switch (fatherForm.TypeEx)
                    {
                        case "MenuPlanner.Views.Planner":
                            {
                                var dataSource = fatherForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                                groupSrvID = dataSource.GetValue("U_CVA_ID_G_SERVICO", dataSource.Offset);
                            }
                            break;

                        case "MenuPlanner.Views.PriceVolume":
                            {
                                var dataSource = fatherForm.DataSources.DBDataSources.Item("@CVA_TABPRCVOL");
                                groupSrvID = dataSource.GetValue("U_CVA_GRPSERVICO", dataSource.Offset);
                            }
                            break;
                    }

                    var srvGrp = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_GRPSERVICOS");
                    var srvGrpLines = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_GRPSERVICOS");

                    conditions = new Conditions();
                    condition = conditions.Add();
                    condition.Alias = "Code";
                    condition.Operation = BoConditionOperation.co_EQUAL;
                    condition.CondVal = groupSrvID;
                    srvGrp.Query(conditions);
                    srvGrpLines.Query(conditions);

                    UIAPIRawForm.Mode = BoFormMode.fm_OK_MODE;

                    CommonController.FormFatherType = String.Empty;
                    CommonController.FormFatherCount = -1;
                }

                InsertNewRow();

                etAbsID.Item.Width = 1;

                mtServices.Columns.Item("Monday").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
                mtServices.Columns.Item("Tuesday").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
                mtServices.Columns.Item("Wednesday").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
                mtServices.Columns.Item("Thursday").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
                mtServices.Columns.Item("Friday").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
                mtServices.Columns.Item("Saturday").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
                mtServices.Columns.Item("Sunday").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
                mtServices.LoadFromDataSourceEx(true);
                mtServices.AutoResizeColumns();

                var cflItemCode = UIAPIRawForm.ChooseFromLists.Item("cflItemCode");
                var cflItemName = UIAPIRawForm.ChooseFromLists.Item("cflItemName");
                var cflSrvName = UIAPIRawForm.ChooseFromLists.Item("cflSrvName");
                var cflCardCode = UIAPIRawForm.ChooseFromLists.Item("cflCardCode");
                var cflCardName = UIAPIRawForm.ChooseFromLists.Item("cflCardName");

                conditions = new Conditions();
                condition = conditions.Add();
                condition.Alias = "U_CVA_ATIVO";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = "Y";

                cflSrvName.SetConditions(conditions);

                conditions = new Conditions();
                condition = conditions.Add();
                condition.Alias = "frozenFor";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = "N";

                cflItemCode.SetConditions(conditions);
                cflItemName.SetConditions(conditions);

                conditions = new Conditions();
                condition = conditions.Add();
                condition.Alias = "CardType";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = "C";

                cflCardCode.SetConditions(conditions);
                cflCardName.SetConditions(conditions);

                var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
                conditions = new Conditions();
                condition = conditions.Add();
                condition.Alias = "BpType";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = "C";

                cflAgrNumber.SetConditions(conditions);

                SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short);
            }
        }

        private void MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.MenuUID != "1281" && pVal.MenuUID != "1282" && pVal.MenuUID != "RemoveRow") return;
            if (UIAPIRawForm == null) return;
            if (UIAPIRawForm.UniqueID != SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.UniqueID) return;

            if (!pVal.BeforeAction)
            {
                switch (pVal.MenuUID)
                {
                    case "1281":
                        ((EditText)UIAPIRawForm.Items.Item("etName").Specific).Active = true;
                        break;

                    case "1282":
                        ((EditText)UIAPIRawForm.Items.Item("etName").Specific).Active = true;
                        InsertNewRow();
                        break;

                    case "RemoveRow":
                        RemoveRow();
                        break;
                }
            }
        }

        private void etCardCode_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_GRPSERVICOS");

            dbDataSource.SetValue("U_CardName", dbDataSource.Offset, dataTable.GetValue("CardName", 0).ToString());

            var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "BpCode";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();

            cflAgrNumber.SetConditions(conditions);
        }

        private void etCardName_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_GRPSERVICOS");

            dbDataSource.SetValue("U_CardCode", dbDataSource.Offset, dataTable.GetValue("CardCode", 0).ToString());

            var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "BpCode";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();

            cflAgrNumber.SetConditions(conditions);
        }

        private void etAgrNum_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_GRPSERVICOS");

            dbDataSource.SetValue("U_AbsID", dbDataSource.Offset, dataTable.GetValue("AbsID", 0).ToString());
            dbDataSource.SetValue("U_CardCode", dbDataSource.Offset, dataTable.GetValue("BpCode", 0).ToString());
            dbDataSource.SetValue("U_CardName", dbDataSource.Offset, dataTable.GetValue("BpName", 0).ToString());
        }

        private void etSrv_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_GRPSERVICOS");

            dbDataSource.SetValue("U_ServiceId", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }

        private void mtServices_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
                var dataTable = chooseFromListEvent.SelectedObjects;

                if (dataTable == null) return;

                var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_GRPSERVICOS");

                mtServices.FlushToDataSource();

                switch (pVal.ColUID)
                {
                    case "ItemCode":
                    case "ItemName":
                        dbDataSource.SetValue("U_CVA_D_AGRUP", pVal.Row - 1, dataTable.GetValue("ItemName", 0).ToString());
                        dbDataSource.SetValue("U_CVA_ID_AGRUP", pVal.Row - 1, dataTable.GetValue("ItemCode", 0).ToString());
                        break;

                    case "Shift":
                        dbDataSource.SetValue("U_CVA_TURNO", pVal.Row - 1, dataTable.GetValue("Name", 0).ToString());
                        break;
                }

                if (pVal.Row == dbDataSource.Size)
                {
                    dbDataSource.InsertRow();
                }

                if (UIAPIRawForm.Mode == BoFormMode.fm_OK_MODE)
                {
                    UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
                }

                mtServices.LoadFromDataSourceEx();
                mtServices.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message);
                throw;
            }
        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            mtServices.AutoResizeColumns();
        }

        private void LinkedButton0_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            CommonController.LazyProcess = true;
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtServices")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtServices")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = false;
                _menuRow = -1;
            }
        }

        private void Button0_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE) return;

            InsertNewRow();

            UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
        }

        #region FormDataEvents
        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_GRPSERVICOS");
            var lineDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_GRPSERVICOS");
            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");

            lineDataSource.RemoveRecord(lineDataSource.Offset);

            dtQuery.Clear();

            dtQuery.ExecuteQuery(@"select coalesce(max(""DocEntry""), 0) + 1 as ""DocEntry"" from ""@CVA_GRPSERVICOS""");

            dataSource.SetValue("Code", dataSource.Offset, dtQuery.GetValue("DocEntry", 0).ToString());
        }

        private void Form_DataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_GRPSERVICOS");
            if (dataSource.GetValue("U_CVA_TURNO", dataSource.Offset).Trim() == "")
            {
                dataSource.RemoveRecord(dataSource.Offset);
            }
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

        private void InsertNewRow()
        {
            UIAPIRawForm.Freeze(true);

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_GRPSERVICOS");
            dataSource.InsertRow(mtServices.RowCount == 0);

            mtServices.LoadFromDataSourceEx(false);
            mtServices.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        private void RemoveRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_GRPSERVICOS");

            mtServices.FlushToDataSource();
            dataSource.RemoveRecord(_menuRow);
            mtServices.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private SAPbouiCOM.EditText etSrvGrp;
        private SAPbouiCOM.EditText etCardCode;
        private SAPbouiCOM.EditText etCardName;
        private SAPbouiCOM.EditText etAgrNum;
        private SAPbouiCOM.EditText etMinQty;
        private SAPbouiCOM.EditText etAbsID;
        private SAPbouiCOM.Matrix mtServices;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.LinkedButton LinkedButton0;
        private LinkedButton LinkedButton1;
        private EditText etSrv;

        
    }
}
