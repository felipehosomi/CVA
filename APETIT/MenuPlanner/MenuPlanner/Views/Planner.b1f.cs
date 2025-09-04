using MenuPlanner.Controllers;
using MenuPlanner.Extensions;
using MenuPlanner.Models;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.Planner", "Views/Planner.b1f")]
    class Planner : UserFormBase
    {
        private string _CacheInfo;
        private Dictionary<string, List<MealData>> MenuData = new Dictionary<string, List<MealData>>();
        private List<MenuTotalCostsModel> CostsList = new List<MenuTotalCostsModel>();
        private static string DocNum;
        private static bool NeedRecalculateCosts = true;
        private List<string> DeniedItems = new List<string>();

        public Planner()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.etEscape = ((SAPbouiCOM.EditText)(this.GetItem("etEscape").Specific));
            this.etNumber = ((SAPbouiCOM.EditText)(this.GetItem("etNumber").Specific));
            this.etNumber.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etNumber_ChooseFromListAfter);
            this.LinkedButton0 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_4").Specific));
            this.LinkedButton0.PressedBefore += new SAPbouiCOM._ILinkedButtonEvents_PressedBeforeEventHandler(this.LinkedButton0_PressedBefore);
            this.etAbsID = ((SAPbouiCOM.EditText)(this.GetItem("etAbsID").Specific));
            this.etCardCode = ((SAPbouiCOM.EditText)(this.GetItem("etCardCode").Specific));
            this.etCardCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardCode_ChooseFromListAfter);
            this.etCardName = ((SAPbouiCOM.EditText)(this.GetItem("etCardName").Specific));
            this.etCardName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardName_ChooseFromListAfter);
            this.LinkedButton1 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_8").Specific));
            this.etDocNum = ((SAPbouiCOM.EditText)(this.GetItem("etDocNum").Specific));
            this.etRefDate = ((SAPbouiCOM.EditText)(this.GetItem("etRefDate").Specific));
            this.etRefDate.ValidateAfter += new SAPbouiCOM._IEditTextEvents_ValidateAfterEventHandler(this.etRefDate_ValidateAfter);
            this.etEndDate = ((SAPbouiCOM.EditText)(this.GetItem("etEndDate").Specific));
            this.mtMenu = ((SAPbouiCOM.Matrix)(this.GetItem("mtMenu").Specific));
            this.mtMenu.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtMenu_ValidateAfter);
            //   this.mtMenu.KeyDownAfter += new SAPbouiCOM._IMatrixEvents_KeyDownAfterEventHandler(this.mtMenu_KeyDownAfter);
            this.mtMenu.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtMenu_LinkPressedBefore);
            this.mtMenu.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtMenu_ChooseFromListAfter);
            this.mtMenu.ChooseFromListBefore += new SAPbouiCOM._IMatrixEvents_ChooseFromListBeforeEventHandler(this.mtMenu_ChooseFromListBefore);
            this.mtMenu.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this.mtMenu_ClickAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button0_PressedBefore);
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.etTotMP = ((SAPbouiCOM.EditText)(this.GetItem("etTotMP").Specific));
            this.etTotCon = ((SAPbouiCOM.EditText)(this.GetItem("etTotCon").Specific));
            this.etTotCT = ((SAPbouiCOM.EditText)(this.GetItem("etTotCT").Specific));
            this.etTotCPC = ((SAPbouiCOM.EditText)(this.GetItem("etTotCPC").Specific));
            this.etTotMet = ((SAPbouiCOM.EditText)(this.GetItem("etTotMet").Specific));
            this.etTotBal = ((SAPbouiCOM.EditText)(this.GetItem("etTotBal").Specific));
            this.etATotMP = ((SAPbouiCOM.EditText)(this.GetItem("etATotMP").Specific));
            this.etATotCon = ((SAPbouiCOM.EditText)(this.GetItem("etATotCon").Specific));
            this.etATotCT = ((SAPbouiCOM.EditText)(this.GetItem("etATotCT").Specific));
            this.etATotCPC = ((SAPbouiCOM.EditText)(this.GetItem("etATotCPC").Specific));
            this.etATotPCM = ((SAPbouiCOM.EditText)(this.GetItem("etATotPCM").Specific));
            this.etMTotMP = ((SAPbouiCOM.EditText)(this.GetItem("etMTotMP").Specific));
            this.etMTotCon = ((SAPbouiCOM.EditText)(this.GetItem("etMTotCon").Specific));
            this.etMTotCT = ((SAPbouiCOM.EditText)(this.GetItem("etMTotCT").Specific));
            this.etMTotCPC = ((SAPbouiCOM.EditText)(this.GetItem("etMTotPCM").Specific));
            this.etMTotMet = ((SAPbouiCOM.EditText)(this.GetItem("etMTotMet").Specific));
            this.etMTotBal = ((SAPbouiCOM.EditText)(this.GetItem("etMTotBal").Specific));
            this.etSrvGrp = ((SAPbouiCOM.EditText)(this.GetItem("etSrvGrp").Specific));
            this.etSrvGrp.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etSrvGrp_ChooseFromListAfter);
            this.etSrv = ((SAPbouiCOM.EditText)(this.GetItem("etSrv").Specific));
            this.etSrv.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etSrv_ChooseFromListAfter);
            this.etMenuMod = ((SAPbouiCOM.EditText)(this.GetItem("etMenuMod").Specific));
            this.etMenuMod.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etMenuMod_ChooseFromListAfter);
            this.stDay = ((SAPbouiCOM.StaticText)(this.GetItem("stDay").Specific));
            this.stAccDay = ((SAPbouiCOM.StaticText)(this.GetItem("stAccDay").Specific));
            this.stMonth = ((SAPbouiCOM.StaticText)(this.GetItem("stMonth").Specific));
            this.stTotMP = ((SAPbouiCOM.StaticText)(this.GetItem("stTotMP").Specific));
            this.stTotCon = ((SAPbouiCOM.StaticText)(this.GetItem("stTotCon").Specific));
            this.stTotCT = ((SAPbouiCOM.StaticText)(this.GetItem("stTotCT").Specific));
            this.stTotCPC = ((SAPbouiCOM.StaticText)(this.GetItem("stTotCPC").Specific));
            this.stTotBal = ((SAPbouiCOM.StaticText)(this.GetItem("stTotBal").Specific));
            this.stTotMet = ((SAPbouiCOM.StaticText)(this.GetItem("stTotMet").Specific));
            this.stATotMP = ((SAPbouiCOM.StaticText)(this.GetItem("stATotMP").Specific));
            this.stATotCon = ((SAPbouiCOM.StaticText)(this.GetItem("stATotCon").Specific));
            this.stATotCT = ((SAPbouiCOM.StaticText)(this.GetItem("stATotCT").Specific));
            this.stATotCPC = ((SAPbouiCOM.StaticText)(this.GetItem("stATotCPC").Specific));
            this.stATotMet = ((SAPbouiCOM.StaticText)(this.GetItem("stATotPCM").Specific));
            this.stMTotMP = ((SAPbouiCOM.StaticText)(this.GetItem("stMTotMP").Specific));
            this.stMTotCon = ((SAPbouiCOM.StaticText)(this.GetItem("stMTotCon").Specific));
            this.stMTotCT = ((SAPbouiCOM.StaticText)(this.GetItem("stMTotCT").Specific));
            this.stMTotPCM = ((SAPbouiCOM.StaticText)(this.GetItem("stMTotPCM").Specific));
            this.stMTotBal = ((SAPbouiCOM.StaticText)(this.GetItem("stMTotBal").Specific));
            this.stMTotMet = ((SAPbouiCOM.StaticText)(this.GetItem("stMTotMet").Specific));
            this.btTotals = ((SAPbouiCOM.Button)(this.GetItem("btTotals").Specific));
            this.btTotals.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btTotals_PressedAfter);
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("Item_5").Specific));
            this.Button2.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button2_PressedAfter);
            this.Button3 = ((SAPbouiCOM.Button)(this.GetItem("Item_6").Specific));
            this.Button3.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button3_PressedAfter);
            this.mtMenuDB = ((SAPbouiCOM.Matrix)(this.GetItem("mtMenuDB").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.cmStatus = ((SAPbouiCOM.ComboBox)(this.GetItem("cmStatus").Specific));
            this.lkSrvGrp = ((SAPbouiCOM.LinkedButton)(this.GetItem("lkSrvGrp").Specific));
            this.lkSrvGrp.PressedAfter += new SAPbouiCOM._ILinkedButtonEvents_PressedAfterEventHandler(this.lkSrvGrp_PressedAfter);
            this.lkMenuMod = ((SAPbouiCOM.LinkedButton)(this.GetItem("lkMenuMod").Specific));
            this.lkMenuMod.PressedAfter += new SAPbouiCOM._ILinkedButtonEvents_PressedAfterEventHandler(this.lkMenuMod_PressedAfter);
            this.lkSrv = ((SAPbouiCOM.LinkedButton)(this.GetItem("lkSrv").Specific));
            this.lkSrv.PressedAfter += new SAPbouiCOM._ILinkedButtonEvents_PressedAfterEventHandler(this.lkSrv_PressedAfter);
            this.stMTotCom = ((SAPbouiCOM.StaticText)(this.GetItem("stMTotCom").Specific));
            this.etMTotCom = ((SAPbouiCOM.EditText)(this.GetItem("etMTotCom").Specific));
            this.btRelease = ((SAPbouiCOM.Button)(this.GetItem("btRelease").Specific));
            this.btRelease.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btRelease_PressedAfter);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("stTotCom").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("etTotCom").Specific));
            this.btCons = ((SAPbouiCOM.ButtonCombo)(this.GetItem("btCons").Specific));
            this.btCons.PressedAfter += this.BtCons_PressedAfter;
            this.btCons.ClickAfter += this.BtCons_ClickAfter;
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_10").Specific));

            udDayMP = UIAPIRawForm.DataSources.UserDataSources.Item("udDayMP");
            udAccMP = UIAPIRawForm.DataSources.UserDataSources.Item("udAccMP");
            udMonthMP = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthMP");
            udDayCon = UIAPIRawForm.DataSources.UserDataSources.Item("udDayCon");
            udAccCon = UIAPIRawForm.DataSources.UserDataSources.Item("udAccCon");
            udMonthCon = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthCon");
            udDayCT = UIAPIRawForm.DataSources.UserDataSources.Item("udDayCT");
            udAccCT = UIAPIRawForm.DataSources.UserDataSources.Item("udAccCT");
            udMonthCT = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthCT");
            udDayCPC = UIAPIRawForm.DataSources.UserDataSources.Item("udDayCPC");
            udDayCom = UIAPIRawForm.DataSources.UserDataSources.Item("udDayCom");
            udAccCom = UIAPIRawForm.DataSources.UserDataSources.Item("udAccCom");
            udMonthCom = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthCom");
            udAccPCM = UIAPIRawForm.DataSources.UserDataSources.Item("udAccPCM");
            udMonthPCM = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthPCM");
            udDayMet = UIAPIRawForm.DataSources.UserDataSources.Item("udDayMet");
            udAccMet = UIAPIRawForm.DataSources.UserDataSources.Item("udAccMet");
            udMonthMet = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthMet");
            udDayBal = UIAPIRawForm.DataSources.UserDataSources.Item("udDayBal");
            udAccBal = UIAPIRawForm.DataSources.UserDataSources.Item("udAccBal");
            udMonthBal = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthBal");
            this.OnCustomInitialize();

        }

        private void BtCons_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (btCons.Selected != null)
                {
                    if (btCons.Selected.Value == "1")
                    {
                        var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                        ProdutionOrder.AutoCreatingSpecial = true;
                        ProdutionOrder.AbsId = plannerData.GetValue("U_AbsID", plannerData.Offset).Trim();
                        ProdutionOrder.PlanCode = plannerData.GetValue("DocNum", plannerData.Offset).Trim();
                        SAPbouiCOM.Framework.Application.SBO_Application.OpenForm(BoFormObjectEnum.fo_ProductionOrder, "", null);
                    }
                    else
                    {
                        SAPbouiCOM.MenuItem menu = SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("54016");

                        for (int i = 0; i < menu.SubMenus.Count; i++)
                        {
                            if (menu.SubMenus.Item(i).String.StartsWith("CVA - OPs Consumo Fixo"))
                            {
                                menu.SubMenus.Item(i).Activate();
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("BtCons_ClickAfter: " + ex.Message, BoMessageTime.bmt_Short);
            }
        }

        private void BtCons_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.ActivateAfter += new SAPbouiCOM.Framework.FormBase.ActivateAfterHandler(this.Form_ActivateAfter);
            this.RightClickBefore += new SAPbouiCOM.Framework.FormBase.RightClickBeforeHandler(this.Form_RightClickBefore);
            this.RightClickAfter += new SAPbouiCOM.Framework.FormBase.RightClickAfterHandler(this.Form_RightClickAfter);
            this.DataAddAfter += new SAPbouiCOM.Framework.FormBase.DataAddAfterHandler(this.Form_DataAddAfter);
            this.DataAddBefore += Planner_DataAddBefore;

        }

        private void OnCustomInitialize()
        {
            #region [ Auto Managed Attribute ]
            UIAPIRawForm.PaneLevel = 0;
            etDocNum.Item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
            etEndDate.Item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
            cmStatus.Item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
            #endregion

            btCons.ValidValues.Add("1", "Adicionar OP");
            btCons.ValidValues.Add("2", "OPs Geradas");

            stDay.Item.TextStyle = 5;
            stAccDay.Item.TextStyle = 5;
            stMonth.Item.TextStyle = 5;

            etAbsID.Item.Width = 1;
            etEscape.Item.Width = 1;

            cmStatus.ExpandType = BoExpandType.et_DescriptionOnly;

            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
            var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

            dtQuery.Clear();
            dtQuery.ExecuteQuery(@"select coalesce(max(""DocNum""), 0) + 1 as ""DocNum"" from ""@CVA_PLANEJAMENTO""");

            DocNum = dtQuery.GetValue("DocNum", 0).ToString();

            plannerData.SetValue("DocNum", plannerData.Offset, DocNum);
            plannerData.SetValue("U_Status", plannerData.Offset, "P");
            plannerData.SetValue("U_CVA_DATA_REF", plannerData.Offset, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd"));

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

            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;

            cmStatus.ValidValues.Add("P", "Planejado");
            cmStatus.Item.Enabled = false;
        }

        private void MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.MenuUID != "1281" && pVal.MenuUID != "1282" && pVal.MenuUID != "RDayQuantity") return;
            if (UIAPIRawForm == null) return;
            if (UIAPIRawForm.UniqueID != SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.UniqueID) return;

            if (!pVal.BeforeAction)
            {
                switch (pVal.MenuUID)
                {
                    case "1281":
                        ((EditText)UIAPIRawForm.Items.Item("etDocNum").Specific).Active = true;

                        #region Tratamento dos valores válidos do combobox Status
                        while (cmStatus.ValidValues.Count > 0)
                        {
                            cmStatus.ValidValues.Remove(0, BoSearchKey.psk_Index);
                        }

                        cmStatus.ValidValues.Add("P", "Planejado");
                        cmStatus.ValidValues.Add("R", "Liberado");
                        cmStatus.ValidValues.Add("L", "Fechado");
                        cmStatus.ValidValues.Add("C", "Cancelado");
                        cmStatus.Item.Enabled = true;

                        #endregion

                        break;

                    case "1282":
                        UIAPIRawForm.Freeze(true);
                        var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");
                        var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
                        var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

                        dtMenu.Clear();
                        mtMenu.Clear();

                        while (mtMenu.Columns.Count > 0)
                        {
                            mtMenu.Columns.Remove(0);
                        }

                        #region Tratamento dos valores válidos do combobox Status
                        while (cmStatus.ValidValues.Count > 0)
                        {
                            cmStatus.ValidValues.Remove(0, BoSearchKey.psk_Index);
                        }

                        cmStatus.ValidValues.Add("P", "Planejado");
                        cmStatus.Item.Enabled = false;

                        #endregion

                        dtQuery.Clear();
                        dtQuery.ExecuteQuery(@"select coalesce(max(""DocNum""), 0) + 1 as ""DocNum"" from ""@CVA_PLANEJAMENTO""");
                        DocNum = dtQuery.GetValue("DocNum", 0).ToString();

                        plannerData.SetValue("DocNum", plannerData.Offset, DocNum);
                        plannerData.SetValue("U_Status", plannerData.Offset, "P");
                        plannerData.SetValue("U_CVA_DATA_REF", plannerData.Offset, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd"));

                        UIAPIRawForm.Freeze(false);

                        ((EditText)UIAPIRawForm.Items.Item("etCardCode").Specific).Active = true;
                        break;

                    case "RDayQuantity":
                        OpenDayQuantity();
                        break;
                }
            }
        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            try
            {
                mtMenu.AutoResizeColumns();
            }
            catch
            {

            }
        }

        private void Form_ActivateAfter(SBOItemEventArg pVal)
        {
            UIAPIRawForm.Freeze(false);
        }

        private void etRefDate_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!pVal.ItemChanged) return;

            try
            {
                UIAPIRawForm.Freeze(true);
                var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");
                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

                dtMenu.Clear();
                mtMenu.Clear();

                while (mtMenu.Columns.Count > 0)
                {
                    mtMenu.Columns.Remove(0);
                }

                if (String.IsNullOrEmpty(plannerData.GetValue("U_CVA_DES_MODELO_CARD", plannerData.Offset).ToString())) return;

                MenuData = new Dictionary<string, List<MealData>>();
                CostsList = new List<MenuTotalCostsModel>();
                NeedRecalculateCosts = true;

                var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");
                plannerDataLine.Clear();
                mtMenuDB.LoadFromDataSourceEx();

                SetMealColumns(plannerData.GetValue("U_CVA_DES_MODELO_CARD", plannerData.Offset), ref dtMenu);
                SetDays();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("etRefDate_ValidateAfter: " + ex.Message, BoMessageTime.bmt_Short);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        #region [ ChooseFromList ]
        private void etCardCode_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

            dbDataSource.SetValue("U_CVA_DES_CLIENTE", dbDataSource.Offset, dataTable.GetValue("CardName", 0).ToString());

            var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "BpCode";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();

            cflAgrNumber.SetConditions(conditions);

            var cflSrvGrp = UIAPIRawForm.ChooseFromLists.Item("cflSrvGrp");
            conditions = new Conditions();
            condition = conditions.Add();
            condition.Alias = "U_CardCode";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();

            cflSrvGrp.SetConditions(conditions);
        }

        private void etCardName_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

            dbDataSource.SetValue("U_CVA_ID_CLIENTE", dbDataSource.Offset, dataTable.GetValue("CardCode", 0).ToString());

            var cflAgrNumber = UIAPIRawForm.ChooseFromLists.Item("cflAgrNumber");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "BpCode";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();

            cflAgrNumber.SetConditions(conditions);

            var cflSrvGrp = UIAPIRawForm.ChooseFromLists.Item("cflSrvGrp");
            conditions = new Conditions();
            condition = conditions.Add();
            condition.Alias = "U_CardCode";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("CardCode", 0).ToString();

            cflSrvGrp.SetConditions(conditions);
        }

        private void etNumber_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

            dbDataSource.SetValue("U_AbsID", dbDataSource.Offset, dataTable.GetValue("AbsID", 0).ToString());
            dbDataSource.SetValue("U_CVA_ID_CLIENTE", dbDataSource.Offset, dataTable.GetValue("BpCode", 0).ToString());
            dbDataSource.SetValue("U_CVA_DES_CLIENTE", dbDataSource.Offset, dataTable.GetValue("BpName", 0).ToString());
            dbDataSource.SetValue("U_CVA_VIGENCIA_CONTR", dbDataSource.Offset, DateTime.Parse(dataTable.GetValue("EndDate", 0).ToString()).ToString("yyyyMMdd"));

            var cflSrvGrp = UIAPIRawForm.ChooseFromLists.Item("cflSrvGrp");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "U_AbsID";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("AbsID", 0).ToString();

            cflSrvGrp.SetConditions(conditions);

            var cflMenuModel = UIAPIRawForm.ChooseFromLists.Item("cflMenuModel");
            conditions = new Conditions();
            condition = conditions.Add();
            condition.Alias = "U_AbsID";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("AbsID", 0).ToString();

            cflMenuModel.SetConditions(conditions);
        }

        private void etSrvGrp_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

            dbDataSource.SetValue("U_CVA_ID_G_SERVICO", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());
        }

        private void etSrv_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

            var dbDataSource = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

            dbDataSource.SetValue("U_CVA_ID_SERVICO", dbDataSource.Offset, dataTable.GetValue("Code", 0).ToString());

            var cflMenuModel = UIAPIRawForm.ChooseFromLists.Item("cflMenuModel");
            var conditions = new Conditions();
            var condition = conditions.Add();
            condition.Alias = "U_CVA_ID_SERVICO";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = dataTable.GetValue("Code", 0).ToString();

            if (!String.IsNullOrEmpty(dbDataSource.GetValue("U_AbsID", dbDataSource.Offset)))
            {
                condition.Relationship = BoConditionRelationship.cr_AND;

                condition = conditions.Add();
                condition.Alias = "U_AbsID";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = dbDataSource.GetValue("U_AbsID", dbDataSource.Offset).ToString();
            }

            cflMenuModel.SetConditions(conditions);
        }

        private void etMenuMod_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
                var dataTable = chooseFromListEvent.SelectedObjects;

                if (dataTable == null || UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE) return;

                UIAPIRawForm.Freeze(true);

                var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");
                var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

                plannerData.SetValue("U_CVA_DES_MODELO_CARD", plannerData.Offset, dataTable.GetValue("U_CVA_DESCRICAO", 0).ToString());
                plannerData.SetValue("U_CVA_ID_MODEL_CARD", plannerData.Offset, dataTable.GetValue("Code", 0).ToString());

                var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");
                plannerDataLine.Clear();
                mtMenuDB.LoadFromDataSourceEx();

                SetMealColumns(dataTable.GetValue("U_CVA_DESCRICAO", 0).ToString(), ref dtMenu);
                SetDays();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("etMenuMod_ChooseFromListAfter: " + ex.Message, BoMessageTime.bmt_Short);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private void mtMenu_ChooseFromListBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                UIAPIRawForm.Freeze(true);

                mtMenu.FlushToDataSource();

                var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");
                var cellValue = ((EditText)mtMenu.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific).Value;
                var food = String.Empty;
                var percent = "100";

                if (cellValue.Contains("%"))
                {
                    percent = cellValue.Split('%')[0];
                    food = cellValue.Split('%')[1];
                }
                else
                {
                    food = cellValue;
                }

                _CacheInfo = percent;

                dtMenu.SetValue(pVal.ColUID, pVal.Row - 1, food);

                mtMenu.LoadFromDataSourceEx();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("mtMenu_ChooseFromListBefore - ERRO GERAL: " + ex.Message, BoMessageTime.bmt_Medium);
            }
        }

        private void mtMenu_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");

                mtMenu.FlushToDataSource();

                if (!String.IsNullOrWhiteSpace(dtMenu.GetValue(pVal.ColUID, pVal.Row - 1).ToString())) return;

                var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");
                var dtPosition = UIAPIRawForm.DataSources.DataTables.Item("dtPosition");
                var xml = dtPosition.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly);
                var doc = XDocument.Parse(xml);
                var rows = doc.Descendants().Where(d => d.Name == "Cell" && d.Descendants().Any(e => e.Name == "Value" && e.Value == $"{pVal.Row - 1}_{pVal.ColUID}"));

                mtMenuDB.FlushToDataSource();
                if (rows.Count() > 0 && !String.IsNullOrEmpty(dtPosition.GetValue("Position", dtPosition.Rows.Count - 1).ToString()))
                {
                    var lineId = int.Parse((rows.FirstOrDefault().NextNode as XElement).Descendants("Value").Single().Value);

                    plannerDataLine.SetValue("U_CVA_INSUMO", lineId, "");
                    plannerDataLine.SetValue("U_CVA_INSUMO_DES", lineId, "");
                    plannerDataLine.SetValue("U_CVA_PERCENT", lineId, "");
                }

                //mtMenuDB.FlushToDataSource();

                //if (rows.Count() > 0)
                //{
                //    var lineId = int.Parse((rows.FirstOrDefault().NextNode as XElement).Descendants("Value").Single().Value);

                //    plannerDataLine.RemoveRecord(lineId);
                //    dtPosition.Rows.Remove(lineId);

                //    for (var i = lineId; i < dtPosition.Rows.Count; i++)
                //    {
                //        dtPosition.SetValue("LineId", i, int.Parse(dtPosition.GetValue("LineId", i).ToString()) - 1);
                //    }
                //}

                mtMenuDB.LoadFromDataSourceEx();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("mtMenu_ValidateAfter - ERRO GERAL: " + ex.Message, BoMessageTime.bmt_Medium);
            }
        }

        private void mtMenu_KeyDownAfter(object sboObject, SBOItemEventArg pVal)
        {
            //if (pVal.CharPressed != 8 && pVal.CharPressed != 36) return;

            //var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");
            //var dtPosition = UIAPIRawForm.DataSources.DataTables.Item("dtPosition");
            //var xml = dtPosition.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly);
            //var doc = XDocument.Parse(xml);
            //var rows = doc.Descendants().Where(d => d.Name == "Cell" && d.Descendants().Any(e => e.Name == "Value" && e.Value == $"{pVal.Row - 1}_{pVal.ColUID}"));

            //mtMenuDB.FlushToDataSource();

            //if (rows.Count() > 0)
            //{
            //    var lineId = int.Parse((rows.FirstOrDefault().NextNode as XElement).Descendants("Value").Single().Value);

            //    plannerDataLine.RemoveRecord(lineId);
            //    dtPosition.Rows.Remove(lineId);

            //    for (var i = lineId; i < dtPosition.Rows.Count; i++)
            //    {
            //        dtPosition.SetValue("LineId", i, int.Parse(dtPosition.GetValue("LineId", i).ToString()) - 1);
            //    }
            //}

            //mtMenuDB.LoadFromDataSourceEx();
        }

        private void mtMenu_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                NeedRecalculateCosts = true;
                var chooseFromListEvent = (SBOChooseFromListEventArg)pVal;
                var dataTable = chooseFromListEvent.SelectedObjects;

                if (dataTable == null)
                {
                    UIAPIRawForm.Freeze(false);
                    return;
                }


                if (DeniedItems.Contains(dataTable.GetValue("ItemCode", 0).ToString()))
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText($"Item {dataTable.GetValue("ItemCode", 0).ToString()} denegado", BoMessageTime.bmt_Medium);
                    return;
                }

                var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");
                dtMenu.SetValue(pVal.ColUID, pVal.Row - 1, $"{_CacheInfo}% {dataTable.GetValue("ItemName", 0).ToString()}");
                mtMenu.LoadFromDataSourceEx();

                var linId = GetmtMenuDBLineId(pVal.Row, pVal.ColUID);

                if (linId == -1)
                {
                    MenuData[pVal.ColUID].Add(new MealData { LineId = pVal.Row - 1, ItemCode = dataTable.GetValue("ItemCode", 0).ToString() });
                }
                else
                {
                    if (!MenuData[pVal.ColUID].Any(x => x.LineId == pVal.Row - 1))
                    {
                        MenuData[pVal.ColUID].Add(new MealData { LineId = pVal.Row - 1, ItemCode = dataTable.GetValue("ItemCode", 0).ToString() });
                    }
                    else
                    {
                        MenuData[pVal.ColUID].FirstOrDefault(x => x.LineId == pVal.Row - 1).ItemCode = dataTable.GetValue("ItemCode", 0).ToString();
                    }
                }

                mtMenu.AutoResizeColumns();

                UIAPIRawForm.Freeze(false);

                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                var dishType = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");
                var menuModel = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MCARDAPIO");
                var menuLineModel = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");
                var conditions = new Conditions();
                var condition = conditions.Add();

                condition.Alias = "Name";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = mtMenu.Columns.Item(pVal.ColUID).Title;
                dishType.Query(conditions);

                conditions = new Conditions();
                condition = conditions.Add();
                condition.Alias = "U_CVA_DESCRICAO";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = plannerData.GetValue("U_CVA_DES_MODELO_CARD", plannerData.Offset);

                // Obtém os dados do modelo de cardápio selecionado
                menuModel.Query(conditions);

                conditions = new Conditions();
                condition = conditions.Add();
                condition.Alias = "Code";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = menuModel.GetValue("Code", 0);
                condition.Relationship = BoConditionRelationship.cr_AND;

                try
                {
                    string dishCode = dishType.GetValue("Code", 0);
                    condition = conditions.Add();
                    condition.Alias = "U_CVA_TIPO_PRATO";
                    condition.Operation = BoConditionOperation.co_EQUAL;
                    condition.CondVal = dishCode;
                }
                catch
                {
                    throw new Exception("Tipo de prato não encontrado");
                }

                menuLineModel.Query(conditions);

                var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");
                var dtPosition = UIAPIRawForm.DataSources.DataTables.Item("dtPosition");

                var xml = dtPosition.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly);

                var doc = XDocument.Parse(xml);
                var rows = doc.Descendants()
                              .Where(d => d.Name == "Cell" && d.Descendants().Any(e => e.Name == "Value" && e.Value == $"{pVal.Row - 1}_{pVal.ColUID}"));

                mtMenuDB.FlushToDataSource();

                MenuTotalCostsModel menuTotalCostsModel;

                if (rows.Count() > 0 && !String.IsNullOrEmpty(dtPosition.GetValue("Position", dtPosition.Rows.Count - 1).ToString()))
                {
                    var lineId = int.Parse((rows.FirstOrDefault().NextNode as XElement).Descendants("Value").Single().Value);
                    menuTotalCostsModel = CostsList.FirstOrDefault(m => m.LineId == lineId);
                    if (menuTotalCostsModel == null)
                    {
                        menuTotalCostsModel = new MenuTotalCostsModel();
                    }
                    menuTotalCostsModel.LineId = lineId;

                    plannerDataLine.SetValue("U_CVA_INSUMO", lineId, dataTable.GetValue("ItemCode", 0).ToString());
                    plannerDataLine.SetValue("U_CVA_INSUMO_DES", lineId, dataTable.GetValue("ItemName", 0).ToString());
                    plannerDataLine.SetValue("U_CVA_PERCENT", lineId, _CacheInfo);
                }
                else
                {
                    menuTotalCostsModel = new MenuTotalCostsModel();
                    CostsList.Add(menuTotalCostsModel);

                    if (dtPosition.Rows.Count == 0 || !String.IsNullOrEmpty(dtPosition.GetValue("Position", dtPosition.Rows.Count - 1).ToString()))
                    {
                        dtPosition.Rows.Add();
                    }

                    plannerDataLine.InsertRow(firstRow: mtMenuDB.RowCount == 0);

                    dtPosition.SetValue("Position", dtPosition.Rows.Count - 1, $"{pVal.Row - 1}_{pVal.ColUID}");
                    dtPosition.SetValue("LineId", dtPosition.Rows.Count - 1, dtPosition.Rows.Count - 1);

                    plannerDataLine.SetValue("U_CVA_TIPO_PRATO", plannerDataLine.Size - 1, dishType.GetValue("Code", dishType.Offset));
                    plannerDataLine.SetValue("U_CVA_TIPO_PRATO_DES", plannerDataLine.Size - 1, mtMenu.Columns.Item(pVal.ColUID).Title);
                    plannerDataLine.SetValue("U_CVA_INSUMO", plannerDataLine.Size - 1, dataTable.GetValue("ItemCode", 0).ToString());
                    plannerDataLine.SetValue("U_CVA_INSUMO_DES", plannerDataLine.Size - 1, dataTable.GetValue("ItemName", 0).ToString());
                    plannerDataLine.SetValue("U_CVA_PERCENT", plannerDataLine.Size - 1, _CacheInfo);
                    plannerDataLine.SetValue("U_CVA_MODELO_LIN_ID", plannerDataLine.Size - 1, (int.Parse(pVal.ColUID) + 1).ToString());
                    plannerDataLine.SetValue("U_CVA_DIA_SEMANA", plannerDataLine.Size - 1, dtMenu.GetValue("Day", pVal.Row - 1).ToString().Substring(5, 3));
                    plannerDataLine.SetValue("U_Day", plannerDataLine.Size - 1, dtMenu.GetValue("Day", pVal.Row - 1).ToString().Substring(0, 2));
                }

                var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
                dtQuery.Clear();

                dtQuery.ExecuteQuery(String.Format(@"select sum(OITW.""AvgPrice"" * ITT1.""Quantity"") * {1} / 100 as ""TotalCost""
                                                       from OITT
                                                      inner join ITT1 on ITT1.""Father"" = OITT.""Code""
                                                      inner join OITW on OITW.""ItemCode"" = ITT1.""Code""   
                                                      where OITT.""Code"" = '{0}'   
                                                        and OITW.""WhsCode"" = (select OWHS.""WhsCode""
   						                                                          from OWHS
   						                                                         inner join OOAT on OOAT.""U_CVA_FILIAL"" = OWHS.""BPLid""
   						                                                         where OOAT.""AbsID"" = {2})", dataTable.GetValue("ItemCode", 0).ToString(), _CacheInfo, plannerData.GetValue("U_AbsID", plannerData.Offset)));

                //dtQuery.ExecuteQuery(sql);

                plannerDataLine.SetValue("U_CVA_CUSTO_ITEM", plannerDataLine.Size - 1, double.Parse(dtQuery.GetValue("TotalCost", 0).ToString()).ToString(CultureInfo.InvariantCulture));
                menuTotalCostsModel.ItemCode = dataTable.GetValue("ItemCode", 0).ToString();
                menuTotalCostsModel.RawMaterial = (double)dtQuery.GetValue("TotalCost", 0);
                menuTotalCostsModel.Day = Convert.ToInt32(dtMenu.GetValue("Day", pVal.Row - 1).ToString().Substring(0, 2));

                mtMenuDB.LoadFromDataSourceEx();

                _CacheInfo = String.Empty;

                if (UIAPIRawForm.Mode == BoFormMode.fm_OK_MODE)
                {
                    UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("mtMenu_ChooseFromListAfter - ERRO GERAL: " + ex.Message, BoMessageTime.bmt_Medium);
            }
        }
        #endregion

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

        private void lkMenuMod_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            CommonController.FormFatherType = UIAPIRawForm.TypeEx;
            CommonController.FormFatherCount = UIAPIRawForm.TypeCount;

            var activeForm = new MenuModel();
            activeForm.Show();
        }

        private void mtMenu_LinkPressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            try
            {
                var lineId = GetmtMenuDBLineId(pVal.Row, pVal.ColUID);
                SAPbouiCOM.Framework.Application.SBO_Application.OpenForm(BoFormObjectEnum.fo_ProductTree, "", MenuData[pVal.ColUID].Where(x => x.LineId == pVal.Row - 1).FirstOrDefault().ItemCode);
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("mtMenu_LinkPressedBefore - ERRO GERAL: " + ex.Message, BoMessageTime.bmt_Medium);
            }
            BubbleEvent = false;
        }
        #endregion

        private void Button0_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (UIAPIRawForm.Mode != BoFormMode.fm_ADD_MODE) return;

                var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");

                if (plannerDataLine.Size == 1 && String.IsNullOrEmpty(plannerDataLine.GetValue("U_Day", 0)))
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Não é possível inserir um planejamento de cardápio vazio.", BoMessageTime.bmt_Short);
                    BubbleEvent = false;
                }

                DateTime refDate;
                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                if (!DateTime.TryParseExact(plannerData.GetValue("U_CVA_DATA_REF", plannerData.Offset), "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out refDate))
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Data de referência deve ser informada", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    BubbleEvent = false;
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Button0_PressedBefore - ERRO GERAL: " + ex.Message, BoMessageTime.bmt_Medium);
            }
        }

        private void mtMenu_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID == "Day")
                {
                    if (pVal.Row == 0)
                    {
                        mtMenu.ClearSelections();
                        btTotals.Item.Enabled = false;
                        btRelease.Item.Visible = false;
                        if (stDay.Item.Visible) HideTotals();
                        return;
                    }

                    mtMenu.SelectRow(pVal.Row, true, false);
                    btTotals.Item.Enabled = true;

                    var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                    btRelease.Item.Visible = plannerData.GetValue("U_Status", plannerData.Offset) != "L";

                    var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");

                    SetTotals(int.Parse(dtMenu.GetValue("Day", pVal.Row - 1).ToString().Substring(0, 2)));
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("mtMenu_ClickAfter - ERRO GERAL: " + ex.Message, BoMessageTime.bmt_Medium);
            }
        }

        private void btTotals_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            switch (btTotals.Caption)
            {
                case "Apresentar totalizadores":
                    ShowTotals();
                    break;

                case "Esconder totalizadores":
                    HideTotals();
                    break;
            }
        }

        private void Button3_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            CommonController.FormFatherType = UIAPIRawForm.TypeEx;
            CommonController.FormFatherCount = UIAPIRawForm.TypeCount;

            var activeForm = new IncidenceMap();
            activeForm.Show();
        }

        private void Button0_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);

                if (UIAPIRawForm.Mode == BoFormMode.fm_OK_MODE)
                {
                    SetItemsEnabledByStatus();
                }
                else if (UIAPIRawForm.Mode != BoFormMode.fm_ADD_MODE)
                {
                    var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");
                    var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
                    var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

                    dtMenu.Clear();
                    mtMenu.LoadFromDataSourceEx(true);

                    while (mtMenu.Columns.Count > 0)
                    {
                        mtMenu.Columns.Remove(mtMenu.Columns.Count - 1);
                    }

                    //dtQuery.Clear();
                    //dtQuery.ExecuteQuery(@"select coalesce(max(""DocNum""), 0) + 1 as ""DocNum"" from ""@CVA_PLANEJAMENTO""");

                    //plannerData.SetValue("DocNum", plannerData.Offset, dtQuery.GetValue("DocNum", 0).ToString());
                    //plannerData.SetValue("U_Status", plannerData.Offset, "P");
                    //plannerData.SetValue("U_CVA_DATA_REF", plannerData.Offset, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd"));

                    ((EditText)UIAPIRawForm.Items.Item("etCardCode").Specific).Active = true;
                    UIAPIRawForm.Freeze(false);
                }
                else
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("O nº real do documento lançado é: " + DocNum, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);
                }
            }
            catch (Exception ex)
            {
                //SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Button0_PressedAfter - ERRO GERAL: " + ex.Message, BoMessageTime.bmt_Medium);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private void Button2_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

                var reportParams = new Hashtable();
                reportParams.Add("idPlanejamento", plannerData.GetValue("DocEntry", plannerData.Offset));

                var crRelatorio = new ReportsController();
                crRelatorio.ExecuteCrystalReport("MenuProjection.rpt", plannerData.GetValue("DocEntry", plannerData.Offset));
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Button2_PressedAfter: " + ex.Message);
            }
        }

        private void btRelease_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            ReleaseLines();
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            bool hasLineClosed = false;

            var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");
            for (int i = 0; i < plannerDataLine.Size; i++)
            {
                if (plannerDataLine.GetValue("U_LineStatus", i) == "L")
                {
                    hasLineClosed = true;
                    break;
                }
            }

            if (UIAPIRawForm.Mode != BoFormMode.fm_ADD_MODE)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1283").Enabled = !hasLineClosed;
            }
            if (eventInfo.ItemUID == "mtMenu")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RDayQuantity").Enabled = true;
                UIAPIRawForm.DataSources.UserDataSources.Item("MenuRow").Value = eventInfo.Row.ToString();
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtMenu")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RDayQuantity").Enabled = false;
            }
        }

        private void LinkedButton0_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            CommonController.LazyProcess = true;
        }

        #region [ FormDataEvents ]
        private void Planner_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
            plannerData.SetValue("U_Status", plannerData.Offset, "P");
        }

        private void Form_DataAddAfter(ref BusinessObjectInfo pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);

                var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");
                var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

                dtMenu.Clear();
                mtMenu.LoadFromDataSourceEx(true);

                while (mtMenu.Columns.Count > 0)
                {
                    mtMenu.Columns.Remove(mtMenu.Columns.Count - 1);
                }

                dtQuery.Clear();
                //dtQuery.ExecuteQuery(@"select coalesce(max(""DocNum""), 0) + 1 as ""DocNum"" from ""@CVA_PLANEJAMENTO""");

                //plannerData.SetValue("DocNum", plannerData.Offset, dtQuery.GetValue("DocNum", 0).ToString());
                //plannerData.SetValue("U_Status", plannerData.Offset, "P");
                //plannerData.SetValue("U_CVA_DATA_REF", plannerData.Offset, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd"));

                //SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("O nº real do documento lançado é: " + plannerData.GetValue("DocNum", plannerData.Offset), BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);

                if (stDay.Item.Visible) HideTotals();

                MenuData = new Dictionary<string, List<MealData>>();
                CostsList = new List<MenuTotalCostsModel>();
                NeedRecalculateCosts = true;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Form_DataAddAfter - ERRO GERAL: " + ex.Message, BoMessageTime.bmt_Medium);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Obtendo cardápio, aguarde.", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
            ProgressBar pgb = null;

            try
            {
                UIAPIRawForm.Freeze(true);

                if (stDay.Item.Visible) HideTotals();

                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");
                var serviceGroup = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_GRPSERVICOS");

                var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");
                var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
                var dtPosition = UIAPIRawForm.DataSources.DataTables.Item("dtPosition");
                var dayPosition = new Dictionary<string, int>();

                // Obtém as estruturas de produto cujos itens denegados fazem parte
                dtQuery.ExecuteQuery(String.Format(@"select ITT1.""Father"" 
                                                   from ""@CVA_ODNI"" as ODNI
                                                  inner join ""@CVA_DNI1"" as DNI1 on DNI1.""Code"" = ODNI.""Code""
                                                  inner join ITT1 on ITT1.""Code"" = DNI1.""U_ItemCode""
                                                  where ODNI.""U_AbsID"" = {0}", plannerData.GetValue("U_AbsId", plannerData.Offset)));
                DeniedItems = new List<string>();
                for (int i = 0; i < dtQuery.Rows.Count; i++)
                {
                    DeniedItems.Add(dtQuery.GetValue(0, i).ToString());
                }

                #region Tratamento dos valores válidos do combobox Status
                while (cmStatus.ValidValues.Count > 0)
                {
                    cmStatus.ValidValues.Remove(0, BoSearchKey.psk_Index);
                }

                switch (plannerData.GetValue("U_Status", plannerData.Offset))
                {
                    case "P":
                        cmStatus.ValidValues.Add("P", "Planejado");
                        cmStatus.ValidValues.Add("R", "Liberado");
                        cmStatus.Item.Enabled = false;
                        break;

                    case "R":
                        cmStatus.ValidValues.Add("R", "Liberado");
                        cmStatus.Item.Enabled = false;
                        break;

                    case "L":
                        cmStatus.ValidValues.Add("L", "Fechado");
                        cmStatus.Item.Enabled = false;
                        break;

                    case "C":
                        cmStatus.ValidValues.Add("C", "Cancelado");
                        cmStatus.Item.Enabled = false;
                        break;
                }
                #endregion

                dtPosition.Rows.Clear();

                var conditions = new Conditions();
                var condition = conditions.Add();
                condition.Alias = "Code";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = plannerData.GetValue("U_CVA_ID_G_SERVICO", plannerData.Offset);
                condition.Relationship = BoConditionRelationship.cr_AND;
                condition = conditions.Add();
                condition.Alias = "U_CVA_ID_SERVICO";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = plannerData.GetValue("U_CVA_ID_SERVICO", plannerData.Offset);

                serviceGroup.Query(conditions);

                SetMealColumns(plannerData.GetValue("U_CVA_DES_MODELO_CARD", plannerData.Offset), ref dtMenu);
                SetDays(false);

                var query = $@"delete from ""@CVA_LN_PLANEJAMENTO"" where ""U_Day"" is null;";
                dtQuery.ExecuteQuery(query);

                query = $@"select case when U_CVA_PERCENT > 0 then cast(cast(""U_CVA_PERCENT"" as int) as varchar) || '% ' || ""U_CVA_INSUMO_DES"" 
                                  else '' end as ""Food"", ""LineId"",
                                  ""U_Day"" || ' - ' || ""U_CVA_DIA_SEMANA"" || '.' as ""Day"", ""U_CVA_QTD_ORI"" as ""Qty"",
                                  ""U_CVA_INSUMO"" as ""ItemCode"", cast(""U_CVA_MODELO_LIN_ID"" as int) - 1 as ""Dish"", ""U_LineStatus"" as ""LineStatus""
                             from ""@CVA_LN_PLANEJAMENTO""
                            where ""DocEntry"" = {plannerData.GetValue("DocEntry", plannerData.Offset)}
                            order by ""LineId"";";
                dtQuery.ExecuteQuery(query);

                pgb = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar("", dtQuery.Rows.Count + dtMenu.Rows.Count, false);
                pgb.Text = "Gerando cardápio...";

                for (int i = 0; i < dtMenu.Rows.Count; i++)
                {
                    pgb.Value++;
                    if (dayPosition.ContainsKey(dtMenu.GetValue("Day", i).ToString())) continue;

                    dayPosition.Add(dtMenu.GetValue("Day", i).ToString(), i);
                }

                for (var i = 0; i < dtQuery.Rows.Count; i++)
                {
                    pgb.Value++;
                    dtPosition.Rows.Add();
                    dtPosition.SetValue("Position", dtPosition.Rows.Count - 1, $"{dayPosition[dtQuery.GetValue("Day", i).ToString()]}_{dtQuery.GetValue("Dish", i)}");
                    dtPosition.SetValue("LineId", dtPosition.Rows.Count - 1, i);

                    MenuData[dtQuery.GetValue("Dish", i).ToString()].Add(new MealData { ItemCode = dtQuery.GetValue("ItemCode", i).ToString(), LineId = dayPosition[dtQuery.GetValue("Day", i).ToString()] });

                    dtMenu.SetValue(dtQuery.GetValue("Dish", i).ToString(), dayPosition[dtQuery.GetValue("Day", i).ToString()], dtQuery.GetValue("Food", i));

                    if (dtQuery.GetValue("LineStatus", i).ToString() == "P") continue;

                    mtMenu.CommonSetting.SetRowEditable(dayPosition[dtQuery.GetValue("Day", i).ToString()] + 1, false);
                }

                //for (int x = 0, y = 0; x < dtMenu.Rows.Count; x++)
                //{
                //    if (y > dtQuery.Rows.Count - 1) break;

                //    if (String.IsNullOrEmpty(dtQuery.GetValue("Day", y).ToString()))
                //    {
                //        plannerDataLine.RemoveRecord(y);
                //        continue;
                //    }

                //    while (dtMenu.GetValue("Day", x).ToString() == dtQuery.GetValue("Day", y).ToString())
                //    {
                //        dtPosition.Rows.Add();
                //        dtPosition.SetValue("Position", dtPosition.Rows.Count - 1, $"{x}_{dtQuery.GetValue("Dish", y).ToString()}");
                //        dtPosition.SetValue("LineId", dtPosition.Rows.Count - 1, y);

                //        MenuData[dtQuery.GetValue("Dish", y).ToString()].Add(new MealData { ItemCode = dtQuery.GetValue("ItemCode", y).ToString(), LineId = x });

                //        dtMenu.SetValue(dtQuery.GetValue("Dish", y).ToString(), x, dtQuery.GetValue("Food", y++));

                //        if (y > dtQuery.Rows.Count - 1) break;
                //    }
                //}

                mtMenu.SelectionMode = BoMatrixSelect.ms_Auto;
                mtMenu.LoadFromDataSourceEx(true);
                mtMenu.AutoResizeColumns();

                mtMenuDB.LoadFromDataSource();

                SetItemsEnabledByStatus();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Form_DataLoadAfter: " + ex.Message, BoMessageTime.bmt_Short);
            }
            finally
            {
                if (pgb != null)
                {
                    pgb.Stop();
                    Marshal.ReleaseComObject(pgb);
                    pgb = null;
                }

                UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        private void OpenDayQuantity()
        {
            if (mtMenu.GetNextSelectedRow() >= 0)
            {
                DayQuantity.Editable = mtMenu.CommonSetting.GetCellEditable(mtMenu.GetNextSelectedRow(), 1);

                CommonController.FormFatherType = UIAPIRawForm.TypeEx;
                CommonController.FormFatherCount = UIAPIRawForm.TypeCount;

                var dayQuantity = new DayQuantity();
                dayQuantity.Show();
            }
        }

        private void ReleaseLines()
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");
                var dtPosition = UIAPIRawForm.DataSources.DataTables.Item("dtPosition");
                var xml = dtPosition.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly);
                var doc = XDocument.Parse(xml);

                mtMenuDB.FlushToDataSource();

                for (var i = 1; i <= mtMenu.RowCount; i++)
                {
                    if (!mtMenu.IsRowSelected(i)) continue;

                    var rows = doc.Descendants().Where(d => d.Name == "Cell" && d.Descendants().Any(e => e.Name == "Value" && e.Value.StartsWith($"{i - 1}_")));

                    foreach (var row in rows)
                    {
                        var position = (row as XElement).Descendants("Value").Single().Value.Split('_');
                        var lineId = int.Parse((row.NextNode as XElement).Descendants("Value").Single().Value);

                        if (plannerDataLine.GetValue("U_LineStatus", lineId) != "P") continue;

                        plannerDataLine.SetValue("U_LineStatus", lineId, "R");
                        mtMenu.CommonSetting.SetRowEditable(int.Parse(position[0]) + 1, false);
                    }
                }

                plannerData.SetValue("U_Status", plannerData.Offset, "R");
                mtMenuDB.LoadFromDataSourceEx();
                UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
                SetItemsEnabledByStatus();
                btTotals.Item.Enabled = true;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("ReleaseLines: " + ex.Message, BoMessageTime.bmt_Short);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private void SetDays(bool resetDayQuantity = true)
        {
            var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");
            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
            var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
            var freeDays = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_CALENDSC");
            var mnp2 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MNP2");
            var conditions = new Conditions();
            var condition = conditions.Add();


            if (plannerData.GetValue("DocNum", plannerData.Offset) == "")
            {
                dtQuery.Clear();
                dtQuery.ExecuteQuery(@"select coalesce(max(""DocNum""), 0) + 1 as ""DocNum"" from ""@CVA_PLANEJAMENTO""");

                DocNum = dtQuery.GetValue("DocNum", 0).ToString();

                plannerData.SetValue("DocNum", plannerData.Offset, DocNum);
                plannerData.SetValue("U_Status", plannerData.Offset, "P");
                plannerData.SetValue("U_CVA_DATA_REF", plannerData.Offset, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd"));
            }

            condition.Alias = "Code";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = $"{plannerData.GetValue("U_CVA_ID_G_SERVICO", plannerData.Offset)}_{plannerData.GetValue("U_AbsID", plannerData.Offset)}";

            // Obtém os dados do modelo de cardápio selecionado
            freeDays.Query(conditions);

            DateTime refDate;
            DateTime.TryParseExact(plannerData.GetValue("U_CVA_DATA_REF", plannerData.Offset), "yyyyMMdd", null, DateTimeStyles.AssumeLocal, out refDate);
            var referenceDate = new DateTime(refDate.Year, refDate.Month, 1, 0, 0, 0);
            referenceDate = referenceDate.AddMonths(1);
            referenceDate = referenceDate.Subtract(new TimeSpan(0, 1, 0));

            // Insere linhas de acordo com a quantidade de dias do mês de referência
            dtMenu.Rows.Add(DateTime.DaysInMonth(referenceDate.Year, referenceDate.Month));

            // Define o dias do mês
            for (var i = 1; i <= referenceDate.Day; i++)
            {
                var day = new DateTime(referenceDate.Year, referenceDate.Month, i);
                dtMenu.SetValue("Day", i - 1, $"{i.ToString("00")} - {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(day.ToString("ddd", new CultureInfo("pt-BR")).ToLower())}.");
            }

            mtMenu.LoadFromDataSourceEx(true);
            if (mtMenu.Columns.Count > 0)
            {
                mtMenu.AutoResizeColumns();
            }

            // Verifica se existe algum dia sem serviço
            if (freeDays.Size > 0)
            {
                for (var i = 1; i <= referenceDate.Day; i++)
                {
                    var day = new DateTime(referenceDate.Year, referenceDate.Month, i);

                    for (var y = 0; y < freeDays.Size; y++)
                    {
                        if (day != DateTime.ParseExact(freeDays.GetValue("U_CVA_DATA", y), "yyyyMMdd", null)) continue;

                        mtMenu.CommonSetting.SetRowEditable(i, false);
                    }
                }
            }

            if (resetDayQuantity)
            {
                dtQuery.ExecuteQuery(String.Format(@"select ""U_CVA_TURNO"",
                                                        ""U_CVA_SEGUNDA"" as ""Seg"", ""U_CVA_TERCA"" as ""Ter"", ""U_CVA_QUARTA"" as ""Qua"", ""U_CVA_QUINTA"" as ""Qui"", 
                                                        ""U_CVA_SEXTA"" as ""Sex"", ""U_CVA_SABADO"" as ""Sáb"", ""U_CVA_DOMINGO"" as ""Dom""
                                                   from ""@CVA_LIN_GRPSERVICOS""
                                                  where ""Code"" = '{0}'", plannerData.GetValue("U_CVA_ID_G_SERVICO", plannerData.Offset)));

                mnp2.Clear();

                for (var i = 0; i < dtMenu.Rows.Count; i++)
                {
                    for (var y = 0; y < dtQuery.Rows.Count; y++)
                    {
                        mnp2.InsertRow();
                        mnp2.SetValue("LineId", mnp2.Size - 1, mnp2.Size.ToString());
                        mnp2.SetValue("U_Shift", mnp2.Size - 1, dtQuery.GetValue("U_CVA_TURNO", y).ToString());
                        mnp2.SetValue("U_Day", mnp2.Size - 1, $"{dtMenu.GetValue("Day", i).ToString().Substring(0, 2)}");
                        mnp2.SetValue("U_Weekday", mnp2.Size - 1, $"{dtMenu.GetValue("Day", i).ToString().Substring(5, 3)}");
                        mnp2.SetValue("U_Quantity", mnp2.Size - 1, dtQuery.GetValue($"{dtMenu.GetValue("Day", i).ToString().Substring(5, 3)}", y).ToString());
                    }
                }
            }
        }

        private int GetmtMenuDBLineId(int row, string colUID)
        {
            var dtPosition = UIAPIRawForm.DataSources.DataTables.Item("dtPosition");
            var xml = dtPosition.SerializeAsXML(BoDataTableXmlSelect.dxs_DataOnly);
            var doc = XDocument.Parse(xml);
            var rows = doc.Descendants().Where(d => d.Name == "Cell" && d.Descendants().Any(e => e.Name == "Value" && e.Value == $"{row - 1}_{colUID}"));

            //mtMenuDB.FlushToDataSource();

            if (rows.Count() > 0)
            {
                return int.Parse((rows.FirstOrDefault().NextNode as XElement).Descendants("Value").Single().Value);
            }
            else
            {
                return -1;
            }
        }

        private void SetMealColumns(string menuDescription, ref DataTable dtMenu)
        {
            ProgressBar pgb = null;
            try
            {
                MenuData = new Dictionary<string, List<MealData>>();
                CostsList = new List<MenuTotalCostsModel>();
                NeedRecalculateCosts = true;

                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                var menuModel = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MCARDAPIO");
                var menuLineModel = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LIN_MCARDAPIO");
                var dishType = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_TIPOPRATO");
                var dst1 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_DST1");

                string absId = plannerData.GetValue("U_AbsID", plannerData.Offset);
                if (String.IsNullOrEmpty(absId))
                {
                    absId = "0";
                }

                var conditions = new Conditions();
                var condition = conditions.Add();

                condition.Alias = "U_CVA_DESCRICAO";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = menuDescription;

                // Obtém os dados do modelo de cardápio selecionado
                menuModel.Query(conditions);
                if (menuModel.Size == 0)
                {
                    return;
                }

                conditions = new Conditions();
                condition = conditions.Add();
                condition.Alias = "Code";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = menuModel.GetValue("Code", 0);

                menuLineModel.Query(conditions);

                dtMenu.Clear();
                mtMenu.Clear();

                while (mtMenu.Columns.Count > 0)
                {
                    mtMenu.Columns.Remove(0);
                }

                dtMenu.Columns.Add("Day", BoFieldsType.ft_AlphaNumeric, 254);
                var newCol = mtMenu.Columns.Add("Day", BoFormItemTypes.it_EDIT);
                newCol.Editable = false;
                newCol.TitleObject.Caption = "Dia";
                newCol.Visible = true;
                newCol.RightJustified = false;
                newCol.DataBind.Bind("dtMenu", "Day");

                pgb = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar("", menuLineModel.Size, false);
                pgb.Text = "Criando colunas...";

                // Insere novas colunas de acordo com o modelo de cardápio
                for (var i = 0; i < menuLineModel.Size; i++)
                {
                    pgb.Value++;

                    if (menuLineModel.GetValue("U_CVA_TIPO_PRATO_DES", i) == "")
                    {
                        continue;
                    }

                    //var cflParams = ((ChooseFromListCreationParams)(SAPbouiCOM.Framework.Application.SBO_Application.CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams)));
                    //cflParams.MultiSelection = false;
                    //cflParams.ObjectType = "4";
                    //cflParams.UniqueID = $"cfl{i}";
                    ChooseFromList cfl = UIAPIRawForm.ChooseFromLists.Item($"cfl{i}");

                    // Obtém as informações do tipo de prato da refeição
                    conditions = new Conditions();
                    condition = conditions.Add();
                    condition.Alias = "Code";
                    condition.Operation = BoConditionOperation.co_EQUAL;
                    condition.CondVal = menuLineModel.GetValue("U_CVA_TIPO_PRATO", i);
                    dishType.Query(conditions);

                    // Obtém as informações das linhas tipo de prato da refeição
                    conditions = new Conditions();
                    condition = conditions.Add();
                    condition.Alias = "Code";
                    condition.Operation = BoConditionOperation.co_EQUAL;
                    condition.CondVal = menuLineModel.GetValue("U_CVA_TIPO_PRATO", i);
                    dst1.Query(conditions);

                    // Define quais itens farão parte do CFL das colunas, 
                    // obtendo apenas os itens que sejam do tipo estrutura de produto
                    conditions = new Conditions();
                    condition = conditions.Add();
                    condition.Alias = "TreeType";
                    condition.Operation = BoConditionOperation.co_EQUAL;
                    condition.CondVal = "P";

                    //// Filtra também se a refeição é formada apenas por proteína.
                    //condition.Relationship = BoConditionRelationship.cr_AND;
                    //condition = conditions.Add();
                    //condition.Alias = "U_CVA_PROTEINA";
                    //condition.Operation = BoConditionOperation.co_EQUAL;
                    //condition.CondVal = dishType.GetValue("U_CVA_PROTEINA", dishType.Offset).ToString();

                    // Filtra também se o item não está inativo.
                    condition.Relationship = BoConditionRelationship.cr_AND;
                    condition = conditions.Add();
                    condition.Alias = "frozenFor";
                    condition.Operation = BoConditionOperation.co_EQUAL;
                    condition.CondVal = "N";

                    // Caso tenham sido definidos grupso de itens no tipo de pratos, realiza a filtragem por grupos de itens.
                    if (dst1.Size > 0)
                    {
                        condition.Relationship = BoConditionRelationship.cr_AND;
                        condition.BracketOpenNum = 1;

                        for (var y = 0; y < dst1.Size; y++)
                        {
                            if (y > 0) condition.Relationship = BoConditionRelationship.cr_OR;

                            condition = conditions.Add();
                            condition.Alias = "ItmsGrpCod";
                            condition.Operation = BoConditionOperation.co_EQUAL;
                            condition.CondVal = dst1.GetValue("U_ItmsGrpCod", y).ToString();
                        }

                        condition.BracketCloseNum = 1;
                    }

                    cfl.SetConditions(conditions);

                    dtMenu.Columns.Add(i.ToString(), BoFieldsType.ft_AlphaNumeric, 254);

                    newCol = mtMenu.Columns.Add(i.ToString(), BoFormItemTypes.it_LINKED_BUTTON);
                    newCol.Editable = true;
                    newCol.TitleObject.Caption = menuLineModel.GetValue("U_CVA_TIPO_PRATO_DES", i);
                    newCol.Visible = true;
                    newCol.RightJustified = false;
                    newCol.DataBind.Bind("dtMenu", i.ToString());
                    newCol.ChooseFromListUID = cfl.UniqueID;
                    newCol.ChooseFromListAlias = "ItemName";

                    var oLink = (LinkedButton)newCol.ExtendedObject;
                    oLink.LinkedObject = BoLinkedObject.lf_ProductTree;

                    MenuData.Add(i.ToString(), new List<MealData>());
                }

                mtMenu.SelectionMode = BoMatrixSelect.ms_Single;
            }
            catch (Exception ex)
            {
                throw new Exception("SetMealColumns: " + ex.Message);
            }
            finally
            {
                if (pgb != null)
                {
                    pgb.Stop();
                    Marshal.ReleaseComObject(pgb);
                    pgb = null;
                }
            }
        }

        private void SetItemsEnabledByStatus()
        {
            var omnp = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

            try
            {
                etEscape.Active = true;
            }
            catch (Exception ex)
            {

            }
            UIAPIRawForm.Freeze(true);

            switch (omnp.GetValue("U_Status", omnp.Offset))
            {
                case "P":
                    etCardCode.Item.Enabled = true;
                    etCardName.Item.Enabled = true;
                    etNumber.Item.Enabled = true;
                    etSrvGrp.Item.Enabled = true;
                    etSrv.Item.Enabled = true;
                    etMenuMod.Item.Enabled = true;
                    etRefDate.Item.Enabled = true;
                    mtMenu.Item.Enabled = true;
                    break;

                case "R":
                    etCardCode.Item.Enabled = false;
                    etCardName.Item.Enabled = false;
                    etNumber.Item.Enabled = false;
                    etSrvGrp.Item.Enabled = false;
                    etSrv.Item.Enabled = false;
                    etMenuMod.Item.Enabled = false;
                    cmStatus.Item.Enabled = false;
                    etRefDate.Item.Enabled = false;
                    mtMenu.Item.Enabled = true;
                    break;

                default:
                    etCardCode.Item.Enabled = false;
                    etCardName.Item.Enabled = false;
                    etNumber.Item.Enabled = false;
                    etSrvGrp.Item.Enabled = false;
                    etSrv.Item.Enabled = false;
                    etMenuMod.Item.Enabled = false;
                    cmStatus.Item.Enabled = false;
                    etRefDate.Item.Enabled = false;
                    mtMenu.Item.Enabled = false;
                    break;
            }

            UIAPIRawForm.Freeze(false);
        }

        private void ShowTotals()
        {
            UIAPIRawForm.Freeze(true);

            mtMenu.Item.Width -= 269;

            UIAPIRawForm.PaneLevel = 1;
            btTotals.Caption = "Esconder totalizadores";

            int selectedRow = mtMenu.GetNextSelectedRow();
            if (selectedRow == -1)
            {
                selectedRow = 1;
            }

            mtMenu.SelectRow(selectedRow, true, false);
            selectedRow--;

            var dtMenu = UIAPIRawForm.DataSources.DataTables.Item("dtMenu");
            SetTotals(int.Parse(dtMenu.GetValue("Day", selectedRow).ToString().Substring(0, 2)), true);

            UIAPIRawForm.Freeze(false);
        }

        private void HideTotals()
        {
            UIAPIRawForm.Freeze(true);

            mtMenu.Item.Width += 269;
            UIAPIRawForm.PaneLevel = 0;

            btTotals.Caption = "Apresentar totalizadores";

            UIAPIRawForm.Freeze(false);
        }

        private void SetTotals(int day, bool forceGetCosts = false)
        {
            if (UIAPIRawForm.PaneLevel == 0)
            {
                return;
            }

            ProgressBar pgb = null;
            try
            {
                this.UIAPIRawForm.Freeze(true);
                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");
                var mnp2 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MNP2");
                var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
                var dinersDict = new Dictionary<int, int>();
                var daysDict = new Dictionary<int, int>();

                if (plannerDataLine.Size == 0 || String.IsNullOrEmpty(plannerDataLine.GetValue("U_Day", 0))) return;

                for (var i = 0; i < mnp2.Size; i++)
                {
                    if (dinersDict.ContainsKey(int.Parse(mnp2.GetValue("U_Day", i))))
                    {
                        dinersDict[int.Parse(mnp2.GetValue("U_Day", i))] += int.Parse(mnp2.GetValue("U_Quantity", i));
                    }
                    else
                    {
                        dinersDict.Add(int.Parse(mnp2.GetValue("U_Day", i)), int.Parse(mnp2.GetValue("U_Quantity", i)));
                    }
                }

                dtQuery.ExecuteQuery(String.Format(@"select ""U_CVA_Valor""
                                                   from ""@CVA_CUSTO_PADRAO"" 
                                                  where ""U_CVA_Contrato"" = {0}
                                                    and ""U_CVA_Id_Servico"" = '{1}'
                                                    and ""U_CVA_Mes"" = '{2}'", plannerData.GetValue("U_CVA_ID_CONTRATO", plannerData.Offset),
                                                                                    plannerData.GetValue("U_CVA_ID_SERVICO", plannerData.Offset),
                                                                                    DateTime.ParseExact(plannerData.GetValue("U_CVA_DATA_REF", plannerData.Offset), "yyyyMMdd", null).ToString("MM/yyyy")));

                var meta = dtQuery.IsEmpty ? 0.0 : double.Parse(dtQuery.GetValue("U_CVA_Valor", 0).ToString());

                dtQuery.ExecuteQuery($@"SELECT SUM(OITW.""AvgPrice"") ""Consumption"" FROM OWOR
	                                INNER JOIN WOR1 ON WOR1.""DocEntry"" = OWOR.""DocEntry""
	                                INNER JOIN OITW ON OITW.""ItemCode"" = WOR1.""ItemCode"" AND OITW.""WhsCode"" = WOR1.""wareHouse""
                                WHERE OWOR.""Type"" = 'P'
                                AND OWOR.""ItemCode"" = '09.99.99.999'
                                AND OWOR.""U_CVA_PlanCode"" = {plannerData.GetValue("DocNum", plannerData.Offset)}");


                var totalConsumption = dtQuery.IsEmpty ? 0.0 : double.Parse(dtQuery.GetValue("Consumption", 0).ToString());
                var totalDayDiners = dinersDict.Where(x => x.Key == day).Sum(x => x.Value);
                var totalAccumulatedDiners = dinersDict.Where(x => x.Key <= day).Sum(x => x.Value);
                var totalMonthDiners = dinersDict.Sum(x => x.Value);

                var totalDayMeta = meta;

                var refDate = DateTime.ParseExact(plannerData.GetValue("U_CVA_DATA_REF", plannerData.Offset), "yyyyMMdd", null);

                stDay.Caption = String.Format("Dia {0:D2}/{1:D2}/{2}", day, refDate.Month, refDate.Year);
                stAccDay.Caption = String.Format("Acumulado até {0:D2}/{1:D2}/{2}", day, refDate.Month, refDate.Year);

                dtQuery.Clear();

                string query = @"select (sum(OITW.""AvgPrice"" * ITT1.""Quantity"") * {1} / 100) * {3} as ""TotalCost""
                                                       from OITT
                                                      inner join ITT1 on ITT1.""Father"" = OITT.""Code""
                                                      inner join OITW on OITW.""ItemCode"" = ITT1.""Code""   
                                                      where OITT.""Code"" = '{0}'   
                                                        and OITW.""WhsCode"" = (select OWHS.""WhsCode""
   						                                                          from OWHS
   						                                                         inner join OOAT on OOAT.""U_CVA_FILIAL"" = OWHS.""BPLid""
   						                                                         where OOAT.""AbsID"" = {2})";


                pgb = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar("", plannerDataLine.Size, false);
                pgb.Text = "Calculando totais";

                if (NeedRecalculateCosts || forceGetCosts)
                {
                    for (var i = 0; i < plannerDataLine.Size; i++)
                    {
                        pgb.Value++;
                        if (!String.IsNullOrEmpty(plannerDataLine.GetValue("U_CVA_INSUMO", i)))
                        {
                            if (!daysDict.ContainsKey(int.Parse(plannerDataLine.GetValue("U_Day", i))))
                            {
                                daysDict.Add(int.Parse(plannerDataLine.GetValue("U_Day", i)), 1);
                            }
                        }

                        MenuTotalCostsModel menuTotalCostsModel = CostsList.FirstOrDefault(m => m.LineId == i);
                        double dinners = dinersDict.Where(x => x.Key == int.Parse(plannerDataLine.GetValue("U_Day", i))).Sum(x => x.Value);
                        if (menuTotalCostsModel == null)
                        {
                            menuTotalCostsModel = new MenuTotalCostsModel();
                            menuTotalCostsModel.LineId = i;
                            menuTotalCostsModel.Day = int.Parse(plannerDataLine.GetValue("U_Day", i));
                            menuTotalCostsModel.ItemCode = plannerDataLine.GetValue("U_CVA_INSUMO", i);
                            if (dinners > 0)
                            {
                                dtQuery.ExecuteQuery(String.Format(query, plannerDataLine.GetValue("U_CVA_INSUMO", i), plannerDataLine.GetValue("U_CVA_PERCENT", i), plannerData.GetValue("U_AbsID", plannerData.Offset), dinners));
                                menuTotalCostsModel.RawMaterial = (double)dtQuery.GetValue("TotalCost", 0);
                            }
                            CostsList.Add(menuTotalCostsModel);
                        }
                        else
                        {
                            if (forceGetCosts && dinners > 0)
                            {
                                dtQuery.ExecuteQuery(String.Format(query, plannerDataLine.GetValue("U_CVA_INSUMO", i), plannerDataLine.GetValue("U_CVA_PERCENT", i), plannerData.GetValue("U_AbsID", plannerData.Offset), dinners));
                                menuTotalCostsModel.RawMaterial = (double)dtQuery.GetValue("TotalCost", 0);
                            }
                        }
                        menuTotalCostsModel.Goal = totalDayMeta;
                    }
                }

                CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

                // Matéria-prima = somatório do custo dos itens nas tabelas padrões do SAP
                List<MenuTotalCostsModel> totalDay = CostsList.Where(m => m.Day == day).ToList();
                List<MenuTotalCostsModel> totalAccumulated = CostsList.Where(m => m.Day <= day).ToList();

                udDayMP.Value = totalDay.Sum(m => m.RawMaterial).ToString("f2");
                udAccMP.Value = totalAccumulated.Sum(m => m.RawMaterial).ToString("f2");
                udMonthMP.Value = CostsList.Sum(m => m.RawMaterial).ToString("f2");

                // Consumo = Total dos pedidos dentro do mês
                udDayCon.Value = totalConsumption.ToString("f2");
                udAccCon.Value = totalConsumption.ToString("f2");
                udMonthCon.Value = totalConsumption.ToString("f2");

                // Custo total = Matéria-prima + consumo
                double costDay = totalDay.Sum(m => m.RawMaterial) + totalConsumption;
                double costAccumulated = totalAccumulated.Sum(m => m.RawMaterial) + totalConsumption;
                double costMonth = CostsList.Sum(m => m.RawMaterial) + totalConsumption;

                udDayCT.Value = costDay.ToString("f2");
                udAccCT.Value = costAccumulated.ToString("f2");
                udMonthCT.Value = costMonth.ToString("f2");

                // Comensais = Quantidade de pratos
                udDayCom.Value = totalDayDiners.ToString("f2");
                udAccCom.Value = totalAccumulatedDiners.ToString("f2");
                udMonthCom.Value = totalMonthDiners.ToString("f2");

                // Custo per capita = Custo total / total pratos
                double perCapitaDay = costDay > 0 ? costDay / totalDayDiners : 0;
                double perCapitaAccumulated = costAccumulated > 0 ? costAccumulated / totalAccumulatedDiners : 0;
                double perCapitaMonth = costMonth > 0 ? costMonth / totalMonthDiners + totalConsumption : 0;

                udDayCPC.Value = perCapitaDay.ToString("f2");
                udAccPCM.Value = perCapitaAccumulated.ToString("f2");
                udMonthPCM.Value = perCapitaMonth.ToString("f2");

                // Padrão (meta) = campo U_CVA_CUSTO_ITEM da tabela @CVA_CUSTO_PADRAO
                udDayMet.Value = totalDayMeta.ToString("f2");
                udAccMet.Value = totalDayMeta.ToString("f2");
                udMonthMet.Value = totalDayMeta.ToString("f2");

                // Saldo = Per capta - Padrão (meta)
                udDayBal.Value = (perCapitaDay - totalDayMeta).ToString("f2");
                udAccBal.Value = (perCapitaAccumulated - totalDayMeta).ToString("f2");
                udMonthBal.Value = (perCapitaMonth - totalDayMeta).ToString("f2");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao calcular totais: " + ex.Message);
            }
            finally
            {
                if (pgb != null)
                {
                    pgb.Stop();
                    Marshal.ReleaseComObject(pgb);
                    pgb = null;
                }
                NeedRecalculateCosts = false;
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void SetTotalsOld(int day)
        {
            if (UIAPIRawForm.PaneLevel == 0)
            {
                return;
            }

            ProgressBar pgb = null;
            try
            {
                var plannerData = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");
                var plannerDataLine = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_LN_PLANEJAMENTO");
                var mnp2 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_MNP2");
                var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
                var dinersDict = new Dictionary<int, int>();
                var daysDict = new Dictionary<int, int>();

                if (plannerDataLine.Size == 0 || String.IsNullOrEmpty(plannerDataLine.GetValue("U_Day", 0))) return;

                for (var i = 0; i < mnp2.Size; i++)
                {
                    if (dinersDict.ContainsKey(int.Parse(mnp2.GetValue("U_Day", i))))
                    {
                        dinersDict[int.Parse(mnp2.GetValue("U_Day", i))] += int.Parse(mnp2.GetValue("U_Quantity", i));
                    }
                    else
                    {
                        dinersDict.Add(int.Parse(mnp2.GetValue("U_Day", i)), int.Parse(mnp2.GetValue("U_Quantity", i)));
                    }
                }

                dtQuery.ExecuteQuery(String.Format(@"select ""U_CVA_Valor""
                                                   from ""@CVA_CUSTO_PADRAO"" 
                                                  where ""U_CVA_Contrato"" = {0}
                                                    and ""U_CVA_Id_Servico"" = '{1}'
                                                    and ""U_CVA_Mes"" = '{2}'", plannerData.GetValue("U_CVA_ID_CONTRATO", plannerData.Offset),
                                                                                    plannerData.GetValue("U_CVA_ID_SERVICO", plannerData.Offset),
                                                                                    DateTime.ParseExact(plannerData.GetValue("U_CVA_DATA_REF", plannerData.Offset), "yyyyMMdd", null).ToString("MM/yyyy")));

                var meta = dtQuery.IsEmpty ? 0.0 : double.Parse(dtQuery.GetValue("U_CVA_Valor", 0).ToString());

                dtQuery.ExecuteQuery($@"SELECT SUM(OITW.""AvgPrice"") ""Consumption"" FROM OWOR
	                                INNER JOIN WOR1 ON WOR1.""DocEntry"" = OWOR.""DocEntry""
	                                INNER JOIN OITW ON OITW.""ItemCode"" = WOR1.""ItemCode"" AND OITW.""WhsCode"" = WOR1.""wareHouse""
                                WHERE OWOR.""Type"" = 'P'
                                AND OWOR.""ItemCode"" = '09.99.99.999'
                                AND OWOR.""U_CVA_PlanCode"" = {plannerData.GetValue("DocNum", plannerData.Offset)}");


                var totalConsumption = dtQuery.IsEmpty ? 0.0 : double.Parse(dtQuery.GetValue("Consumption", 0).ToString());

                // Matéria-prima
                var udDayMP = UIAPIRawForm.DataSources.UserDataSources.Item("udDayMP");
                var udAccMP = UIAPIRawForm.DataSources.UserDataSources.Item("udAccMP");
                var udMonthMP = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthMP");

                // Consumo
                var udDayCon = UIAPIRawForm.DataSources.UserDataSources.Item("udDayCon");
                var udAccCon = UIAPIRawForm.DataSources.UserDataSources.Item("udAccCon");
                var udMonthCon = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthCon");

                // Custo total
                var udDayCT = UIAPIRawForm.DataSources.UserDataSources.Item("udDayCT");
                var udAccCT = UIAPIRawForm.DataSources.UserDataSources.Item("udAccCT");
                var udMonthCT = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthCT");

                // Custo per capita
                var udDayCPC = UIAPIRawForm.DataSources.UserDataSources.Item("udDayCPC");

                // Comensais
                var udDayCom = UIAPIRawForm.DataSources.UserDataSources.Item("udDayCom");
                var udAccCom = UIAPIRawForm.DataSources.UserDataSources.Item("udAccCom");
                var udMonthCom = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthCom");

                // Per capta médio
                var udAccPCM = UIAPIRawForm.DataSources.UserDataSources.Item("udAccPCM");
                var udMonthPCM = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthPCM");

                // Padrão (meta)
                var udDayMet = UIAPIRawForm.DataSources.UserDataSources.Item("udDayMet");
                var udAccMet = UIAPIRawForm.DataSources.UserDataSources.Item("udAccMet");
                var udMonthMet = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthMet");

                // Saldo
                var udDayBal = UIAPIRawForm.DataSources.UserDataSources.Item("udDayBal");
                var udAccBal = UIAPIRawForm.DataSources.UserDataSources.Item("udAccBal");
                var udMonthBal = UIAPIRawForm.DataSources.UserDataSources.Item("udMonthBal");

                var totalDayCost = 0.0;
                var totalAccumulatedCost = 0.0;
                var totalMonthCost = 0.0;

                //var totalDayConsumption = 0.0;
                //var totalAccumulatedConsumption = 0.0;
                //var totalMonthConsumption = totalConsumption;

                var totalDayDiners = dinersDict.Where(x => x.Key == day).Sum(x => x.Value);
                var totalAccumulatedDiners = dinersDict.Where(x => x.Key <= day).Sum(x => x.Value);
                var totalMonthDiners = dinersDict.Sum(x => x.Value);

                var totalDayMeta = meta;

                var refDate = DateTime.ParseExact(plannerData.GetValue("U_CVA_DATA_REF", plannerData.Offset), "yyyyMMdd", null);

                stDay.Caption = String.Format("Dia {0:D2}/{1:D2}/{2}", day, refDate.Month, refDate.Year);
                stAccDay.Caption = String.Format("Acumulado até {0:D2}/{1:D2}/{2}", day, refDate.Month, refDate.Year);

                dtQuery.Clear();

                string query = @"select (sum(OITW.""AvgPrice"" * ITT1.""Quantity"") * {1} / 100) * {3} as ""TotalCost""
                                                       from OITT
                                                      inner join ITT1 on ITT1.""Father"" = OITT.""Code""
                                                      inner join OITW on OITW.""ItemCode"" = ITT1.""Code""   
                                                      where OITT.""Code"" = '{0}'   
                                                        and OITW.""WhsCode"" = (select OWHS.""WhsCode""
   						                                                          from OWHS
   						                                                         inner join OOAT on OOAT.""U_CVA_FILIAL"" = OWHS.""BPLid""
   						                                                         where OOAT.""AbsID"" = {2})";


                pgb = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar("", plannerDataLine.Size, false);
                pgb.Text = "Calculando totais";

                for (var i = 0; i < plannerDataLine.Size; i++)
                {
                    pgb.Value++;
                    if (!String.IsNullOrEmpty(plannerDataLine.GetValue("U_CVA_INSUMO", i)))
                    {
                        if (!daysDict.ContainsKey(int.Parse(plannerDataLine.GetValue("U_Day", i))))
                        {
                            daysDict.Add(int.Parse(plannerDataLine.GetValue("U_Day", i)), 1);
                        }
                    }

                    var dinners = dinersDict.Where(x => x.Key == int.Parse(plannerDataLine.GetValue("U_Day", i))).Sum(x => x.Value);
                    if (dinners > 0)
                    {
                        dtQuery.ExecuteQuery(String.Format(query, plannerDataLine.GetValue("U_CVA_INSUMO", i), plannerDataLine.GetValue("U_CVA_PERCENT", i), plannerData.GetValue("U_AbsID", plannerData.Offset), dinners));

                        if (int.Parse(plannerDataLine.GetValue("U_Day", i)) <= day)
                        {
                            totalAccumulatedCost += (double)dtQuery.GetValue("TotalCost", 0);
                        }

                        if (int.Parse(plannerDataLine.GetValue("U_Day", i)) == day)
                        {
                            totalDayCost += (double)dtQuery.GetValue("TotalCost", 0);
                        }

                        totalMonthCost += (double)dtQuery.GetValue("TotalCost", 0);
                    }
                }

                //totalDayConsumption = totalConsumption / daysDict.Sum(x => x.Value);
                //totalAccumulatedConsumption = totalConsumption / daysDict.Sum(x => x.Value) * daysDict.Where(x => x.Key <= day).Sum(x => x.Value);

                CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

                // Matéria-prima = somatório do custo dos itens nas tabelas padrões do SAP
                udDayMP.Value = totalDayCost.ToString("f2");
                udAccMP.Value = totalAccumulatedCost.ToString("f2");
                udMonthMP.Value = totalMonthCost.ToString("f2");

                // Consumo = Total dos pedidos dentro do mês
                udDayCon.Value = totalConsumption.ToString("f2"); // dividido pela quantidade de dias
                udAccCon.Value = totalConsumption.ToString("f2"); // dividido pela quantidade de dias até a data
                udMonthCon.Value = totalConsumption.ToString("f2");

                // Custo total = Matéria-prima + consumo
                udDayCT.Value = (totalDayCost + totalConsumption).ToString("f2");
                udAccCT.Value = (totalAccumulatedCost + totalConsumption).ToString("f2");
                udMonthCT.Value = (totalMonthCost + totalConsumption).ToString("f2");

                // Custo per capita = Custo total / total pratos
                udDayCPC.Value = totalDayDiners == 0 ? "0" : ((totalDayCost + totalConsumption) / totalDayDiners).ToString("f2");

                // Comensais = Quantidade de pratos
                udDayCom.Value = totalDayDiners.ToString("f2");
                udAccCom.Value = totalAccumulatedDiners.ToString("f2");
                udMonthCom.Value = totalMonthDiners.ToString("f2");

                // Per capta médio =  Custo total / total pratos
                udAccPCM.Value = totalAccumulatedDiners == 0 ? "0" : ((totalAccumulatedCost + totalConsumption) / totalAccumulatedDiners).ToString("f2");
                udMonthPCM.Value = totalMonthDiners == 0 ? "0" : ((totalMonthCost + totalConsumption) / totalMonthDiners).ToString("f2");

                // Padrão (meta) = campo U_CVA_CUSTO_ITEM da tabela @CVA_CUSTO_PADRAO
                udDayMet.Value = totalDayMeta.ToString("f2");
                udAccMet.Value = totalDayMeta.ToString("f2");
                udMonthMet.Value = totalDayMeta.ToString("f2");

                // Saldo = Per capta - Padrão (meta)
                udDayBal.Value = ((totalDayDiners == 0 ? 0 : ((totalDayCost + totalConsumption) / totalDayDiners)) - totalDayMeta).ToString("f2");
                udAccBal.Value = ((totalAccumulatedDiners == 0 ? 0 : ((totalAccumulatedCost + totalConsumption) / totalAccumulatedDiners)) - totalDayMeta).ToString("f2");
                udMonthBal.Value = ((totalMonthDiners == 0 ? 0 : ((totalMonthCost + totalConsumption) / totalMonthDiners)) - totalDayMeta).ToString("f2");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao calcular totais: " + ex.Message);
            }
            finally
            {
                if (pgb != null)
                {
                    pgb.Stop();
                    Marshal.ReleaseComObject(pgb);
                    pgb = null;
                }
                NeedRecalculateCosts = false;
            }
        }

        private Button Button0;
        private Button Button1;
        private Button Button2;
        private Button Button3;
        private Button btTotals;

        private ComboBox cmStatus;

        private EditText etNumber;
        private EditText etAbsID;
        private EditText etCardCode;
        private EditText etCardName;
        private EditText etDocNum;
        private EditText etRefDate;
        private EditText etEndDate;
        private EditText etSrvGrp;
        private EditText etSrv;
        private EditText etMenuMod;
        private EditText etTotMP;
        private EditText etTotCon;
        private EditText etTotCT;
        private EditText etTotCPC;
        private EditText etTotMet;
        private EditText etTotBal;
        private EditText etATotMP;
        private EditText etATotCon;
        private EditText etATotCT;
        private EditText etATotCPC;
        private EditText etATotPCM;
        private EditText etMTotMP;
        private EditText etMTotCon;
        private EditText etMTotCT;
        private EditText etMTotCPC;
        private EditText etMTotMet;
        private EditText etMTotBal;
        private EditText etMTotCom;

        private LinkedButton LinkedButton0;
        private LinkedButton LinkedButton1;
        private LinkedButton lkSrvGrp;
        private LinkedButton lkMenuMod;
        private LinkedButton lkSrv;

        private Matrix mtMenu;
        private Matrix mtMenuDB;

        private StaticText stDay;
        private StaticText stAccDay;
        private StaticText stMonth;
        private StaticText stTotMP;
        private StaticText stTotCon;
        private StaticText stTotCT;
        private StaticText stTotCPC;
        private StaticText stTotBal;
        private StaticText stTotMet;
        private StaticText stATotMP;
        private StaticText stATotCon;
        private StaticText stATotCT;
        private StaticText stATotCPC;
        private StaticText stATotMet;
        private StaticText stMTotMP;
        private StaticText stMTotCon;
        private StaticText stMTotCT;
        private StaticText stMTotPCM;
        private StaticText stMTotBal;
        private StaticText stMTotMet;
        private StaticText StaticText0;
        private StaticText stMTotCom;
        private Button btRelease;
        private StaticText StaticText1;
        private EditText EditText0;
        private ButtonCombo btCons;
        private StaticText StaticText2;
        private StaticText StaticText3;

        private UserDataSource udDayMP;
        private UserDataSource udAccMP;
        private UserDataSource udMonthMP;
        private UserDataSource udDayCon;
        private UserDataSource udAccCon;
        private UserDataSource udMonthCon;
        private UserDataSource udDayCT;
        private UserDataSource udAccCT;
        private UserDataSource udMonthCT;
        private UserDataSource udDayCPC;
        private UserDataSource udDayCom;
        private UserDataSource udAccCom;
        private UserDataSource udMonthCom;
        private UserDataSource udAccPCM;
        private UserDataSource udMonthPCM;
        private UserDataSource udDayMet;
        private UserDataSource udAccMet;
        private UserDataSource udMonthMet;
        private UserDataSource udDayBal;
        private UserDataSource udAccBal;
        private UserDataSource udMonthBal;

        public EditText etEscape { get; private set; }
    }
}
