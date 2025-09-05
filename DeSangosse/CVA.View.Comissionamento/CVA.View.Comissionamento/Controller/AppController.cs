using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVA.View.Comissionamento.Helpers;
using System.Data;


namespace CVA.View.Comissionamento.Controller
{
    public class AppController
    {
        private SAPbobsCOM.Company Company { get; set; }
        private SAPbouiCOM.Application Application { get; set; }
        private bool pickerClicked = false;
        private bool added = false;
        private SAPbouiCOM.Grid grid;

        public AppController()
        {
            Connect();
            VerifyData();
            SetMenus();
            SetFilters();
            SetEvents();
        }

        private void Connect()
        {
            Company = B1Connection.Instance.Company;
            Application = B1Connection.Instance.Application;
        }

        private void VerifyData()
        {
            try
            {
                #region CVA_TIPO_COMISSAO
                if (!UserTables.Exists("CVA_TIPO_COMISSAO"))
                    UserTables.Create("CVA_TIPO_COMISSAO", "[CVA] Tipos de comissão", SAPbobsCOM.BoUTBTableType.bott_MasterData);

                if (!UserFields.Exists("@CVA_TIPO_COMISSAO", "TIPO"))
                    UserFields.Create("CVA_TIPO_COMISSAO", "TIPO", "É tipo vendedor", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "Y", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                #endregion

                #region CVA_CRIT_COMISSAO
                if (!UserTables.Exists("CVA_CRIT_COMISSAO"))
                    UserTables.Create("CVA_CRIT_COMISSAO", "[CVA] Critérios de comissão", SAPbobsCOM.BoUTBTableType.bott_MasterData);

                if (!UserFields.Exists("@CVA_CRIT_COMISSAO", "POS"))
                    UserFields.Create("CVA_CRIT_COMISSAO", "POS", "Posição", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_CRIT_COMISSAO", "ATIVO"))
                    UserFields.Create("CVA_CRIT_COMISSAO", "ATIVO", "Ativo?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "Y", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                #endregion

                #region CVA_REGR_COMISSAO
                if (!UserTables.Exists("CVA_REGR_COMISSAO"))
                    UserTables.Create("CVA_REGR_COMISSAO", "[CVA] Regras de comissão", SAPbobsCOM.BoUTBTableType.bott_MasterData);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "TIPO"))
                    UserFields.Create("CVA_REGR_COMISSAO", "TIPO", "Tipo de comissão", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "CVA_TIPO_COMISSAO");

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "COMISSIONADO"))
                    UserFields.Create("CVA_REGR_COMISSAO", "COMISSIONADO", "Comissionado", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "MOMENTO"))
                    UserFields.Create("CVA_REGR_COMISSAO", "MOMENTO", "Momento da comissão", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, new Dictionary<object, object> { { "F", "Faturamento" }, { "R", "Recebimento" }, { "P", "Pedido" } });

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "VENDEDOR"))
                    UserFields.Create("CVA_REGR_COMISSAO", "VENDEDOR", "Vendedor", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "ITEM"))
                    UserFields.Create("CVA_REGR_COMISSAO", "ITEM", "Código do item", 20);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "GRUPO"))
                    UserFields.Create("CVA_REGR_COMISSAO", "GRUPO", "Grupo de item", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "CENTROCUSTO"))
                    UserFields.Create("CVA_REGR_COMISSAO", "CENTROCUSTO", "Centro de custo", 8);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "FABRICANTE"))
                    UserFields.Create("CVA_REGR_COMISSAO", "FABRICANTE", "Fabricante", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "CLIENTE"))
                    UserFields.Create("CVA_REGR_COMISSAO", "CLIENTE", "Cliente", 15);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "CIDADE"))
                    UserFields.Create("CVA_REGR_COMISSAO", "CIDADE", "Cidade", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "ESTADO"))
                    UserFields.Create("CVA_REGR_COMISSAO", "ESTADO", "Estado", 2);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "SETOR"))
                    UserFields.Create("CVA_REGR_COMISSAO", "SETOR", "Setor", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "COMISSAO"))
                    UserFields.Create("CVA_REGR_COMISSAO", "COMISSAO", "% de comissão", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "PRIORIDADE"))
                    UserFields.Create("CVA_REGR_COMISSAO", "PRIORIDADE", "Prioridade", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "ATIVO"))
                    UserFields.Create("CVA_REGR_COMISSAO", "ATIVO", "Ativo?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "Y", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });

                if (!UserFields.Exists("@CVA_REGR_COMISSAO", "IPI"))
                    UserFields.Create("CVA_REGR_COMISSAO", "IPI", "IPI?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                #endregion

                #region CVA_CALC_COMISSAO
                if (!UserTables.Exists("CVA_CALC_COMISSAO"))
                    UserTables.Create("CVA_CALC_COMISSAO", "[CVA] Cálculo de comissão", SAPbobsCOM.BoUTBTableType.bott_MasterData);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "COMISSIONADO"))
                    UserFields.Create("CVA_CALC_COMISSAO", "COMISSIONADO", "Comissionado", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "CARDCODE"))
                    UserFields.Create("CVA_CALC_COMISSAO", "CARDCODE", "Cód. cliente", 15);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "CARDNAME"))
                    UserFields.Create("CVA_CALC_COMISSAO", "CARDNAME", "Razão social", 100);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "REGRA"))
                    UserFields.Create("CVA_CALC_COMISSAO", "REGRA", "Regra de comissão", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "CVA_REGR_COMISSAO");

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "DOCDATE"))
                    UserFields.Create("CVA_CALC_COMISSAO", "DOCDATE", "Data do documento", 12, SAPbobsCOM.BoFieldTypes.db_Date);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "DUEDATE"))
                    UserFields.Create("CVA_CALC_COMISSAO", "DUEDATE", "Data do vencimento", 12, SAPbobsCOM.BoFieldTypes.db_Date);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "DOCENTRY"))
                    UserFields.Create("CVA_CALC_COMISSAO", "DOCENTRY", "Chave do documento", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "OBJTYPE"))
                    UserFields.Create("CVA_CALC_COMISSAO", "OBJTYPE", "Tipo do documento", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "ITEMCODE"))
                    UserFields.Create("CVA_CALC_COMISSAO", "ITEMCODE", "Cód. item", 20);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "ITEMNAME"))
                    UserFields.Create("CVA_CALC_COMISSAO", "ITEMNAME", "Descrição do item", 100);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "LINENUM"))
                    UserFields.Create("CVA_CALC_COMISSAO", "LINENUM", "Linha", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "VALOR"))
                    UserFields.Create("CVA_CALC_COMISSAO", "VALOR", "Valor", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "PARCELA"))
                    UserFields.Create("CVA_CALC_COMISSAO", "PARCELA", "Parcela", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "IMPOSTOS"))
                    UserFields.Create("CVA_CALC_COMISSAO", "IMPOSTOS", "Impostos", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "COMISSAO"))
                    UserFields.Create("CVA_CALC_COMISSAO", "COMISSAO", "% de comissão", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "VALORCOMISSAO"))
                    UserFields.Create("CVA_CALC_COMISSAO", "VALORCOMISSAO", "Valor de comissão", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "CENTROCUSTO"))
                    UserFields.Create("CVA_CALC_COMISSAO", "CENTROCUSTO", "Centro de custo", 8);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "PAGO"))
                    UserFields.Create("CVA_CALC_COMISSAO", "PAGO", "Comissão paga?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "DATAPAGAMENTO"))
                    UserFields.Create("CVA_CALC_COMISSAO", "DATAPAGAMENTO", "Data de pagamento", 12, SAPbobsCOM.BoFieldTypes.db_Date);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "DOCENTRY_NF"))
                    UserFields.Create("CVA_CALC_COMISSAO", "DOCENTRY_NF", "Chave do documento NFS", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);

                if (!UserFields.Exists("@CVA_CALC_COMISSAO", "DOCENTRY_CR"))
                    UserFields.Create("CVA_CALC_COMISSAO", "DOCENTRY_CR", "Chave do documento ContaReceber", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                #endregion

                #region VENDEDORES & COMPRADORES                
                if (!UserFields.Exists("OSLP", "CVA_IMPINCL"))
                    UserFields.Create("OSLP", "CVA_IMPINCL", "Comissões: Impostos inclusos no preço?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });

                if (!UserFields.Exists("OSLP", "CVA_IMPADC"))
                    UserFields.Create("OSLP", "CVA_IMPADC", "Comissões: Impostos adicionais?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                #endregion

                #region UDOs
                if (!UserObjects.Exists("UDOTIPO"))
                    UserObjects.Create($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\UDOTIPO.xml");

                if (!UserObjects.Exists("UDOCRIT"))
                    UserObjects.Create($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\UDOCRIT.xml");

                if (!UserObjects.Exists("UDOREGR"))
                    UserObjects.Create($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\UDOREGR.xml");

                if (!UserObjects.Exists("UDOCALC"))
                    UserObjects.Create($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\UDOCALC.xml");
                #endregion

                #region Formatted Searches
                UserQueries.AssignFormattedSearch("TIPOCODE", Properties.Resources.TIPOCODE, "UDOTIPO", "Code");
                UserQueries.AssignFormattedSearch("CRITCODE", Properties.Resources.CRITCODE, "UDOCRIT", "Code");
                UserQueries.AssignFormattedSearch("CALCCODE", Properties.Resources.CALCCODE, "UDOCALC", "Code");
                UserQueries.AssignFormattedSearch("REGRCODE", Properties.Resources.REGRCODE, "UDOREGR", "Code");
                UserQueries.AssignFormattedSearch("REGRCENTROCUSTO", Properties.Resources.REGRCENTROCUSTO, "UDOREGR", "U_CENTROCUSTO");
                UserQueries.AssignFormattedSearch("REGRCIDADE", Properties.Resources.REGRCIDADE, "UDOREGR", "U_CIDADE");
                UserQueries.AssignFormattedSearch("REGRCLIENTE", Properties.Resources.REGRCLIENTE, "UDOREGR", "U_CLIENTE");
                UserQueries.AssignFormattedSearch("REGRCODIGOITEM", Properties.Resources.REGRCODIGOITEM, "UDOREGR", "U_ITEM");
                UserQueries.AssignFormattedSearch("REGRCOMISSIONADO", Properties.Resources.REGRCOMISSIONADO, "UDOREGR", "U_COMISSIONADO");
                UserQueries.AssignFormattedSearch("REGRESTADO", Properties.Resources.REGRESTADO, "UDOREGR", "U_ESTADO");
                UserQueries.AssignFormattedSearch("REGRFABRICANTE", Properties.Resources.REGRFABRICANTE, "UDOREGR", "U_FABRICANTE");
                UserQueries.AssignFormattedSearch("REGRGRUPOITEM", Properties.Resources.REGRGRUPOITEM, "UDOREGR", "U_GRUPO");
                UserQueries.AssignFormattedSearch("REGRSETOR", Properties.Resources.REGRSETOR, "UDOREGR", "U_SETOR");
                UserQueries.AssignFormattedSearch("REGRVENDEDOR", Properties.Resources.REGRVENDEDOR, "UDOREGR", "U_VENDEDOR");
                #endregion

                DIHelper.AddReport();

                //DIHelper.AddProcs();

                Application.StatusBar.SetText("Add-on CVA.View.Comissionamento pronto para uso.",
                    SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Application.StatusBar.SetText(ex.Message);
                throw;
            }
        }

        private void SetMenus()
        {
            Menus.Add("43520", "CVA", "CVA - Comissionamento", -1, SAPbouiCOM.BoMenuType.mt_POPUP, $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\1495154259_bank-building.jpg");
            Menus.Add("CVA", "TIPO", "Tipos de comissão", 0, SAPbouiCOM.BoMenuType.mt_STRING);
            //Menus.Add("CVA", "CRIT", "Critérios de comissão", 1, SAPbouiCOM.BoMenuType.mt_STRING);
            Menus.Add("CVA", "PRIC", "Priorização de critérios", 2, SAPbouiCOM.BoMenuType.mt_STRING);
            Menus.Add("CVA", "REGR", "Regras de comissão", 3, SAPbouiCOM.BoMenuType.mt_STRING);
            Menus.Add("CVA", "CALC", "Cálculo de comissão", 4, SAPbouiCOM.BoMenuType.mt_STRING);
            Menus.Add("CVA", "PGTO", "Pagar comissão", 5, SAPbouiCOM.BoMenuType.mt_STRING);
            Menus.Add("CVA", "REPT", "Relatório de comissão", 6, SAPbouiCOM.BoMenuType.mt_STRING);
            Menus.Add("CVA", "DEL", "Apagar Registros comissão", 7, SAPbouiCOM.BoMenuType.mt_STRING);
        }

        private void SetFilters()
        {
            Filters.Add("TIPO", SAPbouiCOM.BoEventTypes.et_MENU_CLICK);
            Filters.Add("CRIT", SAPbouiCOM.BoEventTypes.et_MENU_CLICK);
            Filters.Add("PRIC", SAPbouiCOM.BoEventTypes.et_MENU_CLICK);
            Filters.Add("REGR", SAPbouiCOM.BoEventTypes.et_MENU_CLICK);
            Filters.Add("CALC", SAPbouiCOM.BoEventTypes.et_MENU_CLICK);
            Filters.Add("PGTO", SAPbouiCOM.BoEventTypes.et_MENU_CLICK);
            Filters.Add("REPT", SAPbouiCOM.BoEventTypes.et_MENU_CLICK);
            Filters.Add("DEL", SAPbouiCOM.BoEventTypes.et_MENU_CLICK);

            Filters.Add("1282", SAPbouiCOM.BoEventTypes.et_MENU_CLICK);

            Filters.Add(View.RegraComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_COMBO_SELECT);
            Filters.Add(View.RegraComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            Filters.Add(View.RegraComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_PICKER_CLICKED);
            Filters.Add(View.RegraComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_VALIDATE);
            Filters.Add(View.RegraComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_LOST_FOCUS);
            Filters.Add(View.RegraComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            Filters.Add(View.RegraComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);
            Filters.Add(View.RegraComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);
            Filters.Add(View.RegraComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);

            Filters.Add(View.PagarComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_PICKER_CLICKED);
            Filters.Add(View.PagarComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_VALIDATE);
            Filters.Add(View.PagarComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);
            Filters.Add(View.PagarComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED);

            Filters.Add(View.CalculoComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);
            Filters.Add(View.CalculoComissaoForm.Type, SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED);

            Filters.Add(View.PriorizacaoCriteriosForm.Type, SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);
            Filters.Add(View.ApagaRegistroForm.Type, SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);

        }

        private void SetEvents()
        {
            Application.AppEvent += Application_AppEvent;
            Application.FormDataEvent += Application_FormDataEvent;
            Application.ItemEvent += Application_ItemEvent;
            Application.MenuEvent += Application_MenuEvent;
        }

        private void Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                if (pVal.MenuUID.Equals("TIPO") && !pVal.BeforeAction)
                {
                    var oForm = Forms.LoadForm(View.TipoComissaoForm.FilePath);
                    oForm.DataBrowser.BrowseBy = View.TipoComissaoForm.EditCode;
                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                    oForm.Visible = true;
                    EnableMenus(oForm, true);
                }

                if (pVal.MenuUID.Equals("CRIT") && !pVal.BeforeAction)
                {
                    var oForm = Forms.LoadForm(View.CriterioComissaoForm.FilePath);
                    oForm.DataBrowser.BrowseBy = View.CriterioComissaoForm.EditCode;
                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                    oForm.Visible = true;
                    EnableMenus(oForm, true);
                }

                if (pVal.MenuUID.Equals("REGR") && !pVal.BeforeAction)
                {
                    var oForm = Forms.LoadForm(View.RegraComissaoForm.FilePath);
                    oForm.DataBrowser.BrowseBy = View.RegraComissaoForm.EditCode;
                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                    oForm.Visible = true;
                    Forms.LoadRegrForm(oForm);
                    EnableMenus(oForm, true);
                }

                if (pVal.MenuUID.Equals("CALC") && !pVal.BeforeAction)
                {
                    var oForm = Forms.LoadForm(View.CalculoComissaoForm.FilePath);
                    oForm.Visible = true;
                    Forms.LoadCalcForm(oForm);
                    EnableMenus(oForm, false);
                }

                if (pVal.MenuUID.Equals("PGTO") && !pVal.BeforeAction)
                {
                    var oForm = Forms.LoadForm(View.PagarComissaoForm.FilePath);
                    oForm.Visible = true;
                    Forms.LoadPgtoForm(oForm);
                    EnableMenus(oForm, false);
                    oForm.Items.Item(View.PagarComissaoForm.FolderResumido).Click();
                }

                if (pVal.MenuUID.Equals("REPT") && !pVal.BeforeAction)
                {
                    //TODO:
                    var menuItem = B1Connection.Instance.Application.Menus.Item("43531");
                    if (menuItem.SubMenus.Count > 0)
                    {
                        for (int i = 0; i < menuItem.SubMenus.Count; i++)
                            if (menuItem.SubMenus.Item(i).String.Contains("[CVA] Monitoramento de Comissões"))
                                menuItem.SubMenus.Item(i).Activate();
                    }
                }

                if (pVal.MenuUID.Equals("PRIC") && !pVal.BeforeAction)
                {
                    var oForm = Forms.LoadForm(View.PriorizacaoCriteriosForm.FilePath);
                    oForm.Visible = true;
                    Forms.LoadPricForm(oForm);
                    EnableMenus(oForm, false);
                }

                if (pVal.MenuUID.Equals("DEL") && !pVal.BeforeAction)
                {
                    var oForm = Forms.LoadForm(View.ApagaRegistroForm.FilePath);
                    oForm.Visible = true;
                    Forms.LoadDelForm(oForm);
                    EnableMenus(oForm, false);
                }

                if (pVal.MenuUID.Equals("1282") && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;

                    if (oForm.BusinessObject.Type.Equals(View.RegraComissaoForm.ObjType))
                    {
                        var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery(Properties.Resources.REGRCODE);

                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCode).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();

                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCardName).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCounty).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItemName).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItmsGrpNam).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourcePrcName).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName1).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName2).ValueEx = null;
                    }

                    if (oForm.BusinessObject.Type.Equals(View.TipoComissaoForm.ObjType))
                    {
                        var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery(Properties.Resources.TIPOCODE);

                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.TipoComissaoForm.EditCode).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                    }

                    if (oForm.BusinessObject.Type.Equals(View.CriterioComissaoForm.ObjType))
                    {
                        var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery(Properties.Resources.CRITCODE);

                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.CriterioComissaoForm.EditCode).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            bubbleEvent = ret;
        }

        private void Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                #region [CVA] Regras de comissão
                if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST))
                {
                    var oCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pVal;
                    string sCFL_ID = oCFLEvento.ChooseFromListUID;

                    var oForm = Application.Forms.Item(FormUID);
                    oForm.ChooseFromLists.Item(sCFL_ID);

                    if (oForm.BusinessObject.Type.Equals(View.RegraComissaoForm.ObjType))
                    {
                        if (!oCFLEvento.BeforeAction)
                        {
                            var oDataTable = oCFLEvento.SelectedObjects;
                            string val1 = null;
                            string val2 = null;

                            try
                            {
                                val1 = Convert.ToString(oDataTable.GetValue(0, 0));
                                val2 = Convert.ToString(oDataTable.GetValue(1, 0));
                            }
                            catch { }

                            if (pVal.ItemUID.Equals(View.RegraComissaoForm.EditCodigoItem))
                            {
                                oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_ITEM", 0, val1);
                                oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItemName).ValueEx = val2;
                            }

                            if (pVal.ItemUID.Equals(View.RegraComissaoForm.EditCentroCusto))
                            {
                                oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_CENTROCUSTO", 0, val1);
                                oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourcePrcName).ValueEx = val2;
                            }

                            if (pVal.ItemUID.Equals(View.RegraComissaoForm.EditCliente))
                            {
                                oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_CLIENTE", 0, val1);
                                oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCardName).ValueEx = val2;
                            }
                        }
                    }
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditComissionado) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_PICKER_CLICKED) && pVal.BeforeAction)
                {
                    pickerClicked = true;
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditComissionado) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_VALIDATE) && !pVal.BeforeAction && pickerClicked)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditComissionado).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery($"SELECT SlpName FROM OSLP WHERE SlpCode = {oEdit.Value.ToString()}");

                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName1).ValueEx = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName1).ValueEx = string.Empty;
                        }
                    }
                    else
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName1).ValueEx = string.Empty;
                    }

                    pickerClicked = false;
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditVendedor) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_PICKER_CLICKED) && pVal.BeforeAction)
                {
                    pickerClicked = true;
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditVendedor) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_VALIDATE) && !pVal.BeforeAction && pickerClicked)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditVendedor).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery($"SELECT SlpName FROM OSLP WHERE SlpCode = {oEdit.Value.ToString()}");

                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName2).ValueEx = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName2).ValueEx = string.Empty;
                        }
                    }
                    else
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName2).ValueEx = string.Empty;
                    }

                    pickerClicked = false;
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditGrupoItem) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_PICKER_CLICKED) && pVal.BeforeAction)
                {
                    pickerClicked = true;
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditGrupoItem) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_VALIDATE) && !pVal.BeforeAction && pickerClicked)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditGrupoItem).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery($"SELECT ItmsGrpNam FROM OITB WHERE ItmsGrpCod = {oEdit.Value.ToString()}");

                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItmsGrpNam).ValueEx = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItmsGrpNam).ValueEx = string.Empty;
                        }
                    }
                    else
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItmsGrpNam).ValueEx = string.Empty;
                    }

                    pickerClicked = false;
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditCidade) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_PICKER_CLICKED) && pVal.BeforeAction)
                {
                    pickerClicked = true;
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditCidade) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_VALIDATE) && !pVal.BeforeAction && pickerClicked)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCidade).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery($"SELECT Name FROM OCNT WHERE AbsId = {oEdit.Value.ToString()}");

                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCounty).ValueEx = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCounty).ValueEx = string.Empty;
                        }
                    }
                    else
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCounty).ValueEx = string.Empty;
                    }

                    pickerClicked = false;
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditComissionado) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditComissionado).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery("SELECT Code FROM [@CVA_CRIT_COMISSAO] WHERE Name LIKE '%Vendedor%'");

                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                        }
                    }
                    else
                    {
                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                    }
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditCodigoItem) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCodigoItem).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery("SELECT Code FROM [@CVA_CRIT_COMISSAO] WHERE Name LIKE '%Item%'");

                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                        }
                    }
                    else
                    {
                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                    }
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditGrupoItem) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditGrupoItem).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery("SELECT Code FROM [@CVA_CRIT_COMISSAO] WHERE Name LIKE '%Grupo de itens%'");

                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                        }
                    }
                    else
                    {
                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                    }
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditCentroCusto) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCentroCusto).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery("SELECT Code FROM [@CVA_CRIT_COMISSAO] WHERE Name LIKE '%Centro de custo%'");

                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                        }
                    }
                    else
                    {
                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                    }
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.ComboFabricante) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oCombo = (SAPbouiCOM.ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboFabricante).Specific;

                    if (oCombo.Selected != null)
                    {
                        if (!string.IsNullOrEmpty(oCombo.Selected.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery("SELECT Code FROM [@CVA_CRIT_COMISSAO] WHERE Name LIKE '%Fabricante%'");

                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                        }
                    }
                    else
                    {
                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                    }
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditCliente) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCliente).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery("SELECT Code FROM [@CVA_CRIT_COMISSAO] WHERE Name LIKE '%Cliente%'");

                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                        }
                    }
                    else
                    {
                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                    }
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.EditCidade) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCidade).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery("SELECT Code FROM [@CVA_CRIT_COMISSAO] WHERE Name LIKE '%Cidade%'");

                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                        }
                    }
                    else
                    {
                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                    }
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.ComboEstado) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oCombo = (SAPbouiCOM.ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboEstado).Specific;

                    if (oCombo.Selected != null)
                    {
                        if (!string.IsNullOrEmpty(oCombo.Selected.Value.ToString()))
                        {
                            oForm.Items.Item(View.RegraComissaoForm.ComboEstado).Enabled = true;

                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery("SELECT Code FROM [@CVA_CRIT_COMISSAO] WHERE Name LIKE '%Estado%'");

                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                        }
                    }
                    else
                    {
                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                    }
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.ItemUID.Equals(View.RegraComissaoForm.ComboSetor) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oCombo = (SAPbouiCOM.ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboSetor).Specific;

                    if (oCombo.Selected != null)
                    {
                        if (!string.IsNullOrEmpty(oCombo.Selected.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery("SELECT Code FROM [@CVA_CRIT_COMISSAO] WHERE Name LIKE '%Setor%'");

                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                        }
                    }
                    else
                    {
                        ((SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific).Value = string.Empty;
                    }
                }

                if (!pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_VALIDATE))
                {
                    if (pVal.FormTypeEx == View.RegraComissaoForm.Type && pVal.ItemUID == View.RegraComissaoForm.ButtonOk && pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == false && pVal.ActionSuccess == true && pVal.FormMode == 3)
                    {
                        var oForm = Application.Forms.ActiveForm;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCardName).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCounty).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItemName).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItmsGrpNam).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourcePrcName).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName1).ValueEx = null;
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName2).ValueEx = null;
                        Application.ActivateMenuItem("1281");
                    }
                }

                if (pVal.FormTypeEx == View.RegraComissaoForm.Type && pVal.ItemUID == View.RegraComissaoForm.ButtonOk && pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == true)
                {
                    var oForm = Application.Forms.ActiveForm;

                    if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE || oForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                    {
                        var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditComissionado).Specific;

                        if (oEdit == null || oEdit.Value == null || string.IsNullOrEmpty(oEdit.Value.ToString()))
                            throw new Exception("Comissionado não pode ser em branco");

                        oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPercentualComissao).Specific;

                        if (oEdit == null || oEdit.Value == null || string.IsNullOrEmpty(oEdit.Value.ToString()))
                            throw new Exception("% de comissão não pode ser em branco");

                        var oCombo = (SAPbouiCOM.ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboMomentoComissao).Specific;

                        if (oCombo == null || oCombo.Selected == null || string.IsNullOrEmpty(oCombo.Selected.Value.ToString()))
                            throw new Exception("Momento da comissão deve ser selecionado.");

                    }
                }

                #endregion

                #region [CVA] Pagar comissão
                if (pVal.FormTypeEx.Equals(View.PagarComissaoForm.Type) && pVal.ItemUID.Equals(View.PagarComissaoForm.EditVendedor) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_PICKER_CLICKED) && pVal.BeforeAction)
                {
                    pickerClicked = true;
                }

                if (pVal.FormTypeEx.Equals(View.PagarComissaoForm.Type) && pVal.ItemUID.Equals(View.PagarComissaoForm.EditVendedor) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_VALIDATE) && !pVal.BeforeAction && pickerClicked)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.PagarComissaoForm.EditVendedor).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery($"SELECT SlpName FROM OSLP WHERE SlpCode = {oEdit.Value.ToString()}");

                            oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_21).ValueEx = oRecordset.Fields.Item(0).Value.ToString();
                        }
                    }

                    pickerClicked = false;
                }

                if (pVal.FormTypeEx.Equals(View.PagarComissaoForm.Type) && pVal.ItemUID.Equals(View.PagarComissaoForm.ButtonBuscar) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    var dataInicial = (SAPbouiCOM.EditText)oForm.Items.Item(View.PagarComissaoForm.EditDataInicial).Specific;
                    var dataFinal = (SAPbouiCOM.EditText)oForm.Items.Item(View.PagarComissaoForm.EditDataFinal).Specific;
                    var vendedor = (SAPbouiCOM.EditText)oForm.Items.Item(View.PagarComissaoForm.EditVendedor).Specific;

                    if (dataInicial == null || dataInicial.Value == null || string.IsNullOrEmpty(dataInicial.Value.ToString()))
                    {
                        oForm.Freeze(false);
                        throw new Exception("Data inicial não pode ser em branco.");
                    }

                    if (dataFinal == null || dataFinal.Value == null || string.IsNullOrEmpty(dataFinal.Value.ToString()))
                    {
                        oForm.Freeze(false);
                        throw new Exception("Data final não pode ser em branco.");
                    }

                    var resumido = ComissoesController.GetResumido(dataInicial.Value.ToString(), dataFinal.Value.ToString(), vendedor.Value.ToString());
                    var detalhado = ComissoesController.GetDetalhado(dataInicial.Value.ToString(), dataFinal.Value.ToString(), vendedor.Value.ToString());

                    var oMatrixDetalhado = (SAPbouiCOM.Matrix)oForm.Items.Item(View.PagarComissaoForm.MatrixDetalhado).Specific;
                    var oMatrixResumido = (SAPbouiCOM.Matrix)oForm.Items.Item(View.PagarComissaoForm.MatrixResumido).Specific;

                    oMatrixResumido.Clear();

                    var ud22 = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_22);
                    var ud23 = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_23);
                    var ud24 = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_24);

                    ud22.Value = null;
                    ud23.Value = null;
                    ud24.Value = null;

                    oMatrixResumido.Clear();
                    oMatrixResumido.LoadFromDataSourceEx(true);

                    int i = 0;
                    int j = 1;

                    foreach (DataRow item in resumido.Rows)
                    {
                        oMatrixResumido.AddRow();
                        oMatrixResumido.ClearRowData(i);
                        i++;
                    }

                    foreach (DataRow dr in resumido.Rows)
                    {
                        oMatrixResumido.ClearRowData(j);

                        ud22.Value = dr["U_COMISSIONADO"].ToString();
                        ud23.Value = dr["U_SLPNAME"].ToString();
                        ud24.Value = dr["U_VALORCOMISSAO"].ToString();

                        oMatrixResumido.SetLineData(j);

                        j++;
                    }

                    oMatrixResumido.AutoResizeColumns();

                    oMatrixDetalhado.Clear();

                    var udColRow = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col1);
                    var udColComissionado = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col2);
                    var udColCodCliente = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col3);
                    var udColRazaoSocial = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col4);
                    var udColRegraDeComissao = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col5);
                    var udColDataDoDocumento = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col6);
                    var udColDataDoVencimento = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col7);
                    var udColChaveDoDocumento = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col8);
                    var udColCodDoItem = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col9);
                    var udColDescricaoDoItem = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col10);
                    var udColCentroDeCusto = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col11);
                    var udColValor = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col12);
                    var udColParcela = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col13);
                    var udColImpostos = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col14);
                    var udColPercentualDeComissao = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col15);
                    var udColValorDeComissao = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col16);
                    var udColTipoDoDocumento = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col22);
                    var udColLinha = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col23);
                    var udColMomento = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col24);

                    udColRow.Value = null;
                    udColComissionado.Value = null;
                    udColCodCliente.Value = null;
                    udColRazaoSocial.Value = null;
                    udColRegraDeComissao.Value = null;
                    udColDataDoDocumento.Value = null;
                    udColDataDoVencimento.Value = null;
                    udColChaveDoDocumento.Value = null;
                    udColCodDoItem.Value = null;
                    udColDescricaoDoItem.Value = null;
                    udColCentroDeCusto.Value = null;
                    udColValor.Value = null;
                    udColParcela.Value = null;
                    udColImpostos.Value = null;
                    udColPercentualDeComissao.Value = null;
                    udColValorDeComissao.Value = null;
                    udColTipoDoDocumento.Value = null;
                    udColLinha.Value = null;
                    udColMomento.Value = null;

                    oMatrixDetalhado.Clear();
                    oMatrixDetalhado.LoadFromDataSourceEx(true);

                    i = 0;
                    j = 1;

                    foreach (DataRow item in detalhado.Rows)
                    {
                        oMatrixDetalhado.AddRow();
                        oMatrixDetalhado.ClearRowData(i);
                        i++;
                    }

                    foreach (DataRow dr in detalhado.Rows)
                    {
                        oMatrixDetalhado.ClearRowData(j);

                        udColRow.Value = dr["Row"].ToString();
                        udColComissionado.Value = dr["Comissionado"].ToString();
                        udColCodCliente.Value = dr["CardCode"].ToString();
                        udColRazaoSocial.Value = dr["CardName"].ToString();
                        udColRegraDeComissao.Value = dr["Regra"].ToString();
                        udColDataDoDocumento.Value = DateTime.Parse(dr["DocDate"].ToString()).Date.ToString("yyyyMMdd");
                        udColDataDoVencimento.Value = DateTime.Parse(dr["DueDate"].ToString()).Date.ToString("yyyyMMdd");
                        udColChaveDoDocumento.Value = dr["DocEntry"].ToString();
                        udColCodDoItem.Value = dr["ItemCode"].ToString();
                        udColDescricaoDoItem.Value = dr["ItemName"].ToString();
                        udColCentroDeCusto.Value = dr["OcrCode"].ToString();
                        udColValor.Value = dr["Total"].ToString();
                        udColParcela.Value = dr["InstId"].ToString();
                        udColImpostos.Value = dr["TaxSum"].ToString();
                        udColPercentualDeComissao.Value = dr["PercentComissao"].ToString();
                        udColValorDeComissao.Value = dr["ValorComissao"].ToString();
                        udColTipoDoDocumento.Value = dr["ObjType"].ToString();
                        udColLinha.Value = dr["Linha"].ToString();
                        udColMomento.Value = dr["Momento"].ToString();

                        oMatrixDetalhado.SetLineData(j);

                        j++;
                    }

                    oMatrixDetalhado.AutoResizeColumns();

                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                    oForm.Freeze(false);
                }

                if (pVal.FormTypeEx.Equals(View.PagarComissaoForm.Type) && pVal.ItemUID.Equals(View.PagarComissaoForm.ButtonConfirmar) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    if (Application.MessageBox("Confirmar o pagamento das comissões?", 1, "Sim", "Não") == 1)
                    {
                        oForm.Freeze(true);

                        if (oForm.PaneLevel == 1)
                        {
                            var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(View.PagarComissaoForm.MatrixResumido).Specific;

                            for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                            {
                                oMatrix.GetLineData(i);
                                var vendedor = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_22).ValueEx;
                                var pagar = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_25).ValueEx;
                                var dataPagamento = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_26).ValueEx;

                                if (pagar == "Y")
                                {
                                    if (string.IsNullOrEmpty(dataPagamento))
                                    {
                                        oForm.Freeze(false);
                                        throw new Exception("Data de pagamento obrigatória.");
                                    }
                                    ComissoesController.SetInvoices(vendedor, dataPagamento);
                                }
                            }
                            oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                            oForm.Freeze(false);
                            Application.MessageBox("Processo finalizado com sucesso.");
                            oForm.Close();
                            Application.ActivateMenuItem("PGTO");
                        }
                        else if (oForm.PaneLevel == 2)
                        {
                            var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(View.PagarComissaoForm.MatrixDetalhado).Specific;

                            for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                            {
                                oMatrix.GetLineData(i);
                                var docEntry = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col8).ValueEx;
                                var objType = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col22).ValueEx;
                                var lineNum = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col23).ValueEx;
                                var pagar = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col17).ValueEx;
                                var dataPagamento = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col18).ValueEx;

                                if (pagar == "Y")
                                {
                                    if (string.IsNullOrEmpty(dataPagamento))
                                    {
                                        oForm.Freeze(false);
                                        throw new Exception("Data de pagamento obrigatória.");
                                    }
                                    ComissoesController.SetInvoices(docEntry, objType, lineNum, dataPagamento);
                                }
                            }

                            oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                            oForm.Freeze(false);
                            Application.MessageBox("Processo finalizado com sucesso.");
                            oForm.Close();
                            Application.ActivateMenuItem("PGTO");
                        }
                    }
                }

                if (pVal.FormTypeEx.Equals(View.PagarComissaoForm.Type) && pVal.ItemUID.Equals(View.PagarComissaoForm.MatrixDetalhado) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED) && pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(View.PagarComissaoForm.MatrixDetalhado).Specific;
                    oMatrix.GetLineData(pVal.Row);
                    SAPbouiCOM.Column oColumn;

                    if (pVal.ColUID.Equals(View.PagarComissaoForm.DetalhadoColumnEditComissionado))
                    {
                        var ds = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col2).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            bubbleEvent = false;
                            var slpName = DIHelper.GetSlpName(ds);

                            Application.ActivateMenuItem("8454");

                            var oFormAux = Application.Forms.ActiveForm;

                            var oMatrixAux = (SAPbouiCOM.Matrix)oFormAux.Items.Item("3").Specific;
                            for (var i = 0; i < oMatrixAux.VisualRowCount; i++)
                            {
                                var col = oFormAux.DataSources.DBDataSources.Item("OSLP").GetValue("SlpName", i);
                                if (col.TrimEnd(' ') == slpName)
                                {
                                    oMatrixAux.Columns.Item(0).Cells.Item(i + 1).Click(SAPbouiCOM.BoCellClickType.ct_Regular, 0);
                                    break;
                                }
                            }
                        }
                        else
                            bubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(View.PagarComissaoForm.DetalhadoColumnEditCodigoCliente))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col3).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                            oLink.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_BusinessPartner;
                        else
                            bubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(View.PagarComissaoForm.DetalhadoColumnEditChaveNota))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col8).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            var sObjectType = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col22).ValueEx;
                            oLink.LinkedObjectType = sObjectType;
                        }
                        else
                            bubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(View.PagarComissaoForm.DetalhadoColumnEditCodigoItem))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col9).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                            oLink.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_Items;
                        else
                            bubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(View.PagarComissaoForm.DetalhadoColumnEditCentroCusto))
                    {
                        var ds = oForm.DataSources.UserDataSources.Item(View.PagarComissaoForm.UserDatasourceUD_Col11).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            bubbleEvent = false;
                            var slpName = DIHelper.GetSlpName(ds);

                            Application.ActivateMenuItem("1793");

                            var oFormAux = Application.Forms.ActiveForm;
                            oFormAux.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                            ((SAPbouiCOM.EditText)oFormAux.Items.Item("5").Specific).Value = ds;
                            oFormAux.Items.Item("1").Click();
                        }
                        else
                            bubbleEvent = false;
                    }
                    oForm.Freeze(false);
                }
                #endregion

                #region [CVA] Priorização de critérios de comissão
                if (pVal.FormTypeEx.Equals(View.PriorizacaoCriteriosForm.Type) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(View.PriorizacaoCriteriosForm.ButtonUp) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(View.PriorizacaoCriteriosForm.MatrixItens).Specific;

                    for (int i = 1; i <= oMatrix.RowCount; i++)
                    {
                        if (oMatrix.IsRowSelected(i))
                        {
                            var line = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("#", i);
                            var name = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_0", i);
                            var ativo = (SAPbouiCOM.CheckBox)oMatrix.GetCellSpecific("Col_1", i);
                            var code = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_2", i);
                            var pos = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_3", i);

                            if (!line.Value.ToString().Equals("1"))
                            {
                                if (i - 1 != 1)
                                {
                                    var linePrev = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("#", i - 1);
                                    var namePrev = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_0", i - 1);
                                    var ativoPrev = (SAPbouiCOM.CheckBox)oMatrix.GetCellSpecific("Col_1", i - 1);
                                    var codePrev = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_2", i - 1);
                                    var posPrev = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_3", i - 1);

                                    var sCmp = B1Connection.Instance.Company.GetCompanyService();
                                    var oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                                    var oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                                    oGeneralParams.SetProperty("Code", code.Value.ToString());

                                    var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_POS", (Int32.Parse(pos.Value.ToString()) - 1).ToString());
                                    oGeneralService.Update(oGeneralData);

                                    oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                                    oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                                    oGeneralParams.SetProperty("Code", codePrev.Value.ToString());

                                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_POS", (Int32.Parse(posPrev.Value.ToString()) + 1).ToString());
                                    oGeneralService.Update(oGeneralData);

                                    Forms.LoadPricForm(oForm);
                                }
                            }
                        }
                    }
                }

                if (pVal.FormTypeEx.Equals(View.PriorizacaoCriteriosForm.Type) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(View.PriorizacaoCriteriosForm.ButtonDown) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(View.PriorizacaoCriteriosForm.MatrixItens).Specific;

                    var i = oMatrix.GetNextSelectedRow();

                    if (oMatrix.IsRowSelected(i))
                    {
                        var line = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("#", i);
                        var name = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_0", i);
                        var ativo = (SAPbouiCOM.CheckBox)oMatrix.GetCellSpecific("Col_1", i);
                        var code = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_2", i);
                        var pos = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_3", i);

                        if (!line.Value.ToString().Equals("1"))
                        {
                            if (i != 1)
                            {
                                if (i < oMatrix.RowCount)
                                {
                                    var linePos = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("#", i + 1);
                                    var namePos = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_0", i + 1);
                                    var ativoPos = (SAPbouiCOM.CheckBox)oMatrix.GetCellSpecific("Col_1", i + 1);
                                    var codePos = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_2", i + 1);
                                    var posPos = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific("Col_3", i + 1);

                                    var sCmp = B1Connection.Instance.Company.GetCompanyService();
                                    var oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                                    var oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                                    oGeneralParams.SetProperty("Code", code.Value.ToString());

                                    var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_POS", (Int32.Parse(pos.Value.ToString()) + 1).ToString());
                                    oGeneralService.Update(oGeneralData);

                                    oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                                    oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                                    oGeneralParams.SetProperty("Code", codePos.Value.ToString());

                                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_POS", (Int32.Parse(posPos.Value.ToString()) - 1).ToString());
                                    oGeneralService.Update(oGeneralData);

                                    Forms.LoadPricForm(oForm);
                                }
                            }
                        }
                    }

                    //for (int i = 1; i <= oMatrix.RowCount; i++)
                    //{

                    //}
                }

                if (pVal.FormTypeEx.Equals(View.PriorizacaoCriteriosForm.Type) && pVal.ItemUID.Equals(View.PriorizacaoCriteriosForm.MatrixItens) && pVal.ColUID.Equals(View.PriorizacaoCriteriosForm.ColumnCheckAtivo) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;

                    var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(pVal.ItemUID).Specific;

                    var oCheck = (SAPbouiCOM.CheckBox)oMatrix.GetCellSpecific(pVal.ColUID, pVal.Row);

                    var code = (SAPbouiCOM.EditText)oMatrix.GetCellSpecific(View.PriorizacaoCriteriosForm.ColumnCode, pVal.Row);

                    if (oCheck.Item.Enabled)
                    {
                        if (oCheck.Checked)
                        {
                            var sCmp = B1Connection.Instance.Company.GetCompanyService();
                            var oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                            var oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                            oGeneralParams.SetProperty("Code", code.Value.ToString());

                            var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                            oGeneralData.SetProperty("U_ATIVO", "Y");
                            oGeneralService.Update(oGeneralData);
                        }
                        else
                        {
                            var sCmp = B1Connection.Instance.Company.GetCompanyService();
                            var oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                            var oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                            oGeneralParams.SetProperty("Code", code.Value.ToString());

                            var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                            oGeneralData.SetProperty("U_ATIVO", "N");
                            oGeneralService.Update(oGeneralData);
                        }

                        Forms.LoadPricForm(oForm);
                    }
                }
                #endregion

                #region [CVA] Cálculo de comissão
                if (pVal.FormTypeEx.Equals(View.CalculoComissaoForm.Type) && pVal.ItemUID.Equals(View.CalculoComissaoForm.ButtonRecalcular) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(View.CalculoComissaoForm.MatrixItens).Specific;
                    var dataInicial = ((SAPbouiCOM.EditText)oForm.Items.Item(View.CalculoComissaoForm.EditDataInicial).Specific).Value.ToString();
                    var dataFinal = ((SAPbouiCOM.EditText)oForm.Items.Item(View.CalculoComissaoForm.EditDataFinal).Specific).Value.ToString();
                    var radioTodas = ((SAPbouiCOM.OptionBtn)oForm.Items.Item(View.CalculoComissaoForm.OptionTodas).Specific).Selected;
                    var radioPagas = ((SAPbouiCOM.OptionBtn)oForm.Items.Item(View.CalculoComissaoForm.OptionPagas).Specific).Selected;
                    var radioNaoPagas = ((SAPbouiCOM.OptionBtn)oForm.Items.Item(View.CalculoComissaoForm.OptionNaoPagas).Specific).Selected;

                    if (!radioTodas && !radioPagas && !radioNaoPagas)
                    {
                        oForm.Freeze(false);
                        throw new Exception("Selecione uma opção de filtro, pelo menos.");
                    }

                    if (string.IsNullOrEmpty(dataInicial))
                    {
                        oForm.Freeze(false);
                        throw new Exception("Data inicial é obrigatória.");
                    }

                    if (string.IsNullOrEmpty(dataFinal))
                    {
                        oForm.Freeze(false);
                        throw new Exception("Data final é obrigatória.");
                    }

                    var data = ComissoesController.GetInvoices(dataInicial, dataFinal, radioTodas, radioPagas, radioNaoPagas);
                    oMatrix.Clear();

                    var udColRow = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col1);
                    var udColComissionado = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col2);
                    var udColCodCliente = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col3);
                    var udColRazaoSocial = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col4);
                    var udColRegraDeComissao = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col5);
                    var udColDataDoDocumento = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col6);
                    var udColDataDoVencimento = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col7);
                    var udColChaveDoDocumento = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col8);
                    var udColSerial = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col23);
                    var udColCodDoItem = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col9);
                    var udColDescricaoDoItem = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col10);
                    var udColCentroDeCusto = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col11);
                    var udColValor = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col12);
                    var udColParcela = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col13);
                    var udColImpostos = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col14);
                    var udColPercentualDeComissao = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col15);
                    var udColValorDeComissao = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col16);
                    var udColComissaoPaga = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col17);
                    var udColDataDoPagamento = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col18);
                    var udColPrioridade = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col19);
                    var udColTipoDoDocumento = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col20);
                    var udColLinha = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col21);
                    var udColMomento = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col22);
                    var udColDataRecebimento = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col24);

                    udColRow.Value = null;
                    udColComissionado.Value = null;
                    udColCodCliente.Value = null;
                    udColRazaoSocial.Value = null;
                    udColRegraDeComissao.Value = null;
                    udColDataDoDocumento.Value = null;
                    udColDataDoVencimento.Value = null;
                    udColChaveDoDocumento.Value = null;
                    udColCodDoItem.Value = null;
                    udColDescricaoDoItem.Value = null;
                    udColCentroDeCusto.Value = null;
                    udColValor.Value = null;
                    udColParcela.Value = null;
                    udColImpostos.Value = null;
                    udColPercentualDeComissao.Value = null;
                    udColValorDeComissao.Value = null;
                    udColComissaoPaga.Value = null;
                    udColDataDoPagamento.Value = null;
                    udColPrioridade.Value = null;
                    udColTipoDoDocumento.Value = null;
                    udColLinha.Value = null;
                    udColMomento.Value = null;
                    udColSerial.Value = null;
                    udColDataRecebimento.Value = null;

                    oMatrix.Clear();
                    oMatrix.LoadFromDataSourceEx(true);

                    int i = 0;
                    int j = 1;

                    foreach (DataRow item in data.Rows)
                    {
                        oMatrix.AddRow();
                        oMatrix.ClearRowData(i);
                        i++;
                    }

                    foreach (DataRow dr in data.Rows)
                    {
                        oMatrix.ClearRowData(j);

                        udColRow.Value = dr["Row"].ToString();
                        udColComissionado.Value = dr["Comissionado"].ToString();
                        udColCodCliente.Value = dr["CardCode"].ToString();
                        udColRazaoSocial.Value = dr["CardName"].ToString();
                        udColRegraDeComissao.Value = dr["Regra"].ToString();
                        udColDataDoDocumento.Value = DateTime.Parse(dr["DocDate"].ToString()).Date.ToString("yyyyMMdd");
                        udColDataDoVencimento.Value = DateTime.Parse(dr["DueDate"].ToString()).Date.ToString("yyyyMMdd");
                        udColChaveDoDocumento.Value = dr["DocEntry"].ToString();
                        udColCodDoItem.Value = dr["ItemCode"].ToString();
                        udColDescricaoDoItem.Value = dr["ItemName"].ToString();
                        udColCentroDeCusto.Value = dr["OcrCode"].ToString();
                        udColValor.Value = dr["Total"].ToString();
                        udColParcela.Value = dr["InstId"].ToString();
                        udColImpostos.Value = dr["TaxSum"].ToString();
                        udColPercentualDeComissao.Value = dr["PercentComissao"].ToString();
                        udColValorDeComissao.Value = dr["ValorComissao"].ToString();
                        udColComissaoPaga.Value = dr["ComissaoPaga"].ToString();
                        udColMomento.Value = dr["Momento"].ToString();
                        udColSerial.Value = dr["Serial"].ToString();
                        var dtp = dr["DataPagamento"].ToString();

                        if (dr["DataPagamento"].ToString() != "01/01/1900 00:00:00")
                            udColDataDoPagamento.Value = DateTime.Parse(dr["DataPagamento"].ToString()).Date.ToString("yyyyMMdd");
                        else
                            udColDataDoPagamento.Value = null;

                        if (dr["DataRecebimento"].ToString() != "01/01/1900 00:00:00")
                            udColDataRecebimento.Value = DateTime.Parse(dr["DataRecebimento"].ToString()).Date.ToString("yyyyMMdd");
                        else
                            udColDataRecebimento.Value = null;

                        udColPrioridade.Value = dr["Prioridade"].ToString();
                        udColTipoDoDocumento.Value = dr["ObjType"].ToString();
                        udColLinha.Value = dr["Linha"].ToString();

                        oMatrix.SetLineData(j);

                        j++;
                    }

                    oMatrix.Columns.Item(View.CalculoComissaoForm.ColumnEditDataVencimento).Visible = false;
                    oMatrix.Columns.Item(View.CalculoComissaoForm.ColumnEditCodigoItem).Visible = false;
                    oMatrix.Columns.Item(View.CalculoComissaoForm.ColumnEditDescricaoItem).Visible = false;
                    oMatrix.Columns.Item(View.CalculoComissaoForm.ColumnEditLinhaItem).Visible = false;
                    oMatrix.Columns.Item(View.CalculoComissaoForm.ColumnEditCentroCusto).Visible = false;
                    oMatrix.AutoResizeColumns();
                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;

                    oForm.Freeze(false);
                }

                if (pVal.FormTypeEx.Equals(View.CalculoComissaoForm.Type) && pVal.ItemUID.Equals(View.CalculoComissaoForm.MatrixItens) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED) && pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(View.CalculoComissaoForm.MatrixItens).Specific;
                    oMatrix.GetLineData(pVal.Row);
                    SAPbouiCOM.Column oColumn;

                    if (pVal.ColUID.Equals(View.CalculoComissaoForm.ColumnEditComissionado))
                    {
                        var ds = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col2).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            bubbleEvent = false;
                            var slpName = DIHelper.GetSlpName(ds);

                            Application.ActivateMenuItem("8454");

                            var oFormAux = Application.Forms.ActiveForm;

                            var oMatrixAux = (SAPbouiCOM.Matrix)oFormAux.Items.Item("3").Specific;
                            for (var i = 0; i < oMatrixAux.VisualRowCount; i++)
                            {
                                var col = oFormAux.DataSources.DBDataSources.Item("OSLP").GetValue("SlpName", i);
                                if (col.TrimEnd(' ') == slpName)
                                {
                                    oMatrixAux.Columns.Item(0).Cells.Item(i + 1).Click(SAPbouiCOM.BoCellClickType.ct_Regular, 0);
                                    break;
                                }
                            }
                        }
                        else
                            bubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(View.CalculoComissaoForm.ColumnEditCodigoCliente))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col3).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                            oLink.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_BusinessPartner;
                        else
                            bubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(View.CalculoComissaoForm.ColumnEditChaveNota))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col8).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            var sObjectType = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col20).ValueEx;
                            oLink.LinkedObjectType = sObjectType;
                        }
                        else
                            bubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(View.CalculoComissaoForm.ColumnEditCodigoItem))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col9).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                            oLink.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_Items;
                        else
                            bubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(View.CalculoComissaoForm.ColumnEditCentroCusto))
                    {
                        var ds = oForm.DataSources.UserDataSources.Item(View.CalculoComissaoForm.UserDatasourceUD_Col11).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            bubbleEvent = false;
                            var slpName = DIHelper.GetSlpName(ds);

                            Application.ActivateMenuItem("1793");

                            var oFormAux = Application.Forms.ActiveForm;
                            oFormAux.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                            ((SAPbouiCOM.EditText)oFormAux.Items.Item("5").Specific).Value = ds;
                            oFormAux.Items.Item("1").Click();
                        }
                        else
                            bubbleEvent = false;
                    }
                    oForm.Freeze(false);
                }

                if (pVal.FormTypeEx.Equals(View.CalculoComissaoForm.Type) && pVal.ItemUID.Equals(View.CalculoComissaoForm.ButtonConfirmar) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    if (Application.MessageBox("Enviar estas comissões não pagas para pagamento?", 1, "Sim", "Não") == 1)
                    {
                        var oForm = Application.Forms.ActiveForm;
                        oForm.Freeze(true);

                        var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(View.CalculoComissaoForm.MatrixItens).Specific;
                        var dataInicial = ((SAPbouiCOM.EditText)oForm.Items.Item(View.CalculoComissaoForm.EditDataInicial).Specific).Value.ToString();
                        var dataFinal = ((SAPbouiCOM.EditText)oForm.Items.Item(View.CalculoComissaoForm.EditDataFinal).Specific).Value.ToString();
                        var radioTodas = ((SAPbouiCOM.OptionBtn)oForm.Items.Item(View.CalculoComissaoForm.OptionTodas).Specific).Selected;
                        var radioPagas = ((SAPbouiCOM.OptionBtn)oForm.Items.Item(View.CalculoComissaoForm.OptionPagas).Specific).Selected;
                        var radioNaoPagas = ((SAPbouiCOM.OptionBtn)oForm.Items.Item(View.CalculoComissaoForm.OptionNaoPagas).Specific).Selected;

                        var msg = ComissoesController.SetInvoices(dataInicial, dataFinal, radioTodas, radioPagas, radioNaoPagas);

                        if (!string.IsNullOrEmpty(msg))
                            Application.MessageBox(msg);

                        oForm.Freeze(false);
                        Application.MessageBox("Processo finalizado com sucesso.");
                        oForm.Close();

                        Application.ActivateMenuItem("CALC");
                    }
                }
                #endregion


                #region [CVA] Apaga Registro Comissão

                if (pVal.FormTypeEx.Equals("CVADEL") && pVal.ItemUID.Equals("bt_Busca") && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var _oForm = Application.Forms.ActiveForm;
                    _oForm.Freeze(true);

                    var _dataInicial = "1900-01-01";
                    if (!string.IsNullOrEmpty(((SAPbouiCOM.EditText)_oForm.Items.Item("tx_DataDe").Specific).Value.ToString()))
                    {
                        _dataInicial = ((SAPbouiCOM.EditText)_oForm.Items.Item("tx_DataDe").Specific).Value.ToString();
                    }
                    var _dataFinal = "1900-01-01";

                    if (!string.IsNullOrEmpty(((SAPbouiCOM.EditText)_oForm.Items.Item("tx_DataAte").Specific).Value.ToString()))
                    {
                        _dataFinal = ((SAPbouiCOM.EditText)_oForm.Items.Item("tx_DataAte").Specific).Value.ToString();
                    }

                    var _NfInicial = 0;
                    if (!string.IsNullOrEmpty(((SAPbouiCOM.EditText)_oForm.Items.Item("tx_NFde").Specific).Value.ToString()))
                    {
                        _NfInicial = Convert.ToInt32(((SAPbouiCOM.EditText)_oForm.Items.Item("tx_NFde").Specific).Value);
                    }

                    var _NfFinal = 0;
                    if (!string.IsNullOrEmpty(((SAPbouiCOM.EditText)_oForm.Items.Item("tx_NfAte").Specific).Value.ToString()))
                    {
                        _NfFinal = Convert.ToInt32(((SAPbouiCOM.EditText)_oForm.Items.Item("tx_NfAte").Specific).Value);
                    }

                    var _DocInicial = 0;
                    if (!string.IsNullOrEmpty(((SAPbouiCOM.EditText)_oForm.Items.Item("tx_NDoc").Specific).Value.ToString()))
                    {
                        _DocInicial = Convert.ToInt32(((SAPbouiCOM.EditText)_oForm.Items.Item("tx_NDoc").Specific).Value);
                    }
                    var _DocFinal = 0;
                    if (!string.IsNullOrEmpty(((SAPbouiCOM.EditText)_oForm.Items.Item("tx_DocAte").Specific).Value.ToString()))
                    {
                        _DocFinal = Convert.ToInt32(((SAPbouiCOM.EditText)_oForm.Items.Item("tx_DocAte").Specific).Value.ToString());
                    }

                    grid = _oForm.Items.Item("gr_Grid").Specific;

                    if (string.IsNullOrEmpty(_dataInicial) && string.IsNullOrEmpty(_dataFinal) &&
                        string.IsNullOrEmpty(_NfInicial.ToString()) && string.IsNullOrEmpty(_NfFinal.ToString()) &&
                        string.IsNullOrEmpty(_DocInicial.ToString()) && string.IsNullOrEmpty(_DocFinal.ToString()))
                    {
                        throw new Exception("É necessário preencher pelo menos um dos Filtros.");
                    }

                    var sql = string.Format(@"EXEC sp_CVA_LISTA_REGISTROS '{0}','{1}',{2}, {3},{4},{5}", _dataInicial, _dataFinal, Convert.ToInt32(_NfInicial), Convert.ToInt32(_NfFinal), Convert.ToInt32(_DocInicial), Convert.ToInt32(_DocFinal));

                    SAPbouiCOM.DataTable dt;

                    try
                    {
                        dt = _oForm.DataSources.DataTables.Item("dt_grid");
                    }
                    catch
                    {
                        dt = _oForm.DataSources.DataTables.Add("dt_grid");
                    }
                    dt.ExecuteQuery(sql);
                    grid.DataTable = dt;
                    grid.Columns.Item("#").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox;

                    for (int i = 1; i <= grid.Columns.Count - 1; i++)
                    {
                        grid.Columns.Item(i).Editable = false;
                    }

                    _oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                    _oForm.Freeze(false);
                }

                if (pVal.FormTypeEx.Equals("CVADEL") && pVal.ItemUID.Equals("bt_Apaga") && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var _oForm = Application.Forms.ActiveForm;

                    var result = Application.MessageBox("Está operação é irreversível. Deseja continuar ? ",1,"Sim","Não");
                    if (result == 1)
                    {
                        for (int i = 0; i < grid.Rows.Count; i++)
                        {
                            if (grid.DataTable.GetValue("#", i).ToString() == "Y")
                            {
                                int docentry = grid.DataTable.GetValue("N° Documento", i);
                                int parcela = grid.DataTable.GetValue("Parcela", i);

                                var sql = string.Format(@"delete from [@CVA_CALC_COMISSAO] where U_DOCENTRY = {0} and U_PARCELA = {1}", Convert.ToInt32(docentry), Convert.ToInt32(parcela));

                                var oRecordset = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                oRecordset.DoQuery(sql);
                            }
                        }
                        //grid.DataTable.Clear();                        
                        Application.SetStatusBarMessage("Registro(s) apagado(s) com sucesso...", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                        _oForm.Items.Item("bt_Busca").Click();

                    }                    

                }

                if (pVal.FormTypeEx.Equals("CVADEL") && pVal.ItemUID.Equals("bt_OK") && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var _oForm = Application.Forms.ActiveForm;

                    _oForm.Close();
                }
                if (pVal.FormTypeEx.Equals("CVADEL") && pVal.ItemUID.Equals("bt_cancel") && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var _oForm = Application.Forms.ActiveForm;

                    _oForm.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            bubbleEvent = ret;
        }

        private void Application_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;
            var pVal = BusinessObjectInfo;

            try
            {
                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD) && pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var prioridade = 0;

                    var editComissionado = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditComissionado).Specific;
                    var editCodigoItem = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCodigoItem).Specific;
                    var editGrupoItem = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditGrupoItem).Specific;
                    var editCentroCusto = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCentroCusto).Specific;
                    var comboFabricante = (SAPbouiCOM.ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboFabricante).Specific;
                    var editCliente = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCliente).Specific;
                    var editCidade = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCidade).Specific;
                    var comboEstado = (SAPbouiCOM.ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboEstado).Specific;
                    var comboSetor = (SAPbouiCOM.ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboSetor).Specific;
                    var editPrioridade = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific;
                    int vendedor = 0;
                    int grupoItens = 0;
                    string cliente = "";
                    string item = "";
                    string centroCusto = "";
                    string estado = "";
                    int cidade = 0;
                    int fabricante = 0;
                    int setor = 0;

                    if (comboSetor != null && comboSetor.Selected != null && !string.IsNullOrEmpty(comboSetor.Selected.Value.ToString()))
                    {
                        setor = int.Parse(comboSetor.Selected.Value.ToString());
                    }

                    if (editCidade != null && editCidade.Value != null && !string.IsNullOrEmpty(editCidade.Value.ToString()))
                    {
                        cidade = int.Parse(editCidade.Value.ToString());
                        oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_ESTADO", 0, "");
                    }
                    else
                    {
                        if (comboEstado != null && comboEstado.Selected != null && !string.IsNullOrEmpty(comboEstado.Selected.Value.ToString()))
                        {
                            estado = comboEstado.Selected.Value.ToString();
                            oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_CIDADE", 0, "");
                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCounty).ValueEx = "";
                        }
                    }

                    if (editCliente != null && editCliente.Value != null && !string.IsNullOrEmpty(editCliente.Value.ToString()))
                    {
                        cliente = editCliente.Value.ToString();
                    }

                    if (comboFabricante != null && comboFabricante.Selected != null && !string.IsNullOrEmpty(comboFabricante.Selected.Value.ToString()))
                    {
                        fabricante = int.Parse(comboFabricante.Selected.Value.ToString());
                    }

                    if (editCentroCusto != null && editCentroCusto.Value != null && !string.IsNullOrEmpty(editCentroCusto.Value.ToString()))
                    {
                        centroCusto = editCentroCusto.Value.ToString();
                    }

                    if (editCodigoItem != null && editCodigoItem.Value != null && !string.IsNullOrEmpty(editCodigoItem.Value.ToString()))
                    {
                        item = editCodigoItem.Value.ToString();
                        oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_GRUPO", 0, "");
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItmsGrpNam).ValueEx = "";
                    }
                    else
                    {
                        if (editGrupoItem != null && editGrupoItem.Value != null && !string.IsNullOrEmpty(editGrupoItem.Value.ToString()))
                        {
                            grupoItens = int.Parse(editGrupoItem.Value.ToString());
                            oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_ITEM", 0, "");
                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItemName).ValueEx = "";
                        }
                    }

                    if (editComissionado != null && editComissionado.Value != null && !string.IsNullOrEmpty(editComissionado.Value.ToString()))
                    {
                        vendedor = int.Parse(editComissionado.Value.ToString());
                    }

                    prioridade = DIHelper.GetCrieria(vendedor, grupoItens, cliente, item, centroCusto, estado, cidade, fabricante, setor);


                    oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_PRIORIDADE", 0, prioridade.ToString());
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) && pVal.BeforeAction)
                {
                    //var oForm = Application.Forms.ActiveForm;
                    var oForm = Application.Forms.Item(pVal.FormUID);
                    var prioridade = 0;

                    var editComissionado = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditComissionado).Specific;
                    var editCodigoItem = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCodigoItem).Specific;
                    var editGrupoItem = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditGrupoItem).Specific;
                    var editCentroCusto = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCentroCusto).Specific;
                    var comboFabricante = (SAPbouiCOM.ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboFabricante).Specific;
                    var editCliente = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCliente).Specific;
                    var editCidade = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCidade).Specific;
                    var comboEstado = (SAPbouiCOM.ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboEstado).Specific;
                    var comboSetor = (SAPbouiCOM.ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboSetor).Specific;
                    var editPrioridade = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditPrioridade).Specific;
                    int vendedor = 0;
                    int grupoItens = 0;
                    string cliente = "";
                    string item = "";
                    string centroCusto = "";
                    string estado = "";
                    int cidade = 0;
                    int fabricante = 0;
                    int setor = 0;

                    if (comboSetor != null && comboSetor.Selected != null && !string.IsNullOrEmpty(comboSetor.Selected.Value.ToString()))
                    {
                        setor = int.Parse(comboSetor.Selected.Value.ToString());
                    }

                    if (editCidade != null && editCidade.Value != null && !string.IsNullOrEmpty(editCidade.Value.ToString()))
                    {
                        cidade = int.Parse(editCidade.Value.ToString());
                        oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_ESTADO", 0, "");
                    }
                    else
                    {
                        if (comboEstado != null && comboEstado.Selected != null && !string.IsNullOrEmpty(comboEstado.Selected.Value.ToString()))
                        {
                            estado = comboEstado.Selected.Value.ToString();
                            oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_CIDADE", 0, "");
                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCounty).ValueEx = "";
                        }
                    }

                    if (editCliente != null && editCliente.Value != null && !string.IsNullOrEmpty(editCliente.Value.ToString()))
                    {
                        cliente = editCliente.Value.ToString();
                    }

                    if (comboFabricante != null && comboFabricante.Selected != null && !string.IsNullOrEmpty(comboFabricante.Selected.Value.ToString()))
                    {
                        fabricante = int.Parse(comboFabricante.Selected.Value.ToString());
                    }

                    if (editCentroCusto != null && editCentroCusto.Value != null && !string.IsNullOrEmpty(editCentroCusto.Value.ToString()))
                    {
                        centroCusto = editCentroCusto.Value.ToString();
                    }

                    if (editCodigoItem != null && editCodigoItem.Value != null && !string.IsNullOrEmpty(editCodigoItem.Value.ToString()))
                    {
                        item = editCodigoItem.Value.ToString();
                        oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_GRUPO", 0, "");
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItmsGrpNam).ValueEx = "";
                    }
                    else
                    {
                        if (editGrupoItem != null && editGrupoItem.Value != null && !string.IsNullOrEmpty(editGrupoItem.Value.ToString()))
                        {
                            grupoItens = int.Parse(editGrupoItem.Value.ToString());
                            oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_ITEM", 0, "");
                            oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItemName).ValueEx = "";
                        }
                    }

                    if (editComissionado != null && editComissionado.Value != null && !string.IsNullOrEmpty(editComissionado.Value.ToString()))
                    {
                        vendedor = int.Parse(editComissionado.Value.ToString());
                    }

                    prioridade = DIHelper.GetCrieria(vendedor, grupoItens, cliente, item, centroCusto, estado, cidade, fabricante, setor);

                    oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_PRIORIDADE", 0, prioridade.ToString());
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD) && !pVal.BeforeAction)
                {
                    //var oForm = Application.Forms.ActiveForm;
                    var oForm = Application.Forms.Item(pVal.FormUID);

                    var oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCliente).Specific;
                    if (oEdit != null && oEdit.Value != null && !string.IsNullOrEmpty(oEdit.Value.ToString()))
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCardName).ValueEx = DIHelper.GetCardName(oEdit.Value.ToString());
                    }

                    oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCidade).Specific;
                    if (oEdit != null && oEdit.Value != null && !string.IsNullOrEmpty(oEdit.Value.ToString()))
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCounty).ValueEx = DIHelper.GetCounty(oEdit.Value.ToString());
                    }

                    oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCodigoItem).Specific;
                    if (oEdit != null && oEdit.Value != null && !string.IsNullOrEmpty(oEdit.Value.ToString()))
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItemName).ValueEx = DIHelper.GetItemName(oEdit.Value.ToString());
                    }

                    oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditGrupoItem).Specific;
                    if (oEdit != null && oEdit.Value != null && !string.IsNullOrEmpty(oEdit.Value.ToString()))
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItmsGrpNam).ValueEx = DIHelper.GetItmsGrpNam(oEdit.Value.ToString());
                    }

                    oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditCentroCusto).Specific;
                    if (oEdit != null && oEdit.Value != null && !string.IsNullOrEmpty(oEdit.Value.ToString()))
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourcePrcName).ValueEx = DIHelper.GetPrcName(oEdit.Value.ToString());
                    }

                    oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditComissionado).Specific;
                    if (oEdit != null && oEdit.Value != null && !string.IsNullOrEmpty(oEdit.Value.ToString()))
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName1).ValueEx = DIHelper.GetSlpName(oEdit.Value.ToString());
                    }

                    oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(View.RegraComissaoForm.EditVendedor).Specific;
                    if (oEdit != null && oEdit.Value != null && !string.IsNullOrEmpty(oEdit.Value.ToString()))
                    {
                        oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName2).ValueEx = DIHelper.GetSlpName(oEdit.Value.ToString());
                    }

                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                }

                if (pVal.FormTypeEx.Equals(View.RegraComissaoForm.Type) && pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD) && pVal.BeforeAction)
                {
                    //var oForm = Application.Forms.ActiveForm;
                    var oForm = Application.Forms.Item(pVal.FormUID);

                    oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCardName).ValueEx = string.Empty;
                    oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceCounty).ValueEx = string.Empty;
                    oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItemName).ValueEx = string.Empty;
                    oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceItmsGrpNam).ValueEx = string.Empty;
                    oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourcePrcName).ValueEx = string.Empty;
                    oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName1).ValueEx = string.Empty;
                    oForm.DataSources.UserDataSources.Item(View.RegraComissaoForm.UserDataSourceSlpName2).ValueEx = string.Empty;

                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                }


            }
            catch (Exception ex)
            {
                Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            bubbleEvent = ret;
        }

        private void Application_AppEvent(SAPbouiCOM.BoAppEventTypes eventType)
        {
            switch (eventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    if (Application.Menus.Exists("REPT")) Application.Menus.RemoveEx("REPT");
                    if (Application.Menus.Exists("PGTO")) Application.Menus.RemoveEx("PGTO");
                    if (Application.Menus.Exists("CALC")) Application.Menus.RemoveEx("CALC");
                    if (Application.Menus.Exists("REGR")) Application.Menus.RemoveEx("REGR");
                    if (Application.Menus.Exists("PRIC")) Application.Menus.RemoveEx("PRIC");
                    if (Application.Menus.Exists("DEL")) Application.Menus.RemoveEx("DEL");
                    //if (Application.Menus.Exists("CRIT")) Application.Menus.RemoveEx("CRIT");
                    if (Application.Menus.Exists("TIPO")) Application.Menus.RemoveEx("TIPO");
                    if (Application.Menus.Exists("CVA")) Application.Menus.RemoveEx("CVA");
                    Environment.Exit(-1);
                    break;
            }
        }

        private void EnableMenus(SAPbouiCOM.Form oForm, bool enable)
        {
            oForm.EnableMenu("1281", enable); //Find Record
            oForm.EnableMenu("1282", enable); //Add New Record
            oForm.EnableMenu("1288", enable); //Next Record
            oForm.EnableMenu("1289", enable); //Previous Record
            oForm.EnableMenu("1290", enable); //Fist Record
            oForm.EnableMenu("1291", enable); //Last Record
        }
    }
}
