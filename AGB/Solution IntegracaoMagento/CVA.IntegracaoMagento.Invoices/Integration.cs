using Flurl.Http;
using ServiceLayerHelper;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.Invoices
{
    public class Integration
    {
        internal static SLConnection SLConnection;
        private static readonly string ServiceLayerURL = ConfigurationManager.AppSettings["ServiceLayerURL"];
        internal static readonly string Database = ConfigurationManager.AppSettings["Database"];
        private static readonly string B1User = ConfigurationManager.AppSettings["B1User"];
        private static readonly string B1Password = ConfigurationManager.AppSettings["B1Password"];

        public Integration()
        {

        }

        public async Task SetIntegration()
        {
            System.Threading.Thread.Sleep(1000);
            string sCaminho = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Log.txt";
            System.Threading.Thread.Sleep(1000);

            if (!(System.IO.File.Exists(sCaminho)))
                System.IO.File.Create(sCaminho).Close();

            Util.GravarLog(sCaminho, "[PROCESSO] - Iniciando o processo.");

            try
            {
                SLConnection = new SLConnection(ServiceLayerURL, Database, B1User, B1Password, 29);
                //var objConfig = await SLConnection.GetAsync<List<Metadata_Config.CVA_CONFIG_MAG>>("CVA_CONFIG_MAG");
                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão SAP (OK): " + SLConnection.CompanyDB);

            }
            catch (FlurlHttpException ex)
            {
                var responseString = await ex.Call.Response.GetStringAsync();
                Util.GravarLog(sCaminho, "[PROCESSO] - (Erro): " + responseString);
            }
            catch (Exception ex)
            {
                Util.GravarLog(sCaminho, "[PROCESSO] - (Erro): " + ex.Message);
            }
        }
    }
}
