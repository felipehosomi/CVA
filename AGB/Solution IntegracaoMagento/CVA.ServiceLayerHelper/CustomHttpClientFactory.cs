using Flurl.Http.Configuration;
using System.Net.Http;

namespace ServiceLayerHelper
{
    public class CustomHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            var handler = (HttpClientHandler)base.CreateMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
            return handler;
        }

        public override HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            var httpClient = base.CreateHttpClient(handler);
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            return httpClient;
        }
    }
}