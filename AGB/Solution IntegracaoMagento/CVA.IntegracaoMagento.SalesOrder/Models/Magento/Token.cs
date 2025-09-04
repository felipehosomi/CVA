using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CVA.IntegracaoMagento.SalesOrder.Models.Magento
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

        [JsonProperty("MagentoClientSecret")]
        public string MagentoClientSecret { get; set; }

        [JsonProperty("bearerTolken")]
        public string bearerToken { get; set; }

        internal static Token create_CN(Token token, ref string sMensagemErro)
        {
            sMensagemErro = String.Empty;

            try
            {
                var json = "";
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("username", token.username);
                client.DefaultRequestHeaders.Add("password", token.password);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoClientSecret);
                HttpResponseMessage message = client.PostAsync(geturi, data).Result;
                if (message.IsSuccessStatusCode)
                    token.bearerToken = message.Content.ReadAsStringAsync().Result;

                if (!message.IsSuccessStatusCode)
                    sMensagemErro = message.Content.ReadAsStringAsync().Result;

                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
