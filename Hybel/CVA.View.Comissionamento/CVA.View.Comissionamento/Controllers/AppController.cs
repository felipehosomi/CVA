using System;
using SAPbouiCOM;
using Company = SAPbobsCOM.Company;
using SAPbobsCOM;
using CVA.View.Comissionamento.Helpers;
using System.Data;
using SimpleInjector;
using CVA.View.Comissionamento.Models;

namespace CVA.View.Comissionamento.Controllers
{
    public class AppController
    {
        private Company Company { get; set; }
        private Application Application { get; set; }
        private EventFilters Filters { get; set; }
        private bool pickerClicked = false;
        private SapFactory factory;
        private DbHelper dbHelper;
        private MenuHelper menuHelper;
        private FilterHelper filterHelper;
        private FormHelper formHelper;
        private DIHelper diHelper;
        private ComissoesController comissoes;
        private Container container;

        #region AppController
        public AppController(Container container)
        {
            this.container = container;

            Connect();

            dbHelper.VerifyData();

            menuHelper.Add("43520", "CVA", "CVA - Comissionamento", -1, BoMenuType.mt_POPUP, $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Images\\if_bank-building_532649.bmp");
            //menuHelper.Add("CVA", "TIPO", "Tipos de comissão", 0, BoMenuType.mt_STRING);
            //menuHelper.Add("CVA", "PRIC", "Priorização de critérios", 2, BoMenuType.mt_STRING);
            //menuHelper.Add("CVA", "UPDT", "Atualização de regras", 7, BoMenuType.mt_STRING);
            menuHelper.Add("CVA", "REGR", "Regras de comissão", 3, BoMenuType.mt_STRING);
            menuHelper.Add("CVA", "CALC", "Cálculo de comissão", 4, BoMenuType.mt_STRING);
            menuHelper.Add("CVA", "PGTO", "Pagar comissão", 5, BoMenuType.mt_STRING);
            menuHelper.Add("CVA", "REPT", "Relatório de comissão", 6, BoMenuType.mt_STRING);
            menuHelper.Add("CVA", "META", "Metas", 8, BoMenuType.mt_STRING);
            menuHelper.Add("CVA", "EQPE", "Equipes", 10, BoMenuType.mt_STRING);
            menuHelper.Add("CVA", "GRNT", "Gerentes", 9, BoMenuType.mt_STRING);

            //filterHelper.Add("TIPO", BoEventTypes.et_MENU_CLICK);
            //filterHelper.Add("CRIT", BoEventTypes.et_MENU_CLICK);
            //filterHelper.Add("UPDT", BoEventTypes.et_MENU_CLICK);
            filterHelper.Add("PRIC", BoEventTypes.et_MENU_CLICK);
            filterHelper.Add("REGR", BoEventTypes.et_MENU_CLICK);
            filterHelper.Add("CALC", BoEventTypes.et_MENU_CLICK);
            filterHelper.Add("PGTO", BoEventTypes.et_MENU_CLICK);
            filterHelper.Add("REPT", BoEventTypes.et_MENU_CLICK);
            filterHelper.Add("META", BoEventTypes.et_MENU_CLICK);
            filterHelper.Add("EQPE", BoEventTypes.et_MENU_CLICK);
            filterHelper.Add("GRNT", BoEventTypes.et_MENU_CLICK);

            filterHelper.Add("1282", BoEventTypes.et_MENU_CLICK);

            filterHelper.Add(Views.MetaComissaoForm.Type, BoEventTypes.et_LOST_FOCUS);
            filterHelper.Add(Views.MetaComissaoForm.Type, BoEventTypes.et_VALIDATE);
            filterHelper.Add(Views.MetaComissaoForm.Type, BoEventTypes.et_FORM_DATA_ADD);

            filterHelper.Add(Views.EquipeForm.Type, BoEventTypes.et_FORM_DATA_ADD);

            filterHelper.Add(Views.GerenteForm.Type, BoEventTypes.et_LOST_FOCUS);
            filterHelper.Add(Views.GerenteForm.Type, BoEventTypes.et_PICKER_CLICKED);
            filterHelper.Add(Views.GerenteForm.Type, BoEventTypes.et_VALIDATE);

            filterHelper.Add(Views.RegraComissaoForm.Type, BoEventTypes.et_COMBO_SELECT);
            filterHelper.Add(Views.RegraComissaoForm.Type, BoEventTypes.et_PICKER_CLICKED);
            filterHelper.Add(Views.RegraComissaoForm.Type, BoEventTypes.et_VALIDATE);
            filterHelper.Add(Views.RegraComissaoForm.Type, BoEventTypes.et_LOST_FOCUS);
            filterHelper.Add(Views.RegraComissaoForm.Type, BoEventTypes.et_FORM_DATA_ADD);
            filterHelper.Add(Views.RegraComissaoForm.Type, BoEventTypes.et_FORM_DATA_UPDATE);
            filterHelper.Add(Views.RegraComissaoForm.Type, BoEventTypes.et_FORM_DATA_LOAD);
            filterHelper.Add(Views.RegraComissaoForm.Type, BoEventTypes.et_ITEM_PRESSED);

            filterHelper.Add(Views.PagarComissaoForm.Type, BoEventTypes.et_PICKER_CLICKED);
            filterHelper.Add(Views.PagarComissaoForm.Type, BoEventTypes.et_VALIDATE);
            filterHelper.Add(Views.PagarComissaoForm.Type, BoEventTypes.et_ITEM_PRESSED);
            filterHelper.Add(Views.PagarComissaoForm.Type, BoEventTypes.et_MATRIX_LINK_PRESSED);

            filterHelper.Add(Views.CalculoComissaoForm.Type, BoEventTypes.et_ITEM_PRESSED);
            filterHelper.Add(Views.CalculoComissaoForm.Type, BoEventTypes.et_MATRIX_LINK_PRESSED);

            filterHelper.Add(Views.PriorizacaoCriteriosForm.Type, BoEventTypes.et_ITEM_PRESSED);

            filterHelper.Add("9999", BoEventTypes.et_ALL_EVENTS);

            Application.AppEvent += AppEvents;
            Application.FormDataEvent += FormDataEvents;
            Application.ItemEvent += ItemEvents;
            Application.MenuEvent += MenuEvents;

            Application.StatusBar.SetText("Add-on CVA.View.Comissionamento pronto para uso.", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);

        }
        #endregion

        #region MenuEvents
        private void MenuEvents(ref MenuEvent pVal, out bool BubbleEvent)
        {
            var ret = true;

            try
            {
                if (pVal.MenuUID.Equals("TIPO") && !pVal.BeforeAction)
                {
                    var oForm = formHelper.LoadForm(Views.TipoComissaoForm.FilePath);
                    oForm.DataBrowser.BrowseBy = Views.TipoComissaoForm.EditCode;
                    oForm.Mode = BoFormMode.fm_FIND_MODE;
                    oForm.Visible = true;
                    EnableMenus(oForm, true);
                }

                if (pVal.MenuUID.Equals("CRIT") && !pVal.BeforeAction)
                {
                    var oForm = formHelper.LoadForm(Views.CriterioComissaoForm.FilePath);
                    oForm.DataBrowser.BrowseBy = Views.CriterioComissaoForm.EditCode;
                    oForm.Mode = BoFormMode.fm_FIND_MODE;
                    oForm.Visible = true;
                    EnableMenus(oForm, true);
                }

                if (pVal.MenuUID.Equals("REGR") && !pVal.BeforeAction)
                {
                    var oForm = formHelper.LoadForm(Views.RegraComissaoForm.FilePath);
                    oForm.Mode = BoFormMode.fm_FIND_MODE;
                    oForm.Visible = true;
                    formHelper.LoadRegrForm(oForm);
                    EnableMenus(oForm, true);
                    oForm.EnableFormatSearch();
                    oForm.DataBrowser.BrowseBy = Views.RegraComissaoForm.EditCode;
                }

                if (pVal.MenuUID.Equals("CALC") && !pVal.BeforeAction)
                {
                    var oForm = formHelper.LoadForm(Views.CalculoComissaoForm.FilePath);
                    oForm.Visible = true;
                    formHelper.LoadCalcForm(oForm);
                    EnableMenus(oForm, false);
                }

                if (pVal.MenuUID.Equals("META") && !pVal.BeforeAction)
                {
                    var oForm = formHelper.LoadForm(Views.MetaComissaoForm.FilePath);
                    oForm.DataBrowser.BrowseBy = Views.MetaComissaoForm.EditCode;
                    oForm.Mode = BoFormMode.fm_FIND_MODE;
                    oForm.Visible = true;
                    formHelper.LoadMetaForm(oForm);
                    EnableMenus(oForm, true);
                }

                if (pVal.MenuUID.Equals("GRNT") && !pVal.BeforeAction)
                {
                    var oForm = formHelper.LoadForm(Views.GerenteForm.FilePath);
                    oForm.DataBrowser.BrowseBy = Views.GerenteForm.EditCode;
                    oForm.Mode = BoFormMode.fm_FIND_MODE;
                    oForm.Visible = true;
                    formHelper.LoadGerenteForm(oForm);
                    EnableMenus(oForm, true);
                }

                if (pVal.MenuUID.Equals("EQPE") && !pVal.BeforeAction)
                {
                    var oForm = formHelper.LoadForm(Views.EquipeForm.FilePath);
                    oForm.DataBrowser.BrowseBy = Views.EquipeForm.EditCode;
                    oForm.Mode = BoFormMode.fm_FIND_MODE;
                    oForm.Visible = true;
                    EnableMenus(oForm, true);
                }

                if (pVal.MenuUID.Equals("PGTO") && !pVal.BeforeAction)
                {
                    var oForm = formHelper.LoadForm(Views.PagarComissaoForm.FilePath);
                    oForm.Visible = true;
                    formHelper.LoadPgtoForm(oForm);
                    EnableMenus(oForm, false);
                    oForm.Items.Item(Views.PagarComissaoForm.FolderResumido).Click();
                }

                if (pVal.MenuUID.Equals("REPT") && !pVal.BeforeAction)
                {
                    var menuItem = Application.Menus.Item("43531");
                    if (menuItem.SubMenus.Count > 0)
                    {
                        for (int i = 0; i < menuItem.SubMenus.Count; i++)
                            if (menuItem.SubMenus.Item(i).String.Contains("[CVA] Monitoramento de Comissões"))
                                menuItem.SubMenus.Item(i).Activate();
                    }
                }

                if (pVal.MenuUID.Equals("UPDT") && !pVal.BeforeAction)
                {
                    if (Application.MessageBox($"Atenção!{Environment.NewLine}Deseja confirmar a atualização das regras de comissão?{Environment.NewLine}Este processo é irreversível.", 2, "Sim", "Não") == 1)
                    {
                        diHelper.AtualizaRegras();
                        Application.MessageBox("Processo finalizado.");
                    }
                }

                if (pVal.MenuUID.Equals("PRIC") && !pVal.BeforeAction)
                {
                    var oForm = formHelper.LoadForm(Views.PriorizacaoCriteriosForm.FilePath);
                    oForm.Visible = true;
                    formHelper.LoadPricForm(oForm);
                    EnableMenus(oForm, false);
                }

                if (pVal.MenuUID.Equals("1282") && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;

                    if (oForm.BusinessObject.Type.Equals(Views.GerenteForm.ObjType))
                    {
                        Matrix mt_Item = (Matrix)oForm.Items.Item("mt_Item").Specific;
                        mt_Item.AddRow();
                    }
                    if (oForm.BusinessObject.Type.Equals(Views.MetaComissaoForm.ObjType))
                    {
                        Matrix mt_Meta = (Matrix)oForm.Items.Item("mt_Meta").Specific;
                        mt_Meta.AddRow();
                    }

                    if (oForm.BusinessObject.Type.Equals(Views.RegraComissaoForm.ObjType))
                    {
                        var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery("SELECT COALESCE(MAX(CAST(Code AS INT)), 0) + 1 FROM [@CVA_REGR_COMISSAO]");

                        ((EditText)oForm.Items.Item(Views.RegraComissaoForm.EditCode).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                    }

                    if (oForm.BusinessObject.Type.Equals(Views.TipoComissaoForm.ObjType))
                    {
                        var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery("SELECT COALESCE(MAX(CAST(Code AS INT)), 0) + 1 FROM [@CVA_TIPO_COMISSAO]");

                        ((EditText)oForm.Items.Item(Views.TipoComissaoForm.EditCode).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                    }

                    if (oForm.BusinessObject.Type.Equals(Views.MetaComissaoForm.ObjType))
                    {
                        var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery("SELECT COALESCE(MAX(CAST(Code AS INT)), 0) + 1 FROM [@CVA_META]");

                        ((EditText)oForm.Items.Item(Views.MetaComissaoForm.EditCode).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                    }

                    if (oForm.BusinessObject.Type.Equals(Views.EquipeForm.ObjType))
                    {
                        var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery("SELECT COALESCE(MAX(CAST(Code AS INT)), 0) + 1 FROM [@CVA_EQUIPE]");

                        ((EditText)oForm.Items.Item(Views.EquipeForm.EditCode).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                    }

                    if (oForm.BusinessObject.Type.Equals(Views.CriterioComissaoForm.ObjType))
                    {
                        var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery("SELECT COALESCE(MAX(CAST(Code AS INT)), 0) + 1 FROM [@CVA_CRIT_COMISSAO]");

                        ((EditText)oForm.Items.Item(Views.CriterioComissaoForm.EditCode).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            BubbleEvent = ret;
        }
        #endregion

        #region ItemEvents
        private void ItemEvents(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            var ret = true;

            try
            {
                #region [CVA] Regras de comissão                
                if (pVal.FormTypeEx.Equals(Views.RegraComissaoForm.Type) && pVal.ItemUID.Equals(Views.RegraComissaoForm.EditComissionado) && pVal.EventType.Equals(BoEventTypes.et_PICKER_CLICKED) && pVal.BeforeAction)
                {
                    pickerClicked = true;
                }

                if (pVal.FormTypeEx.Equals(Views.RegraComissaoForm.Type) && pVal.ItemUID.Equals(Views.RegraComissaoForm.EditComissionado) && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && !pVal.BeforeAction && pickerClicked)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionado).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery($"SELECT SlpName FROM OSLP WHERE SlpCode = {oEdit.Value.ToString()}");
                            ((EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionadoDesc).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                        else
                        {
                            ((EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionadoDesc).Specific).Value = string.Empty;
                        }
                    }
                    else
                    {
                        ((EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionadoDesc).Specific).Value = string.Empty;
                    }

                    pickerClicked = false;
                }

                if (pVal.FormTypeEx.Equals(Views.RegraComissaoForm.Type) && pVal.ItemUID.Equals(Views.RegraComissaoForm.EditComissionado) && pVal.EventType.Equals(BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionado).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!pickerClicked)
                        {
                            if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                            {
                                var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                                oRecordset.DoQuery($"SELECT SlpName FROM OSLP WHERE SlpCode = {oEdit.Value.ToString()}");
                                ((EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionadoDesc).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                            }
                            else
                            {
                                ((EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionadoDesc).Specific).Value = string.Empty;
                            }
                        }
                    }
                }

                if (pVal.FormTypeEx == Views.RegraComissaoForm.Type && pVal.ItemUID == Views.RegraComissaoForm.ButtonOk && pVal.EventType == BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == false && pVal.ActionSuccess == true && pVal.FormMode == 3)
                {
                    if (!pVal.EventType.Equals(BoEventTypes.et_VALIDATE))
                    {
                        var oForm = Application.Forms.ActiveForm;
                        Application.ActivateMenuItem("1281");
                    }
                }

                if (pVal.FormMode.In((int)BoFormMode.fm_ADD_MODE, (int)BoFormMode.fm_UPDATE_MODE))
                {
                    if (pVal.FormTypeEx == Views.RegraComissaoForm.Type && pVal.ItemUID == Views.RegraComissaoForm.ButtonOk && pVal.EventType == BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == true)
                    {
                        var oForm = Application.Forms.ActiveForm;

                        if (oForm.Mode == BoFormMode.fm_ADD_MODE || oForm.Mode == BoFormMode.fm_UPDATE_MODE)
                        {
                            var oEdit = (EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionado).Specific;

                            if (oEdit == null || oEdit.Value == null || string.IsNullOrEmpty(oEdit.Value.ToString()))
                                throw new Exception("Comissionado não pode ser em branco");

                            oEdit = (EditText)oForm.Items.Item(Views.RegraComissaoForm.EditPercentualComissao).Specific;

                            if (oEdit == null || oEdit.Value == null || string.IsNullOrEmpty(oEdit.Value.ToString()))
                                throw new Exception("% de comissão não pode ser em branco");

                            var oCombo = (ComboBox)oForm.Items.Item(Views.RegraComissaoForm.ComboMomentoComissao).Specific;

                            if (oCombo == null || oCombo.Selected == null || string.IsNullOrEmpty(oCombo.Selected.Value.ToString()))
                                throw new Exception("Momento da comissão deve ser selecionado.");

                        }
                    }
                }

                if (pVal.FormTypeEx.Equals(Views.RegraComissaoForm.Type) && pVal.ItemUID.Equals(Views.RegraComissaoForm.ButtonDuplicar) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;

                    if (oForm.Mode != BoFormMode.fm_EDIT_MODE || oForm.Mode != BoFormMode.fm_UPDATE_MODE)
                    {
                        var code = ((EditText)oForm.Items.Item(Views.RegraComissaoForm.EditCode).Specific).Value.ToString();

                        oForm.Close();

                        Application.ActivateMenuItem("REGR");
                        Application.ActivateMenuItem("1282");

                        oForm = Application.Forms.ActiveForm;
                        oForm.Freeze(true);
                        var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery($"SELECT * FROM [@CVA_REGR_COMISSAO] WHERE Code = N'{code}'");

                        while (!oRecordset.EoF)
                        {
                            var name = oRecordset.Fields.Item("Name").Value.ToString();
                            var tipo = oRecordset.Fields.Item("U_TIPO").Value.ToString();
                            var filial = oRecordset.Fields.Item("U_FILIAL").Value.ToString();
                            var equipe = oRecordset.Fields.Item("U_EQUIPE").Value.ToString();
                            var meta = oRecordset.Fields.Item("U_META").Value.ToString();
                            var comissionado = oRecordset.Fields.Item("U_COMISSIONADO").Value.ToString();
                            var comissionado_desc = oRecordset.Fields.Item("U_COMNAME").Value.ToString();
                            var momento = oRecordset.Fields.Item("U_MOMENTO").Value.ToString();
                            var vendedor = oRecordset.Fields.Item("U_VENDEDOR").Value.ToString();
                            var vendedor_desc = oRecordset.Fields.Item("U_VNDNAME").Value.ToString();
                            var item = oRecordset.Fields.Item("U_ITEM").Value.ToString();
                            var item_desc = oRecordset.Fields.Item("U_ITEMNAME").Value.ToString();
                            var grupo = oRecordset.Fields.Item("U_GRUPO").Value.ToString();
                            var grupo_desc = oRecordset.Fields.Item("U_ITMSGRPNAM").Value.ToString();
                            var centrocusto = oRecordset.Fields.Item("U_CENTROCUSTO").Value.ToString();
                            var centrocusto_desc = oRecordset.Fields.Item("U_PRCNAME").Value.ToString();
                            var fabricante = oRecordset.Fields.Item("U_FABRICANTE").Value.ToString();
                            var cliente = oRecordset.Fields.Item("U_CLIENTE").Value.ToString();
                            var cliente_desc = oRecordset.Fields.Item("U_CARDNAME").Value.ToString();
                            var cidade = oRecordset.Fields.Item("U_CIDADE").Value.ToString();
                            var cidade_desc = oRecordset.Fields.Item("U_COUNTY").Value.ToString();
                            var estado = oRecordset.Fields.Item("U_ESTADO").Value.ToString();
                            var setor = oRecordset.Fields.Item("U_SETOR").Value.ToString();
                            var group = oRecordset.Fields.Item("U_GROUP").Value.ToString();
                            var group_desc = oRecordset.Fields.Item("U_GROUPNAME").Value.ToString();
                            var comissao = oRecordset.Fields.Item("U_COMISSAO").Value.ToString().Replace(".", "").Replace(",", ".");
                            var comissao_real = oRecordset.Fields.Item("U_COMISSAO_REAL").Value.ToString().Replace(".", "").Replace(",", ".");
                            var prioridade = oRecordset.Fields.Item("U_PRIORIDADE").Value.ToString();
                            var ativo = oRecordset.Fields.Item("U_ATIVO").Value.ToString();
                            var porcMeta = oRecordset.Fields.Item("U_PORCMETA").Value.ToString();

                            if (name != "" && name != "0")
                                ((EditText)oForm.Items.Item(Views.RegraComissaoForm.EditName).Specific).Value = oRecordset.Fields.Item("Name").Value.ToString();
                            if (tipo != "" && tipo != "0")
                                ((ComboBox)oForm.Items.Item(Views.RegraComissaoForm.ComboTipoComissao).Specific).Select(oRecordset.Fields.Item("U_TIPO").Value.ToString());
                            if (filial != "" && filial != "0")
                                ((ComboBox)oForm.Items.Item(Views.RegraComissaoForm.ComboFilial).Specific).Select(filial);
                            if (meta != "" && meta != "0")
                                ((ComboBox)oForm.Items.Item(Views.RegraComissaoForm.ComboMeta).Specific).Select(meta);
                            if (equipe != "" && equipe != "0")
                                ((ComboBox)oForm.Items.Item(Views.RegraComissaoForm.ComboEquipe).Specific).Select(equipe);
                            if (comissionado != "" && comissionado != "0")
                                ((EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionado).Specific).Value = oRecordset.Fields.Item("U_COMISSIONADO").Value.ToString();
                            if (comissionado_desc != "" && comissionado_desc != "0")
                                ((EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionadoDesc).Specific).Value = oRecordset.Fields.Item("U_COMNAME").Value.ToString();
                            if (momento != "" && momento != "0")
                                ((ComboBox)oForm.Items.Item(Views.RegraComissaoForm.ComboMomentoComissao).Specific).Select(oRecordset.Fields.Item("U_MOMENTO").Value.ToString());
                            if (porcMeta != "" && porcMeta != "0")
                                oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_PORCMETA", 0, porcMeta);
                            if (comissao != "" && comissao != "0")
                                oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_COMISSAO", 0, comissao);
                            if (comissao_real != "" && comissao_real != "0")
                                oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_COMISSAO_REAL", 0, comissao_real);
                            if (ativo != "" && ativo != "0")
                                ((CheckBox)oForm.Items.Item(Views.RegraComissaoForm.CheckAtivo).Specific).Checked = oRecordset.Fields.Item("U_ATIVO").Value.ToString() == "Y" ? true : false;

                            oRecordset.MoveNext();
                        }
                        oForm.Freeze(false);
                    }
                }

                #endregion

                #region [CVA] Pagar comissão
                if (pVal.FormTypeEx.Equals(Views.PagarComissaoForm.Type) && pVal.ItemUID.Equals(Views.PagarComissaoForm.EditVendedor) && pVal.EventType.Equals(BoEventTypes.et_PICKER_CLICKED) && pVal.BeforeAction)
                {
                    pickerClicked = true;
                }

                if (pVal.FormTypeEx.Equals(Views.PagarComissaoForm.Type) && pVal.ItemUID.Equals(Views.PagarComissaoForm.EditVendedor) && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && !pVal.BeforeAction && pickerClicked)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (EditText)oForm.Items.Item(Views.PagarComissaoForm.EditVendedor).Specific;

                    if (oEdit.Value != null)
                    {
                        if (!string.IsNullOrEmpty(oEdit.Value.ToString()))
                        {
                            var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                            oRecordset.DoQuery($"SELECT SlpName FROM OSLP WHERE SlpCode = {oEdit.Value.ToString()}");

                            oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_21).ValueEx = oRecordset.Fields.Item(0).Value.ToString();
                        }
                    }

                    pickerClicked = false;
                }

                if (pVal.FormTypeEx.Equals(Views.PagarComissaoForm.Type) && pVal.ItemUID.Equals(Views.PagarComissaoForm.ButtonBuscar) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    var dataInicial = (EditText)oForm.Items.Item(Views.PagarComissaoForm.EditDataInicial).Specific;
                    var dataFinal = (EditText)oForm.Items.Item(Views.PagarComissaoForm.EditDataFinal).Specific;
                    var vendedor = (EditText)oForm.Items.Item(Views.PagarComissaoForm.EditVendedor).Specific;

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

                    var resumido = comissoes.GetResumido(dataInicial.Value.ToString(), dataFinal.Value.ToString(), vendedor.Value.ToString());
                    var detalhado = comissoes.GetDetalhado(dataInicial.Value.ToString(), dataFinal.Value.ToString(), vendedor.Value.ToString());

                    var oMatrixDetalhado = (Matrix)oForm.Items.Item(Views.PagarComissaoForm.MatrixDetalhado).Specific;
                    var oMatrixResumido = (Matrix)oForm.Items.Item(Views.PagarComissaoForm.MatrixResumido).Specific;

                    oMatrixResumido.Clear();

                    var ud22 = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_22);
                    var ud23 = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_23);
                    var ud24 = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_24);

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

                    var udColRow = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col1);
                    var udColComissionado = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col2);
                    var udColCodCliente = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col3);
                    var udColRazaoSocial = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col4);
                    var udColRegraDeComissao = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col5);
                    var udColDataDoDocumento = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col6);
                    var udColDataDoVencimento = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col7);
                    var udColChaveDoDocumento = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col8);
                    var udColCodDoItem = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col9);
                    var udColDescricaoDoItem = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col10);
                    var udColCentroDeCusto = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col11);
                    var udColValor = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col12);
                    var udColParcela = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col13);
                    var udColImpostos = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col14);
                    var udColPercentualDeComissao = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col15);
                    var udColValorDeComissao = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col16);
                    var udColTipoDoDocumento = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col22);
                    var udColLinha = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col23);
                    var udColMomento = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col24);

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

                    oForm.Mode = BoFormMode.fm_OK_MODE;
                    oForm.Freeze(false);
                }

                if (pVal.FormTypeEx.Equals(Views.PagarComissaoForm.Type) && pVal.ItemUID.Equals(Views.PagarComissaoForm.ButtonConfirmar) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    if (Application.MessageBox("Confirmar o pagamento das comissões?", 1, "Sim", "Não") == 1)
                    {
                        oForm.Freeze(true);

                        if (oForm.PaneLevel == 1)
                        {
                            var oMatrix = (Matrix)oForm.Items.Item(Views.PagarComissaoForm.MatrixResumido).Specific;

                            for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                            {
                                oMatrix.GetLineData(i);
                                var vendedor = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_22).ValueEx;
                                var pagar = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_25).ValueEx;
                                var dataPagamento = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_26).ValueEx;

                                if (pagar == "Y")
                                {
                                    if (string.IsNullOrEmpty(dataPagamento))
                                    {
                                        oForm.Freeze(false);
                                        throw new Exception("Data de pagamento obrigatória.");
                                    }
                                    comissoes.SetInvoices(vendedor, dataPagamento);
                                }
                            }
                            oForm.Mode = BoFormMode.fm_OK_MODE;
                            oForm.Freeze(false);
                            Application.MessageBox("Processo finalizado com sucesso.");
                            //oForm.Close();
                            //Application.ActivateMenuItem("PGTO");
                        }
                        else if (oForm.PaneLevel == 2)
                        {
                            var oMatrix = (Matrix)oForm.Items.Item(Views.PagarComissaoForm.MatrixDetalhado).Specific;

                            for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                            {
                                oMatrix.GetLineData(i);
                                var docEntry = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col8).ValueEx;
                                var objType = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col22).ValueEx;
                                var lineNum = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col23).ValueEx;
                                var pagar = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col17).ValueEx;
                                var dataPagamento = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col18).ValueEx;

                                if (pagar == "Y")
                                {
                                    if (string.IsNullOrEmpty(dataPagamento))
                                    {
                                        oForm.Freeze(false);
                                        throw new Exception("Data de pagamento obrigatória.");
                                    }
                                    comissoes.SetInvoices(docEntry, objType, lineNum, dataPagamento);
                                }
                            }

                            oForm.Mode = BoFormMode.fm_OK_MODE;
                            oForm.Freeze(false);
                            Application.MessageBox("Processo finalizado com sucesso.");
                            //oForm.Close();
                            //Application.ActivateMenuItem("PGTO");
                        }
                    }
                }

                if (pVal.FormTypeEx.Equals(Views.PagarComissaoForm.Type) && pVal.ItemUID.Equals(Views.PagarComissaoForm.MatrixDetalhado) && pVal.EventType.Equals(BoEventTypes.et_MATRIX_LINK_PRESSED) && pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    var oMatrix = (Matrix)oForm.Items.Item(Views.PagarComissaoForm.MatrixDetalhado).Specific;
                    oMatrix.GetLineData(pVal.Row);
                    Column oColumn;

                    if (pVal.ColUID.Equals(Views.PagarComissaoForm.DetalhadoColumnEditComissionado))
                    {
                        var ds = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col2).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            BubbleEvent = false;
                            var slpName = diHelper.GetSlpName(ds);

                            Application.ActivateMenuItem("8454");

                            var oFormAux = Application.Forms.ActiveForm;

                            var oMatrixAux = (Matrix)oFormAux.Items.Item("3").Specific;
                            for (var i = 0; i < oMatrixAux.VisualRowCount; i++)
                            {
                                var col = oFormAux.DataSources.DBDataSources.Item("OSLP").GetValue("SlpName", i);
                                if (col.TrimEnd(' ') == slpName)
                                {
                                    oMatrixAux.Columns.Item(0).Cells.Item(i + 1).Click(BoCellClickType.ct_Regular, 0);
                                    break;
                                }
                            }
                        }
                        else
                            BubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(Views.PagarComissaoForm.DetalhadoColumnEditCodigoCliente))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col3).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                            oLink.LinkedObject = BoLinkedObject.lf_BusinessPartner;
                        else
                            BubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(Views.PagarComissaoForm.DetalhadoColumnEditChaveNota))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col8).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            var sObjectType = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col22).ValueEx;
                            oLink.LinkedObjectType = sObjectType;
                        }
                        else
                            BubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(Views.PagarComissaoForm.DetalhadoColumnEditCodigoItem))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col9).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                            oLink.LinkedObject = BoLinkedObject.lf_Items;
                        else
                            BubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(Views.PagarComissaoForm.DetalhadoColumnEditCentroCusto))
                    {
                        var ds = oForm.DataSources.UserDataSources.Item(Views.PagarComissaoForm.UserDatasourceUD_Col11).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            BubbleEvent = false;
                            var slpName = diHelper.GetSlpName(ds);

                            Application.ActivateMenuItem("1793");

                            var oFormAux = Application.Forms.ActiveForm;
                            oFormAux.Mode = BoFormMode.fm_FIND_MODE;
                            ((EditText)oFormAux.Items.Item("5").Specific).Value = ds;
                            oFormAux.Items.Item("1").Click();
                        }
                        else
                            BubbleEvent = false;
                    }
                    oForm.Freeze(false);
                }

                if (pVal.FormTypeEx.Equals(Views.PagarComissaoForm.Type) && pVal.ItemUID.Equals(Views.PagarComissaoForm.ButtonRemover) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    if (oForm.PaneLevel == 2)
                    {
                        var oMatrix = (Matrix)oForm.Items.Item(Views.PagarComissaoForm.MatrixDetalhado).Specific;

                        if (Application.MessageBox("Remover linhas selecionadas?", 2, "Sim", "Não") == 1)
                        {
                            for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                            {
                                if (oMatrix.IsRowSelected(i))
                                {
                                    var docEntry = (EditText)oMatrix.GetCellSpecific(Views.PagarComissaoForm.DetalhadoColumnEditChaveNota, i);
                                    var objType = (EditText)oMatrix.GetCellSpecific(Views.PagarComissaoForm.DetalhadoColumnEditTipoDocumento, i);
                                    var lineNum = (EditText)oMatrix.GetCellSpecific(Views.PagarComissaoForm.DetalhadoColumnEditLinhaItem, i);
                                    var parcela = (EditText)oMatrix.GetCellSpecific(Views.PagarComissaoForm.DetalhadoColumnEditParcela, i);

                                    comissoes.RemovePagamentoDetalhado(docEntry.Value.ToString(), objType.Value.ToString(), lineNum.Value.ToString(), parcela.Value.ToString());
                                }
                            }

                            oForm.Freeze(false);
                            Application.MessageBox("Processo finalizado com sucesso.");
                            //oForm.Close();
                            //Application.ActivateMenuItem("PGTO");
                        }
                    }
                    else
                    {
                        var oMatrix = (Matrix)oForm.Items.Item(Views.PagarComissaoForm.MatrixResumido).Specific;

                        if (Application.MessageBox("Remover linhas selecionadas?", 2, "Sim", "Não") == 1)
                        {
                            for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                            {
                                if (oMatrix.IsRowSelected(i))
                                {
                                    var comissionado = (EditText)oMatrix.GetCellSpecific(Views.PagarComissaoForm.ResumidoColumnEditComissionado, i);

                                    comissoes.RemovePagamentoResumido(comissionado.Value.ToString());
                                }
                            }

                            oForm.Freeze(false);
                            Application.MessageBox("Processo finalizado com sucesso.");
                            //oForm.Close();
                            //Application.ActivateMenuItem("PGTO");
                        }
                    }

                    oForm.Freeze(false);
                }
                #endregion

                #region [CVA] Priorização de critérios de comissão
                if (pVal.FormTypeEx.Equals(Views.PriorizacaoCriteriosForm.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(Views.PriorizacaoCriteriosForm.ButtonUp) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oMatrix = (Matrix)oForm.Items.Item(Views.PriorizacaoCriteriosForm.MatrixItens).Specific;

                    for (int i = 1; i <= oMatrix.RowCount; i++)
                    {
                        if (oMatrix.IsRowSelected(i))
                        {
                            var line = (EditText)oMatrix.GetCellSpecific("#", i);
                            var name = (EditText)oMatrix.GetCellSpecific("Col_0", i);
                            var ativo = (CheckBox)oMatrix.GetCellSpecific("Col_1", i);
                            var code = (EditText)oMatrix.GetCellSpecific("Col_2", i);
                            var pos = (EditText)oMatrix.GetCellSpecific("Col_3", i);

                            if (!line.Value.ToString().Equals("1"))
                            {
                                if (i - 1 != 1)
                                {
                                    var linePrev = (EditText)oMatrix.GetCellSpecific("#", i - 1);
                                    var namePrev = (EditText)oMatrix.GetCellSpecific("Col_0", i - 1);
                                    var ativoPrev = (CheckBox)oMatrix.GetCellSpecific("Col_1", i - 1);
                                    var codePrev = (EditText)oMatrix.GetCellSpecific("Col_2", i - 1);
                                    var posPrev = (EditText)oMatrix.GetCellSpecific("Col_3", i - 1);

                                    var sCmp = Company.GetCompanyService();
                                    var oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                                    var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                                    oGeneralParams.SetProperty("Code", code.Value.ToString());

                                    var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_POS", (Int32.Parse(pos.Value.ToString()) - 1).ToString());
                                    oGeneralService.Update(oGeneralData);

                                    oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                                    oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                                    oGeneralParams.SetProperty("Code", codePrev.Value.ToString());

                                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_POS", (Int32.Parse(posPrev.Value.ToString()) + 1).ToString());
                                    oGeneralService.Update(oGeneralData);

                                    formHelper.LoadPricForm(oForm);
                                }
                            }
                        }
                    }
                }

                if (pVal.FormTypeEx.Equals(Views.PriorizacaoCriteriosForm.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(Views.PriorizacaoCriteriosForm.ButtonDown) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oMatrix = (Matrix)oForm.Items.Item(Views.PriorizacaoCriteriosForm.MatrixItens).Specific;

                    var i = oMatrix.GetNextSelectedRow();

                    if (oMatrix.IsRowSelected(i))
                    {
                        var line = (EditText)oMatrix.GetCellSpecific("#", i);
                        var name = (EditText)oMatrix.GetCellSpecific("Col_0", i);
                        var ativo = (CheckBox)oMatrix.GetCellSpecific("Col_1", i);
                        var code = (EditText)oMatrix.GetCellSpecific("Col_2", i);
                        var pos = (EditText)oMatrix.GetCellSpecific("Col_3", i);

                        if (!line.Value.ToString().Equals("1"))
                        {
                            if (i != 1)
                            {
                                if (i < oMatrix.RowCount)
                                {
                                    var linePos = (EditText)oMatrix.GetCellSpecific("#", i + 1);
                                    var namePos = (EditText)oMatrix.GetCellSpecific("Col_0", i + 1);
                                    var ativoPos = (CheckBox)oMatrix.GetCellSpecific("Col_1", i + 1);
                                    var codePos = (EditText)oMatrix.GetCellSpecific("Col_2", i + 1);
                                    var posPos = (EditText)oMatrix.GetCellSpecific("Col_3", i + 1);

                                    var sCmp = Company.GetCompanyService();
                                    var oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                                    var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                                    oGeneralParams.SetProperty("Code", code.Value.ToString());

                                    var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_POS", (Int32.Parse(pos.Value.ToString()) + 1).ToString());
                                    oGeneralService.Update(oGeneralData);

                                    oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                                    oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                                    oGeneralParams.SetProperty("Code", codePos.Value.ToString());

                                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_POS", (Int32.Parse(posPos.Value.ToString()) - 1).ToString());
                                    oGeneralService.Update(oGeneralData);

                                    formHelper.LoadPricForm(oForm);
                                }
                            }
                        }
                    }

                }

                if (pVal.FormTypeEx.Equals(Views.PriorizacaoCriteriosForm.Type) && pVal.ItemUID.Equals(Views.PriorizacaoCriteriosForm.MatrixItens) && pVal.ColUID.Equals(Views.PriorizacaoCriteriosForm.ColumnCheckAtivo) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Mode = BoFormMode.fm_OK_MODE;

                    var oMatrix = (Matrix)oForm.Items.Item(pVal.ItemUID).Specific;

                    var oCheck = (CheckBox)oMatrix.GetCellSpecific(pVal.ColUID, pVal.Row);

                    var code = (EditText)oMatrix.GetCellSpecific(Views.PriorizacaoCriteriosForm.ColumnCode, pVal.Row);

                    if (oCheck.Item.Enabled)
                    {
                        if (oCheck.Checked)
                        {
                            var sCmp = Company.GetCompanyService();
                            var oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                            var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                            oGeneralParams.SetProperty("Code", code.Value.ToString());

                            var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                            oGeneralData.SetProperty("U_ATIVO", "Y");
                            oGeneralService.Update(oGeneralData);
                        }
                        else
                        {
                            var sCmp = Company.GetCompanyService();
                            var oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                            var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                            oGeneralParams.SetProperty("Code", code.Value.ToString());

                            var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                            oGeneralData.SetProperty("U_ATIVO", "N");
                            oGeneralService.Update(oGeneralData);
                        }

                        formHelper.LoadPricForm(oForm);
                    }
                }
                #endregion

                #region [CVA] Cálculo de comissão
                if (pVal.FormTypeEx.Equals(Views.CalculoComissaoForm.Type) && pVal.ItemUID.Equals(Views.CalculoComissaoForm.ButtonRecalcular) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);

                    try
                    {
                        var oItem = oForm.Items.Item("Item_1");
                        oItem.TextStyle = 0;
                        oItem.ForeColor = 255;
                        oItem.Visible = true;

                        var oMatrix = (Matrix)oForm.Items.Item(Views.CalculoComissaoForm.MatrixItens).Specific;
                        var dataMetalInicial = ((EditText)oForm.Items.Item(Views.CalculoComissaoForm.EditMetaInicial).Specific).Value.ToString();
                        var dataMetaFinal = ((EditText)oForm.Items.Item(Views.CalculoComissaoForm.EditMetaFinal).Specific).Value.ToString();
                        var dataComissaolInicial = ((EditText)oForm.Items.Item(Views.CalculoComissaoForm.EditComissaoInicial).Specific).Value.ToString();
                        var dataComissaoFinal = ((EditText)oForm.Items.Item(Views.CalculoComissaoForm.EditComissaoFinal).Specific).Value.ToString();
                        var radioTodas = ((OptionBtn)oForm.Items.Item(Views.CalculoComissaoForm.OptionTodas).Specific).Selected;
                        var radioPagas = ((OptionBtn)oForm.Items.Item(Views.CalculoComissaoForm.OptionPagas).Specific).Selected;
                        var radioNaoPagas = ((OptionBtn)oForm.Items.Item(Views.CalculoComissaoForm.OptionNaoPagas).Specific).Selected;

                        int filial;
                        Int32.TryParse(oForm.DataSources.UserDataSources.Item("ud_Filial").Value, out filial);

                        int diasUteis;
                        Int32.TryParse(oForm.DataSources.UserDataSources.Item("ud_DiaUtil").Value, out diasUteis);

                        int domingoFeriado;
                        Int32.TryParse(oForm.DataSources.UserDataSources.Item("ud_Feriado").Value, out domingoFeriado);

                        if (diasUteis > 0 && domingoFeriado == 0)
                        {
                            oForm.Freeze(false);
                            throw new Exception("Informe a quantidade de domingo e feriados.");
                        }
                        if (diasUteis == 0 && domingoFeriado > 0)
                        {
                            oForm.Freeze(false);
                            throw new Exception("Informe a quantidade de dias úteis.");
                        }

                        if (!radioTodas && !radioPagas && !radioNaoPagas)
                        {
                            oForm.Freeze(false);
                            throw new Exception("Selecione uma opção de filtro.");
                        }

                        if (string.IsNullOrEmpty(dataMetalInicial))
                        {
                            oForm.Freeze(false);
                            throw new Exception("Meta inicial é obrigatória.");
                        }

                        if (string.IsNullOrEmpty(dataComissaoFinal))
                        {
                            oForm.Freeze(false);
                            throw new Exception("Meta final é obrigatória.");
                        }

                        if (string.IsNullOrEmpty(dataComissaolInicial))
                        {
                            oForm.Freeze(false);
                            throw new Exception("Comissão inicial é obrigatória.");
                        }

                        if (string.IsNullOrEmpty(dataComissaoFinal))
                        {
                            oForm.Freeze(false);
                            throw new Exception("Comissão final é obrigatória.");
                        }

                        CalculoComissaoFiltroModel filtroModel = new CalculoComissaoFiltroModel();
                        filtroModel.Filial = filial;
                        filtroModel.DataMetaInicial = dataMetalInicial;
                        filtroModel.DataMetaFinal = dataMetaFinal;
                        filtroModel.DataComissaoInicial = dataComissaolInicial;
                        filtroModel.DataComissaoFinal = dataComissaoFinal;
                        filtroModel.DiasUteis = diasUteis;
                        filtroModel.Feriados = domingoFeriado;
                        filtroModel.Todas = radioTodas;
                        filtroModel.Pagas = radioPagas;
                        filtroModel.NaoPagas = radioNaoPagas;

                        string sql = comissoes.GetComissoesSQL(filtroModel);

                        oForm.DataSources.DataTables.Item("dt_Docs").ExecuteQuery(sql);
                        Grid gr_Docs = oForm.Items.Item("gr_Docs").Specific as Grid;

                        gr_Docs.Columns.Item("U_BPLID").Visible = false;
                        gr_Docs.Columns.Item("U_COMISSIONADO").Visible = false;
                        gr_Docs.Columns.Item("U_PRIORIDADE").Visible = false;
                        gr_Docs.Columns.Item("U_REGRA").Visible = false;
                        gr_Docs.Columns.Item("U_OBJTYPE").Visible = false;
                        gr_Docs.Columns.Item("U_ITEMCODE").Visible = false;
                        gr_Docs.Columns.Item("U_ITEMNAME").Visible = false;
                        gr_Docs.Columns.Item("U_LINENUM").Visible = false;
                        gr_Docs.Columns.Item("U_CENTROCUSTO").Visible = false;
                        gr_Docs.Columns.Item("U_PAGO").Visible = false;
                        gr_Docs.Columns.Item("U_MOMENTO").Visible = false;
                        gr_Docs.Columns.Item("U_DATAPAGAMENTO").Visible = false;

                        gr_Docs.Columns.Item("U_COMNAME").TitleObject.Caption = "Comissionado";
                        gr_Docs.Columns.Item("U_CARDCODE").TitleObject.Caption = "Cód. Cliente";
                        gr_Docs.Columns.Item("U_CARDNAME").TitleObject.Caption = "Nome Cliente";
                        gr_Docs.Columns.Item("U_DOCDATE").TitleObject.Caption = "Data Doc.";
                        gr_Docs.Columns.Item("U_DUEDATE").TitleObject.Caption = "Vencimento";
                        //gr_Docs.Columns.Item("U_OBJTYPE").TitleObject.Caption = "Tipo Doc.";
                        gr_Docs.Columns.Item("U_VALOR").TitleObject.Caption = "Valor";
                        gr_Docs.Columns.Item("U_PARCELA").TitleObject.Caption = "Parcela";
                        gr_Docs.Columns.Item("U_IMPOSTOS").TitleObject.Caption = "Impostos";
                        gr_Docs.Columns.Item("U_META").TitleObject.Caption = "Meta";
                        gr_Docs.Columns.Item("U_TOTALVENDAS").TitleObject.Caption = "Vendas";
                        gr_Docs.Columns.Item("U_PORCMETA").TitleObject.Caption = "Atingido";
                        gr_Docs.Columns.Item("U_COMISSAO").TitleObject.Caption = "% Comissão";
                        gr_Docs.Columns.Item("U_COMISSAOEQUIP").TitleObject.Caption = "% Com. Equipe";
                        gr_Docs.Columns.Item("U_VALORCOMISSAO").TitleObject.Caption = "Vlr. Comissão";
                        gr_Docs.Columns.Item("U_VALORCOMEQUIP").TitleObject.Caption = "Vlr. Com. Equipe";
                        gr_Docs.Columns.Item("U_DSR").TitleObject.Caption = "DSR";
                        gr_Docs.Columns.Item("U_VALORCOMTOTAL").TitleObject.Caption = "Comissão Total";
                        gr_Docs.Columns.Item("U_TAXDATE").TitleObject.Caption = "Data Pagamento";
                        gr_Docs.Columns.Item("U_DOCENTRY").TitleObject.Caption = "Nr. Doc.";
                        gr_Docs.Columns.Item("U_SERIAL").TitleObject.Caption = "Nr. NF";

                        EditTextColumn column = gr_Docs.Columns.Item("U_CARDCODE") as EditTextColumn;
                        column.LinkedObjectType = ((int)BoLinkedObject.lf_BusinessPartner).ToString();

                        oForm.Mode = BoFormMode.fm_OK_MODE;
                        oItem.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        Application.SetStatusBarMessage(ex.Message);
                    }
                    finally
                    {
                        oForm.Freeze(false);
                    }
                }

                if (pVal.FormTypeEx.Equals(Views.CalculoComissaoForm.Type) && pVal.ItemUID.Equals(Views.CalculoComissaoForm.MatrixItens) && pVal.EventType.Equals(BoEventTypes.et_MATRIX_LINK_PRESSED) && pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    var oMatrix = (Matrix)oForm.Items.Item(Views.CalculoComissaoForm.MatrixItens).Specific;
                    oMatrix.GetLineData(pVal.Row);
                    Column oColumn;

                    if (pVal.ColUID.Equals(Views.CalculoComissaoForm.ColumnEditComissionado))
                    {
                        var ds = oForm.DataSources.UserDataSources.Item(Views.CalculoComissaoForm.UserDatasourceUD_Col2).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            BubbleEvent = false;
                            var slpName = diHelper.GetSlpName(ds);

                            Application.ActivateMenuItem("8454");

                            var oFormAux = Application.Forms.ActiveForm;

                            var oMatrixAux = (Matrix)oFormAux.Items.Item("3").Specific;
                            for (var i = 0; i < oMatrixAux.VisualRowCount; i++)
                            {
                                var col = oFormAux.DataSources.DBDataSources.Item("OSLP").GetValue("SlpName", i);
                                if (col.TrimEnd(' ') == slpName)
                                {
                                    oMatrixAux.Columns.Item(0).Cells.Item(i + 1).Click(BoCellClickType.ct_Regular, 0);
                                    break;
                                }
                            }
                        }
                        else
                            BubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(Views.CalculoComissaoForm.ColumnEditCodigoCliente))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(Views.CalculoComissaoForm.UserDatasourceUD_Col3).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                            oLink.LinkedObject = BoLinkedObject.lf_BusinessPartner;
                        else
                            BubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(Views.CalculoComissaoForm.ColumnEditChaveNota))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(Views.CalculoComissaoForm.UserDatasourceUD_Col8).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            var sObjectType = oForm.DataSources.UserDataSources.Item(Views.CalculoComissaoForm.UserDatasourceUD_Col20).ValueEx;
                            oLink.LinkedObjectType = sObjectType;
                        }
                        else
                            BubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(Views.CalculoComissaoForm.ColumnEditCodigoItem))
                    {
                        oColumn = oMatrix.Columns.Item(pVal.ColUID);
                        var oLink = oColumn.ExtendedObject;
                        var ds = oForm.DataSources.UserDataSources.Item(Views.CalculoComissaoForm.UserDatasourceUD_Col9).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                            oLink.LinkedObject = BoLinkedObject.lf_Items;
                        else
                            BubbleEvent = false;
                    }
                    else if (pVal.ColUID.Equals(Views.CalculoComissaoForm.ColumnEditCentroCusto))
                    {
                        var ds = oForm.DataSources.UserDataSources.Item(Views.CalculoComissaoForm.UserDatasourceUD_Col11).ValueEx;

                        if (!string.IsNullOrEmpty(ds))
                        {
                            BubbleEvent = false;
                            var slpName = diHelper.GetSlpName(ds);

                            Application.ActivateMenuItem("1793");

                            var oFormAux = Application.Forms.ActiveForm;
                            oFormAux.Mode = BoFormMode.fm_FIND_MODE;
                            ((EditText)oFormAux.Items.Item("5").Specific).Value = ds;
                            oFormAux.Items.Item("1").Click();
                        }
                        else
                            BubbleEvent = false;
                    }
                    oForm.Freeze(false);
                }

                if (pVal.FormTypeEx.Equals(Views.CalculoComissaoForm.Type) && pVal.ItemUID.Equals(Views.CalculoComissaoForm.ButtonConfirmar) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    if (Application.MessageBox("Enviar estas comissões não pagas para pagamento?", 1, "Sim", "Não") == 1)
                    {
                        var oForm = Application.Forms.ActiveForm;
                        try
                        {
                            oForm.Freeze(true);

                            var oItem = oForm.Items.Item("Item_1");
                            oItem.TextStyle = 0;
                            oItem.ForeColor = 255;
                            oItem.Visible = true;

                            int filial;
                            Int32.TryParse(oForm.DataSources.UserDataSources.Item("ud_Filial").Value, out filial);

                            int diasUteis;
                            Int32.TryParse(oForm.DataSources.UserDataSources.Item("ud_DiaUtil").Value, out diasUteis);

                            int domingoFeriado;
                            Int32.TryParse(oForm.DataSources.UserDataSources.Item("ud_Feriado").Value, out domingoFeriado);

                            var oMatrix = (Matrix)oForm.Items.Item(Views.CalculoComissaoForm.MatrixItens).Specific;
                            var dataMetalInicial = ((EditText)oForm.Items.Item(Views.CalculoComissaoForm.EditMetaInicial).Specific).Value.ToString();
                            var dataMetaFinal = ((EditText)oForm.Items.Item(Views.CalculoComissaoForm.EditMetaFinal).Specific).Value.ToString();
                            var dataComissaolInicial = ((EditText)oForm.Items.Item(Views.CalculoComissaoForm.EditComissaoInicial).Specific).Value.ToString();
                            var dataComissaoFinal = ((EditText)oForm.Items.Item(Views.CalculoComissaoForm.EditComissaoFinal).Specific).Value.ToString();
                            var radioTodas = ((OptionBtn)oForm.Items.Item(Views.CalculoComissaoForm.OptionTodas).Specific).Selected;
                            var radioPagas = ((OptionBtn)oForm.Items.Item(Views.CalculoComissaoForm.OptionPagas).Specific).Selected;
                            var radioNaoPagas = ((OptionBtn)oForm.Items.Item(Views.CalculoComissaoForm.OptionNaoPagas).Specific).Selected;

                            CalculoComissaoFiltroModel filtroModel = new CalculoComissaoFiltroModel();
                            filtroModel.Filial = filial;
                            filtroModel.DataMetaInicial = dataMetalInicial;
                            filtroModel.DataMetaFinal = dataMetaFinal;
                            filtroModel.DataComissaoInicial = dataComissaolInicial;
                            filtroModel.DataComissaoFinal = dataComissaoFinal;
                            filtroModel.DiasUteis = diasUteis;
                            filtroModel.Feriados = domingoFeriado;
                            filtroModel.Todas = radioTodas;
                            filtroModel.Pagas = radioPagas;
                            filtroModel.NaoPagas = radioNaoPagas;

                            var msg = comissoes.SetInvoices(filtroModel);

                            if (!string.IsNullOrEmpty(msg))
                                Application.MessageBox(msg);


                            oItem.Visible = false;
                            Application.MessageBox("Processo finalizado com sucesso.");
                            //oForm.Close();
                            //Application.ActivateMenuItem("CALC");
                        }
                        catch (Exception ex)
                        {
                            Application.SetStatusBarMessage(ex.Message);
                        }
                        finally
                        {
                            oForm.Freeze(false);
                        }
                    }

                }

                if (pVal.FormTypeEx.Equals(Views.CalculoComissaoForm.Type) && pVal.ItemUID.Equals(Views.CalculoComissaoForm.ButtonRemover) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);

                    var oItem = oForm.Items.Item("Item_1");
                    oItem.TextStyle = 0;
                    oItem.ForeColor = 255;
                    oItem.Visible = true;

                    var oMatrix = (Matrix)oForm.Items.Item(Views.CalculoComissaoForm.MatrixItens).Specific;

                    if (Application.MessageBox("Remover linhas selecionadas?", 2, "Sim", "Não") == 1)
                    {
                        for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                        {
                            if (oMatrix.IsRowSelected(i))
                            {
                                var docEntry = (EditText)oMatrix.GetCellSpecific(Views.CalculoComissaoForm.ColumnEditChaveNota, i);
                                var objType = (EditText)oMatrix.GetCellSpecific(Views.CalculoComissaoForm.ColumnEditTipoObjeto, i);
                                var lineNum = (EditText)oMatrix.GetCellSpecific(Views.CalculoComissaoForm.ColumnEditLinhaItem, i);
                                var parcela = (EditText)oMatrix.GetCellSpecific(Views.CalculoComissaoForm.ColumnEditParcela, i);

                                comissoes.RemoveCalculo(docEntry.Value.ToString(), objType.Value.ToString(), lineNum.Value.ToString(), parcela.Value.ToString());
                            }
                        }
                    }

                    oForm.Freeze(false);
                    oItem.Visible = false;
                    Application.MessageBox("Processo finalizado com sucesso.");
                    //oForm.Close();
                    //Application.ActivateMenuItem("CALC");
                }
                #endregion

                #region [CVA] Gerente
                if (pVal.FormTypeEx.Equals(Views.GerenteForm.Type) && pVal.ItemUID.Equals(Views.GerenteForm.EditCode) && pVal.EventType.Equals(BoEventTypes.et_PICKER_CLICKED) && pVal.BeforeAction)
                {
                    pickerClicked = true;
                }

                if (pVal.FormTypeEx.Equals(Views.GerenteForm.Type) && pVal.ItemUID.Equals(Views.GerenteForm.Matrix) && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    try
                    {
                        Matrix mt_Item = (Matrix)oForm.Items.Item(Views.GerenteForm.Matrix).Specific;
                        EditText cl_Id = (EditText)mt_Item.GetCellSpecific(Views.GerenteForm.ColumnId, pVal.Row);

                        if (!String.IsNullOrEmpty(cl_Id.Value))
                        {
                            if (pVal.Row == mt_Item.RowCount)
                            {
                                mt_Item.FlushToDataSource();
                                formHelper.GerenteAddRow(oForm);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Application.SetStatusBarMessage(ex.Message);
                    }
                    finally
                    {
                        oForm.Freeze(false);
                    }
                }

                if (pVal.FormTypeEx.Equals(Views.MetaComissaoForm.Type) && pVal.ItemUID.Equals(Views.MetaComissaoForm.Matrix) && pVal.ColUID == Views.MetaComissaoForm.ColumnDe && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    try
                    {
                        Matrix mt_Item = (Matrix)oForm.Items.Item(Views.MetaComissaoForm.Matrix).Specific;
                        EditText cl_De = (EditText)mt_Item.GetCellSpecific(Views.MetaComissaoForm.ColumnDe, pVal.Row);

                        if (!String.IsNullOrEmpty(cl_De.Value.Trim()))
                        {
                            if (pVal.Row == mt_Item.RowCount)
                            {
                                mt_Item.FlushToDataSource();
                                formHelper.MetaComissaoAddRow(oForm);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Application.SetStatusBarMessage(ex.Message);
                    }
                    finally
                    {
                        oForm.Freeze(false);
                    }
                }

                if (pVal.FormTypeEx.Equals(Views.GerenteForm.Type) && pVal.ItemUID.Equals(Views.GerenteForm.EditCode) && pVal.EventType.Equals(BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    oForm.Freeze(true);
                    formHelper.GerenteAddRow(oForm);
                    oForm.Freeze(false);
                }

                if (pVal.FormTypeEx.Equals(Views.MetaComissaoForm.Type) && pVal.ItemUID.Equals(Views.MetaComissaoForm.EditDesc) && pVal.EventType.Equals(BoEventTypes.et_LOST_FOCUS) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    Matrix mt_Item = (Matrix)oForm.Items.Item(Views.MetaComissaoForm.Matrix).Specific;
                    if (mt_Item.RowCount == 0)
                    {
                        oForm.Freeze(true);
                        formHelper.MetaComissaoAddRow(oForm);
                        oForm.Freeze(false);
                    }
                }

                if (pVal.FormTypeEx.Equals(Views.GerenteForm.Type) && pVal.ItemUID.Equals(Views.GerenteForm.Matrix) && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    Matrix mt_Item = (Matrix)oForm.Items.Item(Views.GerenteForm.Matrix).Specific;
                    var oEdit = (EditText)mt_Item.GetCellSpecific(Views.GerenteForm.ColumnId, pVal.Row);
                    var oEditNome = (EditText)mt_Item.GetCellSpecific(Views.GerenteForm.ColumnNome, pVal.Row);

                    if (!String.IsNullOrEmpty(oEdit.Value))
                    {
                        var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery($"SELECT SlpName FROM OSLP WHERE SlpCode = {oEdit.Value.ToString()}");
                        if (oRecordset.RecordCount > 0)
                        {
                            oEditNome.Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                    }
                    else
                    {
                        oEditNome.Value = string.Empty;
                    }
                }
                if (pVal.FormTypeEx.Equals(Views.GerenteForm.Type) && pVal.ItemUID.Equals(Views.GerenteForm.EditCode) && pVal.EventType.Equals(BoEventTypes.et_VALIDATE) && !pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oEdit = (EditText)oForm.Items.Item(Views.GerenteForm.EditCode).Specific;
                    Matrix mt_Item = (Matrix)oForm.Items.Item("mt_Item").Specific;
                    if (mt_Item.RowCount == 0)
                    {
                        mt_Item.AddRow();
                    }
                    if (!String.IsNullOrEmpty(oEdit.Value))
                    {
                        var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery($"SELECT SlpName FROM OSLP WHERE SlpCode = {oEdit.Value.ToString()}");
                        if (oRecordset.RecordCount > 0)
                        {
                            ((EditText)oForm.Items.Item(Views.GerenteForm.EditNome).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                        }
                    }
                    else
                    {
                        ((EditText)oForm.Items.Item(Views.GerenteForm.EditNome).Specific).Value = string.Empty;
                    }
                }
                #endregion

                if (pVal.FormTypeEx.Equals("9999") && !pVal.BeforeAction)
                {
                    try
                    {
                        var oForm = Application.Forms.ActiveForm;

                        if (oForm.Title.In("Lista de CVAREGRASDECOMISSAO", "Lista de [CVA] Regras de comissão"))
                        {
                            if (oForm.Items.Item("11").Visible)
                                oForm.Items.Item("11").Visible = false;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            BubbleEvent = ret;
        }
        #endregion

        #region FormDataEvents
        private void FormDataEvents(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            var ret = true;
            var pVal = BusinessObjectInfo;

            try
            {
                if (pVal.FormTypeEx.Equals(Views.MetaComissaoForm.Type) && pVal.EventType.Equals(BoEventTypes.et_FORM_DATA_ADD) && pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                    oRecordset.DoQuery("SELECT COALESCE(MAX(CAST(Code AS INT)), 0) + 1 FROM [@CVA_META]");

                    ((EditText)oForm.Items.Item(Views.MetaComissaoForm.EditCode).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                }
                if (pVal.FormTypeEx.Equals(Views.EquipeForm.Type) && pVal.EventType.Equals(BoEventTypes.et_FORM_DATA_ADD) && pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;
                    var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                    oRecordset.DoQuery("SELECT COALESCE(MAX(CAST(Code AS INT)), 0) + 1 FROM [@CVA_EQUIPE]");

                    ((EditText)oForm.Items.Item(Views.EquipeForm.EditCode).Specific).Value = oRecordset.Fields.Item(0).Value.ToString();
                }

                if (pVal.FormTypeEx.Equals(Views.RegraComissaoForm.Type) && pVal.EventType.Equals(BoEventTypes.et_FORM_DATA_ADD) && pVal.BeforeAction)
                {
                    var oForm = Application.Forms.ActiveForm;

                    var prioridade = 0;

                    var editComissionado = (EditText)oForm.Items.Item(Views.RegraComissaoForm.EditComissionado).Specific;

                    int vendedor = 0;
                    int grupoItens = 0;
                    string cliente = "";
                    string item = "";
                    string centroCusto = "";
                    string estado = "";
                    int cidade = 0;
                    int fabricante = 0;
                    int setor = 0;
                    int grupoCliente = 0;

                    if (editComissionado != null && editComissionado.Value != null && !string.IsNullOrEmpty(editComissionado.Value.ToString()))
                    {
                        vendedor = int.Parse(editComissionado.Value.ToString());
                    }
                    prioridade = diHelper.GetCriteria(vendedor, grupoItens, cliente, item, centroCusto, estado, cidade, fabricante, setor, grupoCliente);
                    oForm.DataSources.DBDataSources.Item("@CVA_REGR_COMISSAO").SetValue("U_PRIORIDADE", 0, prioridade.ToString());
                }
            }
            catch (Exception ex)
            {
                Application.MessageBox("Erro geral: " + ex.Message);
                ret = false;
            }

            BubbleEvent = ret;
        }
        #endregion

        private void AppEvents(BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case BoAppEventTypes.aet_CompanyChanged:
                case BoAppEventTypes.aet_FontChanged:
                case BoAppEventTypes.aet_LanguageChanged:
                case BoAppEventTypes.aet_ServerTerminition:
                case BoAppEventTypes.aet_ShutDown:
                    //if (Application.Menus.Exists("TIPO")) Application.Menus.RemoveEx("TIPO");
                    //if (Application.Menus.Exists("UPDT")) Application.Menus.RemoveEx("UPDT");
                    //if (Application.Menus.Exists("PRIC")) Application.Menus.RemoveEx("PRIC");
                    if (Application.Menus.Exists("REPT")) Application.Menus.RemoveEx("REPT");
                    if (Application.Menus.Exists("PGTO")) Application.Menus.RemoveEx("PGTO");
                    if (Application.Menus.Exists("CALC")) Application.Menus.RemoveEx("CALC");
                    if (Application.Menus.Exists("REGR")) Application.Menus.RemoveEx("REGR");
                    if (Application.Menus.Exists("META")) Application.Menus.RemoveEx("META");
                    if (Application.Menus.Exists("GRNT")) Application.Menus.RemoveEx("GRNT");
                    if (Application.Menus.Exists("EQPE")) Application.Menus.RemoveEx("EQPE");
                    if (Application.Menus.Exists("CVA")) Application.Menus.RemoveEx("CVA");
                    Environment.Exit(-1);
                    break;
            }
        }

        private void Connect()
        {
            factory = container.GetInstance<SapFactory>();
            Application = factory.Application;
            Company = factory.Company;
            Filters = factory.Filters;
            dbHelper = container.GetInstance<DbHelper>();
            menuHelper = container.GetInstance<MenuHelper>();
            filterHelper = container.GetInstance<FilterHelper>();
            formHelper = container.GetInstance<FormHelper>();
            diHelper = container.GetInstance<DIHelper>();
            comissoes = container.GetInstance<ComissoesController>();
        }

        private void EnableMenus(Form oForm, bool enable)
        {
            oForm.EnableMenu("1281", enable);
            oForm.EnableMenu("1282", enable);
            oForm.EnableMenu("1288", enable);
            oForm.EnableMenu("1289", enable);
            oForm.EnableMenu("1290", enable);
            oForm.EnableMenu("1291", enable);
        }
    }
}
