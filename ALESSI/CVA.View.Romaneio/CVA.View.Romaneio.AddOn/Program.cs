using System;
using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using System.Windows.Forms;
using CVA.View.Romaneio.BLL;

namespace CVA.View.Romaneio.AddOn
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

            var sboApp = new SBOApp(args[0], Application.StartupPath + "\\CVA.View.Romaneio.dll");
            sboApp.InitializeApplication();

            EventFilterController.SetDefaultEvents();
            UserFieldsBLL.CreateUserFields();

            try
            {
                MenuController.LoadFromXML(Application.StartupPath + "\\Menu.xml");
            }
            catch (Exception ex)
            {
                
            }

            ListenerController oListener = new ListenerController();
            System.Threading.Thread oThread = new System.Threading.Thread(new System.Threading.ThreadStart(oListener.startListener));
            oThread.IsBackground = true;
            oThread.Start();

            System.Windows.Forms.Application.Run();
        }
    }
}
