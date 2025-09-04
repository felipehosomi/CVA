using CVA.IntegracaoMagento.Integrator.Models.SAP;
using Flurl.Http;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.Integrator
{
    public class Integration
    {
        internal static SLConnection SLConnection;
        private static readonly string ServiceLayerURL = ConfigurationManager.AppSettings["ServiceLayerURL"];
        internal static readonly string Database = ConfigurationManager.AppSettings["Database"];
        private static readonly string B1User = ConfigurationManager.AppSettings["B1User"];
        private static readonly string B1Password = ConfigurationManager.AppSettings["B1Password"];

        internal static string sTokenMagento;
        private static readonly string MagentoURL = ConfigurationManager.AppSettings["MagentoURL"];
        private static readonly string MagentoUser = ConfigurationManager.AppSettings["MagentoUser"];
        private static readonly string MagentoPassword = ConfigurationManager.AppSettings["MagentoPassword"];

        public Integration()
        {

        }

        public async Task SetIntegration()
        {
            SLConnection = new SLConnection(ServiceLayerURL, Database, B1User, B1Password, 29);

            try
            {
                /*
                string sTeste = GetType().Namespace;
                var oType = Type.GetType($"{GetType().Namespace}.Controllers.BusinessPartnersController")
                    .GetTypeInfo()
                    .GetDeclaredMethod("GetClient_SAP")
                    .Invoke(Type.GetType($"{GetType().Namespace}.Controllers.BusinessPartnersController"), new object[] { "C003063" });
                */

                //await (Task)Type.GetType($"{GetType().Namespace}.Controllers.BusinessPartnersController")
                // .GetTypeInfo()
                // .GetDeclaredMethod("GetClient_SAP")
                // .Invoke(Activator.CreateInstance(Type.GetType($"{GetType().Namespace}.Controllers.BusinessPartnersController")), new object[] { "C003063" });
            }
            catch (FlurlHttpException ex)
            {
                var responseString = await ex.Call.Response.GetStringAsync();

                // Altera o status para erro
                //SetIntegrationTriggerStatus(trigger, TriggerStatus.Error).Wait();

                // Armazena no log de integração a mensagem do erro ocorrido
                //await SetIntegrationLog(trigger.Code, responseString);

                //Logger.Error(ex, responseString);
            }
            catch (Exception ex)
            {
                // Altera o status para erro
                //SetIntegrationTriggerStatus(trigger, TriggerStatus.Error).Wait();

                // Armazena no log de integração a mensagem do erro ocorrido
                //await SetIntegrationLog(trigger.Code, ex.Message);

                //Logger.Error(ex, ex.Message);
            }
        }
    }
}
