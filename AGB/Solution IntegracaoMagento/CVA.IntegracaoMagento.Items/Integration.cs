using CVA.IntegracaoMagento.Items.Controller;
using CVA.IntegracaoMagento.Items.Models.Magento;
using CVA.IntegracaoMagento.Items.Models.SAP;
using Flurl.Http;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.Items
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
                var items = new Metadata_Items.ItemSAP();

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
                var objItem = await SLConnection.GetAsync<List<Metadata_Items.Items>>("sml.svc/CVA_MAGENTO_ITEMS");

                string sWebsiteId = "1";
                if (objItem.Count > 0)
                {
                    foreach (var itemConfig in objConfig[0].CVA_CONFIG_MAG1Collection)
                    {
                        sWebsiteId += String.Format(@", {0}", itemConfig.U_WebsiteId);
                    }
                }

                //Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - sWebsiteId: {0}", sWebsiteId));

                foreach (var item in objItem)
                {
                    product.product = new Products.Product();
                    product.product.name = item.ItemName;
                    product.product.sku = item.ItemCode;
                    //product.product.visibility = 4;
                    product.product.type_id = "simple";
                    product.product.weight = item.Peso;
                    product.product.status = 1;

                    product.product.custom_attributes = new List<Products.Product.Custom_Attributes>();
                    product.product.custom_attributes.Add(new Products.Product.Custom_Attributes
                    {
                        tax_class_id_value = "0",
                        volume_lenght_value = item.Largura,
                        volume_height_value = item.Altura,
                        volume_width_value = item.Comprimento,
                        ean_value = item.CodeBars
                    }) ;

                    string sPrice = "0";
                    if (item.Price > 0)
                    {
                        sPrice = item.Price.ToString();
                        sPrice = sPrice.Replace(',', '.');
                    }
                                        
                    string sMensagemErro = String.Empty;
                    string sRetorno = item.U_CVA_Magento_Id;

                    if (string.IsNullOrEmpty(item.U_CVA_Magento_Id))
                        sRetorno = ItemsController.ItemsToMagentoAdd(token, product, sCaminho, sWebsiteId, ref sMensagemErro);
                    else
                        ItemsController.ItemsToMagentoUpdate(token, product, sCaminho, sWebsiteId, sPrice, ref sMensagemErro);

                    if (!String.IsNullOrEmpty(sMensagemErro))
                        Util.GravarLog(sCaminho, String.Format(@"[ERRO] - {0}", sMensagemErro));

                    DateTime horaAtual = DateTime.Now.AddSeconds(30);
                    items.U_CVA_Magento_Id = sRetorno;
                    items.U_CVA_Magento_Data = horaAtual;
                    items.U_CVA_Magento_Hora = Convert.ToInt32(String.Format(@"{0}{1}{2}", horaAtual.Hour.ToString(), horaAtual.Minute.ToString().PadLeft(2, '0'), horaAtual.Second.ToString().PadLeft(2, '0')));
                    items.U_CVA_Magento_Msg = sMensagemErro;

                    await SLConnection.PatchAsync($"Items('{item.ItemCode}')", items);
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
