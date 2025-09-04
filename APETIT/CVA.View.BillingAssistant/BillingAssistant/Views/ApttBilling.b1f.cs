using System;
using SAPbouiCOM.Framework;
using SAPbouiCOM;
using BillingAssistant.ExtensionMethods;
using BillingAssistant.Controllers;
using Application = SAPbouiCOM.Framework.Application;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using BillingAssistant.Infrastructure;

namespace BillingAssistant.Views
{
    [FormAttribute("BillingAssistant.Views.Billing", "Views/ApttBilling.b1f")]
    class ApttBilling : UserFormBase
    {
        public ApttBilling()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.etBPLID = ((SAPbouiCOM.EditText)(this.GetItem("etBPLID").Specific));
            this.etBPLID.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etBPL_ChooseFromListAfter);
            this.etBPLID.DataBind.SetBound(true, "", "udsBplId");
            this.etBPLName = ((SAPbouiCOM.EditText)(this.GetItem("etBPLName").Specific));
            this.etBPLName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etBPL_ChooseFromListAfter);
            this.etCode = ((SAPbouiCOM.EditText)(this.GetItem("etCode").Specific));
            this.etCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCode_ChooseFromListAfter);
            this.mtItems = ((SAPbouiCOM.Matrix)(this.GetItem("mtItems").Specific));
            this.mtItems.DatasourceLoadAfter += new SAPbouiCOM._IMatrixEvents_DatasourceLoadAfterEventHandler(this.mtItems_DatasourceLoadAfter);
            this.mtItems.DatasourceLoadBefore += new SAPbouiCOM._IMatrixEvents_DatasourceLoadBeforeEventHandler(this.mtItems_DatasourceLoadBefore);
            this.lnkAgr = ((SAPbouiCOM.LinkedButton)(this.GetItem("lnkAgr").Specific));
            this.mtItems = ((SAPbouiCOM.Matrix)(this.GetItem("mtItems").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button0_ClickAfter);
            this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.etDscript = ((SAPbouiCOM.EditText)(this.GetItem("etDscript").Specific));
            this.etDscript.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCode_ChooseFromListAfter);
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("BtUpdate").Specific));
            this.Button2.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button2_ClickAfter);
            this.Button2.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button2_ClickBefore);
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("etDateF").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("etDateT").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("lBranch").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("lAgrNo").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("lData").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("lDataF").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.etLnkAgr = ((SAPbouiCOM.EditText)(this.GetItem("etLnkAgr").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("etTaxDate").Specific));
            this.EditText3 = ((SAPbouiCOM.EditText)(this.GetItem("etDocDate").Specific));
            this.EditText4 = ((SAPbouiCOM.EditText)(this.GetItem("etDueDate").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.EditText5 = ((SAPbouiCOM.EditText)(this.GetItem("etItemFrm").Specific));
            this.EditText5.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.itemCode_ChooseFromListAfter);
            this.EditText6 = ((SAPbouiCOM.EditText)(this.GetItem("etItemTo").Specific));
            this.EditText6.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.itemCode_ChooseFromListAfter);
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_9").Specific));
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_10").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_11").Specific));
            this.cflItemFrm = this.UIAPIRawForm.ChooseFromLists.Item("cflItemFrm");
            this.cflItemTo = this.UIAPIRawForm.ChooseFromLists.Item("cflItemTo");
            this.cflCardCode = this.UIAPIRawForm.ChooseFromLists.Item("cflCardCode");
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private UserDataSource udsTaxDate;
        private UserDataSource udsDueDate;
        private UserDataSource udsDocDate;
        private UserDataSource udsBplId;
        private UserDataSource udsAgrNum;
        private UserDataSource udsDateF;
        private UserDataSource udsDateT;
        private UserDataSource udsItemFrm;
        private UserDataSource udsItemTo;

        private ChooseFromList cflItemFrm;
        private ChooseFromList cflItemTo;
        private ChooseFromList cflCardCode;

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;
            this.dtItems = UIAPIRawForm.DataSources.DataTables.Item("dtItems");

            udsTaxDate = UIAPIRawForm.DataSources.UserDataSources.Item("udsTaxDate");
            udsDueDate = UIAPIRawForm.DataSources.UserDataSources.Item("udsDueDate");
            udsDocDate = UIAPIRawForm.DataSources.UserDataSources.Item("udsDocDate");
            udsBplId = UIAPIRawForm.DataSources.UserDataSources.Item("udsBplId");
            udsAgrNum = UIAPIRawForm.DataSources.UserDataSources.Item("udsAgrNum");
            udsDateF = UIAPIRawForm.DataSources.UserDataSources.Item("udsDateF");
            udsDateT = UIAPIRawForm.DataSources.UserDataSources.Item("udsDateT");
            udsItemFrm = UIAPIRawForm.DataSources.UserDataSources.Item("udsItemFrm");
            udsItemTo = UIAPIRawForm.DataSources.UserDataSources.Item("udsItemTo");

            udsTaxDate.Value = DateTime.Now.ToString("ddMMyyyy");
            udsDocDate.Value = DateTime.Now.ToString("ddMMyyyy");
            
            mtItems.AutoResizeColumns();

            SetCflConditions();
        }

        private void SetCflConditions()
        {
            
            var cfl4Conditions = cflItemFrm.GetConditions();

            var condition = cfl4Conditions.Add();
            condition.Alias = "ItmsGrpCod";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "111";

            cflItemFrm.SetConditions(cfl4Conditions);
            cflItemTo.SetConditions(cfl4Conditions);

            var cfl2Conditions = cflCardCode.GetConditions();

            condition = cfl2Conditions.Add();
            condition.Alias = "CardType";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "C";

            cflCardCode.SetConditions(cfl2Conditions);
        }

        #region MenuEvents
        private void MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (UIAPIRawForm == null) return;
            if (UIAPIRawForm.UniqueID != SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.UniqueID) return;

            if (!pVal.BeforeAction)
            {
                switch (pVal.MenuUID)
                {
                    case "1282":
                        break;
                }
            }
        }
        #endregion

        #region ItemEvents
        private void etBPL_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            var bplId = dataTable.GetValue("BPLId", 0).ToString();

            UIAPIRawForm.DataSources.UserDataSources.Item("udsBplid").Value = dataTable.GetValue("BPLId", 0).ToString();
            UIAPIRawForm.DataSources.UserDataSources.Item("udsBplName").Value = dataTable.GetValue("BPLName", 0).ToString();

            if (bplId.IsNotNullOrEmpty())
            {
                UIAPIRawForm.DataSources.UserDataSources.Item("udsBplId").Value = bplId;

                var sql = $@"
                        select T0.""AbsID""
                        from OOAT T0
	                        inner join CRD8 T1 on       T0.""BpCode"" = T1.""CardCode"" 
                                                    and T1.""BPLId"" = {bplId} 
                                                    and T1.""DisabledBP"" = 'N' 
                                                    and T0.""BpType"" = 'C'
                                                    and T0.""Status"" = 'A'
                ";

                var dtAgrAbsId = UIAPIRawForm.DataSources.DataTables.Item("dtAgrAbsID");

                dtAgrAbsId.Rows.Clear();
                dtAgrAbsId.ExecuteQuery(sql);

                var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
                var conditions = new Conditions();
                var count = dtAgrAbsId.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    var condition = conditions.Add();
                    condition.Alias = "AbsID";
                    condition.Operation = BoConditionOperation.co_EQUAL;
                    condition.CondVal = dtAgrAbsId.GetValue("AbsID", i).ToString();
                    if (i < count - 1)
                        condition.Relationship = BoConditionRelationship.cr_OR;
                }
                cflAgrNumber.SetConditions(conditions);
            }
        }

        private void etCode_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            var absId = dataTable.GetValue("AbsID", 0).ToString();
            var number = dataTable.GetValue("Number", 0).ToString();
            var cardCode = dataTable.GetValue("BpCode", 0).ToString();

            var usage = "";
            using (var q = QueryManager.DoQuery($@"select ""MainUsage"" from OCRD where ""CardCode"" = '{cardCode}'"))
            {
                if (q.HasRow)
                    usage = q.Get<int>("MainUsage").ToString();
            }

            if (usage == "0")
            {
                var decision = Application.SBO_Application.MessageBox("PN do contrato sem informação de Utilização Padrão. O pedido não será gerado corretamente.", 1, "Continuar", "Desistir");
                if (decision != 1) return;
            }

            UIAPIRawForm.DataSources.UserDataSources.Item("udsAgrDscr").Value = dataTable.GetValue("Descript", 0).ToString();
            UIAPIRawForm.DataSources.UserDataSources.Item("udsAgrNum").Value = dataTable.GetValue("Number", 0).ToString();
            UIAPIRawForm.DataSources.UserDataSources.Item("udsAgrId").Value = dataTable.GetValue("AbsID", 0).ToString();

            UIAPIRawForm.DataSources.UserDataSources.Item("dsCardCode").Value = cardCode;
            UIAPIRawForm.DataSources.UserDataSources.Item("dsCardName").Value = dataTable.GetValue("BpName", 0).ToString();
        }
        
        #endregion
        private SAPbouiCOM.EditText etBPLID;
        private SAPbouiCOM.EditText etBPLName;
        private SAPbouiCOM.EditText etCode;
        private SAPbouiCOM.LinkedButton lnkAgr;
        private SAPbouiCOM.Matrix mtItems;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private EditText etDscript;
        private Button Button2;
        private DataTable dtItems;


        private void Button2_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                SetBound();
            }
            catch (COMException ex)
            {
                Application.SBO_Application.SetStatusBarMessage(ex.Message);
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private void Button2_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            try
            {
                UIAPIRawForm.Freeze(true);

                if(!CheckSearchParameters())
                    throw new Exception("Não foram informados todos parametros. Corrigir a seleção antes de seguir.");

                BubbleEvent = true;
                UIAPIRawForm.Mode = BoFormMode.fm_ADD_MODE;
            }
            catch (COMException ex)
            {
                BubbleEvent = false;
                Application.SBO_Application.SetStatusBarMessage(ex.Message);
            }
            catch (Exception ex)
            {
                BubbleEvent = false;
                Application.SBO_Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private bool CheckSearchParameters()
        {
            var bplId = udsBplId.Value;
            var agrNo = udsAgrNum.Value;
            var dateF = udsDateF.Value;
            var dateT = udsDateT.Value;

            return !(bplId.IsNullOrEmpty() || agrNo.IsNullOrEmpty() || dateF.IsNullOrEmpty() || dateT.IsNullOrEmpty());                
        }


        private void SetBound()
        {
            var bplId = udsBplId.Value;
            var agrNo = udsAgrNum.Value;
            var dateF = udsDateF.Value;
            var dateT = udsDateT.Value;

            var itmFr = udsItemFrm.Value;
            var itmTo = udsItemTo.Value;


            var sql = $@"
                select distinct	'Y' as ""Invoice""
	                ,	T0.""DocEntry""
	                ,	T0.""DocNum""
	                ,	T0.""StartDate"" as ""PostDate""
	                ,	COALESCE(T7.""U_CARDCODE"", T2.""BpCode"") as ""BpCode""
	                ,	COALESCE(T8.""CardName"", T2.""BpName"") as ""BpName""
	                ,	T5.""U_CVA_ID_AGRUP"" as ""RdrItmCd""
                    ,   T6.""ItemName"" as ""RdrItmNm""
	                ,	T0.""ItemCode"" as ""ItemCode""
	                ,	T0.""ProdName"" as ""ItemName""
	                ,	COALESCE(T7.""U_QTYAPT"", T0.""CmpltQty"") as ""Quantity""
	                ,	T3.""MainUsage""
	                ,	T0.""Warehouse"" as ""WhsCode""
	                ,	T1.""BPLId""
	                ,	T1.""BPLName""
	                ,	T2.""AbsID"" as ""OoatEntry""
	                ,	T0.""U_CVA_SERVICO"" as ""Servico""
	                ,	T4.""Code""
	                ,	T5.""U_CVA_ID_SERVICO""
                from OWOR T0
	                inner join OBPL T1 on T0.""Warehouse"" = T1.""DflWhs""
	                inner join OOAT T2 on T0.""U_CVA_CONTRATO"" = T2.""AbsID""
	                inner join OCRD T3 on T2.""BpCode"" = T3.""CardCode""
                    inner join ""@CVA_GRPSERVICOS"" T4 on T0.""U_CVA_CONTRATO"" = T4.""U_AbsID""
                      and T0.""U_CVA_SERVICO"" = T4.""U_ServiceId""
                    inner join ""@CVA_LIN_GRPSERVICOS"" T5 on T4.""Code"" = T5.""Code""
	                inner join OITM T6 on T5.""U_CVA_ID_AGRUP"" = T6.""ItemCode""
	                left join ""@CVA_APTO_TERCEIROS1"" T7 on T0.""U_CVA_APO"" = T7.""Code""
	                left join OCRD T8 on  T8.""CardCode"" = T7.""U_CARDCODE""
                where T0.""Status"" = 'L'
                    and T1.""BPLId"" = {bplId}
                    and T2.""Number"" = {agrNo}
                    and T0.""StartDate"" between '{dateF.ToDate().ToHanaFormat()}' and '{dateT.ToDate().ToHanaFormat()}'
                    and T6.""ItemCode"" between     COALESCE({(itmFr.IsNullOrEmpty()?"NULL": $"'{itmFr}'")}, T6.""ItemCode"") 
                                                and COALESCE({(itmTo.IsNullOrEmpty() ? "NULL" : $"'{itmTo}'")}, T6.""ItemCode"") 
                    and COALESCE(T0.""U_CVA_ID_PEDIDO"", 0) = 0
                    ";

            dtItems.Rows.Clear();
            dtItems.ExecuteQuery(sql);

            //mtItems.FlushToDataSource();

            mtItems.Columns.Item("Invoice").DataBind.Bind("dtItems", "Invoice");
            mtItems.Columns.Item("DocEntry").DataBind.Bind("dtItems", "DocEntry");
            mtItems.Columns.Item("DocNum").DataBind.Bind("dtItems", "DocNum");
            mtItems.Columns.Item("StartDate").DataBind.Bind("dtItems", "PostDate");
            mtItems.Columns.Item("ItemCode").DataBind.Bind("dtItems", "ItemCode");
            mtItems.Columns.Item("ItemName").DataBind.Bind("dtItems", "ItemName");
            mtItems.Columns.Item("RdrItmCd").DataBind.Bind("dtItems", "RdrItmCd");
            mtItems.Columns.Item("RdrItmNm").DataBind.Bind("dtItems", "RdrItmNm");
            mtItems.Columns.Item("Quantity").DataBind.Bind("dtItems", "Quantity");
            mtItems.Columns.Item("BpCode").DataBind.Bind("dtItems", "BpCode");
            mtItems.Columns.Item("BpName").DataBind.Bind("dtItems", "BpName");

            mtItems.LoadFromDataSource();
            mtItems.AutoResizeColumns();
        }

        private EditText EditText1;
        private EditText EditText2;

        private void Button0_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!Post) return;

            try
            {
                Application.SBO_Application.StatusBar.SetText("Processando...", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                UIAPIRawForm.Freeze(true);

                var docDate = UIAPIRawForm.DataSources.UserDataSources.Item("udsDateT").Value.ToDate();

                var rowSelected = GetSelectedItems();

                var ordr = from t0 in rowSelected
                           let h = new { t0.BpCode, t0.BpName, t0.BPLId }
                           group t0 by h into g
                           select new
                           {

                               g.Key.BpCode,
                               g.Key.BpName,
                               g.Key.BPLId
                           };

                var rdr1 = from t1 in rowSelected
                           let l = new { t1.BpCode, t1.BpName, t1.BPLId, t1.ItemCode, t1.ItemName, t1.WhsCode, t1.MainUsage, t1.OoatEntry, t1.Servico }
                           group t1 by l into g
                           select new
                           {
                               g.Key.BpCode,
                               g.Key.BpName,
                               g.Key.BPLId,
                               g.Key.ItemCode,
                               g.Key.ItemName,
                               g.Key.WhsCode,
                               Quantity = Convert.ToDouble(g.Sum(q => q.Quantity)),
                               UnitPrice = GetPrice(g.Key.Servico, g.Key.OoatEntry, Convert.ToDouble(g.Sum(q => q.Quantity)), docDate),
                               g.Key.MainUsage,
                               g.Key.OoatEntry
                           };

                CommonController.Company.StartTransaction();

                foreach (var doc in ordr)
                {
                    var document = (SAPbobsCOM.Documents)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                    document.CardCode = doc.BpCode;
                    document.BPL_IDAssignedToInvoice = doc.BPLId;
                    document.DocDate = udsDocDate.Value.ToDateTime();
                    document.TaxDate = udsTaxDate.Value.ToDateTime();
                    document.DocDueDate = udsDueDate.Value.ToDateTime();

                    var idx = 0;
                    var lines = document.Lines;
                    foreach (var item in rdr1.Where(t0 => t0.BpCode == doc.BpCode && t0.BPLId == doc.BPLId))
                    {
                        if (idx == lines.Count) lines.Add();
                        lines.SetCurrentLine(idx);
                        lines.ItemCode = item.ItemCode;
                        lines.Quantity = item.Quantity;
                        lines.UnitPrice = item.UnitPrice;
                        lines.WarehouseCode = item.WhsCode;
                        if(item.MainUsage!="0")
                            lines.Usage = item.MainUsage;
                        //lines.AgreementNo = item.OoatEntry;

                        idx++;
                    }

                    var done = document.Add();
                    if (done != 0)
                        throw new Exception(CommonController.Company.GetLastErrorDescription());

                    var rdrEntry = CommonController.Company.GetNewObjectKey().ToInt32();

                    var sql = $@" update OWOR set ""U_CVA_ID_PEDIDO"" = {rdrEntry} where ""DocEntry"" in ({(from d in rowSelected select d.DocEntry).ToList().ToCsv()}) ";

                    using (QueryManager.DoQuery(sql)) { }
                }

                CommonController.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                UIAPIRawForm.DataSources.DataTables.Item("dtItems").Rows.Clear();
                mtItems.LoadFromDataSource();
                mtItems.AutoResizeColumns();

                udsDueDate.Value = "";
                udsTaxDate.Value = DateTime.Now.ToString("ddMMyyyy");
                udsDocDate.Value = DateTime.Now.ToString("ddMMyyyy");

                Application.SBO_Application.StatusBar.SetText("Operação concluída com êxito.", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
            }
            catch (COMException ex)
            {
                if (CommonController.Company.Connected)
                    if (CommonController.Company.InTransaction)
                        CommonController.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error);
            }
            catch (Exception ex)
            {
                if (CommonController.Company.Connected)
                    if (CommonController.Company.InTransaction)
                        CommonController.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }

        }

        private bool Post = false;

        private void Button0_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            Post = Application
                .SBO_Application.MessageBox("Será lançado um pedido de venda para os itens selecionados.", 1, "Continuar", "Cancelar")
                    == 1;
        }

        private double GetPrice(string servico, int ooatEntry, double quantity, DateTime docDate)
        {
            var sql  = $@"
                            select top 1 TO_DOUBLE(T1.""U_CVA_PRECO_UNIT"") as ""Price"" 
                              from ""@CVA_TABPRCVOL"" T0
	                         inner join ""@CVA_LIN_TABPRCVOL"" T1 on T0.""Code"" = T1.""Code"" 
							   and T0.""U_AbsID"" = {ooatEntry}
							   and T0.""U_CVA_APARTIR"" < '{docDate.ToHanaFormat()}'
							   and {quantity.ToHanaFormat()} between T1.""U_CVA_QTD_DE"" and T1.""U_CVA_QTD_ATE""
                             where T0.""U_ServiceId"" = '{servico}'
                             order by T0.""U_CVA_APARTIR"" desc
                ";
            
            using (var q = QueryManager.DoQuery(sql))
                if(q.HasRow)
                    return q.Get<double>(0);

            return .0;
        }

        private IEnumerable<RowItem> GetSelectedItems()
        {
            var dt = UIAPIRawForm.DataSources.DataTables.Item("dtItems");

            for (var a = 0; a < mtItems.RowCount; a++)
                dt.SetValue("Invoice", a, ((CheckBox)mtItems.Columns.Item("Invoice").Cells.Item(a + 1).Specific).Checked ? "Y" : "N");

            for (var b = 0; b < dt.Rows.Count; b++)
                if (dt.GetValue("Invoice", b) as String == "Y")
                    yield return new RowItem
                    {
                        DocEntry = dt.GetValue("DocEntry", b).ToInt32(),
                        DocNum = dt.GetValue("DocNum", b).ToInt32(),
                        PostDate = dt.GetValue("PostDate", b).ToDateTime(),
                        BpCode = dt.GetValue("BpCode", b).ToString(),
                        BpName = dt.GetValue("BpName", b).ToString(),
                        ItemCode = dt.GetValue("RdrItmCd", b).ToString(),
                        ItemName = dt.GetValue("RdrItmNm", b).ToString(),
                        Quantity = dt.GetValue("Quantity", b).ToDouble(),
                        MainUsage = dt.GetValue("MainUsage", b).ToString(),
                        WhsCode = dt.GetValue("WhsCode", b).ToString(),
                        BPLId = dt.GetValue("BPLId", b).ToInt32(),
                        BPLName = dt.GetValue("BPLName", b).ToString(),
                        OoatEntry = dt.GetValue("OoatEntry", b).ToInt32(),
                        Servico = dt.GetValue("Servico", b).ToString(),
                    };
        }
        
        private StaticText StaticText1;
        private StaticText StaticText2;
        private StaticText StaticText3;
        private StaticText StaticText4;
        private StaticText StaticText5;
        private EditText etLnkAgr;

        private void mtItems_DatasourceLoadBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

        }

        private void mtItems_DatasourceLoadAfter(object sboObject, SBOItemEventArg pVal)
        {

        }

        private EditText EditText0;
        private EditText EditText3;
        private EditText EditText4;
        private StaticText StaticText0;
        private StaticText StaticText6;
        private StaticText StaticText7;
        private EditText EditText5;
        private EditText EditText6;
        private StaticText StaticText8;
        private StaticText StaticText9;
        private StaticText StaticText10;

        private void itemCode_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null) return;

            var itemCode = dataTable.GetValue("ItemCode", 0).ToString();

            string uid = chooseFromListEvent.ItemUID;

            switch (chooseFromListEvent.ItemUID)
            {
                case "etItemFrm":
                    udsItemFrm.Value = itemCode;
                    break;
                case "etItemTo":
                    udsItemTo.Value = itemCode;
                    break;
                default:
                    break;
            }
        }
    }
    public class Key
    {
        public int DocEntry { get; set; }
    }
    public class RowItem
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public DateTime PostDate { get; set; }
        public string BpCode { get; set; }
        public string BpName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public string MainUsage { get; set; }
        public string WhsCode { get; set; }
        public int BPLId { get; set; }
        public string BPLName { get; set; }
        public int OoatEntry { get; set; }
        public string Servico { get; set; }
    }
}
