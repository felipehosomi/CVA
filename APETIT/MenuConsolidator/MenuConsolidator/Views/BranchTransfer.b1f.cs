using SAPbouiCOM.Framework;
using MenuConsolidator.Controllers;
using System;

namespace MenuConsolidator.Views
{
    [FormAttribute("MenuConsolidator.Views.BranchTransfer", "Views/BranchTransfer.b1f")]
    class BranchTransfer : UserFormBase
    {
        public BranchTransfer()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtTransfer = ((SAPbouiCOM.Matrix)(this.GetItem("mtTransfer").Specific));
            this.mtTransfer.DoubleClickAfter += new SAPbouiCOM._IMatrixEvents_DoubleClickAfterEventHandler(this.mtTransfer_DoubleClickAfter);
            this.mtTransfer.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtTransfer_LinkPressedBefore);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.et_Order = ((SAPbouiCOM.EditText)(this.GetItem("et_Order").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.et_BPCode = ((SAPbouiCOM.EditText)(this.GetItem("et_BPCode").Specific));
            this.et_BPName = ((SAPbouiCOM.EditText)(this.GetItem("et_BPName").Specific));
            this.et_BPLId = ((SAPbouiCOM.EditText)(this.GetItem("et_BPLId").Specific));
            this.bt_Search = ((SAPbouiCOM.Button)(this.GetItem("bt_Search").Specific));
            this.bt_Search.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.bt_Search_ClickBefore);
            this.et_BPLName = ((SAPbouiCOM.EditText)(this.GetItem("et_BPLName").Specific));
            this.et_BPCode.ChooseFromListAfter += this.Et_BPCode_ChooseFromListAfter;
            this.et_BPName.ChooseFromListAfter += this.Et_BPCode_ChooseFromListAfter;
            this.et_BPLId.ChooseFromListAfter += this.Et_BPLId_ChooseFromListAfter;
            this.et_BPLName.ChooseFromListAfter += this.Et_BPLId_ChooseFromListAfter;
            this.et_Escape = ((SAPbouiCOM.EditText)(this.GetItem("et_Escape").Specific));
            this.OnCustomInitialize();

        }

        private void Et_BPLId_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SAPbouiCOM.SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            UIAPIRawForm.DataSources.UserDataSources.Item("ud_BPLId").Value = dataTable.GetValue("BPLId", 0).ToString();
            UIAPIRawForm.DataSources.UserDataSources.Item("ud_BPLName").Value = dataTable.GetValue("BPLName", 0).ToString();
        }

        private void Et_BPCode_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SAPbouiCOM.SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            UIAPIRawForm.DataSources.UserDataSources.Item("ud_BPCode").Value = dataTable.GetValue("CardCode", 0).ToString();
            UIAPIRawForm.DataSources.UserDataSources.Item("ud_BPName").Value = dataTable.GetValue("CardFName", 0).ToString();
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private void OnCustomInitialize()
        {
            UIAPIRawForm.EnableMenu("1281", false);
            UIAPIRawForm.EnableMenu("1282", false);

            var cf_CardCode = UIAPIRawForm.ChooseFromLists.Item("cf_BPCode");
            var cf_CardName = UIAPIRawForm.ChooseFromLists.Item("cf_Name");
            var conditions = new SAPbouiCOM.Conditions();
            var condition = conditions.Add();
            condition.Alias = "CardType";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            condition.CondVal = "S";

            cf_CardCode.SetConditions(conditions);
            cf_CardName.SetConditions(conditions);

            var cf_BPLId = UIAPIRawForm.ChooseFromLists.Item("cf_BPLId");
            var cf_BPLName = UIAPIRawForm.ChooseFromLists.Item("cf_BPLName");
            conditions = new SAPbouiCOM.Conditions();
            condition = conditions.Add();
            condition.Alias = "Disabled";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            condition.CondVal = "N";

            cf_BPLId.SetConditions(conditions);
            cf_BPLName.SetConditions(conditions);
        }

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var dtTransfer = UIAPIRawForm.DataSources.DataTables.Item("dtTransfer");
            var counter = 0;
            mtTransfer.FlushToDataSource();

            for (var i = 0; i < dtTransfer.Rows.Count; i++)
            {
                if (dtTransfer.GetValue("Select", i).ToString() == "N") continue;
                counter++;
            }

            var progressBar = Application.SBO_Application.StatusBar.CreateProgressBar("Gerando previsões de transferência entre filiais.", counter, true);
            progressBar.Text = "Gerando previsões de transferência entre filiais.";

            try
            {
                for (var i = 0; i < dtTransfer.Rows.Count; i++)
                {
                    if (dtTransfer.GetValue("Select", i).ToString() == "N") continue;

                    BranchTransferController.SetTransfer(int.Parse(dtTransfer.GetValue("DocEntry", i).ToString()), dtTransfer.GetValue("ShowSCN", i).ToString() == "Y");
                    progressBar.Value++;
                }

                GetOpenTransfers();

                Application.SBO_Application.StatusBar.SetText("Operação completada com êxito  [Mensagem 200-48]", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);
            }
            finally
            {
                progressBar.Stop();
            }
        }

        private void mtTransfer_LinkPressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID != "DocNum") return;

            var dtTransfer = UIAPIRawForm.DataSources.DataTables.Item("dtTransfer");
            var docEntry = dtTransfer.GetValue("DocEntry", pVal.Row - 1);
            Application.SBO_Application.OpenForm((SAPbouiCOM.BoFormObjectEnum)22, "", docEntry.ToString());
            BubbleEvent = false;
        }

        private void mtTransfer_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.ColUID != "Select" && pVal.Row != 0) return;

            UIAPIRawForm.Freeze(true);
            var dtTransfer = UIAPIRawForm.DataSources.DataTables.Item("dtTransfer");
            mtTransfer.FlushToDataSource();

            var newValue = dtTransfer.GetValue("Select", 0).ToString() == "N" ? "Y" : "N";

            for (var i = 0; i < dtTransfer.Rows.Count; i++)
            {
                dtTransfer.SetValue("Select", i, newValue);
            }

            mtTransfer.LoadFromDataSourceEx();
            UIAPIRawForm.Freeze(false);
        }

        private void GetOpenTransfers()
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                et_Escape.Item.Visible = true;
                et_Escape.Item.Click();
                string orderNum = UIAPIRawForm.DataSources.UserDataSources.Item("ud_Order").Value;
                string cardCode = UIAPIRawForm.DataSources.UserDataSources.Item("ud_BPCode").Value;
                string branch = UIAPIRawForm.DataSources.UserDataSources.Item("ud_BPLId").Value;

                string where = String.Empty;
                if (!String.IsNullOrEmpty(orderNum))
                {
                    where += $@" AND OPOR.""DocNum"" = {orderNum}";
                }
                if (!String.IsNullOrEmpty(cardCode))
                {
                    where += $@" AND OPOR.""CardCode"" = '{cardCode}' ";
                }
                if (!String.IsNullOrEmpty(branch))
                {
                    where += $@" AND OPOR.""BPLId"" = {branch}";
                }

                var dtTransfer = UIAPIRawForm.DataSources.DataTables.Item("dtTransfer");
                var query = $@"select distinct 'Y' as ""Select"", OPOR.""DocEntry"", OPOR.""DocNum"", OPOR.""CardCode"", 
	                             OCRD.""CardFName"", OPOR.""BPLName"", case when exists (select * 
 					 	 from OSCN 
 					    inner join OITM on OITM.""ItemCode"" = OSCN.""ItemCode""

                         where OSCN.""CardCode"" = OPOR.""CardCode"" and OSCN.""ShowSCN"" = 'Y') then 'Y' else 'N' end
                         as ""ShowSCN"",
	                             case when case when exists (select *
                           from OSCN
                         inner join OITM on OITM.""ItemCode"" = OSCN.""ItemCode""

                         where OSCN.""CardCode"" = OPOR.""CardCode"" and OSCN.""ShowSCN"" = 'Y') then 'Y' else 'N' end = 'Y'

                                        and not exists (select * 
	                             					 	 from OSCN 
	                             					    inner join POR1 on POR1.""ItemCode"" = OSCN.""ItemCode""
	                             					      and POR1.""SubCatNum"" = OSCN.""Substitute""
	                             					      and POR1.""DocEntry"" = OPOR.""DocEntry""
	                             					    inner join OITM on OITM.""ItemCode"" = OSCN.""Substitute""
	                             					    where OSCN.""CardCode"" = OPOR.""CardCode"") then 'Existem itens sem nº de catálogo vinculados ao fornecedor' 
                            else CASE WHEN Frozen.""ItemCode"" IS NOT NULL
                                THEN 'Item ' || Frozen.""ItemCode"" || ' está inativo'
                                else ''
                                end
                            end as ""Status""
                            from OPOR
                            inner join OBPL on OBPL.""DflVendor"" = OPOR.""CardCode""
                           inner join OCRD on OCRD.""CardCode"" = OPOR.""CardCode""
                            left join OSCN on OSCN.""CardCode"" = OPOR.""CardCode""
                            left join
                            (
                                    SELECT POR1.""ItemCode"", POR1.""DocEntry"" 
                                    FROM POR1
                                        INNER JOIN OITM ON OITM.""ItemCode"" = POR1.""ItemCode"" AND OITM.""frozenFor"" = 'Y'
                            ) Frozen ON Frozen.""DocEntry"" = OPOR.""DocEntry""
                           where OPOR.""DocStatus"" = 'O'
                             and OPOR.""CANCELED"" = 'N'
                             {where}
                             and not exists (select *
 					                           from ""@CVA_OPTR"" as OPTR
 					                          where OPTR.""Code"" = OPOR.""DocEntry""
 					                            and OPTR.""U_DocType"" = OPOR.""ObjType""
 					                            and OPTR.""U_BPLId"" = OPOR.""BPLId"")
							order by OPOR.""DocNum""";

                dtTransfer.ExecuteQuery(query);

                if (dtTransfer.IsEmpty)
                {
                    UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                }
                else
                {
                    UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE;
                }

                mtTransfer.Columns.Item("Select").DataBind.Bind("dtTransfer", "Select");
                mtTransfer.Columns.Item("DocEntry").DataBind.Bind("dtTransfer", "DocEntry");
                mtTransfer.Columns.Item("DocNum").DataBind.Bind("dtTransfer", "DocNum");
                mtTransfer.Columns.Item("CardCode").DataBind.Bind("dtTransfer", "CardCode");
                mtTransfer.Columns.Item("CardName").DataBind.Bind("dtTransfer", "CardFName");
                mtTransfer.Columns.Item("BPLName").DataBind.Bind("dtTransfer", "BPLName");
                mtTransfer.Columns.Item("ShowSCN").DataBind.Bind("dtTransfer", "ShowSCN");
                mtTransfer.Columns.Item("Status").DataBind.Bind("dtTransfer", "Status");
                mtTransfer.LoadFromDataSourceEx();

                for (var i = 0; i < dtTransfer.Rows.Count; i++)
                {
                    if (String.IsNullOrEmpty(dtTransfer.GetValue("Status", i).ToString())) continue;

                    mtTransfer.CommonSetting.SetRowEditable(i + 1, false);
                    dtTransfer.SetValue("Select", i, "N");
                }

                mtTransfer.LoadFromDataSourceEx();
                mtTransfer.AutoResizeColumns();
                
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                et_BPCode.Item.Click();
                et_Escape.Item.Visible = false;
                UIAPIRawForm.Freeze(false);
            }
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            mtTransfer.AutoResizeColumns();
        }

        private void bt_Search_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            GetOpenTransfers();
        }

        private SAPbouiCOM.Matrix mtTransfer;

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText et_Order;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText et_BPCode;
        private SAPbouiCOM.EditText et_BPName;
        private SAPbouiCOM.EditText et_BPLId;
        private SAPbouiCOM.Button bt_Search;
        private SAPbouiCOM.EditText et_BPLName;
        private SAPbouiCOM.EditText et_Escape;
    }
}
