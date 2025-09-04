using CVA.IntegracaoMagento.Stock.Models.Magento;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CVA.IntegracaoMagento.Stock.Controller
{
    public class StockController
    {
        internal static string update_STOCK(Token token, string sku, double quantity)
        {
            try
            {
                string returns = "";
                var json = "{\"stockItem\":{\"qty\":" + quantity + "}}";
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/products/" + sku + "/stockItems/1");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoSecretId);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.bearerTolken.Replace('"', ' ').Trim());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PutAsync(geturi, data).Result;
                if (message.IsSuccessStatusCode)
                {
                    returns = message.Content.ReadAsStringAsync().Result;
                }
                if (!message.IsSuccessStatusCode)
                {
                    returns = message.Content.ReadAsStringAsync().Result;
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
