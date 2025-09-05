using CVA.Core.DSP.Controle.HELPER;
using Dover.Framework.Form;
using SAPbouiCOM.Framework;
using System;
using SAPbouiCOM;
using CVA.Core.DSP.Controle.BLL;

namespace CVA.Core.ControleDesgasteFerramenta.VIEW
{
    public partial class RecursoView
    {
        public Button bt_Reiniciar { get; set; }
        private SAPbouiCOM.Application _application { get; set; }
        private FerramentasBLL _ferramentaBLL { get; set; }
        private EditText et_ToolCode { get; set; }
        private EditText et_Counter { get; set; }
    }


    [FormAttribute(B1Forms.CadastroDoRecurso, "CVA.Core.DSP.Controle.Resources.Forms.RecursoFormPartial.xml")]
    public partial class RecursoView : DoverSystemFormBase
    {
        public RecursoView(SAPbouiCOM.Application application, FerramentasBLL ferramentaBLL)
        {
            _application = application;
            _ferramentaBLL = ferramentaBLL;
        }

        public override void OnInitializeComponent()
        {
            this.UIAPIRawForm.Title = "Cadastro de Recursos e Ferramentas";
            bt_Reiniciar    = this.GetItem("bt_Rein").Specific as Button;
            et_ToolCode     = this.GetItem("1470000013").Specific as EditText;
            et_Counter      = this.GetItem("et_Counter").Specific as EditText;
            et_Counter.Item.Enabled = false;
            bt_Reiniciar.Item.Enabled = false;
            OnCustomInitializeEvents();
        }

        public void OnCustomInitializeEvents()
        {
            bt_Reiniciar.ClickAfter += Bt_Reiniciar_ClickAfter;
        }

        #region Events
        protected override void OnFormDataAddAfter(ref BusinessObjectInfo pVal)
        {
            var controlTool = this.UIAPIRawForm.DataSources.DBDataSources.Item(0).GetValue("U_CVA_ControlTool", 0).Trim();
            if(controlTool == 1.ToString())
            {
                var capacity = this.UIAPIRawForm.DataSources.DBDataSources.Item(0).GetValue("U_CVA_Capacity", 0).Trim();
                if (_ferramentaBLL.Add(et_ToolCode.Value, Convert.ToInt32(capacity)))
                    _application.SetStatusBarMessage("CVA: Controle de ferramenta ativado para este recurso", BoMessageTime.bmt_Medium, false);
                else
                    _application.SetStatusBarMessage("CVA: Erro ao ativar controle de ferramenta para este recurso", BoMessageTime.bmt_Medium);
            }
        }

        protected override void OnFormDataUpdateAfter(ref BusinessObjectInfo pVal)
        {
            var controlTool = this.UIAPIRawForm.DataSources.DBDataSources.Item(0).GetValue("U_CVA_ControlTool", 0).Trim();
            if (controlTool == 1.ToString())
            {
                var capacity = this.UIAPIRawForm.DataSources.DBDataSources.Item(0).GetValue("U_CVA_Capacity", 0).Trim();
                if (_ferramentaBLL.Add(et_ToolCode.Value, Convert.ToInt32(capacity)))
                    _application.SetStatusBarMessage("CVA: Controle de ferramenta ativado para este recurso", BoMessageTime.bmt_Medium, false);
                else
                    _application.SetStatusBarMessage("CVA: Erro ao ativar controle de ferramenta para este recurso", BoMessageTime.bmt_Medium);
            }
        }

        protected override void OnFormDataLoadAfter(ref BusinessObjectInfo pVal)
        {
            et_Counter.Item.Enabled = false;
            bt_Reiniciar.Item.Enabled = false;
            var controlTool = this.UIAPIRawForm.DataSources.DBDataSources.Item(0).GetValue("U_CVA_ControlTool", 0).Trim();
            if (controlTool == 1.ToString())
            {
                et_Counter.Value = _ferramentaBLL.GetCounter(et_ToolCode.Value);
                bt_Reiniciar.Item.Enabled = true;
            }
        }

        protected internal virtual void Bt_Reiniciar_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (_ferramentaBLL.Update(et_ToolCode.Value, 0))
                {
                    _application.SetStatusBarMessage("CVA: Ferramenta reparada com sucesso", BoMessageTime.bmt_Medium, false);
                    et_Counter.Value = 0.ToString();
                }
                else
                    _application.SetStatusBarMessage("CVA: Ocorreu um erro ao reparar ferramenta", BoMessageTime.bmt_Medium);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}