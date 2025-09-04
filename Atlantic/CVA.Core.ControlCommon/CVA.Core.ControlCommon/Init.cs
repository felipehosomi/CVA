using CVA.Core.ControlCommon.BLL;
using CVA.Core.ControlCommon.BLL.BaseReplicadora;
using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using CVA.Core.ControlCommon.SERVICE.CVACommon;
using Dover.Framework.Attribute;
using SAPbouiCOM;
using System;

namespace CVA.Core.ControlCommon
{
    [ResourceBOM("CVA.Core.ControlCommon.VIEW.Components.CVA_UserFields.xml", ResourceType.UserField)]
    [ResourceBOM("CVA.Core.ControlCommon.VIEW.Components.CVA_UserTables.xml", ResourceType.UserTable)]
    [AddIn(Name = "CVA.Core.ControlCommon", Description = "CVA – Controle de funções", Namespace = "CVA Consultoria", InitMethod = "Initialize")]
    public class Init
    {
        private Application _application { get; }
        private BaseBLL _baseBLL { get; }
        private ConfigBLL _configBLL { get; }

        public Init(Application application, GenericBLL genericBLL, BaseBLL baseBLL, ConfigBLL configBLL)
        {
            this._application = application;
            this._baseBLL = baseBLL;
            this._configBLL = configBLL;
        }

        public void Initialize()
        {
            ConfigModel configModel = null;
            try
            {
                configModel = _configBLL.GetConfig(1);
            }
            catch
            {
                this._application.MessageBox("CVA - Tabela [@CVA_CONFIG_DB] não encontrada!");
                StaticKeys.Base = new MODEL.CVACommon.Base();
                StaticKeys.Base.TipoBase = TipoBaseEnum.NaoConectado;
                return;
            }
            if (configModel == null || String.IsNullOrEmpty(configModel.Banco))
            {
                this._application.MessageBox("CVA - Configurações não encontradas para base replicadora. Verifique a tabela [@CVA_CONFIG_DB]");
                StaticKeys.Base = new MODEL.CVACommon.Base();
                StaticKeys.Base.TipoBase = TipoBaseEnum.NaoConectado;
                return;
                //throw new Exception("Falha ao executar AddOn - Configurações não encontradas para base replicadora. Verifique a tabela [@CVA_CONFIG_DB]");
            }

            try
            {
                StaticKeys.ConnectionString = SqlHelper.GetConnectionString(configModel.Servidor, configModel.Banco, configModel.Usuario, configModel.Senha);
                StaticKeys.Base = _baseBLL.GetByName(this._application.Company.DatabaseName);
                if (StaticKeys.Base.ID == 1)
                {
                    StaticKeys.Base.TipoBase = MODEL.TipoBaseEnum.Replicadora;
                }
                else
                {
                    int maxId = _baseBLL.GetMaxId();
                    if (maxId == StaticKeys.Base.ID)
                    {
                        StaticKeys.Base.TipoBase = MODEL.TipoBaseEnum.Consolidadora;
                    }
                    else
                    {
                        StaticKeys.Base.TipoBase = MODEL.TipoBaseEnum.Comum;
                    }
                }

                bool flag = StaticKeys.Base == null || StaticKeys.Base.ID == 0;
                if (flag)
                {
                    this._application.SetStatusBarMessage(string.Format("CVA - Banco de dados {0} não encontrado na base replicadora", this._application.Company.DatabaseName), BoMessageTime.bmt_Medium, true);
                }
            }
            catch (Exception ex)
            {
                StaticKeys.Base = new MODEL.CVACommon.Base();
                StaticKeys.Base.TipoBase = TipoBaseEnum.NaoConectado;
                this._application.SetStatusBarMessage("CVA - Erro ao conectar no banco de replicação: " + ex.Message);
                this._application.MessageBox("CVA - Erro ao conectar no base replicadora: verifique os dados na tabela [@CVA_CONFIG_DB]");
            }
        }
    }
}
