using CVA.Core.ControlCommon.BLL;
using CVA.Core.ControlCommon.BLL.BaseReplicadora;
using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.EditarPlanoContas, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class EditarPlanoContasView : DocumentoView
    {
        private PlanoContasBLL _planoContasBLL { get; set; }
        private DBDataSource dt_OACT { get; set; }
        public string OriginalAcctCode { get; set; }
        private Button bt_InserirConta { get; set; }
        private Matrix mt_Accounts { get; set; }
        private EditText et_Code { get; set; }

        public EditarPlanoContasView(SAPbouiCOM.Application application, RegistroBLL registroBLL, GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            base.FormTitle = "Editar plano de contas";
            base.ObjectType = CVAObjectEnum.PlanoContas;
            base.TableName = "OACT";
            base.CodeColumn = "AcctCode";
            base.CodeField = "12";
            base.FocusField = "11";
        }

        public override void OnInitializeComponent()
        {
            base.OnInitializeComponent();
            this.bt_InserirConta = (base.GetItem("6").Specific as Button);
            this.et_Code = (base.GetItem("12").Specific as EditText);
            this.mt_Accounts = (base.GetItem("3").Specific as Matrix);
            this.dt_OACT = base.UIAPIRawForm.DataSources.DBDataSources.Item("OACT");
            this.bt_InserirConta.ClickAfter += Bt_InserirConta_ClickAfter;
            this.mt_Accounts.ClickAfter += Mt_Accounts_ClickAfter;
        }

        public override void _application_ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base._application_ItemEvent(FormUID, ref pVal, out BubbleEvent);
            if (!pVal.BeforeAction)
            {
                if (pVal.EventType == BoEventTypes.et_CLICK)
                {
                    if (pVal.ItemUID == "1")
                    {
                        Adding = false;
                    }
                    if (pVal.ItemUID == "3")
                    {
                        OriginalAcctCode = ((EditText)UIAPIRawForm.Items.Item("12").Specific).Value;
                        Adding = false;
                    }
                    if (pVal.ItemUID == "6")
                    {
                        OriginalAcctCode = "";
                    }
                }
            }
            else
            {
                if (pVal.EventType == BoEventTypes.et_CLICK)
                {
                    if (pVal.ItemUID == "3")
                    {
                        if (Adding)
                        {
                            _application.SetStatusBarMessage("CVA - Por favor, clique em atualizar antes de continuar");
                            BubbleEvent = false;
                            return;
                        }
                    }
                    if (pVal.ItemUID == "6")
                    {
                        if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Replicadora)
                        {
                            Adding = true;
                        }
                        else
                        {
                            _application.SetStatusBarMessage("CVA - Ação não permitida no banco da dados atual");
                            BubbleEvent = false;
                            return;
                        }
                    }
                }
                if (pVal.EventType == BoEventTypes.et_FORM_CLOSE)
                {
                    Adding = false;
                }
            }
        }

        protected override void OnFormLoadAfter(SBOItemEventArg pVal)
        {
            this.OriginalAcctCode = this.et_Code.Value;
            //it_Focus.Click();
            //it_Code.Enabled = false;
        }

        protected internal virtual void Mt_Accounts_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            this.OriginalAcctCode = this.et_Code.Value;
        }

        protected internal virtual void Bt_InserirConta_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            this.OriginalAcctCode = "";
        }

        protected override void OnFormDataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = base.ValidateDatabaseAction();
            if (BubbleEvent)
            {
                if (String.IsNullOrEmpty(this.OriginalAcctCode))
                {
                    base.ValidateAfterAction(FuncaoEnum.Add);
                }
            }
            else
            {
                ErrorMessage = "CVA - Ação não permitida no banco da dados atual";
            }
        }
    }
}