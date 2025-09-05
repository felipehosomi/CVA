using System;
using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using System.Windows.Forms;
using CVA.Escoteiros.CriadorDeCampos.BLL;

namespace CVA.Escoteiros.CriadorDeCampos.Addon
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

            var sboApp = new SBOApp(args[0], Application.StartupPath + "\\CVA.Escoteiros.CriadorDeCampos.dll");
            sboApp.InitializeApplication();

            EventFilterController.SetDefaultEvents();
            InitializeBLL.Initialize();

            ListenerController oListener = new ListenerController();
            System.Threading.Thread oThread = new System.Threading.Thread(new System.Threading.ThreadStart(oListener.startListener));
            oThread.IsBackground = true;
            oThread.Start();

            Application.Run();
        }
    }
}
