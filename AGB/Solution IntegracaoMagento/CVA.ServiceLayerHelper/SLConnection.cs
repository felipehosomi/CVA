using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceLayerHelper
{
    /// <summary>
    /// Represents a connection to the SAP Business One Service Layer.
    /// </summary>
    public class SLConnection
    {
        private CookieJar cookies;
        private int sessionTimeout;
        private DateTime lastLoginDateTime = DateTime.MinValue;
        private readonly HttpStatusCode[] returnCodesToRetry = new[]
        {
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout
        };

        /// <summary>
        /// Gets or sets the Service Layer root URL. The expected format is
        /// https://[server]:[port]/b1s/[version]
        /// </summary>
        public string ServiceLayerRootUrl { get; set; }
        /// <summary>
        /// Gets or sets the Company database (schema) to connect to.
        /// </summary>
        public string CompanyDB { get; set; }
        /// <summary>
        /// Gets or sets the username to be used for the Service Layer authentication.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the password for the provided username.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the Service Layer language code.
        /// </summary>
        public int? Language { get; set; }
        /// <summary>
        /// Gets or sets the number of attempts for each request in case of an HTTP response code of 401, 500, 502, 503 or 504.
        /// </summary>
        public int NumberOfAttempts { get; set; }

        /// <summary>
        /// Set the default settings to Flurl.Http.
        /// </summary>
        static SLConnection()
        {
            FlurlHttp.Configure(settings =>
            {
                // Ignore SSL error
                settings.HttpClientFactory = new CustomHttpClientFactory();
                // Ignore null values in JSON
                settings.JsonSerializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                });
            });
        }

        /// <summary>
        /// Initializes a new instance of the SLConnection class. Only one instance per company 
        /// should be used in the application, initialized at application startup.
        /// </summary>
        /// <param name="serviceLayerRootUrl">
        /// The Service Layer root URL. The expected format is https://[server]:[port]/b1s/[version]
        /// </param>
        /// <param name="companyDB">
        /// The Company database (schema) to connect to.
        /// </param>
        /// <param name="userName">
        /// The username to be used for the Service Layer authentication.
        /// </param>
        /// <param name="password">
        /// The password for the provided username.
        /// </param>
        /// <param name="language">
        /// The Service Layer language code. If not specified, it is English by default.
        /// A GET request to the UserLanguages resource will return all available languages.
        /// </param>
        /// <param name="numberOfAttempts">
        /// The number of attempts for each request in case of an HTTP response code of 401, 500, 502, 503 or 504.
        /// If the response code is 401 (Unauthorized), a login request will be performed before the new attempt.
        /// </param>
        public SLConnection(string serviceLayerRootUrl,
            string companyDB, string userName, string password, int? language = null, int numberOfAttempts = 3)
        {
            ServiceLayerRootUrl = serviceLayerRootUrl;
            CompanyDB = companyDB;
            UserName = userName;
            Password = password;
            Language = language;
            NumberOfAttempts = numberOfAttempts;
        }

        /// <summary>
        /// If the current session is expired or non-existent, makes a POST Login request
        /// with the provided information. If successfull, keeps the session timeout value
        /// returned to be used as a session lifespan control and the session ID to be sent 
        /// as a cookie to be used for subsequent authenticated requests.
        /// </summary>
        /// <param name="forceLogin">
        /// Whether the login request should be forced even if the current session has not expired.
        /// </param>
        private async Task Login(bool forceLogin = false)
        {
            if (forceLogin)
                lastLoginDateTime = DateTime.MinValue;

            // Session still valid, no need to login again
            if (DateTime.Now.Subtract(lastLoginDateTime).TotalMinutes < sessionTimeout)
                return;

            if (string.IsNullOrEmpty(ServiceLayerRootUrl))
                throw new ArgumentNullException("ServiceLayerBaseUrl not set.");

            if (string.IsNullOrEmpty(CompanyDB))
                throw new ArgumentNullException("CompanyDB not set.");

            if (string.IsNullOrEmpty(UserName))
                throw new ArgumentNullException("UserName not set.");

            if (string.IsNullOrEmpty(Password))
                throw new ArgumentNullException("Password not set.");

            var loginResponse = await ServiceLayerRootUrl
                .AppendPathSegment("Login")
                .WithCookies(out var cookieJar)
                .PostJsonAsync(new { CompanyDB, UserName, Password, Language })
                .ReceiveJson<LoginResponse>();

            lastLoginDateTime = DateTime.Now;
            sessionTimeout = loginResponse.SessionTimeout;
            cookies = cookieJar;
        }

        /// <summary>
        /// Builds the default IFlurlRequest for GET requests with the provided parameters and the current session cookie.
        /// </summary>
        /// <param name="resource">
        /// The resource to be interacted with. It can be a collection of entities or a single entity.
        /// </param>
        /// <param name="filter">
        /// The filters to be used to query a collections of entities.
        /// </param>
        /// <param name="select">
        /// The specific properties of an entity to be returned.
        /// </param>
        /// <param name="orderBy">
        /// The order in which entities are returned.
        /// </param>
        /// <param name="top">
        /// The number of records to be returned in a collection of entities.
        /// </param>
        /// <param name="skip">
        /// The number of first records to be excluded from the result.
        /// </param>
        /// <param name="apply">
        /// The aggregation expression. The supported aggregation methods include sum, avg, min, max, count and distinctcount.
        /// </param>
        /// <param name="expand">
        /// The navigation properties of an entity to be retrieved.
        /// </param>
        private IFlurlRequest BuildGetRequest(string resource, string filter = null, string select = null, string orderBy = null,
            uint? top = null, string skip = null, string apply = null, string expand = null)
        {
            var getRequest = ServiceLayerRootUrl
                .AppendPathSegment(resource)
                .SetQueryParam("$filter", filter)
                .SetQueryParam("$select", select)
                .SetQueryParam("$orderby", orderBy)
                .SetQueryParam("$top", top)
                .SetQueryParam("$skip", skip)
                .SetQueryParam("$apply", apply)
                .SetQueryParam("$expand", expand)
                .WithCookies(cookies);

            return getRequest;
        }

        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in a new instance of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The object type of the result.
        /// </typeparam>
        /// <param name="resource">
        /// The resource to be interacted with. It can be a collection of entities or a single entity.
        /// </param>
        /// <param name="filter">
        /// The filters to be used to query a collections of entities.
        /// </param>
        /// <param name="select">
        /// The specific properties of an entity to be returned.
        /// </param>
        /// <param name="orderBy">
        /// The order in which entities are returned.
        /// </param>
        /// <param name="top">
        /// The number of records to be returned in a collection of entities.
        /// </param>
        /// <param name="skip">
        /// The number of first records to be excluded from the result.
        /// </param>
        /// <param name="apply">
        /// The aggregation expression. The supported aggregation methods include sum, avg, min, max, count and distinctcount.
        /// </param>
        /// <param name="expand">
        /// The navigation properties of an entity to be retrieved.
        /// </param>
        public async Task<T> GetAsync<T>(string resource, string filter = null, string select = null, string orderBy = null,
            uint? top = null, string skip = null, string apply = null, string expand = null)
        {
            return await ExecuteRequest(async () =>
            {
                var getRequest = BuildGetRequest(resource, filter, select, orderBy, top, skip, apply, expand).WithHeader("B1S-CaseInsensitive", "true").WithHeader("B1S-PageSize", "1000");
                string stringResult = await getRequest.GetStringAsync();
                var jObject = JObject.Parse(stringResult);

                // Checks if the result is a collection by selecting the "value" token
                var valueCollection = jObject.SelectToken("value");

                if (valueCollection != null)
                {
                    var objList = valueCollection.ToObject<T>();
                    return objList;
                }
                else
                {
                    var obj = jObject.ToObject<T>();
                    return obj;
                }
            });
        }

        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in a dynamic object.
        /// </summary>
        /// <param name="resource">
        /// The resource to be interacted with. It can be a collection of entities or a single entity.
        /// </param>
        /// <param name="filter">
        /// The filters to be used to query a collections of entities.
        /// </param>
        /// <param name="select">
        /// The specific properties of an entity to be returned.
        /// </param>
        /// <param name="orderBy">
        /// The order in which entities are returned.
        /// </param>
        /// <param name="top">
        /// The number of records to be returned in a collection of entities.
        /// </param>
        /// <param name="skip">
        /// The number of first records to be excluded from the result.
        /// </param>
        /// <param name="apply">
        /// The aggregation expression. The supported aggregation methods include sum, avg, min, max, count and distinctcount.
        /// </param>
        /// <param name="expand">
        /// The navigation properties of an entity to be retrieved.
        /// </param>
        public async Task<dynamic> GetAsync(string resource, string filter = null, string select = null, string orderBy = null,
            uint? top = null, string skip = null, string apply = null, string expand = null)
        {
            return await ExecuteRequest(async () =>
            {
                var result = await BuildGetRequest(resource, filter, select, orderBy, top, skip, apply, expand).GetJsonAsync();
                return result;
            });
        }

        /// <summary>
        /// Performs a GET request with the provided parameters and returns the result in a string.
        /// </summary>
        /// <param name="resource">
        /// The resource to be interacted with. It can be a collection of entities or a single entity.
        /// </param>
        /// <param name="filter">
        /// The filters to be used to query a collections of entities.
        /// </param>
        /// <param name="select">
        /// The specific properties of an entity to be returned.
        /// </param>
        /// <param name="orderBy">
        /// The order in which entities are returned.
        /// </param>
        /// <param name="top">
        /// The number of records to be returned in a collection of entities.
        /// </param>
        /// <param name="skip">
        /// The number of first records to be excluded from the result.
        /// </param>
        /// <param name="apply">
        /// The aggregation expression. The supported aggregation methods include sum, avg, min, max, count and distinctcount.
        /// </param>
        /// <param name="expand">
        /// The navigation properties of an entity to be retrieved.
        /// </param>
        public async Task<string> GetStringAsync(string resource, string filter = null, string select = null, string orderBy = null,
            uint? top = null, string skip = null, string apply = null, string expand = null)
        {
            return await ExecuteRequest(async () =>
            {
                var result = await BuildGetRequest(resource, filter, select, orderBy, top, skip, apply, expand).GetStringAsync();
                return result;
            });
        }

        /// <summary>
        /// Returns the count of an entity collection.
        /// </summary>
        /// <param name="resource">
        /// The collection of entities to be counted.
        /// </param>
        /// <param name="filter">
        /// The filters to be used to query a collections of entities.
        /// </param>
        public async Task<int> GetCount(string resource, string filter = null)
        {
            return await ExecuteRequest(async () =>
            {
                string count = await ServiceLayerRootUrl
                .AppendPathSegment(resource)
                .AppendPathSegment("$count")
                .SetQueryParam("$filter", filter)
                .WithCookies(cookies)
                .GetStringAsync();

                return Convert.ToInt32(count);
            });
        }

        /// <summary>
        /// Performs a POST request (creates an entity) to the provided resource with the 
        /// specified object serialized to JSON as the body.
        /// </summary>
        /// <typeparam name="T">
        /// The object type of the result.
        /// </typeparam>
        /// <param name="resource">
        /// The entity to be created.
        /// </param>
        /// <param name="data">
        /// The object containing the properties of the entity to be created.
        /// </param>
        /// <param name="returnNoContent">
        /// Whether the request should not return the created entity. If the return is not 
        /// needed after the creation, setting this to true will make the request faster.
        /// </param>
        public async Task<T> PostAsync<T>(string resource, T data, bool returnNoContent = false)
        {
            return await ExecuteRequest(async () =>
            {
                if (returnNoContent)
                {
                    var result = await ServiceLayerRootUrl
                        .AppendPathSegment(resource)
                        .WithHeader("Prefer", "return-no-content")
                        .WithCookies(cookies)
                        .PostJsonAsync(data);

                    return default;
                }
                else
                {
                    var result = await ServiceLayerRootUrl
                        .AppendPathSegment(resource)
                        .WithCookies(cookies)
                        .PostJsonAsync(data)
                        .ReceiveJson<T>();

                    return result;
                }
            });
        }

        public async Task PostAsync(string resource)
        {
            await ExecuteRequest(async () =>
            {
                var result = await ServiceLayerRootUrl
                    .AppendPathSegment(resource)
                    .WithCookies(cookies)
                    .PostAsync(null);

                return result;
            });
        }

        public async Task PatchAsync<T>(string resource, T data)
        {
            await ExecuteRequest(async () =>
            {
                var result = await ServiceLayerRootUrl
                    .AppendPathSegment(resource)
                    .WithCookies(cookies)
                    .PatchJsonAsync(data);

                return result;
            });
        }

        public async Task PutAsync<T>(string resource, T data)
        {
            await ExecuteRequest(async () =>
            {
                var result = await ServiceLayerRootUrl
                    .AppendPathSegment(resource)
                    .WithCookies(cookies)
                    .PutJsonAsync(data);

                return result;
            });
        }

        public async Task DeleteAsync(string resource)
        {
            await ExecuteRequest(async () =>
            {
                var result = await ServiceLayerRootUrl
                    .AppendPathSegment(resource)
                    .WithCookies(cookies)
                    .DeleteAsync();

                return result;
            });
        }

        public async Task<HttpResponseMessage> PostBatchAsync(IEnumerable<BatchRequest> requests)
        {
            return await ExecuteRequest(async () =>
            {
                if (requests == null || requests.Count() == 0)
                {
                    throw new ArgumentException("No requests to be sent.");
                }

                var responseMessage = await ServiceLayerRootUrl
                    .AppendPathSegment("$batch")
                    .WithCookies(cookies)
                    .PostMultipartAsync(mp =>
                    {
                        mp.Headers.ContentType.MediaType = "multipart/mixed";
                        foreach (var request in requests)
                        {
                            string boundary = "changeset_" + Guid.NewGuid();
                            mp.Add(BuildMixedMultipartContent(request, boundary));
                        }
                    });

                return responseMessage.ResponseMessage;
            });
        }

        public async Task<HttpResponseMessage> PostBatchInSingleChangesetAsync(IEnumerable<BatchRequest> requests)
        {
            return await ExecuteRequest(async () =>
            {
                if (requests == null || requests.Count() == 0)
                {
                    throw new ArgumentException("No requests to be sent.");
                }

                var responseMessage = await ServiceLayerRootUrl
                    .AppendPathSegment("$batch")
                    .WithCookies(cookies)
                    .PostMultipartAsync(mp =>
                    {
                        mp.Headers.ContentType.MediaType = "multipart/mixed";
                        mp.Add(BuildMixedMultipartContent(requests));
                    });

                return responseMessage.ResponseMessage;
            });
        }

        public async Task<AttachmentResponse> PostAttachmentsAsync(IDictionary<string, Stream> files)
        {
            return await ExecuteRequest(async () =>
            {
                if (files == null || files.Count == 0)
                {
                    throw new ArgumentException("No files to be sent.");
                }

                var result = await ServiceLayerRootUrl
                    .AppendPathSegment("Attachments2")
                    .WithCookies(cookies)
                    .PostMultipartAsync(mp =>
                    {
                        // Removes double quotes from boundary, otherwise the request fails with error 405 Method Not Allowed
                        var boundary = mp.Headers.ContentType.Parameters.First(o => o.Name == "boundary");
                        boundary.Value = boundary.Value.Replace("\"", string.Empty);

                        foreach (var file in files)
                        {
                            var content = new StreamContent(file.Value);
                            content.Headers.Add("Content-Disposition", $"form-data; name=\"files\"; filename=\"{file.Key}\"");
                            content.Headers.Add("Content-Type", "application/octet-stream");
                            mp.Add(content);
                        }
                    })
                    .ReceiveJson<AttachmentResponse>();

                return result;
            });
        }

        public async Task<int> PostAttachmentsAsync(IDictionary<string, byte[]> files)
        {
            return await ExecuteRequest(async () =>
            {
                if (files == null || files.Count == 0)
                {
                    throw new ArgumentException("No files to be sent.");
                }

                var stringResult = await ServiceLayerRootUrl
                    .AppendPathSegment("Attachments2")
                    .WithCookies(cookies)
                    .PostMultipartAsync(mp =>
                    {
                        // Removes double quotes from boundary, otherwise the request fails with error 405 Method Not Allowed
                        var boundary = mp.Headers.ContentType.Parameters.First(o => o.Name == "boundary");
                        boundary.Value = boundary.Value.Replace("\"", string.Empty);

                        foreach (var file in files)
                        {
                            var content = new ByteArrayContent(file.Value);
                            content.Headers.Add("Content-Disposition", $"form-data; name=\"files\"; filename=\"{file.Key}\"");
                            content.Headers.Add("Content-Type", "application/octet-stream");
                            mp.Add(content);
                        }
                    })
                    .ReceiveString();

                int attachmentEntry = JObject.Parse(stringResult).SelectToken("AbsoluteEntry").ToObject<int>();
                return attachmentEntry;
            });
        }

        public async Task<byte[]> GetAttachmentAsBytesAsync(int attachmentEntry, string fileName = null)
        {
            return await ExecuteRequest(async () =>
            {
                var file = await ServiceLayerRootUrl
                    .AppendPathSegment($"Attachments2({attachmentEntry})/$value")
                    .SetQueryParam("filename", !string.IsNullOrEmpty(fileName) ? $"'{fileName}'" : null)
                    .WithCookies(cookies)
                    .GetBytesAsync();

                return file;
            });
        }

        public async Task<Stream> GetAttachmentAsStreamAsync(int attachmentEntry, string fileName = null)
        {
            return await ExecuteRequest(async () =>
            {
                var file = await ServiceLayerRootUrl
                    .AppendPathSegment($"Attachments2({attachmentEntry})/$value")
                    .SetQueryParam("filename", !string.IsNullOrEmpty(fileName) ? $"'{fileName}'" : null)
                    .WithCookies(cookies)
                    .GetStreamAsync();

                return file;
            });
        }

        private MultipartContent BuildMixedMultipartContent(IEnumerable<BatchRequest> requests)
        {
            string boundary = "changeset_" + Guid.NewGuid();
            var multipartContent = new MultipartContent("mixed", boundary);

            foreach (var batchRequest in requests)
            {
                var request = new HttpRequestMessage(new HttpMethod(batchRequest.HttpMethod), Url.Combine(ServiceLayerRootUrl, batchRequest.Resource));
                request.Content = new StringContent(JsonConvert.SerializeObject(batchRequest.Content), batchRequest.Encoding, batchRequest.MediaType);
                var innerContent = new HttpMessageContent(request);
                innerContent.Headers.Add("content-transfer-encoding", "binary");
                multipartContent.Add(innerContent);
            }

            return multipartContent;
        }

        private MultipartContent BuildMixedMultipartContent(BatchRequest batchRequest, string boundary)
        {
            var multipartContent = new MultipartContent("mixed", boundary);
            var request = new HttpRequestMessage(new HttpMethod(batchRequest.HttpMethod), Url.Combine(ServiceLayerRootUrl, batchRequest.Resource));
            request.Content = new StringContent(JsonConvert.SerializeObject(batchRequest.Content), batchRequest.Encoding, batchRequest.MediaType);
            var innerContent = new HttpMessageContent(request);
            innerContent.Headers.Add("content-transfer-encoding", "binary");
            multipartContent.Add(innerContent);

            return multipartContent;
        }

        private async Task<T> ExecuteRequest<T>(Func<Task<T>> action)
        {
            if (NumberOfAttempts < 1)
                throw new ArgumentException("The number of attempts can not be lower than 1.");

            int retryCount = NumberOfAttempts;
            var exceptions = new List<Exception>();

            await Login();

            while (true)
            {
                try
                {
                    return await action();
                }
                catch (FlurlHttpException ex)
                {
                    try
                    {
                        // Tries to obtain the error thrown by the Service Layer
                        string responseString = await ex.Call.Response.GetStringAsync();
                        var response = JObject.Parse(responseString).ToObject<ResponseError>();
                        exceptions.Add(new Exception($"({response.error.code}) {response.error.message.value}", ex));
                    }
                    catch
                    {
                        exceptions.Add(ex);
                    }

                    // Whether the request should be retried
                    if (!returnCodesToRetry.Any(x => x == ex.Call.HttpResponseMessage.StatusCode))
                    {
                        break;
                    }

                    // Forces a login request in case the response is 401 Unauthorized
                    if (ex.Call.HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        await Login(true);
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                retryCount--;

                if (retryCount <= 0)
                {
                    break;
                }

                await Task.Delay(200);
            }

            var uniqueExceptions = exceptions.Distinct(new ExceptionEqualityComparer());

            if (uniqueExceptions.Count() == 1)
                throw uniqueExceptions.First();

            throw new AggregateException("Could not process request", uniqueExceptions);
        }

        /// <summary>
        /// Used to aggregate exceptions that occur on request retries. 
        /// </summary>
        /// <remarks>
        /// In most cases, the same exception will occur multiple times, 
        /// but we don't want to return multiple copies of it. This class is used 
        /// to find exceptions that are duplicates by type and message so we can
        /// only return one of them.
        /// </remarks>
        private class ExceptionEqualityComparer : IEqualityComparer<Exception>
        {
            public bool Equals(Exception e1, Exception e2)
            {
                if (e2 == null && e1 == null)
                    return true;
                else if (e1 == null | e2 == null)
                    return false;
                else if (e1.GetType().Name.Equals(e2.GetType().Name) && e1.Message.Equals(e2.Message))
                    return true;
                else
                    return false;
            }

            public int GetHashCode(Exception e)
            {
                return (e.GetType().Name + e.Message).GetHashCode();
            }
        }
    }
}