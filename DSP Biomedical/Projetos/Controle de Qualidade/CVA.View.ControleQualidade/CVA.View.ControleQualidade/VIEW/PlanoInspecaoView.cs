using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Dover.Framework.Attribute;
using System;
using System.Collections.Generic;
using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.BLL;

namespace CVA.View.ControleQualidade.VIEW
{
    #region Properties
    public partial class PlanoInspecaoView
    {
        public Matrix mt_Lines { get; set; }
        public Button bt_Add { get; set; }
        public Button bt_Del { get; set; }
        public Button btn_dupli { get; set; }

        public EditText et_Code { get; set; }
        public EditText et_Name { get; set; }
        public EditText et_Observation { get; set; }

        public EditText et_Analise { get; set; }
        public ComboBox cb_Especificacao { get; set; }
        public EditText et_ValorNominal { get; set; }
        public EditText et_ValorDe { get; set; }
        public EditText et_ValorAte { get; set; }
        public EditText et_ObservacaoLinha { get; set; }
        public EditText et_DtAprov { get; set; }
        public EditText et_Usuar { get; set; }

        public ComboBox cb_Insp { get; set; }
        public ComboBox cb_Equip { get; set; }

        public CheckBox cb_Aprovado { get; set; }

        public DBDataSource ds_Linha { get; set; }



        private PlanoInspecaoBLL _planoInspecaoBLL { get; set; }
        private AtributoBLL _atributoBLL { get; set; }
        private UsuarioBLL _usuarioBLL { get; set; }
        private AlertaBLL _alertaBLL { get; set; }
        Usuario _usuario { get; set; }

        private SAPbouiCOM.Application _application { get; set; }
        private IForm _oform;


        private bool JaAprovado { get; set; }
    }
    #endregion

    [FormAttribute("CVAPlanoInspecao", "CVA.View.ControleQualidade.Resources.Form.PlanoInspecao.srf")]
    [MenuEvent(UniqueUID = "CVAInspecaoQualidade")]
    public partial class PlanoInspecaoView : DoverUserFormBase
    {
        private PlanoInspecao _planoInspecao = new PlanoInspecao();

        public PlanoInspecaoView(SAPbouiCOM.Application application, PlanoInspecaoBLL planoInspecaoBLL, AtributoBLL atributoBLL, UsuarioBLL usuarioBLL, AlertaBLL alertaBLL)
        {

            _application = application;
            _atributoBLL = atributoBLL;
            _planoInspecaoBLL = planoInspecaoBLL;
            _usuarioBLL = usuarioBLL;
            _alertaBLL = alertaBLL;

            OnCustomInitializeComponents();
        }

        #region Initialize
        public override void OnInitializeComponent()
        {
            mt_Lines = this.GetItem("mt_Line").Specific as Matrix;
            bt_Add = this.GetItem("bt_Add").Specific as Button;
            bt_Del = this.GetItem("bt_Del").Specific as Button;
            btn_dupli = this.GetItem("btn_dupli").Specific as Button;

            et_Code = this.GetItem("et_Cod").Specific as EditText;
            et_Name = this.GetItem("et_Nom").Specific as EditText;
            et_Observation = this.GetItem("et_Obs").Specific as EditText;

            et_Analise = this.GetItem("tb_Anali").Specific as EditText;
            cb_Especificacao = this.GetItem("cb_Espec").Specific as ComboBox;
            et_ObservacaoLinha = this.GetItem("tb_ObsLine").Specific as EditText;
            et_ValorAte = this.GetItem("tb_VlrAte").Specific as EditText;
            et_ValorDe = this.GetItem("tb_VlrDe").Specific as EditText;
            et_ValorNominal = this.GetItem("tb_VlrNom").Specific as EditText;
            et_DtAprov = this.GetItem("et_DtAprov").Specific as EditText;
            et_Usuar = this.GetItem("et_Usuar").Specific as EditText;

            cb_Insp = this.GetItem("cb_Insp").Specific as ComboBox;
            cb_Equip = this.GetItem("cb_Equip").Specific as ComboBox;

            cb_Aprovado = this.GetItem("cb_Aprov").Specific as CheckBox;

            ds_Linha = this.UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_QUALITY_PLA1");

            OnCustomInitializeEvents();
        }

        private void OnCustomInitializeComponents()
        {
            cb_Aprovado.ValOff = "0";
            cb_Aprovado.ValOn = "1";

            GetItem("et_Cod").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
            GetItem("et_Cod").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_True);
            GetItem("et_Cod").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);

            GetItem("tb_Anali").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
            GetItem("cb_Espec").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
            GetItem("tb_ObsLine").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
            GetItem("tb_VlrAte").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
            GetItem("tb_VlrDe").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
            GetItem("tb_VlrNom").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);

            UIAPIRawForm.Mode = BoFormMode.fm_ADD_MODE;

            _usuario = _usuarioBLL.GetUsuario(_application.Company.UserName);
            if (_usuario.AprovadorQualidade == 0)
            {
                GetItem("cb_Aprov").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                GetItem("et_Usuar").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                GetItem("lb_DtAprov").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                GetItem("et_DtAprov").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
            }

            LoadEspecificacao_Field();
        }

        private void OnCustomInitializeEvents()
        {
            bt_Add.ClickAfter += Bt_Add_ClickAfter;
            bt_Del.ClickAfter += Bt_Del_ClickAfter;
            btn_dupli.ClickAfter += Btn_dupli_ClickAfter;

            cb_Especificacao.ComboSelectAfter += Cb_Especificacao_ComboSelectAfter;
            cb_Equip.ComboSelectAfter += Cb_Equip_ComboSelectAfter;
            cb_Aprovado.PressedAfter += Cb_Aprovado_PressedAfter;

            mt_Lines.ClickAfter += Mt_Lines_ClickAfter;
        }


        #endregion

        #region FormEvents
        protected override void OnFormDataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            if (_usuario.AprovadorQualidade == 0)
            {
                cb_Aprovado.Checked = false;
                et_Usuar.Value = String.Empty;
                et_DtAprov.Value = String.Empty;
            }
            BubbleEvent = true;
        }

        protected override void OnFormDataUpdateAfter(ref BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess)
            {
                if (JaAprovado && !cb_Aprovado.Checked)
                {
                    JaAprovado = false;
                    string msg = _alertaBLL.EnviaAlerta(et_Code.Value, et_Name.Value, et_Observation.Value);
                    if (!String.IsNullOrEmpty(msg))
                    {
                        _application.SetStatusBarMessage(msg, BoMessageTime.bmt_Long);
                    }
                }

                ClearFields();
            }
        }

        protected override void OnFormDataLoadAfter(ref BusinessObjectInfo pVal)
        {
            JaAprovado = cb_Aprovado.Checked;
            ClearFields();
        }

        protected override void OnFormCloseAfter(SBOItemEventArg pVal)
        {
            bt_Add.ClickAfter -= Bt_Add_ClickAfter;
            bt_Del.ClickAfter -= Bt_Del_ClickAfter;

            mt_Lines.ClickAfter -= Mt_Lines_ClickAfter;

            cb_Especificacao.ComboSelectAfter -= Cb_Especificacao_ComboSelectAfter;
            cb_Equip.ComboSelectAfter -= Cb_Equip_ComboSelectAfter;
            cb_Aprovado.ClickAfter -= Cb_Aprovado_PressedAfter;
        }

        protected override void OnFormDataAddAfter(ref BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess)
            {
                if (!cb_Aprovado.Checked)
                {
                    string msg = _alertaBLL.EnviaAlerta(et_Code.Value, et_Name.Value, et_Observation.Value);
                    if (!String.IsNullOrEmpty(msg))
                    {
                        _application.SetStatusBarMessage(msg, BoMessageTime.bmt_Long);
                    }
                }

                ClearFields();
            }
        }
        #endregion

        #region FieldEvents
        protected internal virtual void Mt_Lines_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            int row = mt_Lines.GetNextSelectedRow();
            if (row >= 0)
            {
                this.LoadFields(row);
            }
        }

        protected internal virtual void Cb_Aprovado_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (cb_Aprovado.Checked)
            {
                et_DtAprov.Value = DateTime.Today.ToString("yyyyMMdd");
                et_Usuar.Value = _usuario.Nome;
            }
            else
            {
                et_DtAprov.Value = String.Empty; ;
                et_Usuar.Value = String.Empty;
            }
        }

        protected internal virtual void Cb_Equip_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            UIAPIRawForm.Freeze(true);
            if (!String.IsNullOrEmpty(cb_Equip.Value.Trim()))
            {
                et_ValorDe.Item.Enabled = false;
                et_ValorAte.Item.Enabled = false;
                et_ValorNominal.Item.Enabled = false;

                et_ValorDe.Value = String.Empty;
                et_ValorAte.Value = String.Empty;
                et_ValorNominal.Value = String.Empty;
            }
            else
            {
                ValidateEspecificacao();
            }
            UIAPIRawForm.Freeze(false);
        }

        protected internal virtual void Cb_Especificacao_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            UIAPIRawForm.Freeze(true);
            ValidateEspecificacao();
            UIAPIRawForm.Freeze(false);
        }

        protected internal virtual void Bt_Del_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            int row = mt_Lines.GetNextSelectedRow();
            if (row >= 0)
            {
                mt_Lines.DeleteRow(row);
                if (UIAPIRawForm.Mode == BoFormMode.fm_OK_MODE)
                {
                    UIAPIRawForm.Mode = BoFormMode.fm_UPDATE_MODE;
                }
            }
            else
            {
                _application.SetStatusBarMessage("Por favor, selecione a linha desejada");
            }
        }

        protected internal virtual void Bt_Add_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            UIAPIRawForm.Freeze(true);
            try
            {
                if (String.IsNullOrEmpty(et_Analise.Value))
                {
                    _application.SetStatusBarMessage("'Análise' deve ser informado!");
                    return;
                }
                if (String.IsNullOrEmpty(cb_Especificacao.Value))
                {
                    _application.SetStatusBarMessage("'Especificação' deve ser informada!");
                    return;
                }
                if (String.IsNullOrEmpty(et_ValorDe.Value) && et_ValorDe.Item.Enabled)
                {
                    _application.SetStatusBarMessage("'Valor De' deve ser informado!");
                    return;
                }
                if (String.IsNullOrEmpty(et_ValorAte.Value) && et_ValorAte.Item.Enabled)
                {
                    _application.SetStatusBarMessage("'Valor Até' deve ser informado!");
                    return;
                }
                if (String.IsNullOrEmpty(et_ValorNominal.Value) && et_ValorNominal.Item.Enabled)
                {
                    _application.SetStatusBarMessage("Valor Nominal deve ser informado!");
                    return;
                }
                if (String.IsNullOrEmpty(cb_Insp.Value))
                {
                    _application.SetStatusBarMessage("'Inspeção' deve ser informada!");
                    return;
                }
                if (!String.IsNullOrEmpty(cb_Equip.Value) && String.IsNullOrEmpty(et_ObservacaoLinha.Value))
                {
                    _application.SetStatusBarMessage("Se o campo Equipamento for informado, Observação deve ser informada");
                    return;
                }

                int row = mt_Lines.GetNextSelectedRow();
                if (row >= 0)
                {
                    LoadDataTable(row, false);
                    mt_Lines.SelectRow(row, false, false);
                }
                else
                {
                    LoadDataTable(mt_Lines.RowCount + 1, true);
                }
                ClearFields();
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage("CVA Exception: " + ex.Message);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        protected internal virtual void Btn_dupli_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            var code = et_Code.Value;
            var model = _planoInspecaoBLL.GetPlano(code);


            UIAPIRawForm.Mode = BoFormMode.fm_ADD_MODE;

            et_Code.Value = "";
            et_Name.Value = model.Name;
            et_Observation.Value = model.U_Description;
            if (string.IsNullOrEmpty(model.U_DtAprov.ToString()))
            {
                et_DtAprov.String = "";
            }
            else
            {
                et_DtAprov.String = model.U_DtAprov.ToString();
            }
           

            var count = 1;           

            foreach (var item in model.Linha)
            {
                this.mt_Lines.AddRow();
                ((EditText)mt_Lines.GetCellSpecific("cl_Anls", count)).Value = item.U_Analise;
                ((ComboBox)mt_Lines.GetCellSpecific("cl_Espc", count)).Select(item.U_Especificacao);               
                ((EditText)mt_Lines.GetCellSpecific("cl_VlrN", count)).Value = item.U_ValorNominal;
                ((EditText)mt_Lines.GetCellSpecific("cl_VlrD", count)).Value = item.U_ValorDe;
                ((EditText)mt_Lines.GetCellSpecific("cl_VlrT", count)).Value = item.U_ValorAte;
                ((EditText)mt_Lines.GetCellSpecific("cl_Obsv", count)).Value = item.U_Observacao;                
                ((ComboBox)mt_Lines.GetCellSpecific("cl_Equip", count)).Select(item.U_Equipamento);
                ((ComboBox)mt_Lines.GetCellSpecific("cl_Insp", count)).Select(item.U_Inspecao);

                count++;       
            }
        }

        #endregion

        #region SupportMethods
        private void ValidateEspecificacao()
        {
            var attributeCode = cb_Especificacao.Value;
            if (!string.IsNullOrEmpty(attributeCode))
            {
                var type = _atributoBLL.GetAtributoTipo(attributeCode);
                if (type == 2)
                {
                    et_ValorDe.Item.Enabled = true;
                    et_ValorAte.Item.Enabled = true;
                    et_ValorNominal.Item.Enabled = false;
                    et_ValorNominal.Value = String.Empty;
                }
                else
                {
                    et_ValorDe.Item.Enabled = false;
                    et_ValorAte.Item.Enabled = false;
                    et_ValorNominal.Item.Enabled = true;

                    et_ValorDe.Value = String.Empty;
                    et_ValorAte.Value = String.Empty;
                }
            }
        }

        private void ClearFields()
        {
            cb_Equip.Select("");
            cb_Especificacao.Select("");
            cb_Insp.Select("");
            et_ValorDe.Value = String.Empty;
            et_ValorAte.Value = String.Empty;
            et_ValorNominal.Value = String.Empty;
            et_ObservacaoLinha.Value = String.Empty;

            // Não alterar sequência para setar o foco neste campo
            et_Analise.Value = String.Empty;
        }

        private void LoadFields(int row)
        {
            UIAPIRawForm.Freeze(true);
            et_Analise.Value = ((EditText)mt_Lines.GetCellSpecific("cl_Anls", row)).Value;
            cb_Especificacao.Select(((ComboBox)mt_Lines.GetCellSpecific("cl_Espc", row)).Value);
            et_ValorNominal.Value = ((EditText)mt_Lines.GetCellSpecific("cl_VlrN", row)).Value;
            et_ValorDe.Value = ((EditText)mt_Lines.GetCellSpecific("cl_VlrD", row)).Value;
            et_ValorAte.Value = ((EditText)mt_Lines.GetCellSpecific("cl_VlrT", row)).Value;
            et_ObservacaoLinha.Value = ((EditText)mt_Lines.GetCellSpecific("cl_Obsv", row)).Value;
            cb_Equip.Select(((ComboBox)mt_Lines.GetCellSpecific("cl_Equip", row)).Value);
            cb_Insp.Select(((ComboBox)mt_Lines.GetCellSpecific("cl_Insp", row)).Value);
            UIAPIRawForm.Freeze(false);
        }

        private void LoadDataTable(int row, bool addRow)
        {
            if (addRow)
            {
                this.ds_Linha.Clear();
                this.mt_Lines.AddRow();
            }

            ((EditText)mt_Lines.GetCellSpecific("cl_Anls", row)).Value = et_Analise.Value;
            ((ComboBox)mt_Lines.GetCellSpecific("cl_Espc", row)).Select(cb_Especificacao.Value);
            ((EditText)mt_Lines.GetCellSpecific("cl_VlrN", row)).Value = et_ValorNominal.Value;
            ((EditText)mt_Lines.GetCellSpecific("cl_VlrD", row)).Value = et_ValorDe.Value;
            ((EditText)mt_Lines.GetCellSpecific("cl_VlrT", row)).Value = et_ValorAte.Value;
            ((EditText)mt_Lines.GetCellSpecific("cl_Obsv", row)).Value = et_ObservacaoLinha.Value;
            ((ComboBox)mt_Lines.GetCellSpecific("cl_Equip", row)).Select(cb_Equip.Value);
            ((ComboBox)mt_Lines.GetCellSpecific("cl_Insp", row)).Select(cb_Insp.Value);

            //this.mt_Lines.AddRow();
            this.mt_Lines.FlushToDataSource();
        }

        private PlanoInspecao LoadModel()
        {
            try
            {
                if (_planoInspecao.Code == null)
                {
                    _planoInspecao = new PlanoInspecao();
                    _planoInspecao.Code = et_Code.Value;
                    _planoInspecao.Name = et_Name.Value;
                    _planoInspecao.Description = et_Observation.Value;
                    _planoInspecao.Details = new List<PlanoInspecaoLinha>();
                }
                var line = new PlanoInspecaoLinha();

                line.Analise = et_Analise.Value.Trim();
                line.Especificacao = new Atributo();
                line.Especificacao.Code = cb_Especificacao.Value.Trim();
                line.Observacao = et_ObservacaoLinha.Value.Trim();
                line.ValorAte = et_ValorAte.Value.Trim();
                line.ValorDe = et_ValorDe.Value.Trim();
                line.ValorNominal = et_ValorNominal.Value.Trim();

                var codeString = _planoInspecaoBLL.GetNexCodeLine();
                if (string.IsNullOrEmpty(codeString))
                {
                    line.Code = 1.ToString();
                    line.Name = 1.ToString();
                }
                else
                {
                    var code = (Convert.ToInt32(codeString) + 1).ToString();
                    line.Code = code;
                    line.Name = code;
                }

                _planoInspecao.Details.Add(line);
                return _planoInspecao;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadEspecificacao_Field()
        {
            try
            {
                SAPbouiCOM.Column cl_Espc = (SAPbouiCOM.Column)mt_Lines.Columns.Item("cl_Espc");
                SAPbouiCOM.Column cl_Equip = (SAPbouiCOM.Column)mt_Lines.Columns.Item("cl_Equip");

                cb_Especificacao.ValidValues.Add("", "");
                cb_Equip.ValidValues.Add("", "");

                var atributos = _atributoBLL.Get();
                if (atributos.Count > 0)
                {
                    foreach (var atributo in atributos)
                    {
                        cb_Especificacao.ValidValues.Add(atributo.Code, atributo.Name);
                        cb_Equip.ValidValues.Add(atributo.Code, atributo.Name);
                        cl_Espc.ValidValues.Add(atributo.Code, atributo.Name);
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