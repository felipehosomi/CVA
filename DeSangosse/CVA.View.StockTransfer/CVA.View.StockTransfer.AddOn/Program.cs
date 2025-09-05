using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.StockTransfer.BLL;
using System;
using System.Windows.Forms;

namespace CVA.View.StockTransfer.AddOn
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

            var sboApp = new SBOApp(args[0], Application.StartupPath + "\\CVA.View.StockTransfer.dll");
            sboApp.InitializeApplication();

            EventFilterController.SetDefaultEvents();
            UserFieldsBLL.CreateUserFields();
           
            // Gera nova instância do AppListener para realizar o gerenciamento de memória do aplicativo 
            // O gerenciamento é feito em background através de uma nova thread                          
            ListenerController oListener = new ListenerController();
            System.Threading.Thread oThread = new System.Threading.Thread(new System.Threading.ThreadStart(oListener.startListener));
            oThread.IsBackground = true;
            oThread.Start();

            System.Windows.Forms.Application.Run();
        }
    }
}
