using CVA.Escoteiro.Magento.Models.Magento;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;

namespace CVA.Escoteiro.Magento.Client
{
    public class ClientMagento
    {
        public static string Api = ConfigurationManager.AppSettings["ApiMagento"];
        public static string User = ConfigurationManager.AppSettings["ApiMagentoUser"];
        public static string Password = ConfigurationManager.AppSettings["ApiMagentoPassWord"];

        private RestClient Client { get; set; }
        private string Token { get; set; }

        public ClientMagento()
        {
            Client = new RestClient(Api);
            Token = GetAdminToken(User, Password);
        }

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

        #region Orders
        public OrdersListModel GetListOrdersCreate(int currentPage, int pageSize, DateTime data)
        {
            var urlRequest = "/rest/V1/orders";
            
            var filterDate = $"?searchCriteria[pageSize]={pageSize}&searchCriteria[currentPage]={currentPage}&searchCriteria[filter_groups][0][filters][0][field]=created_at&searchCriteria[filter_groups][0][filters][0][value]={data.ToString("yyyy-MM-dd HH:mm:ss")}&searchCriteria[filter_groups][0][filters][0][conditionType]=gt";
            var filterStatePeding = "&searchCriteria[filter_groups][1][filters][0][field]=status&searchCriteria[filter_groups][1][filters][0][value]=pending&searchCriteria[filter_groups][1][filters][0][condition_type]=eq";
            var filterStateProces = "&searchCriteria[filter_groups][1][filters][1][field]=status&searchCriteria[filter_groups][1][filters][1][value]=processing&searchCriteria[filter_groups][1][filters][1][condition_type]=eq";
            var endPoint = urlRequest + filterDate + filterStatePeding + filterStateProces;

            //#if DEBUG
            //            string filterTest = "?searchCriteria[filter_groups][1][filters][0][field]=entity_id&searchCriteria[filter_groups][1][filters][0][value]=86567&searchCriteria[filter_groups][1][filters][0][condition_type]=in";
            //            endPoint = urlRequest + filterTest;
            //#endif

            var request = CreateRequest(endPoint, Method.GET, Token);
            var response = Client.Execute(request);
            var orderList = new OrdersListModel();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                orderList = JsonConvert.DeserializeObject<OrdersListModel>(response.Content);

            return orderList;
        }

        public OrdersListModel GetListPendingOrdersCreate(string pendingOrders)
        {
            var urlRequest = "/rest/V1/orders";
            var filters = $"?searchCriteria[pageSize]=1&searchCriteria[filter_groups][1][filters][1][field]=entity_id&searchCriteria[filter_groups][1][filters][1][value]={pendingOrders}&searchCriteria[filter_groups][1][filters][1][condition_type]=in";

            var request = CreateRequest(String.Concat(urlRequest, filters), Method.GET, Token);

            var response = Client.Execute(request);
            OrdersListModel orderList = new OrdersListModel();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                orderList = JsonConvert.DeserializeObject<OrdersListModel>(response.Content);

            return orderList;
        }

        public OrdersListModel GetListOrdersUpdate(int currentPage, int pageSize, DateTime data)
        {
            var urlRequest = "/rest/V1/orders";
            var filterPage = $"?searchCriteria[pageSize]={pageSize}&searchCriteria[currentPage]={currentPage}";
            var filterCreateDate = "&searchCriteria[filter_groups][0][filters][0][field]=created_at&searchCriteria[filter_groups][0][filters][0][value]=2020-02-29 00:00:00&searchCriteria[filter_groups][0][filters][0][conditionType]=gt";
            var filterUpdateDate = $"&searchCriteria[filter_groups][1][filters][0][field]=updated_at&searchCriteria[filter_groups][1][filters][0][value]={data.ToString("yyyy-MM-dd HH:mm:ss")}&searchCriteria[filter_groups][1][filters][0][conditionType]=gt";
            var filterStateNew = "&searchCriteria[filter_groups][2][filters][0][field]=state&searchCriteria[filter_groups][2][filters][0][value]=new&searchCriteria[filter_groups][2][filters][0][condition_type]=eq";
            var filterStateProces = "&searchCriteria[filter_groups][2][filters][1][field]=state&searchCriteria[filter_groups][2][filters][1][value]=processing&searchCriteria[filter_groups][2][filters][1][condition_type]=eq";
            var filterStatusProces = "&searchCriteria[filter_groups][2][filters][2][field]=status&searchCriteria[filter_groups][2][filters][2][value]=processing&searchCriteria[filter_groups][2][filters][2][condition_type]=eq";
            var filterStateCanceled = "&searchCriteria[filter_groups][2][filters][3][field]=status&searchCriteria[filter_groups][2][filters][3][value]=canceled&searchCriteria[filter_groups][2][filters][3][condition_type]=eq";

            var endPoint = urlRequest + filterPage + filterCreateDate + filterUpdateDate + filterStateNew + filterStateProces + filterStatusProces + filterStateCanceled;

//#if DEBUG
//            string filterTest = "?searchCriteria[filter_groups][1][filters][0][field]=entity_id&searchCriteria[filter_groups][1][filters][0][value]=88791&searchCriteria[filter_groups][1][filters][0][condition_type]=in";
//            endPoint = urlRequest + filterTest;
//#endif

            var request = CreateRequest(endPoint, Method.GET, Token);
            var response = Client.Execute(request);
            var orderList = new OrdersListModel();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                orderList = JsonConvert.DeserializeObject<OrdersListModel>(response.Content);

            return orderList;
        }

        public bool CancelOrder(int entity_id)
        {
            var urlRequest = $"/rest/V1/orders/{entity_id}/cancel";
            var request = CreateRequest(urlRequest, Method.POST, Token);
            var response = Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // return JsonConvert.DeserializeObject<bool>(response.Content);
                return true;
            }

            return false;
        }

        public string GetCreditCardVoucher(int orderId)
        {
            string urlRequest = "/rest/V1/transactions";
            string parameters = $"?searchCriteria[currentPage]=1&searchCriteria[filterGroups][0][filters][0][conditionType]=eq&searchCriteria[filterGroups][0][filters][0][field]=order_id&searchCriteria[filterGroups][0][filters][0][value]={orderId}";

            var request = CreateRequest(urlRequest + parameters, Method.GET, GetAdminToken(User, Password));

            var response = Client.Execute(request);
            var transactions = new TransactionsModel();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                transactions = JsonConvert.DeserializeObject<TransactionsModel>(response.Content);

            if (transactions.items[0].additional_information[0].Contains("\"acquirer_nsu\":"))
            {
                var info = transactions.items[0].additional_information[0].Replace("\"", "").Replace("{", "").Replace("}", "").Split(',');
                var value = info[Array.FindIndex(info, row => row.Contains("acquirer_nsu"))].Split(':');
                return value[1];
            }

            return "";
        }

        public string GetOwnerIdNum(int orderId)
        {
            string urlRequest = "/rest/V1/transactions";
            string parameters = $"?searchCriteria[currentPage]=1&searchCriteria[filterGroups][0][filters][0][conditionType]=eq&searchCriteria[filterGroups][0][filters][0][field]=order_id&searchCriteria[filterGroups][0][filters][0][value]={orderId}";

            var request = CreateRequest(urlRequest + parameters, Method.GET, GetAdminToken(User, Password));

            var response = Client.Execute(request);
            var transactions = new TransactionsModel();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                transactions = JsonConvert.DeserializeObject<TransactionsModel>(response.Content);

            if (transactions.items[0].additional_information[0].Contains("\"acquirer_auth_code\":"))
            {
                var info = transactions.items[0].additional_information[0].Replace("\"", "").Replace("{", "").Replace("}", "").Split(',');
                var value = info[Array.FindIndex(info, row => row.Contains("acquirer_auth_code"))].Split(':');
                return value[1];
            }

            return "";
        }

        public bool HoldOrder(int entity_id)
        {
            var urlRequest = $"/rest/V1/orders/{entity_id}/hold";
            var request = CreateRequest(urlRequest, Method.POST, Token);
            var response = Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(response.Content);
            }

            return false;
        }

        public bool UnholdOrder(int entity_id)
        {
            var urlRequest = $"/rest/V1/orders/{entity_id}/unhold";
            var request = CreateRequest(urlRequest, Method.POST, Token);
            var response = Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(response.Content);
            }

            return false;
        }

        public bool SendOrderComments(int entity_id, string comments, string status)
        {
            var urlRequest = $"/rest/V1/orders/{entity_id}/comments";
            var request = CreateRequest(urlRequest, Method.POST, Token);

            var orderComment = new OrderComment();
            orderComment.statusHistory = new StatusHistory();
            orderComment.statusHistory.comment = comments;
            orderComment.statusHistory.status = status;
            orderComment.statusHistory.is_customer_notified = 1;
            orderComment.statusHistory.is_visible_on_front = 1;
            orderComment.statusHistory.parent_id = entity_id;
            orderComment.statusHistory.created_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string json = JsonConvert.SerializeObject(orderComment, Formatting.Indented);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(response.Content);
            }

            return false;
        }

        public void SendShipping(int entity_id, string comments, string status)
        {
            var urlRequest = $"/rest/V1/order/{entity_id}/ship";
            var request = CreateRequest(urlRequest, Method.POST, Token);

            var response = Client.Execute(request);
        }

        public Address2 GetAddress(int addressid)
        {
            var urlRequest = $"/rest/V1/customers/addresses/{addressid}";
            var request = CreateRequest(urlRequest, Method.GET, Token);
            var response = Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Address2>(response.Content);
            }

            return null;
        }

        public OrdersListModel GetOrder(int entityId)
        {
            var urlRequest = "/rest/V1/orders";
            var endPoint = urlRequest + $"?searchCriteria[filter_groups][1][filters][1][field]=increment_id&searchCriteria[filter_groups][1][filters][1][value]={entityId}&searchCriteria[filter_groups][1][filters][1][condition_type]=eq";
            var request = CreateRequest(endPoint, Method.GET, Token);
            var response = Client.Execute(request);
            var orderList = new OrdersListModel();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                orderList = JsonConvert.DeserializeObject<OrdersListModel>(response.Content);

            return orderList;
        }

        #endregion

        #region Products
        public Product SetProduct(ProductModel product)
        {
            var t = Newtonsoft.Json.JsonConvert.SerializeObject(product);

            var urlRequest = "/rest/V1/products";
            var request = CreateRequest(urlRequest, Method.POST, Token);
            request.AddJsonBody(product);
            var response = Client.Execute(request);
            var productModel = new Product();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                productModel = JsonConvert.DeserializeObject<Product>(response.Content);
            }
            else
            {
                throw new Exception($"Tentativa de inserção/atualização do item {product.product.sku}: {response.StatusDescription}");
            }

            return productModel;
        }

        public Product GetProduct(string sku)
        {
            var urlRequest = $"/rest/V1/products/{sku}";
            var request = CreateRequest(urlRequest, Method.GET, Token);
            var response = Client.Execute(request);
            var productModel = new Product();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                productModel = JsonConvert.DeserializeObject<Product>(response.Content);
            }
            else
            {
                //throw new Exception($"Tentativa de inserção/atualização do item {sku}: {response.StatusDescription}");
            }

            return productModel;
        }
        #endregion
    }
}
