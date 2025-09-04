using CVA.Core.ControlCommon.BLL.BaseReplicadora;
using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using CVA.Core.ControlCommon.MODEL.CVACommon;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Linq;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    public class DocumentoView : DoverSystemFormBase
    {
        #region Properties
        public CVAObjectEnum ObjectType { get; set; }
        public string CodeColumn { get; set; }
        public string CodeField { get; set; }
        public string FocusField { get; set; }
        public string FormTitle { get; set; }
        public string TableName { get; set; }
        public bool Adding { get; set; }

        public string ErrorMessage { get; set; } = String.Empty;
        public RegistroBLL _registroBLL { get; set; }
        public GenericBLL _GenericBLL { get; set; }
        public ItemBLL _itemBLL { get; set; }
        public ParceiroNegocioBLL _parceiroNegocioBLL { get; set; }
        public Registro _registro { get; set; }
        public Item it_Code { get; set; }
        public Item it_Focus { get; set; }

        public bool IsIdentity { get; set; } = false;

        protected SAPbouiCOM.Application _application { get; set; }
        #endregion

        #region Constructor
        public DocumentoView(SAPbouiCOM.Application application, RegistroBLL registroBLL, GenericBLL genericBLL)
        {
            _application = application;
            _registroBLL = registroBLL;
            _GenericBLL = genericBLL;

            _application.StatusBarEvent += _application_StatusBarEvent;
            _application.MenuEvent += _application_MenuEvent;
            _application.ItemEvent += _application_ItemEvent;
        }
        #endregion

        #region Initialize
        public override void OnInitializeFormEvents()
        {
            base.OnInitializeFormEvents();
        }

        public override void OnInitializeComponent()
        {
            if (!String.IsNullOrEmpty(CodeField))
            {
                it_Code = this.GetItem(CodeField);
            }
            if (!String.IsNullOrEmpty(FocusField))
            {
                it_Focus = this.GetItem(FocusField);
            }
        }
        #endregion

        #region ApplicationEvents
        public virtual void _application_ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            // Desabilita o campo de código para não dar problema na replicação dos dados
            try
            {
                UIAPIRawForm.Title = FormTitle;
                if (Adding)
                {
                    BubbleEvent = true;
                    return;
                }
                if (UIAPIRawForm.Mode == BoFormMode.fm_EDIT_MODE || UIAPIRawForm.Mode == BoFormMode.fm_UPDATE_MODE || UIAPIRawForm.Mode == BoFormMode.fm_OK_MODE)
                {
                    if (pVal.EventType == BoEventTypes.et_KEY_DOWN && pVal.ItemUID == CodeField)
                    {
                        if (it_Code?.Enabled == true)
                        {
                            it_Focus.Click();
                            it_Code.Enabled = false;
                        }
                    }
                    else
                    {
                        if (it_Code?.Enabled == true)
                        {
                            it_Code.Enabled = false;
                        }
                    }
                }
            }
            catch { }

            BubbleEvent = true;
        }

        private void _application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.BeforeAction)
            {
                if (pVal.MenuUID == "1283")
                {
                    _application.SetStatusBarMessage("CVA - Ação desabilitada");
                    BubbleEvent = false;
                }
            }
        }

        private void _application_StatusBarEvent(string Text, BoStatusBarMessageType messageType)
        {
            if (Text.Contains("UI_API -7780") && !String.IsNullOrEmpty(ErrorMessage))
            {
                _application.SetStatusBarMessage(ErrorMessage);
            }
        }
        #endregion

        #region FormEvents
        protected override void OnFormDataLoadAfter(ref BusinessObjectInfo pVal)
        {
            if (it_Code?.Enabled == true)
            {
                try
                {
                    it_Code.Enabled = false;
                }
                catch
                {
                    it_Focus.Click();
                    it_Code.Enabled = false;
                }
            }
        }

        protected override void OnFormDataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateDatabaseAction();
        }

        protected override void OnFormDataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            if (ObjectType == CVAObjectEnum.FormaPagamento)
            {
                BubbleEvent = true;
                return;
            }
            BubbleEvent = ValidateDatabaseAction();

            if (BubbleEvent)
            {
                DBDataSource dts = UIAPIRawForm.DataSources.DBDataSources.Item(TableName);
                string code = dts.GetValue(CodeColumn, dts.Offset);

                _registro = _registroBLL.Get(code, ObjectType);
                if (_registro != null && _registro.Status.HasValue)
                {
                    if (_registro.Status != (int)StatusEnum.Replicado && _registro.Status != (int)StatusEnum.Erro)
                    {
                        ErrorMessage = "CVA - Registro não foi replicado em todas as bases, por favor aguarde e tente novamente";
                        BubbleEvent = false;
                    }
                }
            }
        }

        protected override void OnFormDataAddAfter(ref BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess)
            {
                ValidateAfterAction(FuncaoEnum.Add);
            }
        }

        protected override void OnFormDataUpdateAfter(ref BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess)
            {
                ValidateAfterAction(FuncaoEnum.Update);
            }
        }

        protected override void OnFormCloseAfter(SBOItemEventArg pVal)
        {
            _application.StatusBarEvent -= _application_StatusBarEvent;
            _application.MenuEvent -= _application_MenuEvent;
            _application.ItemEvent -= _application_ItemEvent;
        }
        #endregion

        #region SupportMethods
        public bool ValidateDatabaseAction()
        {
            if (StaticKeys.Base?.TipoBase == TipoBaseEnum.NaoConectado)
            {
                ErrorMessage = "CVA - Ação não permitida - AddOn não conectado na base replicadora";
                return false;
            }
            if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Comum || (StaticKeys.Base?.TipoBase == TipoBaseEnum.Consolidadora && ObjectType != CVAObjectEnum.Usuario))
            {
                ErrorMessage = "CVA - Ação não permitida no banco da dados atual";
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ValidateAfterAction(FuncaoEnum funcao)
        {
            if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Replicadora
                || (StaticKeys.Base?.TipoBase == TipoBaseEnum.Consolidadora && ObjectType == CVAObjectEnum.Usuario))
            {
                DBDataSource dts = UIAPIRawForm.DataSources.DBDataSources.Item(TableName);
                string code = dts.GetValue(CodeColumn, dts.Offset);

                if (!this.IsIdentity || funcao == FuncaoEnum.Update)
                {
                    code = dts.GetValue(this.CodeColumn, dts.Offset);
                }
                else
                {
                    code = _GenericBLL.GetMaxId(this.CodeColumn, this.TableName);
                }

                if (funcao == FuncaoEnum.Update && _registro?.Status == (int)StatusEnum.Erro)
                {
                    _registroBLL.RestartErrorReg(code, ObjectType);
                }
                else
                {
                    _registroBLL.Insert(code, ObjectType, funcao);
                }
            }
        }
        #endregion
    }
}
