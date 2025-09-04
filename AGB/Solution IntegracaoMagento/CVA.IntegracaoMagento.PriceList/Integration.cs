using CVA.IntegracaoMagento.PriceList.Controller;
using CVA.IntegracaoMagento.PriceList.Infraestructure;
using CVA.IntegracaoMagento.PriceList.Infrastructure;
using CVA.IntegracaoMagento.PriceList.Models.Magento;
using CVA.IntegracaoMagento.PriceList.Models.SAP;
using Flurl.Http;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.PriceList
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
                var objItem = await SLConnection.GetAsync<List<Metadata_PriceList.Items>>("sml.svc/CVA_MAGENTO_PRICELIST");
                foreach (var item in objItem)
                {
                    Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - Item: {0} - Preço: {1}", item.ItemCode, item.Price));

                    var items = new Metadata_PriceList.ItemSAP();

                    product.product = new Products.Product();
                    product.product.price = item.Price;

                    string sMensagemErro = String.Empty;
                    PriceListController.PriceListToMagentoUpdate(token, item.ItemCode, product, ref sMensagemErro);

                    if (String.IsNullOrEmpty(sMensagemErro))
                    {
                        var hana = new Hana();
                        string sValor = item.Price.ToString();
                        sValor = sValor.Replace(',', '.');
                        string sQuery = (item.Novo.Equals("N") ? HanaCommands.UpdateHana : HanaCommands.InsertHana);
                        sQuery = String.Format(sQuery, Database, item.ItemCode, sValor);
                        //Util.GravarLog(sCaminho, String.Format(@"[QUERY] - {0}", sQuery));
                        await hana.ExecuteNonQueryAsync(sQuery);
                    }
                    else
                        Util.GravarLog(sCaminho, String.Format(@"[MAGENTO] - Mensagem: {0}", sMensagemErro));

                    /*
                    DateTime horaAtual = DateTime.Now.AddSeconds(30);
                    items.U_CVA_Magento_Data = horaAtual;
                    items.U_CVA_Magento_Hora = Convert.ToInt32(String.Format(@"{0}{1}{2}", horaAtual.Hour.ToString(), horaAtual.Minute.ToString().PadLeft(2, '0'), horaAtual.Second.ToString().PadLeft(2, '0')));
                    items.U_CVA_Magento_Msg = sMensagemErro;

                    await SLConnection.PatchAsync($"Items('{item.ItemCode}')", items);
                    */
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
