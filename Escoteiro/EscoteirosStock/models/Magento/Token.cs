using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EscoteirosShip.Models.Magento
{
    public class Token
    {
        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("bearerTolken")]
        public string bearerTolken { get; set; }


        internal static Token create_CN(Token model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri("http://homolog.lojaescoteira.com.br/index.php/rest/V1/integration/admin/token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PostAsync(geturi, data).Result;
                if (message.IsSuccessStatusCode)
                {
                    model.bearerTolken = message.Content.ReadAsStringAsync().Result;
                }
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
