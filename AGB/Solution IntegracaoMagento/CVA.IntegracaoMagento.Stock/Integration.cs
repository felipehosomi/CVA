using CVA.IntegracaoMagento.Stock.Controller;
using CVA.IntegracaoMagento.Stock.Models.Magento;
using CVA.IntegracaoMagento.Stock.Models.SAP;
using Flurl.Http;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.Stock
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
                var objConfig = await SLConnection.GetAsync<List<Metadata_Config.CVA_CONFIG_MAG>>("CVA_CONFIG_MAG");
                var token = new Token();

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão HANA (OK)");

                token.apiAddressUri = objConfig[0].U_ApiUrl; //MagentoURL
                token.username = objConfig[0].U_ApiUsuario; //MagentoUser
                token.password = objConfig[0].U_ApiSenha; //MagentoPassword
                token.MagentoClientId = objConfig[0].U_ApiClientId; //MagentoClientId
                token.MagentoSecretId = objConfig[0].U_ApiClientSecret; //MagentoClientSecret
                Token.create_CN(token);

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão Magento (OK) Token: " + token.bearerTolken);

                if (String.IsNullOrEmpty(token.bearerTolken))
                    throw new Exception("Bearer Token não gerado.");

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão Magento (OK)");
                var stock = await SLConnection.GetAsync<List<Metadata_Stock.Stock>>("sml.svc/CVAMAGENTOSTOCK");
                if (stock.Count > 0)
                {
                    Token.create_CN(token);
                    foreach (var item in stock)
                    {
                        StockController.update_STOCK(token, item.stockItemCode, item.stockOnHand);
                    }
                }
            }
            catch (FlurlHttpException ex)
            {
                var responseString = await ex.Call.Response.GetStringAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
