using System;
using SAPbouiCOM.Framework;
using MenuPlanner.Controllers;
using SAPbobsCOM;
using SAPbouiCOM;
using MenuPlanner.Extensions;

namespace MenuPlanner.Views
{
    [FormAttribute("1250000100", "Views/BlanketAgreement.b1f")]
    class BlanketAgreement : SystemFormBase
    {
        private static int _menuRow;

        public BlanketAgreement()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.tbStdCost = ((SAPbouiCOM.Folder)(this.GetItem("tbStdCost").Specific));
            this.tbStdCost.PressedAfter += new SAPbouiCOM._IFolderEvents_PressedAfterEventHandler(this.tbStdCost_PressedAfter);
            this.mtStdCost = ((SAPbouiCOM.Matrix)(this.GetItem("mtStdCost").Specific));
            this.mtStdCost.ComboSelectAfter += new SAPbouiCOM._IMatrixEvents_ComboSelectAfterEventHandler(this.mtStdCost_ComboSelectAfter);
            this.mtStdCost.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtStdCost_ValidateAfter);
            this.mtStdCost.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtStdCost_ChooseFromListAfter);
            this.cmBPLId = ((SAPbouiCOM.ComboBox)(this.GetItem("cmBPLId").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;
            this.DataUpdateAfter += new SAPbouiCOM.Framework.FormBase.DataUpdateAfterHandler(this.Form_DataUpdateAfter);
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);
            this.RightClickBefore += new RightClickBeforeHandler(this.Form_RightClickBefore);
            this.RightClickAfter += new RightClickAfterHandler(this.Form_RightClickAfter);
        }

        private void OnCustomInitialize()
        {
            tbStdCost.Item.Top = UIAPIRawForm.Items.Item("1320000072").Top;
            tbStdCost.Item.Height = UIAPIRawForm.Items.Item("1320000072").Height;
            tbStdCost.Item.Width = UIAPIRawForm.Items.Item("1320000072").Width;
            tbStdCost.Item.Left = (UIAPIRawForm.Items.Item("1320000072").Left + UIAPIRawForm.Items.Item("1320000072").Width);
            tbStdCost.Item.Enabled = UIAPIRawForm.Items.Item("1320000072").Enabled;
            tbStdCost.Item.AffectsFormMode = false;
            tbStdCost.GroupWith("1320000072");

            mtStdCost.Item.Top = UIAPIRawForm.Items.Item("1250000046").Top;
            mtStdCost.Item.Height = UIAPIRawForm.Items.Item("1250000046").Height;
            mtStdCost.Item.Width = UIAPIRawForm.Items.Item("1250000046").Width;
            mtStdCost.Item.Left = UIAPIRawForm.Items.Item("1250000046").Left;
            mtStdCost.Columns.Item("SrvId").Visible = false;
            mtStdCost.Columns.Item("Month").ExpandType = BoExpandType.et_ValueOnly;
            mtStdCost.AutoResizeColumns();

            cmBPLId.ExpandType = BoExpandType.et_DescriptionOnly;
            cmBPLId.AddValuesFromQuery(@"select ""BPLId"", ""BPLName"" from OBPL where ""Disabled"" = 'N'", "BPLId", "BPLName");
        }

        private void tbStdCost_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (!CommonController.LazyProcess)
            {
                UIAPIRawForm.PaneLevel = tbStdCost.Pane;
                return;
            }

            GetStandartCost();
            InsertNewRow();

            CommonController.LazyProcess = false;
            UIAPIRawForm.PaneLevel = tbStdCost.Pane;
        }

        private void Form_DataUpdateAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            if (mtStdCost.RowCount > 0)
            {
                var dtStdCost = UIAPIRawForm.DataSources.DataTables.Item("dtStdCost");
                var number = UIAPIRawForm.DataSources.DBDataSources.Item("OOAT").GetValue("Number", 0);
                var recordset = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery($@"delete from ""@CVA_CUSTO_PADRAO"" where ""U_CVA_Contrato"" = '{number}';");

                mtStdCost.FlushToDataSource();

                for (int i = 0; i < dtStdCost.Rows.Count - 1; i++)
                {
                    var month = dtStdCost.GetValue("Month", i);
                    var srvId = dtStdCost.GetValue("SrvId", i);
                    var srv = dtStdCost.GetValue("Srv", i);
                    var value = dtStdCost.GetValue("Value", i);

                    recordset.DoQuery($@"insert into ""@CVA_CUSTO_PADRAO"" 
                                         values ('{number}_{i}', '{number}_{i}', '{month}', '{number}', 
                                                 '{value.ToString().Replace(",", ".")}', '{srvId}', '{srv}');");
                }
            }
        }

        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            if (CommonController.LazyProcess) return;

            GetStandartCost();

            if (CommonController.LazyProcess) return;

            InsertNewRow();
        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            mtStdCost.AutoResizeColumns();
        }

        private void mtStdCost_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
                var dataTable = chooseFromListEvent.SelectedObjects;

                if (dataTable == null) return;

                var dtStdCost = UIAPIRawForm.DataSources.DataTables.Item("dtStdCost");
                mtStdCost.FlushToDataSource();

                switch (pVal.ColUID)
                {
                    case "Srv":
                        dtStdCost.SetValue("SrvId", pVal.Row - 1, dataTable.GetValue("Code", 0).ToString());
                        dtStdCost.SetValue("Srv", pVal.Row - 1, dataTable.GetValue("Name", 0).ToString());
                        break;
                }

                mtStdCost.LoadFromDataSource();

                if (pVal.Row == dtStdCost.Rows.Count)
                {
                    InsertNewRow();
                }

                if (UIAPIRawForm.Mode == BoFormMode.fm_OK_MODE)
                {
                    UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message);
                throw;
            }
        }

        private void mtStdCost_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!pVal.ItemChanged || UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private void mtStdCost_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!pVal.ItemChanged || UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        private void GetStandartCost()
        {
            try
            {
                var dtStdCost = UIAPIRawForm.DataSources.DataTables.Item("dtStdCost");
                var ooat = UIAPIRawForm.DataSources.DBDataSources.Item("OOAT");
                var startDate = DateTime.ParseExact(ooat.GetValue("StartDate", ooat.Offset), "yyyyMMdd", null);
                var endDate = DateTime.ParseExact(ooat.GetValue("EndDate", ooat.Offset), "yyyyMMdd", null);
                var currentDate = new DateTime(startDate.Year, startDate.Month, 1, 0, 0, 0);

                while (mtStdCost.Columns.Item("Month").ValidValues.Count > 0)
                {
                    mtStdCost.Columns.Item("Month").ValidValues.Remove(0, BoSearchKey.psk_Index);
                }

                while (currentDate <= endDate)
                {
                    mtStdCost.Columns.Item("Month").ValidValues.Add(currentDate.ToString("MM/yyyy"), currentDate.ToString("MM/yyyy"));
                    currentDate = currentDate.AddMonths(1);
                }

                dtStdCost.ExecuteQuery($@"select ""U_CVA_Mes"" as ""Month"", ""U_CVA_Id_Servico"" as ""SrvId"", 
	                                         ""U_CVA_Des_Servico"" as ""Srv"", ""U_CVA_Valor"" as ""Value"" 
                                        from ""@CVA_CUSTO_PADRAO""
                                       where ""U_CVA_Contrato"" = {ooat.GetValue("Number", ooat.Offset)}");

                mtStdCost.LoadFromDataSourceEx();
            }
            catch
            {
                CommonController.LazyProcess = true;
            }
        }

        private void InsertNewRow()
        {
            UIAPIRawForm.Freeze(true);

            mtStdCost.FlushToDataSource();

            var dtStdCost = UIAPIRawForm.DataSources.DataTables.Item("dtStdCost");
            dtStdCost.AddRow(1, mtStdCost.RowCount == 0);

            mtStdCost.LoadFromDataSourceEx(false);
            mtStdCost.AutoResizeColumns();

            UIAPIRawForm.Freeze(false);
        }

        #region MenuEvents
        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtStdCost")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtStdCost")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveRow").Enabled = false;
                _menuRow = -1;
            }
        }

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

        private void RemoveRow()
        {
            mtStdCost.FlushToDataSource();
            var dtStdCost = UIAPIRawForm.DataSources.DataTables.Item("dtStdCost");
            dtStdCost.Rows.Remove(_menuRow);
            mtStdCost.LoadFromDataSourceEx();

            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE) return;

            UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
        }
        #endregion

        private SAPbouiCOM.Folder tbStdCost;
        private SAPbouiCOM.Matrix mtStdCost;
        private ComboBox cmBPLId;
    }
}
