using Newtonsoft.Json;

namespace CVA.IntegracaoMagento.BusinessPartner.Models.SAP
{
    public partial class BusinessPartners
    {
        //[JsonProperty("BusinessPartners")]
        //public BusinessPartners BusinessPartner { get; set; }

        //[JsonProperty("BP Code")]
        //public string BPCode { get; set; }

        [JsonProperty("CardCode")]
        public string CardCode { get; set; }

        //[JsonProperty("CardName")]
        //public string CardName { get; set; }

        //[JsonProperty("CardType")]
        //public string CardType { get; set; }

        [JsonProperty("U_Magento_Id")]
        public string U_Magento_Id { get; set; }
    }
}
