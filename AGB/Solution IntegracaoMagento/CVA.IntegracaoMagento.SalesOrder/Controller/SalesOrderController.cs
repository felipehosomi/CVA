using CVA.IntegracaoMagento.SalesOrder.Models.Magento;
using CVA.IntegracaoMagento.SalesOrder.Models.SAP;
using Newtonsoft.Json;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.SalesOrder.Controller
{
    public class SalesOrderController
    {

        #region [ SalesToSapEcommerce ]

        public static Sales.Main SalesToSap(Token token, string sOrderSpecific)
        {
            Sales.Main objSales = new Sales.Main();

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("username", token.username);
                client.DefaultRequestHeaders.Add("password", token.password);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoClientSecret);
                client.DefaultRequestHeaders.Add("Authorization", String.Format(@"Bearer {0}", token.bearerToken));
                string sURL = String.Empty;

                if (!String.IsNullOrEmpty(sOrderSpecific))
                {
                    sURL = String.Format(@"{0}/grb/sb/associacao-ecommerce/all/orders?searchCriteria[filterGroups][0][filters][0][field]=entity_id&searchCriteria[filterGroups][0][filters][0][value]={1}&searchCriteria[filterGroups][0][filters][0][conditionType]=eq", token.apiAddressUri, sOrderSpecific);

                    Uri geturi = new Uri(sURL);

                    HttpResponseMessage message = client.GetAsync(geturi).Result;
                    var retorno = message.Content.ReadAsStringAsync().Result;
                    objSales = JsonConvert.DeserializeObject<Sales.Main>(retorno);
                }
                else
                {
                    sURL = String.Format(@"{0}/grb/sb/associacao-ecommerce/all/orders?searchCriteria[filterGroups][0][filters][0][field]=status&searchCriteria[filterGroups][0][filters][0][conditionType]=in&searchCriteria[filterGroups][0][filters][0][value]=pending,complete,pedido_entregue&searchCriteria[filterGroups][2][filters][2][field]=pedido_sap&searchCriteria[filterGroups][2][filters][2][conditionType]=null", token.apiAddressUri);

                    Uri geturi = new Uri(sURL);
                    HttpResponseMessage message = client.GetAsync(geturi).Result;
                    var retorno = message.Content.ReadAsStringAsync().Result;
                    objSales = JsonConvert.DeserializeObject<Sales.Main>(retorno);
                }
                
                //var error = new { httpMessage = "" };
                //var errorDesc = JsonConvert.DeserializeAnonymousType(message.Content.ReadAsStringAsync().Result, error);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return objSales;
        }

        #endregion        

        #region [ UpdateSalesMagento ]

        public static async Task<string> UpdateSalesMagento(Token token, string sIdPedidoMagento, string sIdPedidoSAP, string sMensagem)
        {
            string sRetorno = String.Empty;
            var json = String.Empty;
            string sCaminho = String.Format(@"{0}Log", System.AppDomain.CurrentDomain.BaseDirectory.ToString());

            if (!(System.IO.Directory.Exists(sCaminho)))
                System.IO.Directory.CreateDirectory(sCaminho);

            sCaminho = String.Format(@"{0}\\Log_{1}.txt", sCaminho, String.Format(@"{0}{1}{2}", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0')));

            if (!(System.IO.File.Exists(sCaminho)))
                System.IO.File.Create(sCaminho).Close();

            try
            {
                
                Util.GravarLog(sCaminho, "Entrou no envio");


                if (String.IsNullOrEmpty(sIdPedidoSAP))
                {
                    json = "{\r\n    " +
                                "\"entity\":\r\n        " +
                                    "{\r\n            " +
                                        "\"entity_id\":\"" + sIdPedidoMagento + "\",\r\n    " +
                                        "\"mensagem_sap\":\"" + sMensagem + "\"\r\n    " +
                                    "}\r\n" +
                            "}";

                }
                else
                {
                    json = "{\r\n    " +
                                "\"entity\":\r\n        " +
                                    "{\r\n            " +
                                        "\"entity_id\":\"" + sIdPedidoMagento + "\",\r\n    " +
                                        "\"pedido_sap\":\"" + sIdPedidoSAP + "\",\r\n    " +
                                        "\"mensagem_sap\":\"" + sMensagem + "\",\r\n    " +
                                        "\"status\": \"separacao\"\r\n    " +
                                    "}\r\n" +
                            "}";
                }

                Util.GravarLog(sCaminho, $"Enviando Json para magento: (Parametros:,{token.apiAddressUri},{sIdPedidoMagento},{sIdPedidoSAP},{sMensagem})" + json);

                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/orders");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("username", token.username);
                client.DefaultRequestHeaders.Add("password", token.password);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoClientSecret);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.bearerToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var message = await client.PostAsync(geturi, data);
                if (!message.IsSuccessStatusCode)
                {
                    var error = new { message = "" };
                    var errorDesc = JsonConvert.DeserializeAnonymousType(message.Content.ReadAsStringAsync().Result, error);
                    sRetorno = errorDesc.message;
                }   
            }
            catch (Exception ex)
            {
                Util.GravarLog(sCaminho,ex.ToString());
                sRetorno = ex.Message;
            }

            return sRetorno;
        }

        #endregion

    }
}
