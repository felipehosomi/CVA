using MenuPlanner.Controllers;
using SAPbouiCOM.Framework;
using System;

namespace MenuPlanner
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

                Application.SBO_Application.StatusBar.SetText("Conexão do add-on: Menu Planner", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);

                CommonController.GetCompany();

                var menu = new Menu();
                menu.AddMenuItems(String.Concat(System.Windows.Forms.Application.StartupPath, "\\Menu.xml"));
                application.RegisterMenuEventHandler(menu.SBO_Application_MenuEvent);

                Application.SBO_Application.AppEvent += SBO_Application_AppEvent;
                //Application.SBO_Application.StatusBarEvent += SBO_Application_StatusBarEvent;

                application.Run();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        //private static void SBO_Application_StatusBarEvent(string Text, SAPbouiCOM.BoStatusBarMessageType messageType)
        //{
        //    if (Application.SBO_Application.Forms.ActiveForm.TypeEx == "MenuPlanner.Views.Planner")
        //    {
        //        if (Text.Contains("real do documento lançado é"))
        //        {
        //            Text = "TESTE";
        //        }
        //    }
        //}

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
    }
}
