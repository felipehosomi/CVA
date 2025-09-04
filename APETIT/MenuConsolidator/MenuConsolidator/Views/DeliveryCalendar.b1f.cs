using MenuConsolidator.Controllers;
using MenuConsolidator.Extensions;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MenuConsolidator.Views
{
    [FormAttribute("MenuConsolidator.Views.DeliveryCalendar", "Views/DeliveryCalendar.b1f")]
    class DeliveryCalendar : UserFormBase
    {
        private string RowsDataSource = "@CVA_CLN1";
        private static int _menuRow;

        public DeliveryCalendar()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.mtCalendar = ((SAPbouiCOM.Matrix)(this.GetItem("mtCalendar").Specific));
            this.mtCalendar.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtCalendar_ValidateAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("etCode").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("etName").Specific));
            this.cmPeriod = ((SAPbouiCOM.ComboBox)(this.GetItem("cmPeriod").Specific));
            this.cmPeriod.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cmPeriod_ComboSelectAfter);
            this.etYear = ((SAPbouiCOM.EditText)(this.GetItem("etYear").Specific));
            this.etYear.ValidateAfter += new SAPbouiCOM._IEditTextEvents_ValidateAfterEventHandler(this.etYear_ValidateAfter);
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

            var ocln = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OCLN");
            var cln1 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_CLN1");

            if (!String.IsNullOrEmpty(CommonController.FormFatherType))
            {
                var fatherForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(CommonController.FormFatherType, CommonController.FormFatherCount);
                var fatherData = fatherForm.DataSources.DBDataSources.Item("@CVA_PAM1");

                var conditions = new Conditions();
                var condition = conditions.Add();
                condition.Alias = "Code";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = fatherData.GetValue("U_Calendar", CommonController.FatherRowNumber);
                ocln.Query(conditions);
                cln1.Query(conditions);

                UIAPIRawForm.Mode = BoFormMode.fm_OK_MODE;

                CommonController.FormFatherType = String.Empty;
                CommonController.FormFatherCount = -1;
                CommonController.FatherRowNumber = -1;
            }
            else
            {
                ocln.SetValue("U_Year", ocln.Offset, DateTime.Now.Year.ToString());
            }

            InsertNewRow();

            cmPeriod.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
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
                        var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OCLN");
                        dataSource.SetValue("U_Year", dataSource.Offset, DateTime.Now.Year.ToString());
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
        private void cmPeriod_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (!pVal.ItemChanged) return;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OCLN");
            var calendarDates = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_CLN1");
            var period = int.Parse(dataSource.GetValue("U_Period", dataSource.Offset));
            var year = 0;
                
            if (!int.TryParse(dataSource.GetValue("U_Year", dataSource.Offset), out year))
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Indicar o ano do calendário.", BoMessageTime.bmt_Short);
                return;
            }

            if (period == 8)
            {
                calendarDates.Clear();
                InsertNewRow();
                etYear.Item.Enabled = false;
                return;
            }

            SetDates(period, year);

            etYear.Item.Enabled = true;
        }

        private void etYear_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (!pVal.ItemChanged) return;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OCLN");
            var period = int.Parse(dataSource.GetValue("U_Period", dataSource.Offset));
            var year = int.Parse(dataSource.GetValue("U_Year", dataSource.Offset));

            SetDates(period, year);
        }

        private void mtCalendar_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

            if (pVal.ColUID == "Date" && pVal.ItemChanged && pVal.Row - 1 == dataSource.Offset)
            {
                mtCalendar.FlushToDataSource();
                var data = DateTime.ParseExact(dataSource.GetValue("U_Date", pVal.Row - 1), "yyyyMMdd", null);
                dataSource.SetValue("U_Weekday", pVal.Row - 1, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.ToString("dddd", new CultureInfo("pt-BR"))));
                InsertNewRow();
            }
        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            mtCalendar.AutoResizeColumns();
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtCalendar")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtCalendar")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = false;
                _menuRow = -1;
            }
        }

        private void Button0_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (UIAPIRawForm.Mode != BoFormMode.fm_ADD_MODE) return;

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_OCLN");
            dataSource.SetValue("U_Year", dataSource.Offset, DateTime.Now.Year.ToString());

            etYear.Item.Enabled = false;
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

            mtCalendar.LoadFromDataSourceEx(false);

            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            dataSource.InsertRow(mtCalendar.RowCount == 0);

            mtCalendar.LoadFromDataSourceEx(false);
            mtCalendar.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        private void RemoveRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);

            mtCalendar.FlushToDataSource();
            dataSource.RemoveRecord(_menuRow);
            mtCalendar.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private void RemoveLastRow()
        {
            var dataSource = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            dataSource.RemoveRecord(dataSource.Offset);
        }

        private void SetDates(int period, int year)
        {
            var calendarDates = UIAPIRawForm.DataSources.DBDataSources.Item(RowsDataSource);
            calendarDates.Clear();
            calendarDates.InsertRow(false);

            SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Obtendo todas as datas do ano, aguarde", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            UIAPIRawForm.Freeze(true);

            foreach (var data in GetAllDatesInYear(year).Where(i => i.DayOfWeek == (DayOfWeek)period))
            {
                calendarDates.SetValue("U_Date", calendarDates.Size - 1, data.ToString("yyyyMMdd"));
                calendarDates.SetValue("U_Weekday", calendarDates.Size - 1, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.ToString("dddd", new CultureInfo("pt-BR"))));
                InsertNewRow();
            }

            UIAPIRawForm.Freeze(false);
        }

        private IEnumerable<DateTime> GetAllDatesInYear(int year)
        {
            for (var month = 1; month <= 12; month++)
            {
                var days = DateTime.DaysInMonth(year, month);

                for (int day = 1; day <= days; day++)
                {
                    yield return new DateTime(year, month, day);
                }
            }
        }
        #endregion

        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.Matrix mtCalendar;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.ComboBox cmPeriod;
        private SAPbouiCOM.EditText etYear;
    }
}
