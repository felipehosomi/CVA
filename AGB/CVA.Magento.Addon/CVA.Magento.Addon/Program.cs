using System;
using System.Collections.Generic;
using SAPbouiCOM.Framework;
using System.Threading;

namespace CVA.Magento.Addon
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
                Application oApp = null;
                if (args.Length < 1)
                {
                    oApp = new Application();
                }
                else
                {
                    oApp = new Application(args[0]);
                }                
                Database.Initialize();
                Menu MyMenu = new Menu();
                MyMenu.AddMenuItems();
                oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);
                Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);

                #region [ Tratamento para o add-on não perder conexão com a UI ]

                Program addon = new Program();
                Thread threadCheckConnection = new Thread(new ThreadStart(addon.CheckConnection));
                threadCheckConnection.IsBackground = true;
                threadCheckConnection.Start();

                #endregion

                Application.SBO_Application.SetStatusBarMessage("Add-on Integração Magento conectado com sucesso", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                oApp.Run();
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
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    break;
                default:
                    break;
            }
        }

        #region [ Check Connection ]

        private void CheckConnection()
        {
            while (true)
            {
                try
                {
                    Application.SBO_Application.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, true);
                    bool testConnection;
                    SAPbouiCOM.Company app = Application.SBO_Application.Company;
                    testConnection = (bool)(app == null);
                }
                catch { Environment.Exit(0); }
                Thread.Sleep(10000);
            }
        }

        #endregion

    }
}
