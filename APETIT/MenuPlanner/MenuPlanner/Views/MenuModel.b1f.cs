using System;
using SAPbouiCOM.Framework;
using SAPbouiCOM;
using MenuPlanner.Extensions;
using MenuPlanner.Controllers;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.MenuModel", "Views/MenuModel.b1f")]
    class MenuModel : UserFormBase
    {
        private static int _menuRow;

        public MenuModel()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.etName = ((SAPbouiCOM.EditText)(this.GetItem("etName").Specific));
            this.etSrvName = ((SAPbouiCOM.EditText)(this.GetItem("etSrvName").Specific));
            this.etSrvName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etSrvName_ChooseFromListAfter);
            this.mtDishes = ((SAPbouiCOM.Matrix)(this.GetItem("mtDishes").Specific));
            this.mtDishes.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtDishes_ChooseFromListAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.etCardName = ((SAPbouiCOM.EditText)(this.GetItem("etCardName").Specific));
            this.etCardName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardName_ChooseFromListAfter);
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.etAgrNum = ((SAPbouiCOM.EditText)(this.GetItem("etAgrNum").Specific));
            this.etAgrNum.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etAgrNum_ChooseFromListAfter);
            this.LinkedButton0 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_5").Specific));
            this.etAgrId = ((SAPbouiCOM.EditText)(this.GetItem("etAgrId").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.etCardCode = ((SAPbouiCOM.EditText)(this.GetItem("etCardCode").Specific));
            this.etCardCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardCode_ChooseFromListAfter);
            this.LinkedButton1 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_0").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataAddBefore += new SAPbouiCOM.Framework.FormBase.DataAddBeforeHandler(this.Form_DataAddBefore);
            this.DataUpdateBefore += new SAPbouiCOM.Framework.FormBase.DataUpdateBeforeHandler(this.Form_DataUpdateBefore);
            this.DataUpdateAfter += new SAPbouiCOM.Framework.FormBase.DataUpdateAfterHandler(this.Form_DataUpdateAfter);
            this.DataLoadAfter += new DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.RightClickBefore += new RightClickBeforeHandler(this.Form_RightClickBefore);
            this.RightClickAfter += new RightClickAfterHandler(this.Form_RightClickAfter);

        }

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;

            Conditions conditions;
            Condition condition;

            etAgrId.Item.Width = 1;

            if (!String.IsNullOrEmpty(CommonController.FormFatherType))
            {
                var fatherForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(CommonController.FormFatherType, CommonController.FormFatherCount);
                var plannerData = fatherForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                var menuModel = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MCARDAPIO");
                var menuModelLines = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");

                conditions = new Conditions();
                condition = conditions.Add();
                condition.Alias = "Code";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = plannerData.GetValue("U_CVA_ID_MODEL_CARD", plannerData.Offset);
                menuModel.Query(conditions);
                menuModelLines.Query(conditions);

                UIAPIRawForm.Mode = BoFormMode.fm_OK_MODE;

                CommonController.FormFatherType = String.Empty;
                CommonController.FormFatherCount = -1;
            }
            else
            {
                var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");
                dbDataSource.InsertRow(firstRow: true);
            }

            mtDishes.LoadFromDataSourceEx();
            mtDishes.AutoResizeColumns();

            var cflCardCode = UIAPIRawForm.ChooseFromLists.Item("cflCardCode");
            var cflCardName = UIAPIRawForm.ChooseFromLists.Item("cflCardName");

            conditions = new Conditions();
            condition = conditions.Add();
            condition.Alias = "CardType";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "C";

            cflCardCode.SetConditions(conditions);
            cflCardName.SetConditions(conditions);

            var cflServiceName = UIAPIRawForm.ChooseFromLists.Item("cflServiceName");

            conditions = new Conditions();
            condition = conditions.Add();
            condition.Alias = "U_CVA_ATIVO";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "Y";

            cflServiceName.SetConditions(conditions);
        }

        #region MenuEvents
        private void MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.MenuUID != "1282" && pVal.MenuUID != "RemoveRow") return;
            if (UIAPIRawForm == null) return;
            if (UIAPIRawForm.UniqueID != SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.UniqueID) return;

            if (!pVal.BeforeAction)
            {
                switch (pVal.MenuUID)
                {
                    case "1282":
                        InsertNewRow();
                        break;

                    case "RemoveRow":
                        RemoveRow();
                        break;
                }
            }
        }
        #endregion

        #region ItemEvents
        private void etCardCode_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MCARDAPIO");

            dbDataSource.SetValue("U_CVA_DES_CONTRATO", dbDataSource.Offset, dataTable.GetValue("CardName", 0).ToString());

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

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MCARDAPIO");

            dbDataSource.SetValue("U_CardCode", dbDataSource.Offset, dataTable.GetValue("CardCode", 0).ToString());

            var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "BpCode";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();

            cflAgrNumber.SetConditions(conditions);
        }

        private void etAgrNum_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MCARDAPIO");

            dbDataSource.SetValue("U_AbsID", dbDataSource.Offset, dataTable.GetValue("AbsID", 0).ToString());
            dbDataSource.SetValue("U_CardCode", dbDataSource.Offset, dataTable.GetValue("BpCode", 0).ToString());
            dbDataSource.SetValue("U_CVA_DES_CONTRATO", dbDataSource.Offset, dataTable.GetValue("BpName", 0).ToString());
        }

        private void etSrvName_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MCARDAPIO");

            dbDataSource.SetValue("U_CVA_ID_SERVICO", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }

        private void mtDishes_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
                var dataTable = chooseFromListEvent.SelectedObjects;

                if (dataTable == null) return;

                mtDishes.FlushToDataSource();

                var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");
                dbDataSource.SetValue("U_CVA_TIPO_PRATO", pVal.Row - 1, dataTable.GetValue("Code", 0).ToString());
                dbDataSource.SetValue("U_CVA_TIPO_PRATO_DES", pVal.Row - 1, dataTable.GetValue("Name", 0).ToString());

                if (pVal.Row == dbDataSource.Size)
                {
                    dbDataSource.InsertRecord(dbDataSource.Size);
                }

                if (UIAPIRawForm.Mode == BoFormMode.fm_OK_MODE)
                {
                    UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
                }

                mtDishes.LoadFromDataSourceEx();
                mtDishes.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message);
                throw;
            }
        }

        private void Button0_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (UIAPIRawForm.Mode != BoFormMode.fm_ADD_MODE) return;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");
            dataSource.InsertRow(firstRow: true);

            mtDishes.LoadFromDataSourceEx(false);
            mtDishes.AutoResizeColumns();
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtDishes")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtDishes")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = false;
                _menuRow = -1;
            }
        }
        #endregion

        #region FormDataEvents
        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MCARDAPIO");
            var lineDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");
            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");

            dtQuery.Clear();

            dtQuery.ExecuteQuery(@"select coalesce(max(""DocEntry""), 0) + 1 as ""DocEntry"" from ""@CVA_MCARDAPIO""");

            dataSource.SetValue("Code", dataSource.Offset, dtQuery.GetValue("DocEntry", 0).ToString());
            lineDataSource.RemoveRecord(lineDataSource.Offset);
        }

        private void Form_DataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");
            dataSource.RemoveRecord(dataSource.Offset);
        }

        private void Form_DataUpdateAfter(ref BusinessObjectInfo pVal)
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");
            dataSource.InsertRecord(dataSource.Size);

            mtDishes.LoadFromDataSourceEx(false);
            mtDishes.AutoResizeColumns();
        }

        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");
            dataSource.InsertRecord(dataSource.Size);

            mtDishes.LoadFromDataSourceEx(false);
            mtDishes.AutoResizeColumns();
        }
        #endregion

        private void InsertNewRow()
        {
            UIAPIRawForm.Freeze(true);

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");

            if (mtDishes.RowCount == 0)
            {
                dataSource.InsertRecord(0);
                dataSource.RemoveRecord(1);
            }
            else
            {
                dataSource.InsertRecord(dataSource.Size);
            }

            mtDishes.LoadFromDataSourceEx(false);
            mtDishes.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        private void RemoveRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");

            mtDishes.FlushToDataSource();
            dataSource.RemoveRecord(_menuRow);
            mtDishes.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private StaticText StaticText1;
        private StaticText StaticText2;
        private EditText etName;
        private EditText etSrvName;
        private Matrix mtDishes;
        private Button Button0;
        private Button Button1;
        private StaticText StaticText5;
        private EditText etCardName;
        private StaticText StaticText3;
        private EditText etAgrNum;
        private LinkedButton LinkedButton0;
        private EditText etAgrId;
        private StaticText StaticText4;
        private EditText etCardCode;
        private LinkedButton LinkedButton1;
    }
}
