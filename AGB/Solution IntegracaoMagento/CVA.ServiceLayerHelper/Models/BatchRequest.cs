using System.Text;

namespace ServiceLayerHelper
{
    public class BatchRequest
    {
        public string HttpMethod { get; set; }
        public string Resource { get; set; }
        public object Content { get; set; }
        public string MediaType { get; set; }
        public Encoding Encoding { get; set; }

        public BatchRequest(string httpMethod, string resource, object content, string mediaType = "application/json")
        {
            HttpMethod = httpMethod;
            Resource = resource;
            Content = content;
            MediaType = mediaType;
            Encoding = Encoding.UTF8;
        }
    }
}
