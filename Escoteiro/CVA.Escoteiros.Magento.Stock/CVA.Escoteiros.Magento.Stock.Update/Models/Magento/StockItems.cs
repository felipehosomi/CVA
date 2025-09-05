using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.IO;

namespace CVA.Escoteiros.Magento.Stock.Update.Models.Magento
{
    public class StockItems
    {
        [JsonProperty("sku")]
        public string sku { get; set; }

        [JsonProperty("qty")]
        public string qty { get; set; }

        internal static string update_STOCK(Token model, string sku, string quantity, bool is_in_stock)
        {
            try
            {
                string returns = "";
                var json = "";
                if (is_in_stock)
                {
                    json = "{\"stockItem\":{\"qty\":" + quantity + ", \"is_in_stock\" : true }}";
                }
                else
                {
                    json = "{\"stockItem\":{\"qty\":" + quantity + ",  \"is_in_stock\" : false}}";
                }
                Console.WriteLine("envio: "+json);

                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(model.apiAddressUri + "/rest/V1/products/" + sku + "/stockItems/1");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.bearerTolken.Replace('"', ' ').Trim());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.Timeout.Add(new TimeSpan(1));
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
            catch (Exception e)
            {
                //throw;
                return "Error:" + e.Message;
            }
        }
    }
}
