using CVA.IntegracaoMagento.PriceListSpecial.Models.Magento;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CVA.IntegracaoMagento.PriceListSpecial.Controller
{
    public class PriceListSpecialController
    {
        internal static string PriceListSpecialToMagentoUpdate(Token token, string sSKU, Products objProduct, ref string sMensagemErro)
        {
            sMensagemErro = String.Empty;

            try
            {
                string returns = "";                
                var json = "{\r\n  " +
                                "\"product\": {\r\n    " +
                                        "\"custom_attributes\": [\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"special_price\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].special_price_value.Replace(',','.') + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"special_from_date\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].special_from_date_value + "\"\r\n        " +
                                            "},\r\n    " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"special_to_date\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].special_to_date_value + "\"\r\n        " +
                                            "}\r\n        " +
                                        "]\r\n  " +
                                    "}\r\n" +
                                "}";

                //var json = JsonConvert.SerializeObject(objProduct);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/products/" + sSKU);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("username", token.username);
                client.DefaultRequestHeaders.Add("password", token.password);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoClientSecret);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.bearerToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PutAsync(geturi, data).Result;
                if (!message.IsSuccessStatusCode)
                {
                    var error = new { httpMessage = "" };
                    var errorDesc = JsonConvert.DeserializeAnonymousType(message.Content.ReadAsStringAsync().Result, error);
                    returns = String.Empty;
                    sMensagemErro = message.Content.ReadAsStringAsync().Result;
                }
                return returns;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
