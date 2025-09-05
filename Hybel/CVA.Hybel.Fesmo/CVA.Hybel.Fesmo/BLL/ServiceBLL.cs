using CVA.AddOn.Common.DAO;
using log4net;
using System;
using System.IO;
using System.Threading;

namespace CVA.Hybel.Fesmo.BLL
{
    public class ServiceBLL
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void ExecuteService()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Logger.Debug("Iniciando integração");
                string directory = System.Configuration.ConfigurationManager.AppSettings["Directory"];
                string server = System.Configuration.ConfigurationManager.AppSettings["Server"];
                string database = System.Configuration.ConfigurationManager.AppSettings["Database"];
                string dbUserName = System.Configuration.ConfigurationManager.AppSettings["DBUserName"];
                string dbPassword = System.Configuration.ConfigurationManager.AppSettings["DBPassword"];
                int waitMinutes = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WaitMinutes"]);
                int fileDeleteDays = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FileDeleteDays"]);

                if (!Directory.Exists(directory))
                {
                    Logger.Debug("Criando diretório inexistente " + directory);
                    Directory.CreateDirectory(directory);
                }

                SqlDAO.SetConnectionString(server, database, dbUserName, dbPassword);
                SqlDAO.Connect();
                Logger.Debug("Conexão com banco de dados efetuada com sucesso");
                Logger.Debug("Servidor: " + server);
                Logger.Debug("Nome BD: " + database);
                Logger.Debug("Usuário: " + dbUserName);

                OrdemProducaoBLL ordemProducaoBLL = new OrdemProducaoBLL();
                ArquivoBLL arquivoBLL = new ArquivoBLL();

                while (true)
                {
                    string erro = ordemProducaoBLL.ExecutaIntegracao(directory);
                    if (!String.IsNullOrEmpty(erro))
                    {
                        Logger.Error("Erro OP: " + erro);
                    }

                    erro = arquivoBLL.RemoveArquivos(directory, fileDeleteDays);
                    if (!String.IsNullOrEmpty(erro))
                    {
                        Logger.Error("Erro ao remover arquivos: " + erro);
                    }

                    Thread.Sleep(TimeSpan.FromMinutes(waitMinutes));
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro geral: " + ex.Message);
            }
        }
    }
}
