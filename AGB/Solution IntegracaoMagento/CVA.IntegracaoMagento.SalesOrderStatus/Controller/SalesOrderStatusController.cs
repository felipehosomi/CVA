using CVA.IntegracaoMagento.SalesOrderStatus.Models.Magento;
using CVA.IntegracaoMagento.SalesOrderStatus.Models.SAP;
using Newtonsoft.Json;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.SalesOrderStatus.Controller
{
    public class SalesOrderStatusController
    {

        #region [ SalesStatusDespachadoToMagento ]

        internal static string SalesStatusDespachadoToMagento(Token token, string sPedidoMagento, string sCaminho, SalesStatusDespachado.Item oItem, ref string sMensagemErro)
        {
            sMensagemErro = String.Empty;
            string returns = "";

            try
            {
                /*
                var json = "{\r\n    " +
                                "\"entity\":\r\n        " +
                                    "{\r\n            " +
                                        "\"entity_id\": \"" + sPedidoMagento + "\",\r\n    " +
                                        "\"state\": \"complete\",\r\n    " +
                                        "\"status\": \"pedido_entregue\"\r\n    " +
                                    "}\r\n" +
                             "}";
                */

                var json = JsonConvert.SerializeObject(oItem);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/order/" + sPedidoMagento + "/ship");
                //Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/orders");

                Util.GravarLog(sCaminho, String.Format(@"[URL] - {0}", geturi.AbsoluteUri));
                //Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - ITEM: {0}", oItem));
                Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - JSON: {0}", json));

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("username", token.username);
                client.DefaultRequestHeaders.Add("password", token.password);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoClientSecret);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.bearerToken); //token.bearerToken.Replace('"', ' ').Trim()
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PostAsync(geturi, data).Result;
                if (!message.IsSuccessStatusCode)
                {
                    var error = new { httpMessage = "" };
                    var errorDesc = JsonConvert.DeserializeAnonymousType(message.Content.ReadAsStringAsync().Result, error);
                    returns = String.Empty;
                    sMensagemErro = message.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                sMensagemErro = ex.Message;
                Util.GravarLog(sCaminho, String.Format(@"[ERRO] - {0}", ex));
            }

            return returns;
        }

        #endregion

        #region [ SalesStatusToMagento ]

        public static string SalesStatusToMagento(Token token, string sPedidoMagento, string sStatus, string sCaminho, ref string sMensagemErro)
        {
            sMensagemErro = String.Empty;
            string returns = "";

            try
            {
                var json = "{\r\n    " +
                                "\"entity\":\r\n        " +
                                    "{\r\n            " +
                                        "\"entity_id\": \"" + sPedidoMagento + "\",\r\n    " +
                                        "\"status\": \"" + sStatus + "\"\r\n    " +
                                    "}\r\n" +
                             "}";

                //var json = JsonConvert.SerializeObject(oItem);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/orders");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("username", token.username);
                client.DefaultRequestHeaders.Add("password", token.password);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoClientSecret);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.bearerToken); //token.bearerToken.Replace('"', ' ').Trim()
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PostAsync(geturi, data).Result;
                if (!message.IsSuccessStatusCode)
                {
                    var error = new { httpMessage = "" };
                    var errorDesc = JsonConvert.DeserializeAnonymousType(message.Content.ReadAsStringAsync().Result, error);
                    returns = String.Empty;
                    sMensagemErro = message.Content.ReadAsStringAsync().Result;
                }                
            }
            catch (Exception ex)
            {
                sMensagemErro = ex.Message;
                Util.GravarLog(sCaminho, String.Format(@"[ERRO] - {0}", ex));
            }

            return returns;
        }

        #endregion

    }
}
