using System;
using System.Collections.Generic;
using System.Globalization;
using PackIndicator.Controllers;
using SAPbouiCOM.Framework;

namespace PackIndicator.Views
{
    [FormAttribute("PackIndicator.Views.PickingIndicatorParameters", "Views/PickingIndicatorParameters.b1f")]
    class PickingIndicatorParameters : UserFormBase
    {
        public PickingIndicatorParameters()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtWhs = ((SAPbouiCOM.Matrix)(this.GetItem("mtWhs").Specific));
            this.stWhs = ((SAPbouiCOM.StaticText)(this.GetItem("stWhs").Specific));
            this.etDocNumF = ((SAPbouiCOM.EditText)(this.GetItem("etDocNumF").Specific));
            this.etDocNumF.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.etDocNumF_ValidateBefore);
            this.etDocNumF.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etDocNumF_ChooseFromListAfter);
            this.etDocNumT = ((SAPbouiCOM.EditText)(this.GetItem("etDocNumT").Specific));
            this.etDocNumT.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.etDocNumT_ValidateBefore);
            this.etDocNumT.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etDocNumT_ChooseFromListAfter);
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("etDocDateF").Specific));
            this.EditText2.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.EditText2_ValidateBefore);
            this.EditText3 = ((SAPbouiCOM.EditText)(this.GetItem("etDocDateT").Specific));
            this.EditText3.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.EditText3_ValidateBefore);
            this.etCardCodF = ((SAPbouiCOM.EditText)(this.GetItem("etCardCodF").Specific));
            this.etCardCodF.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.etCardCodF_ValidateBefore);
            this.etCardCodF.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardCodF_ChooseFromListAfter);
            this.etCardCodT = ((SAPbouiCOM.EditText)(this.GetItem("etCardCodT").Specific));
            this.etCardCodT.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.etCardCodT_ValidateBefore);
            this.etCardCodT.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardCodT_ChooseFromListAfter);
            this.etItemCodF = ((SAPbouiCOM.EditText)(this.GetItem("etItemCodF").Specific));
            this.etItemCodF.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.etItemCodF_ValidateBefore);
            this.etItemCodF.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etItemCodF_ChooseFromListAfter);
            this.etItemCodT = ((SAPbouiCOM.EditText)(this.GetItem("etItemCodT").Specific));
            this.etItemCodT.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.etItemCodT_ValidateBefore);
            this.etItemCodT.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etItemCodT_ChooseFromListAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("btOK").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.etStckCatF = ((SAPbouiCOM.EditText)(this.GetItem("etStckCatF").Specific));
            this.etStckCatT = ((SAPbouiCOM.EditText)(this.GetItem("etStckCatT").Specific));
            this.etDueDtF = ((SAPbouiCOM.EditText)(this.GetItem("etDueDtF").Specific));
            this.etDueDtT = ((SAPbouiCOM.EditText)(this.GetItem("etDueDtT").Specific));
            this.etRouteF = ((SAPbouiCOM.EditText)(this.GetItem("etRouteF").Specific));
            this.etRouteT = ((SAPbouiCOM.EditText)(this.GetItem("etRouteT").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.VisibleAfter += new SAPbouiCOM.Framework.FormBase.VisibleAfterHandler(this.Form_VisibleAfter);
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private void OnCustomInitialize()
        {
            stWhs.Item.TextStyle = 4;

            var dtWhs = UIAPIRawForm.DataSources.DataTables.Item("dtWhs");
            dtWhs.ExecuteQuery(@"select 'Y' as ""Select"", OWHS.""WhsCode"", OWHS.""WhsName"", OBPL.""BPLName""
                                   from OWHS
                                  inner join OBPL on OBPL.""BPLId"" = OWHS.""BPLid""
                                  where OWHS.""U_CVA_UomControl"" = 'Y'");

            mtWhs.Columns.Item("Select").DataBind.Bind("dtWhs", "Select");
            mtWhs.Columns.Item("WhsCode").DataBind.Bind("dtWhs", "WhsCode");
            mtWhs.Columns.Item("WhsName").DataBind.Bind("dtWhs", "WhsName");
            mtWhs.Columns.Item("BPLName").DataBind.Bind("dtWhs", "BPLName");
            mtWhs.LoadFromDataSourceEx();
            mtWhs.AutoResizeColumns();
        }

        private void Form_VisibleAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            var formWidth = UIAPIRawForm.ClientWidth;
            var formHeight = UIAPIRawForm.ClientHeight;

            // Centralização do form
            UIAPIRawForm.Left = int.Parse(((Application.SBO_Application.Desktop.Width - formWidth) / 2).ToString(CultureInfo.InvariantCulture));
            UIAPIRawForm.Top = int.Parse(((Application.SBO_Application.Desktop.Height - formHeight) / 3).ToString(CultureInfo.InvariantCulture));
        }

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                var docNumFrom = UIAPIRawForm.DataSources.UserDataSources.Item("DocNumF").ValueEx;
                var docNumTo = UIAPIRawForm.DataSources.UserDataSources.Item("DocNumT").ValueEx;
                var docDateFrom = UIAPIRawForm.DataSources.UserDataSources.Item("DocDateF").ValueEx;
                var docDateTo = UIAPIRawForm.DataSources.UserDataSources.Item("DocDateT").ValueEx;
                var cardCodeFrom = UIAPIRawForm.DataSources.UserDataSources.Item("CardCodF").ValueEx;
                var cardCodeTo = UIAPIRawForm.DataSources.UserDataSources.Item("CardCodT").ValueEx;
                var itemCodeFrom = UIAPIRawForm.DataSources.UserDataSources.Item("ItemCodF").ValueEx;
                var itemCodeTo = UIAPIRawForm.DataSources.UserDataSources.Item("ItemCodT").ValueEx;

                var stockCategoryFrom = UIAPIRawForm.DataSources.UserDataSources.Item("StckCatF").ValueEx;
                var stockCategoryTo = UIAPIRawForm.DataSources.UserDataSources.Item("StckCatT").ValueEx;

                var dueDateFrom = UIAPIRawForm.DataSources.UserDataSources.Item("DueDtF").ValueEx;
                var dueDateTo = UIAPIRawForm.DataSources.UserDataSources.Item("DueDtT").ValueEx;

                var routeFrom = UIAPIRawForm.DataSources.UserDataSources.Item("RouteF").ValueEx;
                var routeTo = UIAPIRawForm.DataSources.UserDataSources.Item("RouteT").ValueEx;

                var whsCodes = new List<string>();

                var dtWhs = UIAPIRawForm.DataSources.DataTables.Item("dtWhs");
                mtWhs.FlushToDataSource();

                for (var i = 0; i < dtWhs.Rows.Count; i++)
                {
                    if (dtWhs.GetValue("Select", i).ToString() != "Y") continue;

                    whsCodes.Add(dtWhs.GetValue("WhsCode", i).ToString());
                }

                CommonController.MotherForm = UIAPIRawForm;

                var pickingIndicatorManager = new PickingIndicatorManager(docNumFrom, docNumTo, 
                                                                          docDateFrom, docDateTo, 
                                                                          cardCodeFrom, cardCodeTo, 
                                                                          itemCodeFrom, itemCodeTo,
                                                                          stockCategoryFrom, stockCategoryTo,
                                                                          dueDateFrom, dueDateTo,
                                                                          routeFrom, routeTo,
                                                                          whsCodes);
                pickingIndicatorManager.Show();
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);
            }
        }

        private void etDocNumF_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SAPbouiCOM.SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            UIAPIRawForm.DataSources.UserDataSources.Item("DocNumF").ValueEx = dataTable.GetValue("DocNum", 0).ToString();

            var cflDocNumT = UIAPIRawForm.ChooseFromLists.Item("cflDocNumT");
            var conditions = new SAPbouiCOM.Conditions();
            var condition = conditions.Add();
            condition.Alias = "DocNum";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_GRATER_EQUAL;
            condition.CondVal = dataTable.GetValue("DocNum", 0).ToString();
            cflDocNumT.SetConditions(conditions);
        }

        private void etDocNumT_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SAPbouiCOM.SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            UIAPIRawForm.DataSources.UserDataSources.Item("DocNumT").ValueEx = dataTable.GetValue("DocNum", 0).ToString();

            var cflDocNumF = UIAPIRawForm.ChooseFromLists.Item("cflDocNumF");
            var conditions = new SAPbouiCOM.Conditions();
            var condition = conditions.Add();
            condition.Alias = "DocNum";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_LESS_EQUAL;
            condition.CondVal = dataTable.GetValue("DocNum", 0).ToString();
            cflDocNumF.SetConditions(conditions);
        }

        private void etCardCodF_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SAPbouiCOM.SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            UIAPIRawForm.DataSources.UserDataSources.Item("CardCodF").ValueEx = dataTable.GetValue("CardCode", 0).ToString();

            var cflCardCodT = UIAPIRawForm.ChooseFromLists.Item("cflCardCodT");
            var conditions = new SAPbouiCOM.Conditions();
            var condition = conditions.Add();
            condition.Alias = "CardCode";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_GRATER_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();
            cflCardCodT.SetConditions(conditions);
        }

        private void etCardCodT_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SAPbouiCOM.SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            UIAPIRawForm.DataSources.UserDataSources.Item("CardCodT").ValueEx = dataTable.GetValue("CardCode", 0).ToString();

            var cflCardCodF = UIAPIRawForm.ChooseFromLists.Item("cflCardCodF");
            var conditions = new SAPbouiCOM.Conditions();
            var condition = conditions.Add();
            condition.Alias = "CardCode";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_LESS_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();
            cflCardCodF.SetConditions(conditions);
        }

        private void etItemCodF_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SAPbouiCOM.SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            UIAPIRawForm.DataSources.UserDataSources.Item("ItemCodF").ValueEx = dataTable.GetValue("ItemCode", 0).ToString();

            var cflItemCodT = UIAPIRawForm.ChooseFromLists.Item("cflItemCodT");
            var conditions = new SAPbouiCOM.Conditions();
            var condition = conditions.Add();
            condition.Alias = "ItemCode";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_GRATER_EQUAL;
            condition.CondVal = dataTable.GetValue("ItemCode", 0).ToString();
            cflItemCodT.SetConditions(conditions);
        }

        private void etItemCodT_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SAPbouiCOM.SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            UIAPIRawForm.DataSources.UserDataSources.Item("ItemCodT").ValueEx = dataTable.GetValue("ItemCode", 0).ToString();

            var cflItemCodF = UIAPIRawForm.ChooseFromLists.Item("cflItemCodF");
            var conditions = new SAPbouiCOM.Conditions();
            var condition = conditions.Add();
            condition.Alias = "ItemCode";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_LESS_EQUAL;
            condition.CondVal = dataTable.GetValue("ItemCode", 0).ToString();
            cflItemCodF.SetConditions(conditions);
        }

        private void etDocNumF_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!String.IsNullOrEmpty(UIAPIRawForm.DataSources.UserDataSources.Item("DocNumF").ValueEx)) return;

            var cflDocNumT = UIAPIRawForm.ChooseFromLists.Item("cflDocNumT");
            var conditions = new SAPbouiCOM.Conditions();
            cflDocNumT.SetConditions(conditions);
        }

        private void etDocNumT_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!String.IsNullOrEmpty(UIAPIRawForm.DataSources.UserDataSources.Item("DocNumT").ValueEx)) return;

            var cflDocNumF = UIAPIRawForm.ChooseFromLists.Item("cflDocNumF");
            var conditions = new SAPbouiCOM.Conditions();
            cflDocNumF.SetConditions(conditions);
        }

        private void etCardCodF_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!String.IsNullOrEmpty(UIAPIRawForm.DataSources.UserDataSources.Item("CardCodF").ValueEx)) return;

            var cflCardCodT = UIAPIRawForm.ChooseFromLists.Item("cflCardCodT");
            var conditions = new SAPbouiCOM.Conditions();
            cflCardCodT.SetConditions(conditions);
        }

        private void etCardCodT_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!String.IsNullOrEmpty(UIAPIRawForm.DataSources.UserDataSources.Item("CardCodT").ValueEx)) return;

            var cflCardCodF = UIAPIRawForm.ChooseFromLists.Item("cflCardCodF");
            var conditions = new SAPbouiCOM.Conditions();
            cflCardCodF.SetConditions(conditions);
        }

        private void etItemCodF_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!String.IsNullOrEmpty(UIAPIRawForm.DataSources.UserDataSources.Item("ItemCodF").ValueEx)) return;

            var cflItemCodT = UIAPIRawForm.ChooseFromLists.Item("cflItemCodT");
            var conditions = new SAPbouiCOM.Conditions();
            cflItemCodT.SetConditions(conditions);
        }

        private void etItemCodT_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!String.IsNullOrEmpty(UIAPIRawForm.DataSources.UserDataSources.Item("ItemCodT").ValueEx)) return;

            var cflItemCodF = UIAPIRawForm.ChooseFromLists.Item("cflItemCodF");
            var conditions = new SAPbouiCOM.Conditions();
            cflItemCodF.SetConditions(conditions);
        }

        private void EditText2_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (String.IsNullOrEmpty(UIAPIRawForm.DataSources.UserDataSources.Item("DocDateT").ValueEx)) return;

            if (DateTime.Parse(UIAPIRawForm.DataSources.UserDataSources.Item("DocDateT").Value) < DateTime.Parse(UIAPIRawForm.DataSources.UserDataSources.Item("DocDateF").Value))
            {
                Application.SBO_Application.StatusBar.SetText(@"No campo ""Data de"", insira uma data anterior à data em ""Data até""", SAPbouiCOM.BoMessageTime.bmt_Short);
                BubbleEvent = false;
            }
        }

        private void EditText3_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (String.IsNullOrEmpty(UIAPIRawForm.DataSources.UserDataSources.Item("DocDateF").ValueEx)) return;

            if (DateTime.Parse(UIAPIRawForm.DataSources.UserDataSources.Item("DocDateT").Value) < DateTime.Parse(UIAPIRawForm.DataSources.UserDataSources.Item("DocDateF").Value))
            {
                Application.SBO_Application.StatusBar.SetText(@"No campo ""Data de"", insira uma data anterior à data em ""Data até""", SAPbouiCOM.BoMessageTime.bmt_Short);
                BubbleEvent = false;
            }
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            mtWhs.AutoResizeColumns();
        }

        private SAPbouiCOM.Matrix mtWhs;

        private SAPbouiCOM.StaticText stWhs;
        private SAPbouiCOM.EditText etDocNumF;
        private SAPbouiCOM.EditText etDocNumT;
        private SAPbouiCOM.EditText EditText2;
        private SAPbouiCOM.EditText EditText3;
        private SAPbouiCOM.EditText etCardCodF;
        private SAPbouiCOM.EditText etCardCodT;
        private SAPbouiCOM.EditText etItemCodF;
        private SAPbouiCOM.EditText etItemCodT;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText etStckCatF;
        private SAPbouiCOM.EditText etStckCatT;
        private SAPbouiCOM.EditText etDueDtF;
        private SAPbouiCOM.EditText etDueDtT;
        private SAPbouiCOM.EditText etRouteF;
        private SAPbouiCOM.EditText etRouteT;
    }
}
