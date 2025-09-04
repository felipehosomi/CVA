using CVA.IntegracaoMagento.StockItems.Models.Magento;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CVA.IntegracaoMagento.StockItems.Controller
{
    public class StockItemsController
    {
        internal static string update_STOCK(Token token, SourceItems sourceItems, ref string sMensagemErro)
        {
            sMensagemErro = String.Empty;

            try
            {
                string returns = "";
                var json = JsonConvert.SerializeObject(sourceItems);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/inventory/source-items");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoSecretId);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.bearerTolken.Replace('"', ' ').Trim());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PostAsync(geturi, data).Result;
                
                if (!message.IsSuccessStatusCode)
                    sMensagemErro = message.Content.ReadAsStringAsync().Result;

                return returns;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
