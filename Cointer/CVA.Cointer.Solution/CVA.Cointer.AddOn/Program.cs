using CVA.Cointer.Core.BLL;
using SBO.Hub;
using SBO.Hub.Services;
using System;
using System.Windows.Forms;

namespace CVA.Cointer.AddOn
{
    static class Program
    {
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
                SBOApp sboApp = new SBOApp(args[0], $"{Application.StartupPath}\\CVA.Cointer.Core.dll");

                sboApp.InitializeApplication();
                SBOApp.AutoTranslateHana = false;

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
