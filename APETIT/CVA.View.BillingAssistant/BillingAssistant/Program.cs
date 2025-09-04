using BillingAssistant.Controllers;
using BillingAssistant.ExtensionMethods;
using BillingAssistant.Infrastructure;
using SAPbouiCOM;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Application = SAPbouiCOM.Framework.Application;

namespace BillingAssistant
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

                Application.SBO_Application.StatusBar.SetText("Conexão do add-on: Billing Assistant", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);

                CommonController.GetCompany();

                var menu = new Menu();
                menu.AddMenuItems(String.Concat(System.Windows.Forms.Application.StartupPath, "\\Menu.xml"));
                application.RegisterMenuEventHandler(menu.SBO_Application_MenuEvent);

                Application.SBO_Application.AppEvent += SBO_Application_AppEvent;

                CriarCampos(); 

                application.Run();
            }
            catch (COMException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private static void CriarCampos()
        {
            using (var q = QueryManager.DoQuery("select \"FieldID\" from CUFD where \"AliasID\" = 'CVA_ID_PEDIDO' and \"TableID\"='OWOR'"))
            {
                if (q.HasRow)
                    return;
            }

            var udf = (SAPbobsCOM.UserFieldsMD)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

            try
            {
                udf.Name = "CVA_ID_PEDIDO";
                udf.Description = "Pedido de venda";
                udf.TableName = "OWOR";
                udf.LinkedSystemObject = SAPbobsCOM.UDFLinkedSystemObjectTypesEnum.ulOrders;
                udf.Type = SAPbobsCOM.BoFieldTypes.db_Numeric;
                udf.SubType = SAPbobsCOM.BoFldSubTypes.st_None;
                udf.EditSize = 11;
                udf.Mandatory = SAPbobsCOM.BoYesNoEnum.tNO;

                var ret = udf.Add();
                if (ret != 0)
                    throw new Exception(CommonController.Company.GetLastErrorDescription());
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                udf.Kill();
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
    }
}
