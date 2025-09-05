using CVA.AddOn.Common;
using log4net;
using SAPbobsCOM;
using System;
using System.Configuration;
using System.Threading;

namespace SapKsaWs.BLL
{
    public class ServiceBLL
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServiceBLL()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void ExecuteService()
        {
            Logger.Info("------------------ Iniciando serviço ---------------------");
            
            int waitSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["WaitSeconds"]);
            try
            {
                ConnectToSBO();
                Logger.Info($"------------------ Serviço em execução a cada {waitSeconds} segundos ---------------------");

                OrdemProducaoBLL ordemProducaoBLL = new OrdemProducaoBLL();
                RegistroTempoBLL registroTempoBLL = new RegistroTempoBLL();
                ConsumoBLL consumoBLL = new ConsumoBLL();
                ProdutoAcabadoBLL produtoAcabadoBLL = new ProdutoAcabadoBLL();

                while (true)
                {
                    try
                    {
                        ordemProducaoBLL.IntegraOPs();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Geração de OPs: " + ex.Message);
                        if (ex.InnerException != null)
                        {
                            Logger.Error("Exceção interna: " + ex.InnerException.Message);
                        }
                    }
                    try
                    {
                        var list = registroTempoBLL.BuscaDadosMes();
                        registroTempoBLL.GeraArquivo(list);
                        registroTempoBLL.AtualizaProducaoMes(list);
                        registroTempoBLL.VerificaRegistroTempo();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Registro de tempo: " + ex.Message);
                        if (ex.InnerException != null)
                        {
                            Logger.Error("Exceção interna: " + ex.InnerException.Message);
                        }
                    }
                    try
                    {
                        consumoBLL.GeraArquivoConsumo();
                        consumoBLL.VerificaSaidaInsumos();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Consumo: " + ex.Message);
                        if (ex.InnerException != null)
                        {
                            Logger.Error("Exceção interna: " + ex.InnerException.Message);
                        }
                    }
                    try
                    {
                        produtoAcabadoBLL.GeraArquivoAcabado();
                        produtoAcabadoBLL.VerificaEntradaAcabado();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Acabado: " + ex.Message);
                        if (ex.InnerException != null)
                        {
                            Logger.Error("Exceção interna: " + ex.InnerException.Message);
                        }
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(waitSeconds));
                }
            }
            catch (Exception ex)
            {
                Logger.Error("SERVIÇO PARADO - Erro geral: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Logger.Error("Exceção interna: " + ex.InnerException.Message);
                }
            }
        }

        public static int ConnectToSBO()
        {
            Logger.Info("------------------ Connectando no B1 ---------------------");
            int connectToCompanyReturn = -1;

            try
            {
                Company company = new Company();

                company.DbServerType = (BoDataServerTypes)Convert.ToInt32(ConfigurationManager.AppSettings["DbServerType"]);
                company.Server = ConfigurationManager.AppSettings["Server"];
                company.CompanyDB = ConfigurationManager.AppSettings["CompanyDB"];
                company.UseTrusted = false;
                company.DbUserName = ConfigurationManager.AppSettings["DbUserName"];
                company.DbPassword = ConfigurationManager.AppSettings["DbPassword"];
                company.UserName = ConfigurationManager.AppSettings["UserName"];
                company.Password = ConfigurationManager.AppSettings["Password"];
                company.language = BoSuppLangs.ln_Portuguese_Br;

                connectToCompanyReturn = SBOApp.ConnectToCompany(company);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro geral ao conectar no B1: " + ex);
            }
            if (connectToCompanyReturn != 0)
            {
                throw new Exception(String.Format("Erro ao conectar no B1: {0}", SBOApp.Company.GetLastErrorDescription()));
            }
            else
            {
                Logger.Info("------------------ Conexão efetuado com sucesso ---------------------");
            }
            return connectToCompanyReturn;
        }
    }
}
