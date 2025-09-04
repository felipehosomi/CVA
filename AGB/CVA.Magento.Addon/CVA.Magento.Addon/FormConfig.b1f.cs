using System;
using System.Collections.Generic;
using System.Xml;
using SAPbouiCOM.Framework;
using SAPbobsCOM;

namespace CVA.Magento.Addon
{
    [FormAttribute("CVA.Magento.Addon.FormConfig", "FormConfig.b1f")]
    class FormConfig : UserFormBase
    {
        public FormConfig()
        {
        }

        private static SAPbouiCOM.Application sboApp = Application.SBO_Application;
        private static Company oCompany = (Company)sboApp.Company.GetDICompany();
        public static string ActiveItem;

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.fldVen = ((SAPbouiCOM.Folder)(this.GetItem("fldVen").Specific));
            this.fldDep = ((SAPbouiCOM.Folder)(this.GetItem("fldApi").Specific));
            this.fldAPI = ((SAPbouiCOM.Folder)(this.GetItem("fldDep").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.mtxDep = ((SAPbouiCOM.Matrix)(this.GetItem("mtxDep").Specific));
            this.mtxDep.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this.mtxDep_ClickAfter);
            this.mtxDep.ComboSelectAfter += new SAPbouiCOM._IMatrixEvents_ComboSelectAfterEventHandler(this.mtxDep_ComboSelectAfter);
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_11").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_12").Specific));
            this.txtApiUrl = ((SAPbouiCOM.EditText)(this.GetItem("txtApiUrl").Specific));
            this.txtApiUs = ((SAPbouiCOM.EditText)(this.GetItem("txtApiUs").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_15").Specific));
            this.txtApiSe = ((SAPbouiCOM.EditText)(this.GetItem("txtApiSe").Specific));
            this.cboUtil = ((SAPbouiCOM.ComboBox)(this.GetItem("cboUtil").Specific));
            this.cboDesp = ((SAPbouiCOM.ComboBox)(this.GetItem("cboDesp").Specific));
            this.txtCode = ((SAPbouiCOM.EditText)(this.GetItem("txtCode").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("txtApiC").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("txtApiCS").Specific));
            this.fldCond = ((SAPbouiCOM.Folder)(this.GetItem("fldCond").Specific));
            this.mtxCond = ((SAPbouiCOM.Matrix)(this.GetItem("mtxCond").Specific));
            this.mtxCond.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this.mtxCond_ClickAfter);
            this.fldForma = ((SAPbouiCOM.Folder)(this.GetItem("fldForma").Specific));
            this.mtxFormas = ((SAPbouiCOM.Matrix)(this.GetItem("mtxFormas").Specific));
            this.mtxFormas.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this.mtxFormas_ClickAfter);
            this.fldDatas = ((SAPbouiCOM.Folder)(this.GetItem("fldDatas").Specific));
            this.mtxDatas = ((SAPbouiCOM.Matrix)(this.GetItem("mtxDatas").Specific));
            this.mtxDatas.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this.mtxDatas_ClickAfter);
            this.fldFrete = ((SAPbouiCOM.Folder)(this.GetItem("fldFrete").Specific));
            this.mtxFrete = ((SAPbouiCOM.Matrix)(this.GetItem("mtxFrete").Specific));
            this.mtxFrete.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this.mtxFrete_ClickAfter);
            this.OnCustomInitialize();
        }

        private SAPbouiCOM.Folder fldVen;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.Folder fldCond;
        private SAPbouiCOM.Matrix mtxCond;
        private SAPbouiCOM.Folder fldForma;
        private SAPbouiCOM.Matrix mtxFormas;

        private SAPbouiCOM.Folder fldDep;
        private SAPbouiCOM.Folder fldAPI;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.Matrix mtxDep;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText txtApiUrl;
        private SAPbouiCOM.EditText txtApiUs;

        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.EditText txtApiSe;
        private SAPbouiCOM.ComboBox cboUtil;
        private SAPbouiCOM.ComboBox cboDesp;
        private SAPbouiCOM.EditText txtCode;

        private SAPbouiCOM.Folder fldDatas;
        private SAPbouiCOM.Matrix mtxDatas;
        private SAPbouiCOM.Folder fldFrete;
        private SAPbouiCOM.Matrix mtxFrete;

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataAddBefore += new SAPbouiCOM.Framework.FormBase.DataAddBeforeHandler(this.Form_DataAddBefore);
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.DataUpdateBefore += new SAPbouiCOM.Framework.FormBase.DataUpdateBeforeHandler(this.Form_DataUpdateBefore);
            this.LoadAfter += new LoadAfterHandler(this.Form_LoadAfter);

        }

        private void OnCustomInitialize()
        {
            //-- Centralizar
            //UIAPIRawForm.Left = (Application.SBO_Application.Desktop.Width - UIAPIRawForm.Width) / 2;
            //UIAPIRawForm.Top = ((Application.SBO_Application.Desktop.Height - UIAPIRawForm.Height) / 2) - 100;

            this.UIAPIRawForm.Visible = true;
            UIAPIRawForm.Freeze(true);

            try
            {
                sboApp.SetStatusBarMessage("Carregando..", SAPbouiCOM.BoMessageTime.bmt_Long, false);
                this.UIAPIRawForm.EnableMenu("1292", true);
                this.UIAPIRawForm.EnableMenu("1293", true);

                Util.PreencherCombo(this.cboUtil, oCompany.CompanyDB, "OUSG", String.Empty);
                Util.PreencherCombo(this.cboDesp, oCompany.CompanyDB, "OEXD", String.Empty);

                var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("DT_0");
                dtQuery.ExecuteQuery(@"SELECT * FROM ""@CVA_CONFIG_MAG""");

                if (!dtQuery.IsEmpty)
                {
                    sboApp.Menus.Item("1291").Activate();

                    /*
                    dtQuery.ExecuteQuery(@"SELECT ""Code"" FROM ""@CVA_CONFIG_MAG""");
                    UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                    this.txtCode.Item.Visible = true;
                    this.txtCode.Item.Enabled = true;
                    this.txtCode.Value = dtQuery.GetValue("Code", 0).ToString();
                    this.Button0.Item.Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                    this.txtCode.Item.Visible = false;
                    */

                    /*
                    UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;

                    dtQuery.ExecuteQuery(@"SELECT ""Code"" FROM ""@CVA_CONFIG_MAG""");

                    var conditions = new SAPbouiCOM.Conditions();
                    var condition = conditions.Add();
                    condition.Alias = "Code";
                    condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    condition.CondVal = dtQuery.GetValue("Code", 0).ToString();

                    var CVA_CONFIG_MAG = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG");
                    CVA_CONFIG_MAG.Query(conditions);
                    var CVA_CONFIG_MAG1 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG1");
                    CVA_CONFIG_MAG1.Query(conditions);
                    var CVA_CONFIG_MAG2 = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG2");
                    CVA_CONFIG_MAG2.Query(conditions);
                    var CVA_CONFIG_MAG3= UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG3");
                    CVA_CONFIG_MAG3.Query(conditions);
                    */
                }

                Util.PreencherMatrix(sboApp.Forms.ActiveForm, oCompany.CompanyDB);
                this.UIAPIRawForm.Items.Item("fldVen").Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                sboApp.SetStatusBarMessage("Carregado com sucesso.", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                UIAPIRawForm.Freeze(false);
                sboApp.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }
        
        private void Form_DataAddBefore(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                SAPbouiCOM.Form oForm = sboApp.Forms.ActiveForm;
                Util.ValidarMatrix(oForm);
                this.txtCode.Value = Util.GetNextCodeHana(oCompany.CompanyDB, "CVA_CONFIG_MAG");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Form_DataUpdateBefore(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                SAPbouiCOM.Form oForm = sboApp.Forms.ActiveForm;
                Util.ValidarMatrix(oForm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            try
            {
                Util.PreencherCombo(this.cboUtil, oCompany.CompanyDB, "OUSG", String.Empty);
                Util.PreencherCombo(this.cboDesp, oCompany.CompanyDB, "OEXD", String.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void mtxDep_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID == "Col_0")
                {
                    SAPbouiCOM.ComboBox objComboFilial = ((SAPbouiCOM.ComboBox)this.mtxDep.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific);
                    SAPbouiCOM.ComboBox objComboDeposito = ((SAPbouiCOM.ComboBox)this.mtxDep.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific);
                    Util.PreencherCombo(objComboDeposito, oCompany.CompanyDB, "OWHS", objComboFilial.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void mtxDep_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                ActiveItem = pVal.ItemUID;
            }
            catch (Exception ex)
            {
            }
        }

        private void mtxCond_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                ActiveItem = pVal.ItemUID;
            }
            catch (Exception ex)
            {
            }
        }

        private void mtxFormas_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                ActiveItem = pVal.ItemUID;
            }
            catch (Exception ex)
            {
            }
        }
        
        private void mtxDatas_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                ActiveItem = pVal.ItemUID;
            }
            catch (Exception ex)
            {
            }
        }
        
        private void mtxFrete_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                ActiveItem = pVal.ItemUID;
            }
            catch (Exception ex)
            {
            }
        }

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            throw new System.NotImplementedException();

        }
    }
}