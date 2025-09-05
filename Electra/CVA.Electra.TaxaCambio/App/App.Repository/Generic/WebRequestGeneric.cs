using App.Domain.Helpers;
using App.Infrastructure.Configuration;
using App.Repository.Repositories;
using App.Repository.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace App.Repository.Generic
{
    public static class WebRequestGeneric
    {
        private static readonly IConfigurationRepository _configurationRepository = new AppSettingsConfigurationRepository();

        public static T Get<T>(this Uri url, CallType _callType) where T : class
        {
            try
            {
                return url
                            .CreateRequest(HttpMethod.Get, _callType)
                            .ParseJson<T>();
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        public static T Post<T>(this Uri url, object data = null, bool sendDefaultValues = true) where T : class
        {
            try
            {
                return url
                        .CreateRequest(HttpMethod.Post)
                        .SendData(data, sendDefaultValues)
                        .ParseJson<T>();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static void PostWithNoReturn<T>(this Uri url, object data = null, bool sendDefaultValues = true) where T : class
        {
            try
            {
                url.CreateRequest(HttpMethod.Post)
                       .SendData(data, sendDefaultValues)
                       .ParseJsonNoReturn<T>();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static T Patch<T>(this Uri url, object data = null, bool sendDefaultValues = true) where T : class
        {
            try
            {
                return url
                                .CreateRequest(new HttpMethod("PATCH"))
                                .SendData(data, sendDefaultValues)
                                .ParseJson<T>();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static T Delete<T>(this Uri url, object data = null) where T : class
        {
            try
            {
                return url
                                .CreateRequest(HttpMethod.Delete)
                                .SendData(data)
                                .ParseJson<T>();
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        public static T Login<T>(this Uri url, object data = null) where T : class
        {
            try
            {
                return url
                                .CreateRequestBasic(HttpMethod.Post)
                                .SendData(data)
                                .ParseJson<T>(true);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static void LoginConnectionString(this Uri url)
        {
            try
            {
                SetCookiesLogin(url);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // ...

        private static ICollection<Cookie> _cookieContainer;
        private static WebHeaderCollection _headerContainer;

        static HttpWebRequest CreateRequest(this Uri url, HttpMethod method, CallType _callType = CallType.ServiceLayer)
        {
            try
            {
                try
                {

                    string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    path = $@"{path}\\CreateRequest.txt";
                    #region Somente arquivo do mesmo dia
                    DateTime creation = File.GetCreationTime(path);
                    if (creation.Date != DateTime.Now.Date)
                    {
                        System.IO.File.Delete(path);
                    }
                    #endregion
                    System.IO.File.AppendAllText(path, Environment.NewLine + Environment.NewLine + Environment.NewLine + "--Request: " + url.OriginalString);

                }
                catch { }
                //Trust all certificates
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                // trust sender
                ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => true);

                // validate cert by calling a function
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = method.ToString().ToUpperInvariant();
                request.ReadWriteTimeout = request.ContinueTimeout = request.Timeout = 10 * 60000;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                request.Proxy = WebRequest.DefaultWebProxy;
                request.ServicePoint.Expect100Continue = false;
                request.ProtocolVersion = HttpVersion.Version11;
                request.KeepAlive = true;
                request.AllowAutoRedirect = true;
                request.PreAuthenticate = true;
                request.UseDefaultCredentials = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Accept = "application/json;odata=nometadata";
                request.ContentType = "application/json;odata=nometadata;charset=utf8";

                if (_callType != CallType.ServiceLayer) //ODATA
                    request.Headers.Add("Authorization", "Basic " + ConnectionCacheService.GetBasicAuthorization());
                //else
                //    ConnectionCacheService.ConnectionServiceLayer();

                    if (_headerContainer?.Count > 0)
                    request.Headers.Add(_headerContainer);

                request.CookieContainer = new CookieContainer();
                if (_cookieContainer?.Count > 0)
                    foreach (var coockie in _cookieContainer)
                        request.CookieContainer.Add(coockie);

                return request;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        static HttpWebRequest CreateRequestBasic(this Uri url, HttpMethod method, CallType _callType = CallType.ServiceLayer)
        {
            try
            {
                try
                {

                    string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    path = $@"{path}\\CreateRequest.txt";
                    #region Somente arquivo do mesmo dia
                    DateTime creation = File.GetCreationTime(path);
                    if (creation.Date != DateTime.Now.Date)
                    {
                        System.IO.File.Delete(path);
                    }
                    #endregion
                    System.IO.File.AppendAllText(path, Environment.NewLine + Environment.NewLine + Environment.NewLine + "--Request: " + url.OriginalString);

                }
                catch { }
                //Trust all certificates
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                // trust sender
                ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => true);

                // validate cert by calling a function
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = method.ToString().ToUpperInvariant();
                request.ReadWriteTimeout = request.ContinueTimeout = request.Timeout = 10 * 60000;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                request.Proxy = WebRequest.DefaultWebProxy;
                request.ServicePoint.Expect100Continue = false;
                request.ProtocolVersion = HttpVersion.Version11;
                request.KeepAlive = true;
                request.AllowAutoRedirect = true;
                request.PreAuthenticate = true;
                request.UseDefaultCredentials = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Accept = "application/json;odata=nometadata";
                request.ContentType = "application/json;odata=nometadata;charset=utf8";
              

                if (_headerContainer?.Count > 0)
                    request.Headers.Add(_headerContainer);

                request.CookieContainer = new CookieContainer();
                if (_cookieContainer?.Count > 0)
                    foreach (var coockie in _cookieContainer)
                        request.CookieContainer.Add(coockie);

                return request;
            }
            catch (System.Exception)
            {

                throw;
            }
        }



        static HttpWebRequest SendData(this HttpWebRequest request, object data, bool sendDefaultValues = true)
        {
            try
            {
                if (data != null)
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        string text = JsonHelper.SerializeToMinimalJson(data, sendDefaultValues);
                        try
                        {
                            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                            path = $@"{path}\\CreateRequest.txt";
                            System.IO.File.AppendAllText(path, Environment.NewLine + "--Send Data: " + text);

                        }
                        catch { }
                        writer.Write(text);
                    }

                return request;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        static T ParseJson<T>(this HttpWebRequest request, bool login = false)
        {
            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), response.GetEncoding()))
                {


                    if (login)
                        SetCookiesLogin(null, response);

                    var ret = reader.ReadToEnd();

                    try
                    {
                        string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        path = $@"{path}\\CreateRequest.txt";
                        System.IO.File.AppendAllText(path, Environment.NewLine + "--Retorn: " + ret);

                    }
                    catch { }

                    if (ret.StartsWith("{") || string.IsNullOrEmpty(ret))
                    {
                        return JsonConvert.DeserializeObject<T>(ret);
                    }
                    else
                    {
                        return TConverter.ChangeType<T>(ret);
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        static void ParseJsonNoReturn<T>(this HttpWebRequest request, bool login = false)
        {
            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), response.GetEncoding()))
                {

                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // callback used to validate the certificate in an SSL conversation
        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        public static Encoding GetEncoding(this HttpWebResponse response) => Encoding.GetEncoding(string.IsNullOrEmpty(response?.CharacterSet) ? "UTF-8" : response?.CharacterSet);

        //

        static void SetCookiesLogin(Uri url = null, HttpWebResponse response = null)
        {
            try
            {
                _cookieContainer = new List<Cookie>();
                _headerContainer = new WebHeaderCollection();

                if (AddonService.isRunning)
                {
                    var stringConnectionContext = AddonService.GetAddonConnectionContext();
                    if (!string.IsNullOrEmpty(stringConnectionContext))
                    {
                        string[] cookieItems = stringConnectionContext.Split(';');
                        foreach (var cookieItem in cookieItems.Where(x => x.StartsWith("B1SESSION=")))
                        {
                            string[] parts = cookieItem.Split('=');
                            if (parts.Length == 2)
                            {
                                _cookieContainer.Add(new Cookie(parts[0].Trim(), parts[1].Trim()) { Domain = url.Host });
                            }
                        }

                        SetCookiesDefault(url.Host);
                    }
                }
                else
                {
                    // Print the properties of each cookie.
                    foreach (Cookie cook in response.Cookies)
                        _cookieContainer.Add(new Cookie(cook.Name, cook.Value) { Domain = response.ResponseUri.Host });

                    SetCookiesDefault(response.ResponseUri.Host);
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        private static void SetCookiesDefault(string host)
        {
            try
            {
                _cookieContainer.Add(new Cookie("B1S-ReplaceCollectionsOnPatch", "true") { Domain = host });
                _headerContainer.Add("Prefer", "odata.maxpagesize=5000");
            }
            catch (System.Exception)
            {

                throw;
            }
        }


    }


}
