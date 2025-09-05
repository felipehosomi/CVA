using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.EDoc.BLL;
using System;
using System.Windows.Forms;

namespace CVA.View.EDoc.AddOn
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

            var sboApp = new SBOApp(args[0], Application.StartupPath + "\\CVA.View.EDoc.dll");
            sboApp.InitializeApplication();

            InitializeBLL.Initialize();

            ListenerController oListener = new ListenerController();
            System.Threading.Thread oThread = new System.Threading.Thread(new System.Threading.ThreadStart(oListener.startListener));
            oThread.IsBackground = true;
            oThread.Start();

            Application.Run();
        }
    }
}
