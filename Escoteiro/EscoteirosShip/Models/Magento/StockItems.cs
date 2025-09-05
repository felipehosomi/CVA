using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EscoteirosShip.Models.Magento
{
    public class StockItems
    {
        [JsonProperty("sku")]
        public string sku { get; set; }

        [JsonProperty("qty")]
        public string qty { get; set; }

        internal static string update_STOCK(Token model, string sku)
        {
            try
            {
                string returns = "";
                var json = "{\"stockItem\":{\"qty\":" + "5000" + "}}";
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri("http://homolog.lojaescoteira.com.br/index.php/rest/V1/products/" + sku + "/stockItems/1");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.bearerTolken.Replace('"', ' ').Trim());
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
