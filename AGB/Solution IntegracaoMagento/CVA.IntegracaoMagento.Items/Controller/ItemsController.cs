using CVA.IntegracaoMagento.Items.Models.Magento;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace CVA.IntegracaoMagento.Items.Controller
{
    public class ItemsController
    {
        internal static string ItemsToMagentoAdd(Token token, Products objProduct, string sCaminho, string sWebsiteId, ref string sMensagemErro)
        {
            sMensagemErro = String.Empty;

            try
            {
                string returns = "";

                string sItemUrl = String.Format(@"{0} {1}", objProduct.product.sku, objProduct.product.name); //"01204 REF QUASAR DES BDY SPR ESPLH 100 ML 3,8";
                sItemUrl = sItemUrl.ToLower();
                string sURLFinal = Regex.Replace(sItemUrl, @"[^0-9a-zA-Z]+", "-");

                var json = "{\r\n  " +
                                "\"product\": {\r\n    " +
                                        "\"attribute_set_id\":\"" + objProduct.product.attribute_set_id + "\",\r\n    " +
                                        "\"name\": \"" + objProduct.product.name + "\",\r\n    " +
                                        "\"sku\": \"" + objProduct.product.sku + "\",\r\n    " +
                                        "\"visibility\": \"" + objProduct.product.visibility + "\",\r\n    " +
                                        "\"Price\": 0,\r\n    " +
                                        "\"type_id\":\"" + objProduct.product.type_id + "\",\r\n    " +
                                        "\"weight\": \"" + objProduct.product.weight + "\",\r\n    " +
                                        "\"status\": \"" + objProduct.product.status + "\",\r\n    " +
                                        "\"extension_attributes\": { \"website_ids\": [ " + sWebsiteId + " ] },\r\n    " +
                                        "\"custom_attributes\": [\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"tax_class_id\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].tax_class_id_value + "\"\r\n        " +
                                            "},\r\n    " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"url_key\",\r\n            " +
                                                "\"value\": \"" + sURLFinal + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"volume_length\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].volume_lenght_value + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                            "   \"attribute_code\": \"volume_height\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].volume_height_value + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"volume_width\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].volume_width_value + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"ean\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].ean_value + "\"\r\n        " +
                                            "}\r\n        " +
                                        "]\r\n  " +
                                    "}\r\n" +
                                "}";

                //var json = JsonConvert.SerializeObject(objProduct);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/products");

                Util.GravarLog(sCaminho, String.Format(@"[SKU] - New: {0}", objProduct.product.sku));
                //Util.GravarLog(sCaminho, String.Format(@"[URL] - {0}", geturi.AbsoluteUri));
                //Util.GravarLog(sCaminho, String.Format(@"[JSON] - {0}", json));

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("username", token.username);
                client.DefaultRequestHeaders.Add("password", token.password);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoClientSecret);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.bearerToken); //token.bearerToken.Replace('"', ' ').Trim()
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PostAsync(geturi, data).Result;
                if (message.IsSuccessStatusCode)
                {
                    var definition = new { id = "" };
                    var customerId = JsonConvert.DeserializeAnonymousType(message.Content.ReadAsStringAsync().Result, definition);
                    Console.WriteLine(customerId.id);
                    returns = customerId.id;
                }
                if (!message.IsSuccessStatusCode)
                {
                    var error = new { httpMessage = "" };
                    var errorDesc = JsonConvert.DeserializeAnonymousType(message.Content.ReadAsStringAsync().Result, error);
                    returns = String.Empty;
                    sMensagemErro = message.Content.ReadAsStringAsync().Result;
                }
                return returns;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal static string ItemsToMagentoUpdate(Token token, Products objProduct, string sCaminho, string sWebsiteId, string sPrice, ref string sMensagemErro)
        {
            sMensagemErro = String.Empty;

            try
            {
                string returns = "";

                string sItemUrl = String.Format(@"{0} {1}", objProduct.product.sku, objProduct.product.name); //"01204 REF QUASAR DES BDY SPR ESPLH 100 ML 3,8";
                sItemUrl = sItemUrl.ToLower();
                string sURLFinal = Regex.Replace(sItemUrl, @"[^0-9a-zA-Z]+", "-");

                /* Luis (09.03.2021) - Aguardando autorização para liberação
                var json = "{\r\n  " +
                                "\"product\": {\r\n    " +
                                        "\"attribute_set_id\":\"" + objProduct.product.attribute_set_id + "\",\r\n    " +
                                        "\"sku\": \"" + objProduct.product.sku + "\",\r\n    " +
                                        "\"visibility\": \"" + objProduct.product.visibility + "\",\r\n    " +
                                        "\"Price\": " + sPrice + ",\r\n    " +
                                        "\"type_id\":\"" + objProduct.product.type_id + "\",\r\n    " +
                                        "\"weight\": \"" + objProduct.product.weight + "\",\r\n    " +
                                        "\"status\": \"" + objProduct.product.status + "\",\r\n    " +
                                        "\"extension_attributes\": { \"website_ids\": [ " + sWebsiteId + " ] },\r\n    " +
                                        "\"custom_attributes\": [\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"tax_class_id\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].tax_class_id_value + "\"\r\n        " +
                                            "},\r\n    " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"volume_length\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].volume_lenght_value + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                            "   \"attribute_code\": \"volume_height\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].volume_height_value + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"volume_width\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].volume_width_value + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"ean\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].ean_value + "\"\r\n        " +
                                            "}\r\n        " +
                                        "]\r\n  " +
                                    "}\r\n" +
                                "}";
                */

                var json = "{\r\n  " +
                                "\"product\": {\r\n    " +
                                        "\"attribute_set_id\":\"" + objProduct.product.attribute_set_id + "\",\r\n    " +
                                        "\"name\": \"" + objProduct.product.name + "\",\r\n    " +
                                        "\"sku\": \"" + objProduct.product.sku + "\",\r\n    " +
                                        "\"visibility\": \"" + objProduct.product.visibility + "\",\r\n    " +
                                        "\"Price\": " + sPrice + ",\r\n    " +
                                        "\"type_id\":\"" + objProduct.product.type_id + "\",\r\n    " +
                                        "\"weight\": \"" + objProduct.product.weight + "\",\r\n    " +
                                        "\"status\": \"" + objProduct.product.status + "\",\r\n    " +
                                        "\"extension_attributes\": { \"website_ids\": [ " + sWebsiteId + " ] },\r\n    " +
                                        "\"custom_attributes\": [\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"tax_class_id\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].tax_class_id_value + "\"\r\n        " +
                                            "},\r\n    " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"url_key\",\r\n            " +
                                                "\"value\": \"" + sURLFinal + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"volume_length\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].volume_lenght_value + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                            "   \"attribute_code\": \"volume_height\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].volume_height_value + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"volume_width\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].volume_width_value + "\"\r\n        " +
                                            "},\r\n        " +
                                            "{\r\n            " +
                                                "\"attribute_code\": \"ean\",\r\n            " +
                                                "\"value\": \"" + objProduct.product.custom_attributes[0].ean_value + "\"\r\n        " +
                                            "}\r\n        " +
                                        "]\r\n  " +
                                    "}\r\n" +
                                "}";

                //var json = JsonConvert.SerializeObject(objProduct);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/products/" + objProduct.product.sku);

                Util.GravarLog(sCaminho, String.Format(@"[SKU] - Update: {0}", objProduct.product.sku));
                //Util.GravarLog(sCaminho, String.Format(@"[URL] - {0}", geturi.AbsoluteUri));
                //Util.GravarLog(sCaminho, String.Format(@"[JSON] - {0}", json));

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
