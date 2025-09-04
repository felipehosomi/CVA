using CVA.Core.DIRF.BLL;
using CVA.Core.DIRF.MODEL;
using Dover.Framework.Attribute;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;

namespace CVA.Core.DIRF.CONTROLLER
{
    [FormAttribute("CVADIRF", "CVA.Core.DIRF.VIEW.GeraDirfView.srf")]
    [MenuEvent(UniqueUID = "ViewDirf")]
    public class DirfController : DoverUserFormBase
    {
        #region Atributos
        public DirfBLL _bll { get; set; }
        public SAPbouiCOM.Application _application { get; set; }

        public Button btGerar { get; set; }
        public ComboBox cbTipo { get; set; }
        public ComboBox cbDRL { get; set; }
        public EditText etAnoRef { get; set; }
        public EditText etAnoCal { get; set; }
        public EditText etRecibo { get; set; }

        public ComboBox cbDECPJ_F { get; set; }
        public ComboBox cbDECPJ_G { get; set; }
        public ComboBox cbDECPJ_H { get; set; }
        public ComboBox cbDECPJ_I { get; set; }
        public ComboBox cbDECPJ_J { get; set; }
        public ComboBox cbDECPJ_K { get; set; }
        public ComboBox cbDECPJ_L { get; set; }
        public EditText etDECPJ_M { get; set; }

        public Form oForm { get; set; }

        public FiltroModel Filtro { get; set; }
        #endregion

        #region Construtores
        public DirfController(SAPbouiCOM.Application application, DirfBLL bll)
        {
            _application = application;
            _bll = bll;

            OnInitializeCustomComponents();
            OnInitializeCustomEvents();
        }

        public override void OnInitializeComponent()
        {
            base.OnInitializeComponent();
        }

        public override void OnInitializeFormEvents()
        {
            base.OnInitializeFormEvents();
        }

        public void OnInitializeCustomComponents()
        {
            oForm = (Form)((DoverFormBase)(this)).UIAPIRawForm;

            btGerar = this.GetItem("btGerar").Specific as Button;
            cbTipo = this.GetItem("cbTipo").Specific as ComboBox;
            cbDRL = this.GetItem("cbDRL").Specific as ComboBox;
            etAnoRef = this.GetItem("etAnoRef").Specific as EditText;
            etAnoCal = this.GetItem("etAnoCal").Specific as EditText;
            etRecibo = this.GetItem("etRecibo").Specific as EditText;

            cbDECPJ_F = this.GetItem("cbDECPJ_F").Specific as ComboBox;
            cbDECPJ_G = this.GetItem("cbDECPJ_G").Specific as ComboBox;
            cbDECPJ_H = this.GetItem("cbDECPJ_H").Specific as ComboBox;
            cbDECPJ_I = this.GetItem("cbDECPJ_I").Specific as ComboBox;
            cbDECPJ_J = this.GetItem("cbDECPJ_J").Specific as ComboBox;
            cbDECPJ_K = this.GetItem("cbDECPJ_K").Specific as ComboBox;
            cbDECPJ_L = this.GetItem("cbDECPJ_L").Specific as ComboBox;
            etDECPJ_M = this.GetItem("etDECPJ_M").Specific as EditText;

            LoadCombos();

            this.UIAPIRawForm.Freeze(false);
        }

        public void OnInitializeCustomEvents()
        {
            btGerar.ClickAfter += btGerar_ClickAfter;
        }

        private void LoadCombos()
        {
            var result = _bll.Get_Representantes(_application.Company.DatabaseName);

            foreach (var item in result)
            {
                cbDRL.ValidValues.Add(item.Id, item.Nome);
            }

            cbTipo.ValidValues.Add("O", "Original");
            cbTipo.ValidValues.Add("R", "Retificadora");
        }
        #endregion

        protected internal virtual void btGerar_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (ValidateModel())
            {
                var result = _bll.Gerar_Arquivo(Filtro, _application.Company.DatabaseName);

                if (result)
                    _application.SetStatusBarMessage($@"CVA: DIRF gerado com sucesso. O arquivo foi salvo em C:/CVA Consultoria/DIRF.", BoMessageTime.bmt_Short, false);
                else
                    _application.SetStatusBarMessage("CVA: Ocorreu um erro ao gerar DIRF.", BoMessageTime.bmt_Short, false);
            }
            else
                _application.SetStatusBarMessage("CVA: Por favor, verifique os filtros informados.", BoMessageTime.bmt_Short, false);
        }

        public bool ValidateModel()
        {
            if (String.IsNullOrEmpty(etAnoRef.Value) ||
                String.IsNullOrEmpty(etAnoCal.Value) ||
                String.IsNullOrEmpty(etRecibo.Value) ||
                String.IsNullOrEmpty(etDECPJ_M.Value)||
                cbDRL.Selected == null ||
                cbTipo.Selected == null ||
                cbDECPJ_F.Selected == null ||
                cbDECPJ_G.Selected == null ||
                cbDECPJ_H.Selected == null ||
                cbDECPJ_I.Selected == null ||
                cbDECPJ_J.Selected == null ||
                cbDECPJ_K.Selected == null ||
                cbDECPJ_L.Selected == null)
                
                return false;
            else
            {
                Filtro.AnoReferencia = etAnoRef.Value;
                Filtro.AnoCalendario = etAnoCal.Value;
                Filtro.Tipo = cbTipo.Selected.Value;
                Filtro.Recibo = etRecibo.Value;
                Filtro.Representante = cbDRL.Selected.Value;

                Filtro.DECPJ_F = cbDECPJ_F.Selected.Value;
                Filtro.DECPJ_G = cbDECPJ_G.Selected.Value;
                Filtro.DECPJ_H = cbDECPJ_H.Selected.Value;
                Filtro.DECPJ_I = cbDECPJ_I.Selected.Value;
                Filtro.DECPJ_J = cbDECPJ_J.Selected.Value;
                Filtro.DECPJ_K = cbDECPJ_K.Selected.Value;
                Filtro.DECPJ_L = cbDECPJ_L.Selected.Value;
                Filtro.DECPJ_M = etDECPJ_M.Value;

                return true;
            }
        }
    }
}
