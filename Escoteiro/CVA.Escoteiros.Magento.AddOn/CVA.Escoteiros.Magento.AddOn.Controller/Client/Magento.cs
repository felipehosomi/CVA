using CVA.Escoteiros.Magento.AddOn.Controller.Client;
using CVA.Escoteiros.Magento.AddOn.Model.Magento;
using Newtonsoft.Json;
using RestSharp;

namespace CVA.Escoteiros.Magento.AddOn.Client
{
    public class ClientMagento
    {
        private RestClient Client { get; set; }
        private string Token { get; set; }

        public ClientMagento(string magentoUrl)
        {
            Client = new RestClient(magentoUrl);
        }

        public ClientMagento(string magentoUrl, string token)
        {
            Client = new RestClient(magentoUrl);
            Token = token;
        }

        public string GetAdminToken(string userName, string passWord)
        {
            var request = CreateRequest("/rest/V1/integration/admin/token", Method.POST);
            var user = new Credentials();
            user.username = userName;
            user.password = passWord;

            string json = JsonConvert.SerializeObject(user, Formatting.Indented);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = Client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var token = response.Content;
                return token.Substring(1, token.Length - 2);
            }
            else
            {
                return "";
            }
        }

        private RestRequest CreateRequest(string endPoint, Method method)
        {
            var request = new RestRequest(endPoint, method);
            request.RequestFormat = DataFormat.Json;
            request.ReadWriteTimeout = 30000;
            return request;
        }

        public CategoryModel GetListCategories(string token)
        {
            var urlRequest = "/rest/V1/categories";
            var request = CreateRequest(urlRequest, Method.GET, token);
            var response = Client.Execute(request);
            var categories = new CategoryModel();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                categories = JsonConvert.DeserializeObject<CategoryModel>(response.Content);

            return categories;
        }

        private RestRequest CreateRequest(string endPoint, Method method, string token)
        {
            var request = new RestRequest(endPoint, method);
            request.ReadWriteTimeout = 30000;
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Accept", "application/json");
            return request;
        }
    }

}
