using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace EscoteirosShip.Models.Magento
{
    public class OrderShip
    {
        [JsonProperty("order_id")]
        public string order_id { get; set; }

        public class ItemsResponse
        {
            [JsonProperty("message")]
            public IList<OrderShip> message { get; set; }
        }

        internal static string create_SHIPMENT(Token model, string entity_id)
        {
            try
            {
                string returns = "";
                var json = "";
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri("http://homolog.lojaescoteira.com.br/index.php/rest/V1/order/" + entity_id + "/ship");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.bearerTolken.Replace('"', ' ').Trim());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PostAsync(geturi, data).Result;
                if (message.IsSuccessStatusCode)
                {
                    returns = message.Content.ReadAsStringAsync().Result;
                }
                if (!message.IsSuccessStatusCode)
                {
                    string result = message.Content.ReadAsStringAsync().Result;
                    var definition = new { message = "" };
                    var ordershippment = JsonConvert.DeserializeAnonymousType(result, definition);
                    returns += ordershippment.message;
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
