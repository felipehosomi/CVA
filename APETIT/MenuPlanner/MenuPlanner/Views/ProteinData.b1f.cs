using System;
using SAPbouiCOM.Framework;
using SAPbouiCOM;
using MenuPlanner.Extensions;
using MenuPlanner.Controllers;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.ProteinData", "Views/ProteinData.b1f")]
    class ProteinData : UserFormBase
    {
        private static int _menuRow;

        public ProteinData()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.etCardCode = ((SAPbouiCOM.EditText)(this.GetItem("etCardCode").Specific));
            this.etCardCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardCode_ChooseFromListAfter);
            this.etCardName = ((SAPbouiCOM.EditText)(this.GetItem("etCardName").Specific));
            this.etCardName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardName_ChooseFromListAfter);
            this.etCode = ((SAPbouiCOM.EditText)(this.GetItem("etCode").Specific));
            this.etCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCode_ChooseFromListAfter);
            this.etAbsID = ((SAPbouiCOM.EditText)(this.GetItem("etAbsID").Specific));
            this.mtProteins = ((SAPbouiCOM.Matrix)(this.GetItem("mtProteins").Specific));
            this.mtProteins.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtProteins_ChooseFromListAfter);
            this.LinkedButton0 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_7").Specific));
            this.LinkedButton0.PressedBefore += new SAPbouiCOM._ILinkedButtonEvents_PressedBeforeEventHandler(this.LinkedButton0_PressedBefore);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.LinkedButton1 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_12").Specific));
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

            InsertNewRow();

            etAbsID.Item.Width = 1;

            var cflCardCode = UIAPIRawForm.ChooseFromLists.Item("cflCardCode");
            var cflCardName = UIAPIRawForm.ChooseFromLists.Item("cflCardName");

            var conditions = new Conditions();
            var condition = conditions.Add();
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
        private void etCardCode_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OIGP");

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

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OIGP");

            dbDataSource.SetValue("U_CardCode", dbDataSource.Offset, dataTable.GetValue("CardCode", 0).ToString());

            var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "BpCode";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();

            cflAgrNumber.SetConditions(conditions);
        }

        private void etCode_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OIGP");

            dbDataSource.SetValue("U_AbsID", dbDataSource.Offset, dataTable.GetValue("AbsID", 0).ToString());
            dbDataSource.SetValue("U_CardCode", dbDataSource.Offset, dataTable.GetValue("BpCode", 0).ToString());
            dbDataSource.SetValue("U_CardName", dbDataSource.Offset, dataTable.GetValue("BpName", 0).ToString());
        }

        private void mtProteins_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
                var dataTable = chooseFromListEvent.SelectedObjects;

                if (dataTable == null) return;

                mtProteins.FlushToDataSource();

                var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_IGP1");
                dbDataSource.SetValue("U_ProteinCode", pVal.Row - 1, dataTable.GetValue("Code", 0).ToString());
                dbDataSource.SetValue("U_ProteinName", pVal.Row - 1, dataTable.GetValue("Name", 0).ToString());

                if (pVal.Row == dbDataSource.Size)
                {
                    dbDataSource.InsertRow();
                }

                mtProteins.LoadFromDataSourceEx();
                mtProteins.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message);
                throw;
            }
        }

        private void LinkedButton0_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            CommonController.LazyProcess = true;
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtProteins")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtProteins")
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
        #endregion

        #region FormDataEvents
        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_IGP1");
            dataSource.RemoveRecord(dataSource.Offset);
        }

        private void Form_DataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_IGP1");
            dataSource.RemoveRecord(dataSource.Offset);
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

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_IGP1");

            if (mtProteins.RowCount == 0)
            {
                dataSource.InsertRecord(0);
                dataSource.RemoveRecord(1);
            }
            else
            {
                dataSource.InsertRecord(dataSource.Size);
            }

            mtProteins.LoadFromDataSourceEx(false);
            mtProteins.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        private void RemoveRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_IGP1");

            mtProteins.FlushToDataSource();
            dataSource.RemoveRecord(_menuRow);
            mtProteins.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private SAPbouiCOM.EditText etCardCode;
        private SAPbouiCOM.EditText etCardName;
        private SAPbouiCOM.EditText etCode;
        private SAPbouiCOM.EditText etAbsID;
        private SAPbouiCOM.Matrix mtProteins;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.LinkedButton LinkedButton0;
        private SAPbouiCOM.LinkedButton LinkedButton1;
    }
}
