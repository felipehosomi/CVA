using CVA.Escoteiro.Magento.Models.Magento;
using Newtonsoft.Json;
using RestSharp;

namespace CVA.Escoteiro.Magento.StockManager.Client
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

        public string GetAdminToken(string userName, string password)
        {
            var request = CreateRequest("/rest/V1/integration/admin/token", Method.POST);
            var user = new Credentials() { username = userName, password = password };
            var json = JsonConvert.SerializeObject(user, Formatting.Indented);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var token = response.Content;
                return token.Substring(1, token.Length - 2);
            }

            return "";
        }

        private RestRequest CreateRequest(string endPoint, Method method)
        {
            var request = new RestRequest(endPoint, method);

            request.RequestFormat = DataFormat.Json;

            return request;
        }

        private RestRequest CreateRequest(string endPoint, Method method, string token)
        {
            var request = new RestRequest(endPoint, method);

            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Accept", "application/json");

            return request;
        }

        public string SetProductStockQuantity(string token, string sku, double whsQty)
        {
            var urlRequest = $"/rest/V1/products/{sku}/stockItems/1";
            var request = CreateRequest(urlRequest, Method.PUT, token);
            request.AddParameter("application/json", "{\"stockItem\":{\"qty\":" + whsQty + "}}", ParameterType.RequestBody);

            var response = Client.Execute(request);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return $"A quantidade em estoque do produto de sku {sku} foi atualizado para {whsQty}.";

                case System.Net.HttpStatusCode.NotFound:
                    return $"Não foi encontrado nenhum produto com o sku {sku}.";

                default:
                    throw new System.Exception(response.ErrorException.ToString());
            }
        }

        public ProductsGetModel GetProductCategories(string token, string sku)
        {
            var urlRequest = $"/rest/V1/products";
            var parameters = $"?searchCriteria[filter_groups][1][filters][1][field]=sku&searchCriteria[filter_groups][1][filters][1][value]={sku}&searchCriteria[filter_groups][1][filters][1][condition_type]=eq";
            var request = CreateRequest(urlRequest + parameters, Method.GET, token);
            var response = Client.Execute(request);
            var product = new ProductsGetModel();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                product = JsonConvert.DeserializeObject<ProductsGetModel>(response.Content);
            }

            return product;
        }

        public string GetCategoryName(string token, int categoryID)
        {
            var urlRequest = $"/rest/V1/categories/{categoryID}";
            var request = CreateRequest(urlRequest, Method.GET, token);
            var response = Client.Execute(request);
            var category = new Category();
            var categoryName = string.Empty;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                category = JsonConvert.DeserializeObject<Category>(response.Content);
                categoryName = category.Name;
            }

            return categoryName;
        }
    }
}
