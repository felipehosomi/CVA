using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Dover.Framework.Attribute;
using System;
using System.Collections.Generic;
using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.BLL;
using log4net;
using System.Linq;
using System.Globalization;

namespace CVA.View.ControleQualidade.VIEW
{
    #region Properties
    public partial class ApontamentoQualidadeView
    {
        private SAPbouiCOM.Application _application { get; set; }
        public ILog _Log { get; set; }

        private ItemBLL _itemBLL { get; set; }
        private PlanoInspecaoBLL _planoInspecaoBLL { get; set; }
        private LoteView _loteView { get; set; }
        private AtributoBLL _atributoBLL { get; set; }
        private ApontamentoBLL _apontamentoBLL { get; set; }
        private UsuarioBLL _usuarioBLL { get; set; }
        private OperadorBLL _operadorBLL { get; set; }

        public string CurrentItem { get; set; }

        public EditText et_Code { get; set; }
        public EditText et_PlanoInspecao { get; set; }
        public EditText et_DocEnt { get; set; }
        public EditText et_OP { get; set; }

        public EditText et_Data { get; set; }
        public ComboBox cb_Oper { get; set; }
        public EditText et_Ini { get; set; }
        public EditText et_Fim { get; set; }
        public EditText et_OK { get; set; }
        public EditText et_NOK { get; set; }

        public CheckBox cb_Peca { get; set; }

        public StaticText lb_DocEnt { get; set; }
        public StaticText lb_Ini { get; set; }
        public StaticText lb_Fim { get; set; }
        public StaticText lb_Nome { get; set; }

        public ComboBox cb_Insp { get; set; }

        public Matrix mt_Resultados { get; set; }
        public DBDataSource ds_Header { get; set; }
        public DBDataSource ds_Linha { get; set; }

        public static PlanoInspecao PlanoInspecao { get; set; }
        public string DocNumEnt { get; set; }
        public string OrdemProducao { get; set; }
        public bool AlreadyExists { get; set; }
        public TipoInspecaoEnum TipoInspecao { get; set; }
        private static string ErrorMessage { get; set; }
        private static CultureInfo Culture = CultureInfo.GetCultureInfo("en-us");
    }
    #endregion

    [FormAttribute("CVAApontamentoQualidade", "CVA.View.ControleQualidade.Resources.Form.ApontamentoQualidade.srf")]
    [MenuEvent(UniqueUID = "CVAApontamentoQualidade")]
    public partial class ApontamentoQualidadeView : DoverUserFormBase
    {
        public ApontamentoQualidadeView(SAPbouiCOM.Application application, UsuarioBLL usuarioBLL, ItemBLL itemBLL, PlanoInspecaoBLL planoInspecaoBLL, LoteView loteView, AtributoBLL atributoBLL, ApontamentoBLL apontamentoBLL, OperadorBLL operadorBLL)
        {
            _application = application;
            _usuarioBLL = usuarioBLL;
            _itemBLL = itemBLL;
            _planoInspecaoBLL = planoInspecaoBLL;
            _loteView = loteView;
            _atributoBLL = atributoBLL;
            _apontamentoBLL = apontamentoBLL;
            _operadorBLL = operadorBLL;
            _Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            OnInitializeCustomComponents();
            OnInitializeCustomEvents();
        }

        #region Initialize
        private void OnInitializeCustomComponents()
        {
            try
            {
                et_Code = this.GetItem("et_Code").Specific as EditText;
                et_PlanoInspecao = this.GetItem("et_PlnIns").Specific as EditText;
                cb_Insp = this.GetItem("cb_Insp").Specific as ComboBox;
                et_DocEnt = this.GetItem("et_DocEnt").Specific as EditText;
                et_OP = this.GetItem("et_OP").Specific as EditText;
                et_Data = this.GetItem("et_Data").Specific as EditText;
                cb_Oper = this.GetItem("cb_Oper").Specific as ComboBox;

                et_Ini = this.GetItem("et_Ini").Specific as EditText;
                et_Fim = this.GetItem("et_Fim").Specific as EditText;
                et_OK = this.GetItem("et_OK").Specific as EditText;
                et_NOK = this.GetItem("et_NOK").Specific as EditText;

                cb_Peca = this.GetItem("cb_Peca").Specific as CheckBox;

                lb_Ini = this.GetItem("lb_Ini").Specific as StaticText;
                lb_Fim = this.GetItem("lb_Fim").Specific as StaticText;
                lb_Nome = this.GetItem("lb_Nome").Specific as StaticText;
                lb_DocEnt = this.GetItem("lb_DocEnt").Specific as StaticText;

                mt_Resultados = this.GetItem("mt_Princ").Specific as Matrix;

                ds_Header = this.UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_QUALITY_RES");
                ds_Linha = this.UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_QUALITY_RES1");

                cb_Peca.ValOff = "0";
                cb_Peca.ValOn = "1";

                List<Operador> operadorList = _operadorBLL.GetOperadores();
                foreach (var item in operadorList)
                {
                    cb_Oper.ValidValues.Add(item.Code, item.Name);
                }

                UIAPIRawForm.AutoManaged = true;
                GetItem("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                GetItem("et_PlnIns").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                GetItem("et_OP").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                GetItem("et_DocEnt").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                GetItem("cb_Insp").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);

                GetItem("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
                GetItem("et_PlnIns").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
                GetItem("et_OP").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
                GetItem("et_DocEnt").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
                GetItem("cb_Insp").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);

                LoadEspecificacao_Field();
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage("CVA Exception: " + ex.Message);
            }
        }

        public void OnInitializeCustomEvents()
        {
            mt_Resultados.LostFocusAfter += Mt_Resultados_LostFocusAfter;
            _application.StatusBarEvent += _application_StatusBarEvent;
        }

        public override void OnInitializeComponent()
        {
            UIAPIRawForm.Mode = BoFormMode.fm_ADD_MODE;
        }

        #endregion

        #region Events
        protected internal virtual void Mt_Resultados_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            switch (pVal.ColUID)
            {
                case "cl_Med":
                    EditText etxMed = mt_Resultados.GetCellSpecific(pVal.ColUID, pVal.Row) as EditText;
                    if (!String.IsNullOrEmpty(etxMed.Value))
                    {
                        double value;
                        double vlrDe;
                        double vlrAte;
                        EditText etxDe = mt_Resultados.GetCellSpecific("cl_VlrDe", pVal.Row) as EditText;
                        EditText etxAte = mt_Resultados.GetCellSpecific("cl_VlrAte", pVal.Row) as EditText;

                        if (double.TryParse(etxDe.Value.Replace(",", "."), NumberStyles.Any, Culture, out vlrDe) && double.TryParse(etxAte.Value.Replace(",", "."), NumberStyles.Any, Culture, out vlrAte))
                        {
                            if (double.TryParse(etxMed.Value.Replace(",", "."), NumberStyles.Any, Culture, out value))
                            {
                                EditText etxRes = mt_Resultados.GetCellSpecific("cl_Res", pVal.Row) as EditText;
                                if (value >= vlrDe && value <= vlrAte)
                                {
                                    etxRes.Value = "Aprovado";
                                }
                                else
                                {
                                    etxRes.Value = "Reprovado";
                                }
                            }
                        }
                    }
                    break;
                case "cl_OK":
                    double valueOK;
                    EditText etxOK = mt_Resultados.GetCellSpecific(pVal.ColUID, mt_Resultados.RowCount) as EditText;
                    if (double.TryParse(etxOK.Value, NumberStyles.Any, Culture, out valueOK))
                    {
                        et_OK.Value = valueOK.ToString(Culture);
                    }
                    break;
                case "cl_NOK":
                    double totalNOK = 0;
                    double valueNOK;
                    for (int i = 1; i <= mt_Resultados.RowCount; i++)
                    {
                        EditText etxNOK = mt_Resultados.GetCellSpecific(pVal.ColUID, i) as EditText;
                        if (double.TryParse(etxNOK.Value, NumberStyles.Any, Culture, out valueNOK))
                        {
                            totalNOK += valueNOK;
                        }
                        et_NOK.Value = totalNOK.ToString(Culture);
                    }
                    break;
            }
        }

        protected override void OnFormVisibleAfter(SBOItemEventArg pVal)
        {
            if (AlreadyExists && UIAPIRawForm.Mode == BoFormMode.fm_FIND_MODE)
            {
                if (!String.IsNullOrEmpty(et_OP.Value) || !String.IsNullOrEmpty(et_DocEnt.Value))
                {
                    AlreadyExists = false;
                    this.GetItem("1").Click();
                }
            }
        }

        protected override void OnFormDataLoadAfter(ref BusinessObjectInfo pVal)
        {
            switch (cb_Insp.Value)
            {
                case "Q":
                    TipoInspecao = TipoInspecaoEnum.Qualidade;
                    break;
                case "O":
                    TipoInspecao = TipoInspecaoEnum.Operador;
                    break;
            }

            this.SetFieldsConfig();
        }

        protected override void OnFormDataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            SaveInvisibleColumns();
            if (String.IsNullOrEmpty(cb_Oper.Value.Trim()))
            {
                ErrorMessage = "Operador deve ser informado!";
                BubbleEvent = false;
            }
            else
            {
                BubbleEvent = true;
            }
        }

        protected override void OnFormDataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            SaveInvisibleColumns();
            if (String.IsNullOrEmpty(cb_Oper.Value.Trim()))
            {
                ErrorMessage = "Operador deve ser informado!";
                BubbleEvent = false;
            }
            else
            {
                BubbleEvent = true;
            }
        }

        protected override void OnFormCloseAfter(SBOItemEventArg pVal)
        {
            mt_Resultados.LostFocusAfter -= Mt_Resultados_LostFocusAfter;
        }

        private void _application_StatusBarEvent(string Text, BoStatusBarMessageType messageType)
        {
            if (Text.Contains("UI_API -7780") && !String.IsNullOrEmpty(ErrorMessage))
            {
                _application.StatusBar.SetText(ErrorMessage);
            }
        }
        #endregion

        #region Support Methods
        private void SaveInvisibleColumns()
        {
            if (TipoInspecao == TipoInspecaoEnum.Operador)
            {
                for (int i = 1; i <= mt_Resultados.RowCount; i++)
                {
                    ((EditText)mt_Resultados.Columns.Item("cl_OK").Cells.Item(i).Specific).Value = et_OK.Value;
                    ((EditText)mt_Resultados.Columns.Item("cl_NOK").Cells.Item(i).Specific).Value = et_NOK.Value;
                }
            }
        }

        public void SetFieldsConfig()
        {
            bool operador = TipoInspecao == TipoInspecaoEnum.Operador;

            et_Data.Item.Click();

            lb_Nome.Caption = operador ? "Operador" : "Inspetor CQ";
            lb_DocEnt.Item.Visible = operador;
            et_DocEnt.Item.Visible = operador;

            lb_Ini.Item.Visible = operador;
            et_Ini.Item.Visible = operador;

            lb_Fim.Item.Visible = operador;
            et_Fim.Item.Visible = operador;

            et_OK.Item.Enabled = operador;
            et_NOK.Item.Enabled = operador;

            mt_Resultados.Columns.Item("cl_Lote").Visible = operador;

            mt_Resultados.Columns.Item("cl_OK").Visible = !operador;
            mt_Resultados.Columns.Item("cl_NOK").Visible = !operador;
        }

        public void Proccess()
        {
            if (!AlreadyExists)
            {
                et_Code.Value = _apontamentoBLL.GetNextCode().PadLeft(10, '0');
                et_OP.Value = OrdemProducao;
                et_DocEnt.Value = DocNumEnt;
                cb_Insp.Select(((char)TipoInspecao).ToString());
                et_Data.Value = DateTime.Now.ToString("yyyyMMdd");

                CarregaPlanoInspecao();
            }
            else
            {
                UIAPIRawForm.Mode = BoFormMode.fm_FIND_MODE;
                cb_Insp.Select(((char)TipoInspecao).ToString());
                et_OP.Value = OrdemProducao;
                et_DocEnt.Value = DocNumEnt;

                //_application.SetStatusBarMessage("Apontamento de Qualidade já iniciado! Por favor, clique no botão Procurar para continuar");
            }
            this.SetFieldsConfig();
        }

        /// <summary>
        /// Insere linhas no grid para de acordo com o plano de inspeção para cada item e cada lote
        /// </summary>
        private void CarregaPlanoInspecao()
        {
            Usuario usuario = _usuarioBLL.GetUsuario(_application.Company.UserName);

            int i = 1;

            if (StaticKeys.ItemList == null)
            {
                return;
            }

            foreach (var item in StaticKeys.ItemList)
            {
                MODEL.Item planoItem = _itemBLL.GetInspecaoPorItem(item.ItemCode);
                PlanoInspecao = _planoInspecaoBLL.Get(planoItem.PlanoInspecao, TipoInspecao);

                if (PlanoInspecao == null || String.IsNullOrEmpty(PlanoInspecao.Code))
                {
                    _application.SetStatusBarMessage("Plano de inspeção não encontrado para o item " + item.ItemCode);
                    return;
                }
                if (PlanoInspecao.Aprovado != "1")
                {
                    _application.SetStatusBarMessage(String.Format("Plano de inspeção {0} não está aprovado!", PlanoInspecao.Code));
                    return;
                }

                if (PlanoInspecao.Details == null || PlanoInspecao.Details.Count == 0)
                {
                    _application.SetStatusBarMessage("Nenhuma etapa de inspeção encontrada para o item " + item.ItemCode);
                    return;
                }

                if (item.LoteList == null)
                {
                    item.LoteList = new List<ItemLote>();
                    item.LoteList.Add(new ItemLote());
                }

                foreach (var itemLote in item.LoteList)
                {
                    mt_Resultados.AddRow(PlanoInspecao.Details.Count);

                    // Verificar (pode ser que existam itens com planos de inspeção diferentes)
                    et_PlanoInspecao.Value = PlanoInspecao.Code;

                    foreach (var itemInsp in PlanoInspecao.Details)
                    {
                        ((EditText)mt_Resultados.GetCellSpecific("#", i)).Value = i.ToString();
                        ((ComboBox)mt_Resultados.GetCellSpecific("cl_Espec", i)).Select(itemInsp.EspecificacaoCode);
                        ((EditText)mt_Resultados.GetCellSpecific("cl_Anls", i)).Value = itemInsp.Analise;
                        ((EditText)mt_Resultados.GetCellSpecific("cl_VlrNom", i)).Value = itemInsp.ValorNominal;
                        ((EditText)mt_Resultados.GetCellSpecific("cl_VlrDe", i)).Value = itemInsp.ValorDe;
                        ((EditText)mt_Resultados.GetCellSpecific("cl_VlrAte", i)).Value = itemInsp.ValorAte;
                        ((ComboBox)mt_Resultados.GetCellSpecific("cl_Equip", i)).Select(itemInsp.Equipamento);

                        ((EditText)mt_Resultados.GetCellSpecific("cl_Obs", i)).Value = itemInsp.Observacao;

                        ((EditText)mt_Resultados.GetCellSpecific("cl_Item", i)).Value = item.ItemCode;
                        ((EditText)mt_Resultados.GetCellSpecific("cl_Lote", i)).Value = itemLote.Lote;
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Preenche comboboxes com o campo especificação
        /// </summary>
        private void LoadEspecificacao_Field()
        {
            try
            {
                SAPbouiCOM.Column cl_Espec = (SAPbouiCOM.Column)mt_Resultados.Columns.Item("cl_Espec");
                SAPbouiCOM.Column cl_Equip = (SAPbouiCOM.Column)mt_Resultados.Columns.Item("cl_Equip");

                var atributos = _atributoBLL.Get();
                if (atributos.Count > 0)
                {
                    foreach (var atributo in atributos)
                    {
                        cl_Espec.ValidValues.Add(atributo.Code, atributo.Name);
                        cl_Equip.ValidValues.Add(atributo.Code, atributo.Name);
                    }
                }
                else
                {
                    _application.SetStatusBarMessage("CVA ALERT: Atenção, você não possui nenhum atributo cadastrado!", BoMessageTime.bmt_Medium);
                }
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage("CVA Exception: " + ex.Message, BoMessageTime.bmt_Medium);
            }
        }
        #endregion
    }
}