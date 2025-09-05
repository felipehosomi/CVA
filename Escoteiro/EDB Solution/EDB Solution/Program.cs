using EDB_Solution.Controller;
using SAPbouiCOM;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Application = SAPbouiCOM.Framework.Application;

namespace EDB_Solution
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var application = args.Length < 1 ? new Application() : new Application(args[0]);

                Application.SBO_Application.StatusBar.SetText("Conexão do add-on: EDB Solution", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);

                CommonController.GetCompany();

                var menu = new Menu();
                menu.AddMenuItems(String.Concat(System.Windows.Forms.Application.StartupPath, "\\Menu.xml"));
                application.RegisterMenuEventHandler(menu.SBO_Application_MenuEvent);

                Application.SBO_Application.AppEvent += SBO_Application_AppEvent;

                // Gera nova instância do Listener para realizar o gerenciamento de memória do aplicativo 
                // O gerenciamento é feito em background através de uma nova thread     
                var thread = new Thread(new ListenerController().StartListener)
                {
                    IsBackground = true
                };

                thread.Start();

                application.Run();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    break;
                default:
                    break;
            }
        }

        public class ListenerController
        {
            private static readonly int IntervalInSeconds;

            static ListenerController()
            {
                IntervalInSeconds = 10;
            }

            public static void FlushMemory()
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                }
            }

            [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

            public void StartListener()
            {
                while (true)
                {
                    if (IntervalInSeconds <= 0)
                    {
                        Thread.Sleep(1000);
                        GC.Collect();
                    }
                    else
                    {
                        Thread.Sleep(IntervalInSeconds * 1000);
                        FlushMemory();
                    }
                }
            }
        }
    }
}
