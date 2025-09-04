using MenuPlanner.Controllers;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.DayQuantity", "Views/DayQuantity.b1f")]
    class DayQuantity : UserFormBase
    {
        public static bool Editable;

        public DayQuantity()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.btOK = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.btOK.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btOK_PressedBefore);
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.mtQty = ((SAPbouiCOM.Matrix)(this.GetItem("mtQty").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("etDate").Specific));
            this.OnCustomInitialize();
            this.mtQty.Item.Enabled = Editable;

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.VisibleAfter += new SAPbouiCOM.Framework.FormBase.VisibleAfterHandler(this.Form_VisibleAfter);
            this.CloseBefore += new CloseBeforeHandler(this.Form_CloseBefore);

        }

        private void OnCustomInitialize()
        {
            var fatherForm = Application.SBO_Application.Forms.GetForm(CommonController.FormFatherType, CommonController.FormFatherCount);
            var plannerData = fatherForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
            var mnp2 = fatherForm.DataSources.DBDataSources.Item("@CVA_MNP2");
            var dtMenu = fatherForm.DataSources.DataTables.Item("dtMenu");
            var day = dtMenu.GetValue("Day", int.Parse(fatherForm.DataSources.UserDataSources.Item("MenuRow").Value) - 1).ToString().Substring(0, 2);
            var weekday = dtMenu.GetValue("Day", int.Parse(fatherForm.DataSources.UserDataSources.Item("MenuRow").Value) - 1).ToString().Substring(5, 3);

            var dtQty = UIAPIRawForm.DataSources.DataTables.Item("dtQty");

            UIAPIRawForm.DataSources.UserDataSources.Item("Date").Value = $"{plannerData.GetValue("U_CVA_DATA_REF", plannerData.Offset).Substring(0, 6)}{day}";

            for (var i = 0; i < mnp2.Size; i++)
            {
                if (mnp2.GetValue("U_Day", i) != day) continue;

                dtQty.Rows.Add();
                dtQty.SetValue("LineId", dtQty.Rows.Count - 1, int.Parse(mnp2.GetValue("LineId", i)) - 1);
                dtQty.SetValue("Shift", dtQty.Rows.Count - 1, mnp2.GetValue("U_Shift", i));
                dtQty.SetValue("Qty", dtQty.Rows.Count - 1, int.Parse(mnp2.GetValue("U_Quantity", i)));
            }

            mtQty.LoadFromDataSourceEx();
            mtQty.AutoResizeColumns();
            mtQty.Columns.Item("Qty").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            mtQty.Item.Update();
        }

        private void Form_VisibleAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            var formWidth = UIAPIRawForm.ClientWidth;
            var formHeight = UIAPIRawForm.ClientHeight;

            // Centralização do form
            UIAPIRawForm.Left = int.Parse(((Application.SBO_Application.Desktop.Width - formWidth) / 2).ToString(CultureInfo.InvariantCulture));
            UIAPIRawForm.Top = int.Parse(((Application.SBO_Application.Desktop.Height - formHeight) / 3).ToString(CultureInfo.InvariantCulture));
        }

        private void Form_CloseBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            CommonController.FormFatherType = String.Empty;
            CommonController.FormFatherCount = -1;
        }

        private void btOK_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) return;

            var fatherForm = Application.SBO_Application.Forms.GetForm(CommonController.FormFatherType, CommonController.FormFatherCount);
            var mnp2 = fatherForm.DataSources.DBDataSources.Item("@CVA_MNP2");
            var dtQty = UIAPIRawForm.DataSources.DataTables.Item("dtQty");

            mtQty.FlushToDataSource();

            for (var i = 0; i < dtQty.Rows.Count; i++)
            {
                mnp2.SetValue("U_Quantity", int.Parse(dtQty.GetValue("LineId", i).ToString()), dtQty.GetValue("Qty", i).ToString());
            }

            if (fatherForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
            {
                fatherForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
            }

        }

        private SAPbouiCOM.Matrix mtQty;
        private SAPbouiCOM.Button btOK;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText EditText0;
    }
}
