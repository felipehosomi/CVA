using MenuConsolidator.Controllers;
using MenuConsolidator.Extensions;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;

namespace MenuConsolidator.Views
{
    [FormAttribute("MenuConsolidator.Views.NonDeliveryDates", "Views/NonDeliveryDates.b1f")]
    class NonDeliveryDates : UserFormBase
    {
        private string RowsDataSource = "@CVA_NDL1";
        private static int _menuRow;

        public NonDeliveryDates()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("etCode").Specific));
            this.mtDates = ((SAPbouiCOM.Matrix)(this.GetItem("mtDates").Specific));
            this.mtDates.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtDates_ValidateAfter);
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

            var ondl = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_ONDL");
            var ndl1 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_NDL1");

            if (!String.IsNullOrEmpty(CommonController.FormFatherType))
            {
                var fatherForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(CommonController.FormFatherType, CommonController.FormFatherCount);
                var fatherData = fatherForm.DataSources.DBDataSources.Item("@CVA_PAM1");

                var conditions = new Conditions();
                var condition = conditions.Add();
                condition.Alias = "Code";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = fatherData.GetValue("U_NonDeliveryDate", CommonController.FatherRowNumber);
                ondl.Query(conditions);
                ndl1.Query(conditions);

                UIAPIRawForm.Mode = BoFormMode.fm_OK_MODE;

                CommonController.FormFatherType = String.Empty;
                CommonController.FormFatherCount = -1;
                CommonController.FatherRowNumber = -1;
            }

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
        private void mtDates_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

            if (pVal.ItemChanged && pVal.Row - 1 == dataSource.Offset)
            {
                InsertNewRow();
            }
        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            mtDates.AutoResizeColumns();
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtDates")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtDates")
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

        private void Form_DataAddAfter(ref BusinessObjectInfo pVal)
        {
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

            mtDates.LoadFromDataSourceEx(false);

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            dataSource.InsertRow(mtDates.RowCount == 0);

            mtDates.LoadFromDataSourceEx(false);
            mtDates.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        private void RemoveRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

            mtDates.FlushToDataSource();
            dataSource.RemoveRecord(_menuRow);
            mtDates.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private void RemoveLastRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            dataSource.RemoveRecord(dataSource.Offset);
        }
        #endregion

        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.Matrix mtDates;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
    }
}
