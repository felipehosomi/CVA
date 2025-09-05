using CVA.AddOn.Common.Util;
using SkaSapWs.BLL;
using SkaSapWs.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkaSapWs.Service
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
                    ServicesToRun = new ServiceBase[] { new Service1() };
                    ServiceBase.Run(ServicesToRun);
                }
                catch (Exception e)
                {
                    LogBLL.GenerateLog("Erro - serviço parado: " + e.Message);
                    if (e.InnerException != null)
                    {
                        LogBLL.GenerateLog("Exceção interna: " + e.InnerException.Message);
                    }
                }
            }
            else
            {
                WindowsServiceInstaller windowsServiceInstaller =
                    new WindowsServiceInstaller(typeof(Service1).Assembly, Service1.ServiceId, Service1.DisplayName);
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
            LogBLL.GenerateLog("Erro - serviço parado: " + e.Message);
            if (e.InnerException != null)
            {
                LogBLL.GenerateLog("Exceção interna: " + e.InnerException.Message);
            }
        }
    }
}
