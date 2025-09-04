using SAPbouiCOM.Framework;
using SAPbouiCOM;
using MenuPlanner.Extensions;
using MenuPlanner.Controllers;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.PriceVolume", "Views/PriceVolume.b1f")]
    class PriceVolume : UserFormBase
    {
        private static int _menuRow;

        public PriceVolume()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.etNumber = ((SAPbouiCOM.EditText)(this.GetItem("etNumber").Specific));
            this.etNumber.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etNumber_ChooseFromListAfter);
            this.etSrvGrp = ((SAPbouiCOM.EditText)(this.GetItem("etSrvGrp").Specific));
            this.etSrvGrp.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etSrvGrp_ChooseFromListAfter);
            this.etStrDt = ((SAPbouiCOM.EditText)(this.GetItem("etStrDt").Specific));
            this.mtVolume = ((SAPbouiCOM.Matrix)(this.GetItem("mtVolume").Specific));
            this.mtVolume.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtVolume_ValidateAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.etAbsID = ((SAPbouiCOM.EditText)(this.GetItem("etAbsID").Specific));
            this.LinkedButton0 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_12").Specific));
            this.LinkedButton0.PressedBefore += new SAPbouiCOM._ILinkedButtonEvents_PressedBeforeEventHandler(this.LinkedButton0_PressedBefore);
            this.etCardCode = ((SAPbouiCOM.EditText)(this.GetItem("etCardCode").Specific));
            this.etCardCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardCode_ChooseFromListAfter);
            this.etCardName = ((SAPbouiCOM.EditText)(this.GetItem("etCardName").Specific));
            this.etCardName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardName_ChooseFromListAfter);
            this.LinkedButton1 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_13").Specific));
            this.etCode = ((SAPbouiCOM.EditText)(this.GetItem("etCode").Specific));
            this.lkSrvGrp = ((SAPbouiCOM.LinkedButton)(this.GetItem("lkSrvGrp").Specific));
            this.lkSrvGrp.PressedAfter += new SAPbouiCOM._ILinkedButtonEvents_PressedAfterEventHandler(this.lkSrvGrp_PressedAfter);
            this.etSrvName = ((SAPbouiCOM.EditText)(this.GetItem("etSrvName").Specific));
            this.etSrvName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etSrvName_ChooseFromListAfter);
            this.lkSrv = ((SAPbouiCOM.LinkedButton)(this.GetItem("lkSrv").Specific));
            this.lkSrv.PressedAfter += new SAPbouiCOM._ILinkedButtonEvents_PressedAfterEventHandler(this.lkSrv_PressedAfter);
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
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);
            this.RightClickBefore += new RightClickBeforeHandler(this.Form_RightClickBefore);
            this.RightClickAfter += new RightClickAfterHandler(this.Form_RightClickAfter);

        }

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;

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

            var cflService = UIAPIRawForm.ChooseFromLists.Item("cflService");
            conditions = new Conditions();
            condition = conditions.Add();
            condition.Alias = "U_CVA_ATIVO";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "Y";

            cflService.SetConditions(conditions);

            var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
            conditions = new Conditions();
            condition = conditions.Add();
            condition.Alias = "BpType";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "C";

            cflAgrNumber.SetConditions(conditions);

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_TABPRCVOL");
            dbDataSource.InsertRow(firstRow: true);

            mtVolume.LoadFromDataSourceEx(true);
            mtVolume.AutoResizeColumns();
        }

        #region [ MenuEvents ]
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

        #region [ ChooseFromList ]
        private void etCardCode_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TABPRCVOL");

            dbDataSource.SetValue("U_CardName", dbDataSource.Offset, dataTable.GetValue("CardName", 0).ToString());

            var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "BpCode";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();

            cflAgrNumber.SetConditions(conditions);
        }

        private void etCardName_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TABPRCVOL");

            dbDataSource.SetValue("U_CardCode", dbDataSource.Offset, dataTable.GetValue("CardCode", 0).ToString());

            var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "BpCode";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();

            cflAgrNumber.SetConditions(conditions);
        }

        private void etNumber_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TABPRCVOL");

            dbDataSource.SetValue("U_AbsID", dbDataSource.Offset, dataTable.GetValue("AbsID", 0).ToString());
            dbDataSource.SetValue("U_CardCode", dbDataSource.Offset, dataTable.GetValue("BpCode", 0).ToString());
            dbDataSource.SetValue("U_CardName", dbDataSource.Offset, dataTable.GetValue("BpName", 0).ToString());
        }

        private void etSrvGrp_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TABPRCVOL");

            dbDataSource.SetValue("U_CVA_GRPSERVICO", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }

        private void etSrvName_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TABPRCVOL");

            dbDataSource.SetValue("U_ServiceId", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }
        #endregion

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            mtVolume.AutoResizeColumns();
        }

        private void mtVolume_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!pVal.ItemChanged) return;

            UIAPIRawForm.Freeze(true);

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_TABPRCVOL");

            mtVolume.FlushToDataSource();

            if (pVal.Row == dbDataSource.Size)
            {
                dbDataSource.InsertRow();
            }

            mtVolume.LoadFromDataSourceEx();

            UIAPIRawForm.Freeze(false);
        }

        #region [ LinkedButton ]
        private void lkSrvGrp_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            CommonController.FormFatherType = UIAPIRawForm.TypeEx;
            CommonController.FormFatherCount = UIAPIRawForm.TypeCount;

            
            var activeForm = new ServiceGroup();
            activeForm.Show();
        }

        private void lkSrv_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            CommonController.FormFatherType = UIAPIRawForm.TypeEx;
            CommonController.FormFatherCount = UIAPIRawForm.TypeCount;

            var activeForm = new Service();
            activeForm.Show();
        }

        private void LinkedButton0_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            CommonController.LazyProcess = true;
        }

        #endregion

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtVolume")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtVolume")
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

        #region [ FormDataEvents ]
        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TABPRCVOL");
            var lineDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_TABPRCVOL");
            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");

            dtQuery.Clear();

            dtQuery.ExecuteQuery(@"select coalesce(max(""DocEntry""), 0) + 1 as ""DocEntry"" from ""@CVA_TABPRCVOL""");

            dataSource.SetValue("Code", dataSource.Offset, dtQuery.GetValue("DocEntry", 0).ToString());

            lineDataSource.RemoveRecord(lineDataSource.Offset);
        }

        private void Form_DataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_TABPRCVOL");
            dataSource.RemoveRecord(dataSource.Offset);
        }

        private void Form_DataUpdateAfter(ref BusinessObjectInfo pVal)
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_TABPRCVOL");
            dataSource.InsertRecord(dataSource.Size);

            mtVolume.LoadFromDataSourceEx(false);
            mtVolume.AutoResizeColumns();
        }

        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_TABPRCVOL");
            dataSource.InsertRecord(dataSource.Size);

            mtVolume.LoadFromDataSourceEx(false);
            mtVolume.AutoResizeColumns();
        }
        #endregion

        private void InsertNewRow()
        {
            UIAPIRawForm.Freeze(true);

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_TABPRCVOL");

            if (mtVolume.RowCount == 0)
            {
                dataSource.InsertRecord(0);
                dataSource.RemoveRecord(1);
            }
            else
            {
                dataSource.InsertRecord(dataSource.Size);
            }

            mtVolume.LoadFromDataSourceEx(false);
            mtVolume.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        private void RemoveRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_TABPRCVOL");

            mtVolume.FlushToDataSource();
            dataSource.RemoveRecord(_menuRow);
            mtVolume.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private SAPbouiCOM.EditText etCardCode;
        private SAPbouiCOM.EditText etNumber;
        private SAPbouiCOM.EditText etSrvGrp;
        private SAPbouiCOM.EditText etStrDt;
        private SAPbouiCOM.EditText etSrvName;
        private SAPbouiCOM.EditText etCardName;
        private SAPbouiCOM.EditText etCode;
        private SAPbouiCOM.EditText etAbsID;

        private SAPbouiCOM.Matrix mtVolume;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;

        private SAPbouiCOM.LinkedButton LinkedButton0;
        private SAPbouiCOM.LinkedButton LinkedButton1;
        private SAPbouiCOM.LinkedButton lkSrvGrp;
        private LinkedButton lkSrv;

        
    }
}
