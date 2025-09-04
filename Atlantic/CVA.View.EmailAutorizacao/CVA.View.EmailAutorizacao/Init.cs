using CVA.View.EmailAutorizacao.BLL;
using CVA.View.EmailAutorizacao.HELPER;
using Dover.Framework.Attribute;
using SAPbouiCOM;
using System;

namespace CVA.View.EmailAutorizacao
{
    [AddIn(Name = "CVA.View.EmailAutorizacao", Description = "CVA – Envio de e-mail para autorização de documentos", Namespace = "CVA Consultoria", InitMethod = "Initialize")]
    public class Init
    {
        private Application _application { get; }
        private ConfigBLL _configBLL { get; }

        public Init(Application application, ConfigBLL configBLL)
        {
            this._application = application;
            this._configBLL = configBLL;
        }

        public void Initialize()
        {
            try
            {
                StaticKeys.ConfigModel = _configBLL.GetConfig(3);
                if (StaticKeys.ConfigModel == null || String.IsNullOrEmpty(StaticKeys.ConfigModel.Banco))
                {
                    this._application.MessageBox("Configurações não encontradas para base do Portal. Verifique a tabela [@CVA_CONFIG_DB]");
                }
            }
            catch
            {
                this._application.MessageBox("CVA - Tabela [@CVA_CONFIG_DB] não encontrada!");
                return;
            }
        }
    }
}
