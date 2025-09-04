using CVA.NF.Import.DAO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CVA.NF.Import.BLL
{
    public class ServiceBLL
    {
        public static void ExecuteService()
        {
            ImpInvoiceDAO impInvoiceDAO = new ImpInvoiceDAO();

            int wait = Convert.ToInt32(ConfigurationManager.AppSettings["WaitSeconds"]);
            string folder = ConfigurationManager.AppSettings["Folder"];

            while (true)
            {
                try
                {
                    // Se memória ultrapassar 1gb, reinicia o serviço
                    long memory = GC.GetTotalMemory(true);
                    if (memory > 1000000000)
                    {
                        RestartService("CVANFImport", wait);
                    }

                    impInvoiceDAO.Import(folder);
                }
                catch (Exception ex)
                {
                    StreamWriter writer = new StreamWriter("c:\\CVA Consultoria\\Erro_NF.txt", true);
                    writer.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    writer.WriteLine("Erro geral: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        writer.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                    writer.WriteLine();
                    writer.Close();
                }
                Thread.Sleep(TimeSpan.FromSeconds(wait));
            }
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
            StreamWriter writer = new StreamWriter("c:\\CVA Consultoria\\Log_Importador_NF.txt", true);
            writer.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            writer.WriteLine("===== Excesso de memória - Reiniciando serviço =====");

            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                writer.WriteLine("Erro geral: " + ex.Message);
                if (ex.InnerException != null)
                {
                    writer.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
            }
            finally
            {
                writer.Close();
            }
        }
    }
}
