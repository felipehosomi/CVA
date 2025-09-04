using Newtonsoft.Json;

namespace CVA.IntegracaoMagento.SalesOrderStatus
{
    public partial class BusinessPartners
    {
        [JsonProperty("CardFName")]
        public string CardFName { get; set; }
    }
}
