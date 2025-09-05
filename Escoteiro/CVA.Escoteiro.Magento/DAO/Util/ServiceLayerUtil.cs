using Escoteiro.Magento.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.DAO.Util
{
    public class ServiceLayerUtil
    {
        private string ApiUrl;
        //private static string SessionId;
        private static List<SessionDataBaseModel> listSessionModel = new List<SessionDataBaseModel>();
        private static SessionModel sessionModel = new SessionModel();

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

        public async Task<string> Login(string Company)
        {
            SessionDataBaseModel session = listSessionModel.Where(i => i.DataBase == Company).FirstOrDefault();

            if (session != null)
            {
                sessionModel = session.Session;

                if (session.SessionFinish > DateTime.Now)
                {
                    return "";
                }
                else
                {
                    await this.Logout();
                    listSessionModel.Remove(session);
                }
            }

            LoginModel loginModel = new LoginModel();
            loginModel.CompanyDB = System.Configuration.ConfigurationManager.AppSettings["Database"];
            loginModel.UserName = System.Configuration.ConfigurationManager.AppSettings["B1User"];
            loginModel.Password = System.Configuration.ConfigurationManager.AppSettings["B1Password"];
            loginModel.Language = "29";

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.ExpectContinue = false;

                var json = JsonConvert.SerializeObject(loginModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(string.Format(ApiUrl + "/Login"), content);

                if (response.IsSuccessStatusCode)
                {
                    sessionModel = JsonConvert.DeserializeObject<SessionModel>(await response.Content.ReadAsStringAsync());

                    SessionDataBaseModel sessionCompanyModel = new SessionDataBaseModel();
                    sessionCompanyModel.DataBase = Company;
                    sessionCompanyModel.SessionFinish = DateTime.Now.AddMinutes(sessionModel.SessionTimeout-1);
                    sessionCompanyModel.Session = sessionModel;
                    listSessionModel.Add(sessionCompanyModel);
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
            handler.CookieContainer.Add(uri, new Cookie("B1SESSION", sessionModel.SessionId));
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
                //sessionModel = "";
                return "";
            }
        }

        public async Task<T> GetByIDAsync<T>(string controller, object id) where T : class
        {
            var type = id.GetType();
            string url = string.Empty;

            if (type.FullName == "System.String")
                url = string.Format(ApiUrl + "/{0}('{1}')", controller, id);
            else
                url = string.Format(ApiUrl + "/{0}({1})", controller, id);

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Uri(url), new Cookie("B1SESSION", sessionModel.SessionId));

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
            handler.CookieContainer.Add(new Uri(url), new Cookie("B1SESSION", sessionModel.SessionId));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.ExpectContinue = false;

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync(url);

            var JsonResult = response.Content.ReadAsStringAsync().Result;
            var rootobject = JsonConvert.DeserializeObject<T>(JsonResult);

            return rootobject;
        }

        public async Task<string> PostFuncAsync(string controller, string param, string func) 
        {
            string url = string.Empty;

            Uri uri = new Uri(string.Format(ApiUrl + "/{0}({1})/{2}", controller, param, func));

            HttpClientHandler handler = new HttpClientHandler() { UseCookies = false };
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.ExpectContinue = false;

            var method = new HttpMethod("POST");
            var request = new HttpRequestMessage(method, uri) { };
            request.Headers.Add("Cookie", "B1SESSION=" + sessionModel.SessionId);

            HttpResponseMessage response = await client.SendAsync(request);
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


        public async Task<string> PatchAsync<T>(string controller, string param, T item) where T : class
        {
            Uri uri = new Uri(string.Format(ApiUrl + "/{0}({1})", controller, param));

            HttpClientHandler handler = new HttpClientHandler() { UseCookies = false };
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.ExpectContinue = false;

            var json = JsonConvert.SerializeObject(item, Formatting.None,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, uri) { Content = content };
            request.Headers.Add("Cookie", "B1SESSION=" + sessionModel.SessionId);

            HttpResponseMessage response = await client.SendAsync(request);
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

        public async Task<string> PatchAsyncWithReplaceCollectionsOnPatch<T>(string controller, string param, T item) where T : class
        {
            Uri uri = new Uri(string.Format(ApiUrl + "/{0}({1})", controller, param));

            HttpClientHandler handler = new HttpClientHandler() { UseCookies = false };
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.ExpectContinue = false;

            var json = JsonConvert.SerializeObject(item, Formatting.None,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, uri) { Content = content };
            request.Headers.Add("Cookie", "B1SESSION=" + sessionModel.SessionId);
            request.Headers.Add("B1S-ReplaceCollectionsOnPatch", "true");

            HttpResponseMessage response = await client.SendAsync(request);
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
                handler.CookieContainer.Add(uri, new Cookie("B1SESSION", sessionModel.SessionId));
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

        public async Task<List<string>> PatchAsyncReturnList<T>(string controller, object param, T item) where T : class
        {
            List<string> listReturn = new List<string>();
            try
            {
                var type = param.GetType();
                string url = string.Empty;

                if (type.FullName == "System.String")
                    url = string.Format(ApiUrl + "/{0}('{1}')", controller, param);
                else
                    url = string.Format(ApiUrl + "/{0}({1})", controller, param);
                Uri uri = new Uri(url);

                HttpClientHandler handler = new HttpClientHandler() { UseCookies = false };
                HttpClient client = new HttpClient(handler);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.ExpectContinue = false;

                var json = JsonConvert.SerializeObject(item, Formatting.None,
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var method = new HttpMethod("PATCH");
                var request = new HttpRequestMessage(method, uri) { Content = content };
                request.Headers.Add("Cookie", "B1SESSION=" + sessionModel.SessionId);

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var contentRead = await response.Content.ReadAsStringAsync();

                    listReturn.Add("OK");
                    listReturn.Add(contentRead);
                    return listReturn;
                }
                else
                {
                    try
                    {
                        JsonModel jsonModel = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());
                        if (!String.IsNullOrEmpty(jsonModel.ExceptionMessage))
                        {
                            listReturn.Add("NOK");
                            listReturn.Add(jsonModel.ExceptionMessage);
                            return listReturn;
                        }
                        SBOErrorModel sboErrorModel = JsonConvert.DeserializeObject<SBOErrorModel>(await response.Content.ReadAsStringAsync());
                        if (sboErrorModel.error != null)
                        {
                            listReturn.Add("NOK");
                            listReturn.Add(sboErrorModel.error.code + " - " + sboErrorModel.error.message.value);
                            return listReturn;
                        }
                    }
                    catch (Exception ex)
                    {
                        await response.Content.ReadAsStringAsync();
                    }
                    var contentRead = await response.Content.ReadAsStringAsync();
                    listReturn.Add("NOK");
                    listReturn.Add(contentRead);
                    return listReturn;
                }
            }
            catch (Exception ex)
            {
                listReturn.Add("NOK");
                listReturn.Add(ex.Message);
                return listReturn;
            }
        }

        public async Task<List<string>> PostAsyncReturnList<T>(string controller, T item) where T : class
        {
            List<string> listReturn = new List<string>();

            try
            {
                Uri uri = new Uri(string.Format(ApiUrl + "/{0}", controller));

                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(uri, new Cookie("B1SESSION", sessionModel.SessionId));
                HttpClient client = new HttpClient(handler);

                var json = JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var contentRead = await response.Content.ReadAsStringAsync();

                    listReturn.Add("OK");
                    listReturn.Add(contentRead);
                    return listReturn;
                }
                else
                {
                    try
                    {
                        JsonModel jsonModel = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());
                        if (!String.IsNullOrEmpty(jsonModel.ExceptionMessage))
                        {
                            listReturn.Add("NOK");
                            listReturn.Add(jsonModel.ExceptionMessage);
                            return listReturn;
                        }
                        SBOErrorModel sboErrorModel = JsonConvert.DeserializeObject<SBOErrorModel>(await response.Content.ReadAsStringAsync());
                        if (sboErrorModel.error != null)
                        {
                            listReturn.Add("NOK");
                            listReturn.Add(sboErrorModel.error.code + " - " + sboErrorModel.error.message.value);
                            return listReturn;
                        }
                    }
                    catch (Exception ex)
                    {
                        await response.Content.ReadAsStringAsync();
                    }
                    var contentRead = await response.Content.ReadAsStringAsync();
                    listReturn.Add("NOK");
                    listReturn.Add(contentRead);
                    return  listReturn;
                }
            }
            catch (Exception ex)
            {
                listReturn.Add("NOK");
                listReturn.Add(ex.Message);
                return listReturn;
            }
        }

        public async Task<string> DeleteAsync(string controller, string param)
        {
            string url = string.Format(ApiUrl + "/{0}/{1}", controller, param);

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("B1SESSION", sessionModel.SessionId));
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
    }
}
