using MenuConsolidator.Controllers;
using MenuConsolidator.Extensions;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;

namespace MenuConsolidator.Views
{
    [FormAttribute("MenuConsolidator.Views.MRPParameters", "Views/MRPParameters.b1f")]
    class MRPParameters : UserFormBase
    {
        private string RowsDataSource = "@CVA_PAM1";
        private static int _menuRow;

        public MRPParameters()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.cmBPLId = ((SAPbouiCOM.ComboBox)(this.GetItem("cmBPLId").Specific));
            this.mtItems = ((SAPbouiCOM.Matrix)(this.GetItem("mtItems").Specific));
            this.mtItems.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtItems_LinkPressedBefore);
            this.mtItems.ComboSelectAfter += new SAPbouiCOM._IMatrixEvents_ComboSelectAfterEventHandler(this.mtItems_ComboSelectAfter);
            this.mtItems.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtItems_ValidateAfter);
            this.mtItems.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtItems_ChooseFromListAfter);
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
            this.DataUpdateBefore += new SAPbouiCOM.Framework.FormBase.DataUpdateBeforeHandler(this.Form_DataUpdateBefore);
            this.DataUpdateAfter += new SAPbouiCOM.Framework.FormBase.DataUpdateAfterHandler(this.Form_DataUpdateAfter);
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.RightClickBefore += new SAPbouiCOM.Framework.FormBase.RightClickBeforeHandler(this.Form_RightClickBefore);
            this.RightClickBefore += new RightClickBeforeHandler(this.Form_RightClickBefore);
            this.RightClickAfter += new RightClickAfterHandler(this.Form_RightClickAfter);
        }

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;

            var cflCardCode = UIAPIRawForm.ChooseFromLists.Item("cflCardCode");
            var cflCardName = UIAPIRawForm.ChooseFromLists.Item("cflCardName");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "CardType";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "S";

            cflCardCode.SetConditions(conditions);
            cflCardName.SetConditions(conditions);

            cmBPLId.ExpandType = BoExpandType.et_DescriptionOnly;
            mtItems.Columns.Item("cItemGroup").ExpandType = BoExpandType.et_DescriptionOnly;
            mtItems.Columns.Item("cFamilia").ExpandType = BoExpandType.et_DescriptionOnly;
            mtItems.Columns.Item("cSFamilia").ExpandType = BoExpandType.et_DescriptionOnly;
            mtItems.Columns.Item("StorageCat").ExpandType = BoExpandType.et_DescriptionOnly;
            mtItems.Columns.Item("BUsage").ExpandType = BoExpandType.et_DescriptionOnly;
            mtItems.Columns.Item("SUsage").ExpandType = BoExpandType.et_DescriptionOnly;
            mtItems.Columns.Item("ListNum").ExpandType = BoExpandType.et_DescriptionOnly;

            cmBPLId.AddValuesFromQuery(@"select ""BPLId"", ""BPLName"" from OBPL order by ""BPLName""", "BPLId", "BPLName");
            mtItems.Columns.Item("cItemGroup").AddValuesFromQuery(@"select ""ItmsGrpCod"", ""ItmsGrpNam"" from OITB order by ""ItmsGrpNam""", "ItmsGrpCod", "ItmsGrpNam");
            mtItems.Columns.Item("cFamilia").AddValuesFromQuery(@"select ""Code"", ""Name"" from ""@CVA_FAMILIA"" order by ""Name""", "Code", "Name");
            mtItems.Columns.Item("cSFamilia").AddValuesFromQuery(@"select ""Code"", ""Name"" from ""@CVA_SUBFAMILA"" order by ""Name""", "Code", "Name");
            mtItems.Columns.Item("StorageCat").AddValuesFromQuery(@"select ""U_Description"" from ""@CVA_SCT1"" order by ""U_Description""", "U_Description", "U_Description");
            mtItems.Columns.Item("BUsage").AddValuesFromQuery(@"select ""ID"", ""Usage"" from OUSG order by ""Usage""", "ID", "Usage");
            mtItems.Columns.Item("SUsage").AddValuesFromQuery(@"select ""ID"", ""Usage"" from OUSG order by ""Usage""", "ID", "Usage");
            mtItems.Columns.Item("ListNum").AddValuesFromQuery(@"select ""ListNum"", ""ListName"" from OPLN order by ""ListName""", "ListNum", "ListName");
            
            InsertNewRow();
        }

        #region [ Menu Events ]
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

        #region [ Item Events ]
        private void mtItems_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
                var dataTable = chooseFromListEvent.SelectedObjects;

                if (dataTable == null) return;

                var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

                mtItems.FlushToDataSource();

                switch (pVal.ColUID)
                {
                    case "ItemCode":
                    case "ItemName":
                        dbDataSource.SetValue("U_ItemCode", pVal.Row - 1, dataTable.GetValue("ItemCode", 0).ToString());
                        dbDataSource.SetValue("U_ItemName", pVal.Row - 1, dataTable.GetValue("ItemName", 0).ToString());
                        break;

                    case "CardCode":
                    case "CardName":
                        dbDataSource.SetValue("U_CardCode", pVal.Row - 1, dataTable.GetValue("CardCode", 0).ToString());
                        dbDataSource.SetValue("U_CardName", pVal.Row - 1, dataTable.GetValue("CardName", 0).ToString());
                        break;

                    case "Calendar":
                        dbDataSource.SetValue("U_Calendar", pVal.Row - 1, dataTable.GetValue("Code", 0).ToString());
                        break;

                    case "NDelivery":
                        dbDataSource.SetValue("U_NonDeliveryDate", pVal.Row - 1, dataTable.GetValue("Code", 0).ToString());
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

                mtItems.LoadFromDataSourceEx();
                mtItems.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message);
                throw;
            }
        }

        private void mtItems_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

            if (pVal.ItemChanged && pVal.Row - 1 == dataSource.Offset)
            {
                mtItems.FlushToDataSource();
                InsertNewRow();
            }
        }

        private void mtItems_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

            if (pVal.ItemChanged && pVal.Row - 1 == dataSource.Offset)
            {
                mtItems.FlushToDataSource();
                InsertNewRow();
            }
        }

        private void mtItems_LinkPressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            CommonController.FormFatherType = UIAPIRawForm.TypeEx;
            CommonController.FormFatherCount = UIAPIRawForm.TypeCount;
            CommonController.FatherRowNumber = pVal.Row - 1;

            switch (pVal.ColUID)
            {
                case "Calendar":
                    {
                        var activeForm = new DeliveryCalendar();
                        activeForm.Show();
                        BubbleEvent = false;
                        return;
                    }

                case "NDelivery":
                    {
                        var activeForm = new NonDeliveryDates();
                        activeForm.Show();
                        BubbleEvent = false;
                        return;
                    }
            }

            BubbleEvent = true;
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtItems")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtItems")
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
            RemoveLastRow();
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

            mtItems.LoadFromDataSourceEx(false);

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            dataSource.InsertRow(mtItems.RowCount == 0);

            mtItems.LoadFromDataSourceEx(false);
            mtItems.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        private void RemoveRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

            mtItems.FlushToDataSource();
            dataSource.RemoveRecord(_menuRow);
            mtItems.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private void RemoveLastRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            dataSource.RemoveRecord(dataSource.Offset);
        }
        #endregion

        private SAPbouiCOM.ComboBox cmBPLId;
        private SAPbouiCOM.Matrix mtItems;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
    }
}
