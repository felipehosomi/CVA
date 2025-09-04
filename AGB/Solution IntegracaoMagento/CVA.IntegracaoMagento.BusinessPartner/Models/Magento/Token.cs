using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace CVA.IntegracaoMagento.BusinessPartner.Models.Magento
{
    public class Token
    {
        [JsonProperty("apiAddressUri")]
        public string apiAddressUri { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("MagentoClientId")]
        public string MagentoClientId { get; set; }

        [JsonProperty("MagentoSecretId")]
        public string MagentoSecretId { get; set; }

        [JsonProperty("bearerTolken")]
        public string bearerTolken { get; set; }

        internal static Token create_CN(Token token)
        {
            try
            {
                var json = "";
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoSecretId);
                client.DefaultRequestHeaders.Add("username", token.username);
                client.DefaultRequestHeaders.Add("password", token.password);
                HttpResponseMessage message = client.PostAsync(geturi, data).Result;
                if (message.IsSuccessStatusCode)
                {
                    token.bearerTolken = message.Content.ReadAsStringAsync().Result;
                }
                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
