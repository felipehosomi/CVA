using EscoteirosShip.Models.Magento;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EscoteirosStock.models.Magento
{
    public class stockStatuses
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

            [JsonProperty("is_in_stock")]
            public bool is_in_stock { get; set; }

            [JsonProperty("is_qty_decimal")]
            public bool is_qty_decimal { get; set; }

            [JsonProperty("show_default_notification_message")]
            public bool show_default_notification_message { get; set; }

            [JsonProperty("use_config_min_qty")]
            public bool use_config_min_qty { get; set; }

            [JsonProperty("min_qty")]
            public int? min_qty { get; set; }

            [JsonProperty("use_config_min_sale_qty")]
            public int? use_config_min_sale_qty { get; set; }

            [JsonProperty("min_sale_qty")]
            public int min_sale_qty { get; set; }

            [JsonProperty("use_config_max_sale_qty")]
            public bool use_config_max_sale_qty { get; set; }

            [JsonProperty("max_sale_qty")]
            public int? max_sale_qty { get; set; }

            [JsonProperty("use_config_backorders")]
            public bool use_config_backorders { get; set; }

            [JsonProperty("backorders")]
            public int? backorders { get; set; }

            [JsonProperty("use_config_notify_stock_qty")]
            public bool use_config_notify_stock_qty { get; set; }

            [JsonProperty("notify_stock_qty")]
            public int? notify_stock_qty { get; set; }

            [JsonProperty("use_config_qty_increments")]
            public bool use_config_qty_increments { get; set; }

            [JsonProperty("qty_increments")]
            public int? qty_increments { get; set; }

            [JsonProperty("use_config_enable_qty_inc")]
            public bool use_config_enable_qty_inc { get; set; }

            [JsonProperty("enable_qty_increments")]
            public bool enable_qty_increments { get; set; }

            [JsonProperty("use_config_manage_stock")]
            public bool use_config_manage_stock { get; set; }

            [JsonProperty("manage_stock")]
            public bool manage_stock { get; set; }

            [JsonProperty("low_stock_date")]
            public object low_stock_date { get; set; }

            [JsonProperty("is_decimal_divided")]
            public bool is_decimal_divided { get; set; }

            [JsonProperty("stock_status_changed_auto")]
            public int? stock_status_changed_auto { get; set; }
        }

        public class Example
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

        internal static async Task<Example> read_STOCK(Token model, string product_id)
        {
            try
            {
                Example item = new Example();
                Uri geturi = new Uri("http://homolog.lojaescoteira.com.br/index.php/rest/V1/stockStatuses/" + product_id);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.bearerTolken.Replace('"', ' ').Trim());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.GetAsync(geturi).Result;
                if (message.IsSuccessStatusCode)
                {
                    string result = message.Content.ReadAsStringAsync().Result;
                    item = JsonConvert.DeserializeObject<Example>(result);
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
