using CVA.NF.Import.BLL;
using CVA.NF.Import.HELPER;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.NF.Import.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            try
            {
                ServiceBLL.ExecuteService();
            }
            catch (Exception e)
            {

            }
#else
             //Verifica se a chamada do Serviço foi ou não chamado pelo usuário.
            if (!Environment.UserInteractive)
            {
                try
                {
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                    Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    //Application.EnableVisualStyles();

                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[] { new Service() };
                    ServiceBase.Run(ServicesToRun);
                }
                catch (Exception e)
                {
                    StreamWriter writer = new StreamWriter("c:\\temp\\erro_nf.txt");
                    writer.WriteLine("Erro - serviço parado: " + e.Message);
                    if (e.InnerException != null)
                    {
                        writer.WriteLine("Exceção interna: " + e.InnerException.Message);
                    }
                    writer.Close();
                }
            }
            else
            {
                WindowsServiceInstaller windowsServiceInstaller =
                    new WindowsServiceInstaller(typeof(Service).Assembly, Service.ServiceId, Service.DisplayName);
                windowsServiceInstaller.StandardInstallation();
            }
#endif
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }

        static void HandleException(Exception e)
        {
            StreamWriter writer = new StreamWriter("c:\\temp\\erro_chamados.txt");
            writer.WriteLine("Erro - serviço parado: " + e.Message);
            if (e.InnerException != null)
            {
                writer.WriteLine("Exceção interna: " + e.InnerException.Message);
            }
            writer.Close();
        }
    }
}
