using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Control.Logic.BLL;
using CVA.AddOn.Control.Logic.BLL.CVACommon;
using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using System;
using System.Windows.Forms;

namespace CVA.AddOn.Control
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.Exit();
                return;
            }

            var sboApp = new SBOApp(args[0], Application.StartupPath + "\\CVA.AddOn.Control.Logic.dll");
            sboApp.InitializeApplication();

            BaseBLL baseBLL = new BaseBLL();
            ConfigBLL configBLL = new ConfigBLL();

            ConfigModel configModel = null;
            try
            {
                configModel = configBLL.GetConfig(1);
            }
            catch
            {
                SBOApp.Application.MessageBox("CVA - Tabela [@CVA_CONFIG_DB] não encontrada!");
                StaticKeys.Base = new Base();
                StaticKeys.Base.TipoBase = TipoBaseEnum.NaoConectado;
                return;
            }
            if (configModel == null || String.IsNullOrEmpty(configModel.Banco))
            {
                SBOApp.Application.MessageBox("CVA - Configurações não encontradas para base replicadora. Verifique a tabela [@CVA_CONFIG_DB]");
                StaticKeys.Base = new Base();
                StaticKeys.Base.TipoBase = TipoBaseEnum.NaoConectado;
                return;
                //throw new Exception("Falha ao executar AddOn - Configurações não encontradas para base replicadora. Verifique a tabela [@CVA_CONFIG_DB]");
            }

            try
            {
                StaticKeys.ConnectionString = SqlController.GetConnectionString(configModel.Servidor, configModel.Banco, configModel.Usuario, configModel.Senha);
                StaticKeys.Base = baseBLL.GetByName(SBOApp.Application.Company.DatabaseName);
                if (StaticKeys.Base.ID == 1)
                {
                    StaticKeys.Base.TipoBase = TipoBaseEnum.Replicadora;
                }
                else
                {
                    int maxId = baseBLL.GetMaxId();
                    if (maxId == StaticKeys.Base.ID)
                    {
                        StaticKeys.Base.TipoBase = TipoBaseEnum.Consolidadora;
                    }
                    else
                    {
                        StaticKeys.Base.TipoBase = TipoBaseEnum.Comum;
                    }
                }

                bool flag = StaticKeys.Base == null || StaticKeys.Base.ID == 0;
                if (flag)
                {
                    SBOApp.Application.SetStatusBarMessage(string.Format("CVA - Banco de dados {0} não encontrado na base replicadora", SBOApp.Application.Company.DatabaseName), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                }
            }
            catch (Exception ex)
            {
                StaticKeys.Base = new Base();
                StaticKeys.Base.TipoBase = TipoBaseEnum.NaoConectado;
                SBOApp.Application.SetStatusBarMessage("CVA - Erro ao conectar no banco de replicação: " + ex.Message);
                SBOApp.Application.MessageBox("CVA - Erro ao conectar no base replicadora: verifique os dados na tabela [@CVA_CONFIG_DB]");
            }


            // Gera nova instância do AppListener para realizar o gerenciamento de memória do aplicativo 
            // O gerenciamento é feito em background através de uma nova thread                          
            ListenerController oListener = new ListenerController();
            System.Threading.Thread oThread = new System.Threading.Thread(new System.Threading.ThreadStart(oListener.startListener));
            oThread.IsBackground = true;
            oThread.Start();

            System.Windows.Forms.Application.Run();
        }
    }
}
