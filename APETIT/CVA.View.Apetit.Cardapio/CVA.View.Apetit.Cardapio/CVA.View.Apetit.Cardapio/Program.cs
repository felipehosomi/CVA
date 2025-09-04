using System;
using System.Collections.Generic;
using SAPbouiCOM.Framework;
using CVA.View.Apetit.Cardapio.Helpers;

namespace CVA.View.Apetit.Cardapio
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
                new Controller.AppController();
                B1Connection.Instance.App.Run();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            try
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
            catch (Exception ex) { }
        }
    }
}
