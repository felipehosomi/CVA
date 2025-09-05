using System;
using System.Configuration;
using System.Threading;

namespace SkaSapWs.BLL
{
    public class ServiceBLL
    {
        public static void ExecuteService()
        {
            int waitSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["WaitSeconds"]);
            try
            {
                var bll = new ProducaoBLL();
                while (true)
                {
                    try
                    {
                        var lst = bll.BuscaDadosMes();
                        if (lst.Count > 0)
                        {
                            bll.GeraArquivoOP(lst);
                            bll.ReturnData(lst);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogBLL.GenerateLog("Erro: " + ex.Message);
                        if (ex.InnerException != null)
                        {
                            LogBLL.GenerateLog("Exceção interna: " + ex.InnerException.Message);
                        }
                    }
                    finally
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(waitSeconds));
                    }
                }
            }
            catch (Exception ex)
            {
                LogBLL.GenerateLog("SERVIÇO PARADO - Erro geral: " + ex.Message);
                if (ex.InnerException != null)
                {
                    LogBLL.GenerateLog("Exceção interna: " + ex.InnerException.Message);
                }
            }
        }
    }
}
