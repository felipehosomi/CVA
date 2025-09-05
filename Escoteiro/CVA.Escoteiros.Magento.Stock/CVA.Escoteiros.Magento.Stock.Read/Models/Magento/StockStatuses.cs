using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Escoteiros.Magento.Stock.Read.Models.Magento
{
    public class StockStatuses
    {
        public class StockItem
        {
            [JsonProperty("item_id")]
            public int? item_id { get; set; }

            [JsonProperty("product_id")]
            public string product_id { get; set; }

            [JsonProperty("stock_id")]
            public string stock_id { get; set; }

            [JsonProperty("qty")]
            public int? qty { get; set; }

            [JsonProperty("min_qty")]
            public int? min_qty { get; set; }
        }

        public class ItemResponse
        {

            [JsonProperty("product_id")]
            public string product_id { get; set; }

            [JsonProperty("stock_id")]
            public string stock_id { get; set; }

            [JsonProperty("qty")]
            public int? qty { get; set; }

            [JsonProperty("stock_status")]
            public int? stock_status { get; set; }

            [JsonProperty("stock_item")]
            public StockItem stock_item { get; set; }
        }

        internal static async Task<ItemResponse> read_STOCK(Token model, string product_id)
        {
            try
            {
                ItemResponse item = new ItemResponse();
                Uri geturi = new Uri(model.apiAddressUri + "/rest/V1/stockStatuses/" + product_id);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.bearerTolken.Replace('"', ' ').Trim());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.GetAsync(geturi).Result;
                if (message.IsSuccessStatusCode)
                {
                    string result = message.Content.ReadAsStringAsync().Result;
                    item = JsonConvert.DeserializeObject<ItemResponse>(result);
                }
                if (!message.IsSuccessStatusCode)
                {
                    string result = message.Content.ReadAsStringAsync().Result;
                    var definition = new { message = "" };
                    var ordershippment = JsonConvert.DeserializeAnonymousType(result, definition);
                    item.product_id = ordershippment.message.ToString();
                    item.qty = 0;
                    item.stock_item = new StockItem();
                    item.stock_item.item_id = 0;
                    item.stock_item.product_id = "";
                    item.stock_item.stock_id = "1";
                    item.stock_item.qty = 0;
                    item.stock_item.min_qty = 0;
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
