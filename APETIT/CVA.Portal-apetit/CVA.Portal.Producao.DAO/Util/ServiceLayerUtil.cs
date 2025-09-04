using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Configuracoes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.DAO.Util
{
    public class ServiceLayerUtil
    {
        private string ApiUrl;
        private static string SessionId;
        private DateTime lastLoginDateTime = DateTime.MinValue;
        private readonly HttpStatusCode[] returnCodesToRetry = new[]
        {
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout
        };

        public ServiceLayerUtil()
        {
            ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceLayerURL"];

            if (ApiUrl.EndsWith("/"))
            {
                ApiUrl = ApiUrl.Substring(0, ApiUrl.Length - 1);
            }

            ServicePointManager.ServerCertificateValidationCallback = new
            RemoteCertificateValidationCallback
            (
               delegate { return true; }
            );
        }

        public async Task<string> Login()
        {
            if (DateTime.Now.Subtract(lastLoginDateTime).TotalMinutes < 30)
                return "";

            LoginModel loginModel = new LoginModel();
            loginModel.CompanyDB = System.Configuration.ConfigurationManager.AppSettings["Database"];
            loginModel.UserName = System.Configuration.ConfigurationManager.AppSettings["B1User"];
            loginModel.Password = System.Configuration.ConfigurationManager.AppSettings["B1Password"];
            loginModel.Language = 29;

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.ExpectContinue = false;

                var json = JsonConvert.SerializeObject(loginModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(string.Format(ApiUrl + "/Login"), content);

                if (response.IsSuccessStatusCode)
                {
                    var rootobject = JsonConvert.DeserializeObject<SessionModel>(await response.Content.ReadAsStringAsync());

                    SessionId = rootobject.SessionId;
                    lastLoginDateTime = DateTime.Now;

                    return "";
                }
                else
                {
                    throw new Exception("Erro ao efetuar login");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> Logout()
        {
            Uri uri = new Uri(string.Format(ApiUrl + "/Logout"));
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(uri, new Cookie("B1SESSION", SessionId));
            HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.ExpectContinue = false;

            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var rootobject = JsonConvert.DeserializeObject<LoginModel>(await response.Content.ReadAsStringAsync());
                SessionId = "";
                return "";
            }
        }

        public async Task<T> GetByIDAsync<T>(string controller, string id) where T : class
        {
            string url = string.Format(ApiUrl + "/{0}({1})", controller, id);
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Uri(url), new Cookie("B1SESSION", SessionId));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.ExpectContinue = false;

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync(url);

            var JsonResult = response.Content.ReadAsStringAsync().Result;
            var rootobject = JsonConvert.DeserializeObject<T>(JsonResult);

            return rootobject;
        }

        public async Task<T> GetAsync<T>(string controller, string param, string paramSeparator = "/") where T : class
        {
            string url = string.Format(ApiUrl + "/{0}{1}{2}", controller, paramSeparator, param);
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Uri(url), new Cookie("B1SESSION", SessionId));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.ExpectContinue = false;

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync(url);

            var JsonResult = response.Content.ReadAsStringAsync().Result;
            var rootobject = JsonConvert.DeserializeObject<T>(JsonResult);

            return rootobject;
        }

        public async Task<List<T>> GetListAsync<T>(string controller, string param = "") where T : class
        {
            try
            {
                string url = string.Format(ApiUrl + "/{0}?type=json&{1}", controller, param);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(new Uri(url), new Cookie("B1SESSION", SessionId));

                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.ExpectContinue = false;

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url);

                var JsonResult = response.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<List<T>>(JsonResult);

                return rootobject;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> PatchAsync<T>(string controller, string param, T item) where T : class
        {
            Uri uri = new Uri(string.Format(ApiUrl + "/{0}({1})", controller, param));

            HttpClientHandler handler = new HttpClientHandler() { UseCookies = false };
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.ExpectContinue = false;

            var json = JsonConvert.SerializeObject(item, Formatting.None,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            HttpResponseMessage response = new HttpResponseMessage();

            for (int numberOfAttempts = 3; numberOfAttempts > 0; numberOfAttempts--)
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var method = new HttpMethod("PATCH");
                var request = new HttpRequestMessage(method, uri) { Content = content };
                request.Headers.Add("Cookie", "B1SESSION=" + SessionId);
                request.Headers.Add("Prefer", "return-no-content");

                response = await client.SendAsync(request);

                if (!returnCodesToRetry.Contains(response.StatusCode))
                {
                    break;
                }
                else
                {
                    await Task.Delay(200);
                }
            }

            if (response.IsSuccessStatusCode)
            {
                return String.Empty;
            }
            else
            {
                try
                {
                    JsonModel jsonModel = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());
                    if (!String.IsNullOrEmpty(jsonModel.ExceptionMessage))
                    {
                        return jsonModel.ExceptionMessage;
                    }
                    SBOErrorModel sboErrorModel = JsonConvert.DeserializeObject<SBOErrorModel>(await response.Content.ReadAsStringAsync());
                    if (sboErrorModel.error != null)
                    {
                        return sboErrorModel.error.code + " - " + sboErrorModel.error.message.value;
                    }
                }
                catch (Exception ex)
                {
                    await response.Content.ReadAsStringAsync();
                }
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> PostAsync<T>(string controller, T item) where T : class
        {
            try
            {
                Uri uri = new Uri(string.Format(ApiUrl + "/{0}", controller));

                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(uri, new Cookie("B1SESSION", SessionId));
                HttpClient client = new HttpClient(handler);

                var json = JsonHelper.SerializeToMinimalJson(item, false);

                HttpResponseMessage response = new HttpResponseMessage();

                for (int numberOfAttempts = 3; numberOfAttempts > 0; numberOfAttempts--)
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    response = await client.PostAsync(uri, content);

                    if (!returnCodesToRetry.Contains(response.StatusCode))
                    {
                        break;
                    }
                    else
                    {
                        await Task.Delay(200);
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    return "";
                }
                else
                {
                    try
                    {
                        JsonModel jsonModel = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());
                        if (!String.IsNullOrEmpty(jsonModel.ExceptionMessage))
                        {
                            return jsonModel.ExceptionMessage;
                        }
                        SBOErrorModel sboErrorModel = JsonConvert.DeserializeObject<SBOErrorModel>(await response.Content.ReadAsStringAsync());
                        if (sboErrorModel.error != null)
                        {
                            return sboErrorModel.error.code + " - " + sboErrorModel.error.message.value;
                        }
                    }
                    catch (Exception ex)
                    {
                        await response.Content.ReadAsStringAsync();
                    }
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> PostNormalAsync<T>(string controller, T item) where T : class
        {
            try
            {
                Uri uri = new Uri(string.Format(ApiUrl + "/{0}", controller));

                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(uri, new Cookie("B1SESSION", SessionId));
                HttpClient client = new HttpClient(handler);

                var json = JsonConvert.SerializeObject(item, Formatting.None,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    return "";
                }
                else
                {
                    try
                    {
                        JsonModel jsonModel = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());
                        if (!String.IsNullOrEmpty(jsonModel.ExceptionMessage))
                        {
                            return jsonModel.ExceptionMessage;
                        }
                        SBOErrorModel sboErrorModel = JsonConvert.DeserializeObject<SBOErrorModel>(await response.Content.ReadAsStringAsync());
                        if (sboErrorModel.error != null)
                        {
                            return sboErrorModel.error.code + " - " + sboErrorModel.error.message.value;
                        }
                    }
                    catch (Exception ex)
                    {
                        await response.Content.ReadAsStringAsync();
                    }
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<Tuple<int, string>> PostAndReturnEntryAsync<T>(string controller, T item) where T : class
        {
            try
            {
                Uri uri = new Uri(string.Format(ApiUrl + "/{0}", controller));

                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(uri, new Cookie("B1SESSION", SessionId));
                HttpClient client = new HttpClient(handler);

                var json = JsonHelper.SerializeToMinimalJson(item, false); 

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var obj = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), new { DocEntry = 0 });
                    return new Tuple<int, string>(obj.DocEntry, string.Empty);
                }
                else
                {
                    try
                    {
                        JsonModel jsonModel = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());
                        if (!String.IsNullOrEmpty(jsonModel.ExceptionMessage))
                        {
                            return new Tuple<int, string>(0, jsonModel.ExceptionMessage);
                        }
                        SBOErrorModel sboErrorModel = JsonConvert.DeserializeObject<SBOErrorModel>(await response.Content.ReadAsStringAsync());
                        if (sboErrorModel.error != null)
                        {
                            return new Tuple<int, string>(0, sboErrorModel.error.code + " - " + sboErrorModel.error.message.value);
                        }
                    }
                    catch (Exception ex)
                    {
                        await response.Content.ReadAsStringAsync();
                    }
                    return new Tuple<int, string>(0, await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, ex.Message);
            }
        }

        public async Task<Tuple<int, string>> PostAndReturnAbsoluteEntryAsync<T>(string controller, T item) where T : class
        {
            try
            {
                Uri uri = new Uri(string.Format(ApiUrl + "/{0}", controller));

                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(uri, new Cookie("B1SESSION", SessionId));
                HttpClient client = new HttpClient(handler);

                var json = JsonHelper.SerializeToMinimalJson(item, false);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var obj = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), new { AbsoluteEntry = 0 });
                    return new Tuple<int, string>(obj.AbsoluteEntry, string.Empty);
                }
                else
                {
                    try
                    {
                        JsonModel jsonModel = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());
                        if (!String.IsNullOrEmpty(jsonModel.ExceptionMessage))
                        {
                            return new Tuple<int, string>(0, jsonModel.ExceptionMessage);
                        }
                        SBOErrorModel sboErrorModel = JsonConvert.DeserializeObject<SBOErrorModel>(await response.Content.ReadAsStringAsync());
                        if (sboErrorModel.error != null)
                        {
                            return new Tuple<int, string>(0, sboErrorModel.error.code + " - " + sboErrorModel.error.message.value);
                        }
                    }
                    catch (Exception ex)
                    {
                        await response.Content.ReadAsStringAsync();
                    }
                    return new Tuple<int, string>(0, await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, ex.Message);
            }
        }

        public async Task<string> DeleteAsync(string controller, string param)
        {
            string url = string.Format(ApiUrl + "/{0}/{1}", controller, param);

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("B1SESSION", SessionId));
            HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var rootobject = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());
                return rootobject.ExceptionMessage;
            }
        }

        public async Task<string> PostBatchAsync(Dictionary<string, object> content)
        {
            throw new NotImplementedException();
        }

    }
}
