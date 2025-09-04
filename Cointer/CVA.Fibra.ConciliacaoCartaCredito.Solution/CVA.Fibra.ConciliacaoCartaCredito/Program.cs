using CVA.Fibra.ConciliacaoCartaCredito.Core.BLL;
using SBO.Hub;
using SBO.Hub.Services;
using System;
using System.Windows.Forms;

namespace CVA.Fibra.ConciliacaoCartaCredito
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

            try
            {
                SBOApp sboApp = new SBOApp(args[0], $"{Application.StartupPath}\\CVA.Fibra.ConciliacaoCartaCredito.Core.dll");

                sboApp.InitializeApplication();
                //SBOApp.AutoTranslateHana = true;

                InitializeBLL.Initialize();

                var oListener = new Listener();
                var oThread = new System.Threading.Thread(oListener.startListener) { IsBackground = true };
                oThread.Start();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
