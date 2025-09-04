using SAPbouiCOM.Framework;
using SAPbouiCOM;
using System;
using MenuPlanner.Extensions;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.DishTypes", "Views/DishTypes.b1f")]
    class DishTypes : UserFormBase
    {
        private static int _menuRow;

        public DishTypes()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.ckProtein = ((SAPbouiCOM.CheckBox)(this.GetItem("ckProtein").Specific));
            this.etName = ((SAPbouiCOM.EditText)(this.GetItem("etName").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.mtItmsGrp = ((SAPbouiCOM.Matrix)(this.GetItem("mtItmsGrp").Specific));
            this.mtItmsGrp.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtItmsGrp_LinkPressedBefore);
            this.mtItmsGrp.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtItmsGrp_ChooseFromListAfter);
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
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
            StaticText2.Item.TextStyle = 5;

            InsertNewRow();

            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;
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

        private void etFmlCode_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");

            dbDataSource.SetValue("U_CVA_D_FAMILIA", dbDataSource.Offset, dataTable.GetValue("Name", 0).ToString());
        }

        private void etFmlName_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");

            dbDataSource.SetValue("U_CVA_FAMILIA", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }

        private void etSFmlCode_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");

            dbDataSource.SetValue("U_CVA_D_SUB_FAMILIA", dbDataSource.Offset, dataTable.GetValue("Name", 0).ToString());
        }

        private void etSFmlName_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");

            dbDataSource.SetValue("U_CVA_SUB_FAMILIA", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }

        private void mtItmsGrp_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
                var dataTable = chooseFromListEvent.SelectedObjects;

                if (dataTable == null) return;

                mtItmsGrp.FlushToDataSource();

                var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_DST1");

                switch (pVal.ColUID)
                {
                    case "ItmsGrpCod":
                    case "ItmsGrpNam":
                        dbDataSource.SetValue("U_ItmsGrpCod", pVal.Row - 1, dataTable.GetValue("ItmsGrpCod", 0).ToString());
                        dbDataSource.SetValue("U_ItmsGrpNam", pVal.Row - 1, dataTable.GetValue("ItmsGrpNam", 0).ToString());
                        break;
                }

                if (pVal.Row == dbDataSource.Size)
                {
                    dbDataSource.InsertRecord(dbDataSource.Size);
                }

                if (UIAPIRawForm.Mode == BoFormMode.fm_OK_MODE)
                {
                    UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
                }

                mtItmsGrp.LoadFromDataSourceEx();
                mtItmsGrp.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message);
                throw;
            }
        }

        private void mtItmsGrp_LinkPressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dst1 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_DST1");
            SAPbouiCOM.Framework.Application.SBO_Application.OpenForm(BoFormObjectEnum.fo_ItemGroups, "", dst1.GetValue("U_ItmsGrpCod", pVal.Row - 1));
            BubbleEvent = false;
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtItmsGrp")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtItmsGrp")
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

        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");
            var lineDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_DST1");
            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");

            lineDataSource.RemoveRecord(lineDataSource.Offset);

            dtQuery.Clear();

            dtQuery.ExecuteQuery(@"select coalesce(max(""DocEntry""), 0) + 1 as ""DocEntry"" from ""@CVA_TIPOPRATO""");

            dataSource.SetValue("Code", dataSource.Offset, dtQuery.GetValue("DocEntry", 0).ToString());
        }

        private void Form_DataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_DST1");
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

        private void InsertNewRow()
        {
            UIAPIRawForm.Freeze(true);

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_DST1");
            dataSource.InsertRow(mtItmsGrp.RowCount == 0);

            mtItmsGrp.LoadFromDataSourceEx(false);
            mtItmsGrp.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        private void RemoveRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_DST1");

            mtItmsGrp.FlushToDataSource();
            dataSource.RemoveRecord(_menuRow);
            mtItmsGrp.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.CheckBox ckProtein;
        private SAPbouiCOM.EditText etName;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private Matrix mtItmsGrp;
        private StaticText StaticText2;

        
    }
}
