using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Escoteiros.Magento.Stock.Read.Models.Magento
{
    public class StockItems
    {
        [JsonProperty("sku")]
        public string sku { get; set; }

        [JsonProperty("qty")]
        public int? qty { get; set; }

        internal static string update_STOCK(Token model, string sku, int quantity)
        {
            try
            {
                string returns = "";
                var json = "{\"stockItem\":{\"qty\":" + quantity + "}}";
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(model.apiAddressUri + "/rest/V1/products/" + sku + "/stockItems/1");
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

        internal static async Task<StockItems> read_STOCK(Token model, string product_id)
        {
            try
            {
                StockItems item = new StockItems();
                Uri geturi = new Uri(model.apiAddressUri + "/rest/V1/stockItems/" + product_id);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.bearerTolken.Replace('"', ' ').Trim());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.GetAsync(geturi).Result;
                if (message.IsSuccessStatusCode)
                {
                    string result = message.Content.ReadAsStringAsync().Result;
                    item = JsonConvert.DeserializeObject<StockItems>(result);
                }
                if (!message.IsSuccessStatusCode)
                {
                    string result = message.Content.ReadAsStringAsync().Result;
                    var definition = new { message = "" };
                    var ordershippment = JsonConvert.DeserializeAnonymousType(result, definition);
                    item.qty = 0;
                }
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
