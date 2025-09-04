using Flurl.Http.Configuration;
using System.Net.Http;

namespace CVA.Portal.Producao.DAO.Util
{
    public class CustomHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (a, b, c, d) => true
            };
        }

        public override HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            var cli = base.CreateHttpClient(handler);
            cli.DefaultRequestHeaders.ExpectContinue = false;
            return cli;
        }
    }
}
}
