using CVA.IntegracaoMagento.PriceListSpecial.Controller;
using CVA.IntegracaoMagento.PriceListSpecial.Models.Magento;
using CVA.IntegracaoMagento.PriceListSpecial.Models.SAP;
using Flurl.Http;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.PriceListSpecial
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
            DateTime dataAtual = DateTime.Now.AddSeconds(1);
            string sCaminho = String.Format(@"{0}Log", System.AppDomain.CurrentDomain.BaseDirectory.ToString());

            if (!(System.IO.Directory.Exists(sCaminho)))
                System.IO.Directory.CreateDirectory(sCaminho);

            sCaminho = String.Format(@"{0}\\Log_{1}.txt", sCaminho, String.Format(@"{0}{1}{2}", dataAtual.Year.ToString(), dataAtual.Month.ToString().PadLeft(2, '0'), dataAtual.Day.ToString().PadLeft(2, '0')));

            if (!(System.IO.File.Exists(sCaminho)))
                System.IO.File.Create(sCaminho).Close();

            Util.GravarLog(sCaminho, "[PROCESSO] - Iniciando o processo.");

            try
            {
                SLConnection = new SLConnection(ServiceLayerURL, Database, B1User, B1Password, 29);
                var objConfig = await SLConnection.GetAsync<List<Metadata_Config.CVA_CONFIG_MAG>>("CVA_CONFIG_MAG");
                var token = new Token();
                var product = new Products();

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão HANA (OK)");

                token.apiAddressUri = objConfig[0].U_ApiUrl; //MagentoURL
                token.username = objConfig[0].U_ApiUsuario; //MagentoUser
                token.password = objConfig[0].U_ApiSenha; //MagentoPassword
                token.MagentoClientId = objConfig[0].U_ApiClientId; //MagentoClientId
                token.MagentoClientSecret = objConfig[0].U_ApiClientSecret; //MagentoClientSecret
                Token.create_CN(token);

                if (String.IsNullOrEmpty(token.bearerToken))
                    throw new Exception("Bearer Token não gerado.");

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão Magento (OK)");

                token.bearerToken = token.bearerToken.Replace('"', ' ').Trim();
                var objItem = await SLConnection.GetAsync<List<Metadata_PriceListSpecial.Items>>("sml.svc/CVA_MAGENTO_PRICELISTSPECIAL");
                foreach (var item in objItem)
                {
                    product.product = new Products.Product();
                    product.product.custom_attributes = new List<Products.Product.Custom_Attributes>();
                    product.product.custom_attributes.Add(new Products.Product.Custom_Attributes
                    {
                        special_price_value = item.Price.ToString(),
                        special_from_date_value = item.ValidFrom.ToString("yyyy-MM-dd"),
                        special_to_date_value = item.ValidTo.ToString("yyyy-MM-dd")
                    });

                    string sMensagemErro = String.Empty;
                    PriceListSpecialController.PriceListSpecialToMagentoUpdate(token, item.ItemCode, product, ref sMensagemErro);

                    if (!String.IsNullOrEmpty(sMensagemErro))
                        Util.GravarLog(sCaminho, String.Format(@"[MAGENTO] - Mensagem: {0}", sMensagemErro));
                }
            }
            catch (FlurlHttpException ex)
            {
                Util.GravarLog(sCaminho, String.Format(@"[ERRO] - {0}", ex.Message));
                var responseString = await ex.Call.Response.GetStringAsync();
            }
            catch (Exception ex)
            {
                Util.GravarLog(sCaminho, String.Format(@"[ERRO] - {0}", ex.Message));
            }

            Util.GravarLog(sCaminho, "[PROCESSO] - Processo finalizado.");
        }
    }
}
