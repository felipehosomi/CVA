using CVA.IntegracaoMagento.BusinessPartner.Models.Magento;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CVA.IntegracaoMagento.BusinessPartner.Controller
{
    public class BusinessPartnersController
    {
        internal static string create_CUSTOMER(Token token, Customers customer, ref string sMensagemErro)
        {
            sMensagemErro = String.Empty;

            try
            {
                string returns = "";
                var json = JsonConvert.SerializeObject(customer);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/customers");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoSecretId);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.bearerTolken.Replace('"', ' ').Trim());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PostAsync(geturi, data).Result;
                if (message.IsSuccessStatusCode)
                {
                    var definition = new { id = "" };
                    var customerId = JsonConvert.DeserializeAnonymousType(message.Content.ReadAsStringAsync().Result, definition);
                    returns = customerId.id;
                }
                if (!message.IsSuccessStatusCode)
                {
                    var error = new { httpMessage = "" };
                    var errorDesc = JsonConvert.DeserializeAnonymousType(message.Content.ReadAsStringAsync().Result, error);
                    returns = String.Empty;
                    sMensagemErro = message.Content.ReadAsStringAsync().Result;
                }
                return returns;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static string update_CUSTOMER(Token token, Customers customer)
        {
            try
            {
                string returns = "";
                var json = JsonConvert.SerializeObject(customer);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                Uri geturi = new Uri(token.apiAddressUri + "/grb/sb/associacao-ecommerce/all/customers/" + customer.customer.id);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", token.MagentoClientId);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", token.MagentoSecretId);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.bearerTolken.Replace('"', ' ').Trim());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PutAsync(geturi, data).Result;
                if (message.IsSuccessStatusCode)
                {
                    returns = "Cliente alterado";
                }
                if (!message.IsSuccessStatusCode)
                {
                    returns = "Erro: " + message.Content.ReadAsStringAsync().Result;
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
