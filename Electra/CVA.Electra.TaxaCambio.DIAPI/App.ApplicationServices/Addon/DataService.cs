using App.ApplicationServices.Services;
using App.Repository.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.Addon
{
    class DataService
    {
        private static string connectionContext;


        public static dynamic Get(string objeto)
        {
            dynamic json = null;
            var jsonAdd = "";
            try
            {
                connectionContext = AddonService.GetAddonConnectionContext();
                var request = WebRequest.Create(AddonService.serviceLayerURL + $@"/{objeto}") as HttpWebRequest;
                request.AllowAutoRedirect = false;
                request.Method = "Get";
                request.KeepAlive = true;
                request.ContentType = "appication/json";
                request.Timeout = 300 * 1000;
                request.ServicePoint.Expect100Continue = false;
                request.CookieContainer = new CookieContainer();
                ServicePointManager.ServerCertificateValidationCallback += BypassSslCallback;

                string[] cookieItems = connectionContext.Split(';');
                foreach (var cookieItem in cookieItems)
                {
                    string[] parts = cookieItem.Split('=');
                    if (parts.Length == 2)
                    {
                        request.CookieContainer.Add(request.RequestUri, new Cookie(parts[0].Trim(), parts[1].Trim()));

                    }
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Get item error");
                    //return -1;
                }

                string responseContent = null;

                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseContent = reader.ReadToEnd();
                    json = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                }
            }
            catch (Exception ex)
            {
                jsonAdd = JsonConvert.SerializeObject(new
                {
                    ErroMessage = ex.Message
                });
                json = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonAdd);
            }

            return json;
        }

        //public static dynamic getPost(string dadosJson)
        //{
        //    Conexao.ConectaServiceLayer();

        //    var httpWebRequest =
        //        (HttpWebRequest)WebRequest.Create(Conexao.serviceLayerAddress + $@"/{dadosJson}");
        //    httpWebRequest.ContentType = "application/json";
        //    httpWebRequest.Method = "POST";

        //    httpWebRequest.AllowAutoRedirect = false;
        //    httpWebRequest.Timeout = 30 * 1000;
        //    httpWebRequest.ServicePoint.Expect100Continue = false;
        //    httpWebRequest.CookieContainer = new CookieContainer();
        //    ServicePointManager.ServerCertificateValidationCallback += BypassSslCallback;

        //    string[] cookieItems1 = Conexao.sConnectionContext.Split(';');
        //    foreach (var cookieItem in cookieItems1)
        //    {
        //        string[] parts = cookieItem.Split('=');
        //        if (parts.Length == 2)
        //        {
        //            httpWebRequest.CookieContainer.Add(httpWebRequest.RequestUri,
        //                new Cookie(parts[0].Trim(), parts[1].Trim()));
        //        }
        //    }

        //    HttpWebResponse response1 = httpWebRequest.GetResponse() as HttpWebResponse;
        //    string responseContent = null;
        //    dynamic json = null;
        //    using (var reader = new StreamReader(response1.GetResponseStream(), Encoding.UTF8))
        //    {
        //        responseContent = reader.ReadToEnd();
        //        json = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //    }

        //    return json;
        //}

        public static dynamic post(string dadosJson, string entidade)
        {
            connectionContext = AddonService.GetAddonConnectionContext();
            var urlRequest = AddonService.serviceLayerURL + string.Format(@"/{0}", entidade);
            var httpWebRequest =(HttpWebRequest)WebRequest.Create(urlRequest);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.AllowAutoRedirect = false;
            httpWebRequest.Timeout = 30 * 1000;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.CookieContainer = new CookieContainer();
            ServicePointManager.ServerCertificateValidationCallback += BypassSslCallback;

            string[] cookieItems1 = connectionContext.Split(';');
            foreach (var cookieItem in cookieItems1)
            {
                string[] parts = cookieItem.Split('=');
                if (parts.Length == 2)
                {
                    httpWebRequest.CookieContainer.Add(httpWebRequest.RequestUri,
                        new Cookie(parts[0].Trim(), parts[1].Trim()));
                }
            }

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(dadosJson);
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse response1 = httpWebRequest.GetResponse() as HttpWebResponse;
            string responseContent = null;
            dynamic json = null;
            using (var reader = new StreamReader(response1.GetResponseStream(), Encoding.UTF8))
            {
                responseContent = reader.ReadToEnd();
                json = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);

            }
            return json;
        }

        //public static dynamic put(string dadosJson, string entidade)
        //{

        //    Conexao.ConectaServiceLayer();
        //    var httpWebRequest =
        //        (HttpWebRequest)WebRequest.Create(Conexao.serviceLayerAddress +
        //                                           string.Format(@"/{0}", entidade));
        //    httpWebRequest.ContentType = "application/json";
        //    httpWebRequest.Method = "PATCH";
        //    httpWebRequest.AllowAutoRedirect = false;
        //    httpWebRequest.Timeout = 30 * 1000;
        //    httpWebRequest.ServicePoint.Expect100Continue = false;
        //    httpWebRequest.CookieContainer = new CookieContainer();
        //    ServicePointManager.ServerCertificateValidationCallback += BypassSslCallback;
        //    string[] cookieItems1 = Conexao.sConnectionContext.Split(';');
        //    foreach (var cookieItem in cookieItems1)
        //    {
        //        string[] parts = cookieItem.Split('=');
        //        if (parts.Length == 2)
        //        {
        //            httpWebRequest.CookieContainer.Add(httpWebRequest.RequestUri,
        //                new Cookie(parts[0].Trim(), parts[1].Trim()));
        //        }
        //    }

        //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //    {
        //        streamWriter.Write(dadosJson);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //    }

        //    HttpWebResponse response1 = httpWebRequest.GetResponse() as HttpWebResponse;
        //    string responseContent = null;
        //    dynamic json = null;
        //    using (var reader = new StreamReader(response1.GetResponseStream(), Encoding.UTF8))
        //    {
        //        responseContent = reader.ReadToEnd();
        //        json = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);

        //    }
        //    return json;
        //}

        public static bool BypassSslCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
