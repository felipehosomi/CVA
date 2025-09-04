using CVA.Core.TransportLCM.BLL;
using CVA.Core.TransportLCM.HELPER;
using CVA.Core.TransportLCM.VIEW.Controller;
using Dover.Framework.Attribute;
using SAPbouiCOM;
using System;

namespace CVA.Core.TransportLCM
{
    [ResourceBOM("CVA.Core.TransportLCM.VIEW.Components.CVA_UserFields.xml", ResourceType.UserField)]
    [AddIn(Name = "CVA.Core.Atlantic", Description = "CVA – AddOn Atlantic", Namespace = "CVA Consultoria", InitMethod = "Initialize")]
    public class Init
    {
        private Application _application { get; }
        private ConfigBLL _configBLL { get; }
        private FormattedSearch _formattedSearch { get; }

        public Init(Application application, ConfigBLL configBLL, FormattedSearch formattedSearch)
        {
            this._application = application;
            this._configBLL = configBLL;
            this._formattedSearch = formattedSearch;
        }

        public void Initialize()
        {
            try
            {
                this._application.MenuEvent += _application_MenuEvent;

                _formattedSearch.AssignFormattedSearch("Conta QUF", SERVICE.Resource.Query.Conta_GetQUF, B1Forms.PlanoContas, "U_CVA_CONTA_QUF");
                _formattedSearch.AssignFormattedSearch("LCM - Conta Destino", SERVICE.Resource.Query.Conta_GetAll, B1Forms.LancamentoContabil, "76", "U_CVA_ContaDestino");

                StaticKeys.ConfigConciliadoraModel = _configBLL.GetConfig(2);
                if (StaticKeys.ConfigConciliadoraModel == null || String.IsNullOrEmpty(StaticKeys.ConfigConciliadoraModel.Banco))
                {
                    this._application.MessageBox("CVA - Configurações não encontradas para base conciliadora. Verifique a tabela [@CVA_CONFIG_DB]");
                }

                StaticKeys.ConfigPortalModel = _configBLL.GetConfig(3);
                if (StaticKeys.ConfigPortalModel == null || String.IsNullOrEmpty(StaticKeys.ConfigPortalModel.Banco))
                {
                    this._application.MessageBox("CVA - Configurações não encontradas para base do portal. Verifique a tabela [@CVA_CONFIG_DB]");
                }
            }
            catch
            {
                this._application.MessageBox("CVA - Tabela [@CVA_CONFIG_DB] não encontrada!");
                return;
            }
        }

        internal virtual void _application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            if (!pVal.BeforeAction && (pVal.MenuUID == "1284" || pVal.MenuUID == "1287") && _application.Forms.ActiveForm.Type == 392)
            {
                LancamentoContabilView.OnCancelOrDuplicate();
            }
            
            BubbleEvent = true;
        }
    }
}

