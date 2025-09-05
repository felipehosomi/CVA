using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Dover.Framework.Attribute;
using System;
using System.Collections.Generic;
using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.BLL;
using CVA.View.ControleQualidade.Resources.Query;
using System.Globalization;

namespace CVA.View.ControleQualidade.VIEW
{
    #region Properties
    public partial class ApontamentoInspetorView
    {
        private SAPbouiCOM.Application _application { get; set; }

        private PlanoInspecaoBLL _planoInspecaoBLL { get; set; }
        private ApontamentoInspetorBLL _apontamentoBLL { get; set; }
        private ItemBLL _itemBLL { get; set; }
        private UsuarioBLL _usuarioBLL { get; set; }

        public string ItemCode { get; set; }

        public EditText et_Code { get; set; }
        public EditText et_Nome { get; set; }
        public EditText et_OP { get; set; }
        public EditText et_Data { get; set; }
        public EditText et_Hora { get; set; }
        public CheckBox cb_Peca { get; set; }

        public Grid gr_Result { get; set; }

        public DataTable dt_Result { get; set; }

        public Button bt_Save { get; set; }
        public Button bt_Add { get; set; }
        public DBDataSource ds_Header { get; set; }

        public PlanoInspecao PlanoInspecao { get; set; }
        public ApontamentoInspetor ApontamentoInspetor { get; set; }
        public static MODEL.Item Item { get; set; }
        public static string ApontamentoCode { get; set; }
        public string OrdemProducao { get; set; }
    }
    #endregion

    [FormAttribute("CVAApontamentoInspetor", "CVA.View.ControleQualidade.Resources.Form.ApontamentoInspetor.srf")]
    [MenuEvent(UniqueUID = "CVAApontamentoInspetor")]
    public partial class ApontamentoInspetorView : DoverUserFormBase
    {
        public ApontamentoInspetorView(SAPbouiCOM.Application application, ItemBLL itemBLL, PlanoInspecaoBLL planoInspecaoBLL, ApontamentoInspetorBLL apontamentoBLL, UsuarioBLL usuarioBLL)
        {
            _application = application;
            _itemBLL = itemBLL;
            _planoInspecaoBLL = planoInspecaoBLL;
            _apontamentoBLL = apontamentoBLL;
            _usuarioBLL = usuarioBLL;

            OnInitializeCustomComponents();
            OnInitializeCustomEvents();
        }

        #region Initialize
        private void OnInitializeCustomComponents()
        {
            bt_Save = this.GetItem("bt_Save").Specific as Button;
            bt_Add = this.GetItem("bt_Add").Specific as Button;

            et_Code = this.GetItem("et_Code").Specific as EditText;
            et_OP = this.GetItem("et_OP").Specific as EditText;
            et_Data = this.GetItem("et_Data").Specific as EditText;
            et_Hora = this.GetItem("et_Hora").Specific as EditText;
            et_Nome = this.GetItem("et_Nome").Specific as EditText;

            cb_Peca = this.GetItem("cb_Peca").Specific as CheckBox;

            gr_Result = this.GetItem("gr_Result").Specific as Grid;
            dt_Result = this.UIAPIRawForm.DataSources.DataTables.Item("dt_Result");

            ds_Header = this.UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_QUALITY_INS");
            ds_Header.SetValue("U_OP", ds_Header.Offset, OrdemProducao);

            et_Hora.Value = DateTime.Now.ToString("HH:mm");

            et_Code.Item.AffectsFormMode = false;
            et_OP.Item.AffectsFormMode = false;
            et_Data.Item.AffectsFormMode = false;
            et_Hora.Item.AffectsFormMode = false;

            gr_Result.Item.AffectsFormMode = false;

            UIAPIRawForm.AutoManaged = true;
            GetItem("1").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
            GetItem("bt_Save").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
            GetItem("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
            GetItem("et_OP").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
            GetItem("et_Data").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);

            GetItem("1").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
            GetItem("bt_Save").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
            GetItem("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
            GetItem("et_OP").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
            GetItem("et_Data").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
        }

        public override void OnInitializeFormEvents()
        {
            base.OnInitializeFormEvents();
        }

        private void OnInitializeCustomEvents()
        {
            bt_Save.ClickAfter += Bt_Save_ClickAfter;
            bt_Add.ClickAfter += Bt_Add_ClickAfter;
        }

        #endregion

        #region Events
        protected internal virtual void Bt_Add_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            UIAPIRawForm.Freeze(true);

            try
            {
                et_Hora.Value = et_Hora.Value.PadLeft(5, '0');

                DateTime date;
                if (!DateTime.TryParseExact(et_Hora.Value, "HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                {
                    _application.SetStatusBarMessage("Hora deve estar em um formato válido (HH:MM)");
                    return;
                }

                for (int i = 3; i < dt_Result.Columns.Count; i++)
                {
                    if (dt_Result.Columns.Item(i).Name == et_Hora.Value)
                    {
                        _application.SetStatusBarMessage("Horário já existente!");
                        return;
                    }
                }

                if (String.IsNullOrEmpty(et_Nome.Value.Trim()))
                {
                    _application.SetStatusBarMessage("Nome Inspetor deve ser informado!");
                    return;
                }

                // Sempre que adicionar uma nova coluna, já salva no banco para ficar mais fácil de mostrar na tela
                if (dt_Result.Columns.Count > 3)
                {
                    if (!this.Save())
                    {
                        return;
                    }
                }

                this.LoadData(et_Hora.Value + " - " + et_Nome.Value, false);                
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

        protected internal virtual void Bt_Save_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            this.Save();
            UIAPIRawForm.Close();
        }

        protected override void OnFormCloseAfter(SBOItemEventArg pVal)
        {
            try
            {
                bt_Save.ClickAfter -= Bt_Save_ClickAfter;
            }
            catch { }
        }

        protected override void OnFormDataLoadAfter(ref BusinessObjectInfo pVal)
        {
            ItemCode = ds_Header.GetValue("U_ItemCode", ds_Header.Offset);
            this.LoadData(String.Empty, true);
        }

        protected override void OnFormKeyDownAfter(SBOItemEventArg pVal)
        {
            //switch (UIAPIRawForm.Mode)
            //{
            //    case BoFormMode.fm_VIEW_MODE:
            //    case BoFormMode.fm_OK_MODE:
            //        bt_Save.Caption = "OK";
            //        break;
            //    case BoFormMode.fm_EDIT_MODE:
            //    case BoFormMode.fm_UPDATE_MODE:
            //        bt_Save.Caption = "Atualizar";
            //        break;
            //    case BoFormMode.fm_ADD_MODE:
            //        bt_Save.Caption = "Adicionar";
            //        break;
            //}
        }
        #endregion

        #region Support Methods
        public void Proccess()
        {
            et_OP.Value = OrdemProducao;
            et_Data.Value = DateTime.Today.ToString("yyyyMMdd");
            this.LoadData(String.Empty, true);
        }

        private bool Save()
        {
            if (String.IsNullOrEmpty(et_Code.Value))
            {
                return false;
            }
            if (String.IsNullOrEmpty(et_Nome.Value))
            {
                _application.SetStatusBarMessage("Nome Inspetor deve ser informado!");
                return false;
            }

            ApontamentoInspetor = new ApontamentoInspetor();
            ApontamentoInspetor.Code = et_Code.Value;
            ApontamentoInspetor.Data = DateTime.ParseExact(et_Data.Value, "yyyyMMdd", CultureInfo.CurrentCulture);
            ApontamentoInspetor.Usuario = _application.Company.UserName;
            ApontamentoInspetor.OP = Convert.ToInt32(OrdemProducao);
            ApontamentoInspetor.PlanoInsp = Item.PlanoInspecao;
            ApontamentoInspetor.ItemCode = ItemCode;
            ApontamentoInspetor.PecaPiloto = cb_Peca.Checked ? "1" : "0";

            ApontamentoInspetor.Linhas = new List<ApontamentoInspetorLinha>();

            // Cada coluna da hora inserida será um registro no banco
            for (int i = 3; i < dt_Result.Columns.Count; i++)
            {
                for (int j = 0; j < dt_Result.Rows.Count; j++)
                {
                    ApontamentoInspetorLinha linha = new ApontamentoInspetorLinha();
                    linha.InspLinha = (int)dt_Result.GetValue(0, j);
                    linha.Hora = dt_Result.Columns.Item(i).Name.Split('-')[0].Trim();
                    linha.Valor = dt_Result.GetValue(i, j).ToString();
                    linha.Nome = et_Nome.Value;
                    
                    ApontamentoInspetor.Linhas.Add(linha);
                }
            }

            string msg = _apontamentoBLL.Save(ApontamentoInspetor);
            if (!String.IsNullOrEmpty(msg))
            {
                _application.SetStatusBarMessage(msg);
                return false;
            }
            else
            {
                _application.SetStatusBarMessage("Salvo com sucesso!", BoMessageTime.bmt_Medium, false);
                UIAPIRawForm.Mode = BoFormMode.fm_OK_MODE;
            }
            return true;
        }

        /// <summary>
        /// Carrega os dados da tela
        /// </summary>
        /// <param name="newColumn">Nova coluna com a Hora</param>
        /// <param name="setHeader">Preencher os dados do cabeçalho?</param>
        private void LoadData(string newColumn, bool setHeader)
        {
            if (setHeader)
            {
                if (Item == null)
                {
                    Item = _itemBLL.GetInspecaoPorItem(ItemCode);
                }
                if (String.IsNullOrEmpty(Item.PlanoInspecao))
                {
                    _application.SetStatusBarMessage("Plano de inspeção não encontrado para o item " + ItemCode);
                    return;
                }

                if (PlanoInspecao == null || String.IsNullOrEmpty(PlanoInspecao.Code))
                {
                    PlanoInspecao = _planoInspecaoBLL.Get(Item.PlanoInspecao, TipoInspecaoEnum.Inspetor);
                }
                if (PlanoInspecao.Aprovado != "1")
                {
                    _application.SetStatusBarMessage(String.Format("Plano de inspeção {0} não está aprovado!", PlanoInspecao.Code));
                    return;
                }

                if (PlanoInspecao.Details == null || PlanoInspecao.Details.Count == 0)
                {
                    _application.SetStatusBarMessage("Nenhuma etapa de inspeção encontrada para o item " + ItemCode);
                    return;
                }

                ApontamentoInspetor = _apontamentoBLL.Get(_application.Company.UserName, et_Data.Value, et_OP.Value);

                ApontamentoCode = String.Empty;

                if (ApontamentoInspetor != null)
                {
                    ApontamentoCode = ApontamentoInspetor.Code;
                    et_Code.Value = ApontamentoInspetor.Code;
                    et_OP.Value = ApontamentoInspetor.OP.ToString();
                    et_Data.Value = ApontamentoInspetor.Data.ToString("yyyyMMdd");
                    cb_Peca.Checked = ApontamentoInspetor.PecaPiloto == "1";
                }
                else
                {
                    et_Code.Value = _apontamentoBLL.GetNextCode();
                    ApontamentoCode = et_Code.Value;
                }
            }

            // Preenche dados da tabela, utilizando PIVOT TABLE
            string query = String.Format(Select.GetApontamentoInspetor, ApontamentoCode, Item.PlanoInspecao, newColumn);
            dt_Result.ExecuteQuery(query);

            gr_Result.Columns.Item("Linha").Visible = false;
            gr_Result.Columns.Item("Especificação").Editable = false;
            gr_Result.Columns.Item("Valor").Editable = false;
        }
        #endregion
    }
}
