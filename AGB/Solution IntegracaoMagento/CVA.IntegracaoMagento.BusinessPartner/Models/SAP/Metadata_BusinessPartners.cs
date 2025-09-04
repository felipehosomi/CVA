using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVA.IntegracaoMagento.BusinessPartner.Models.SAP
{
    public class Metadata_BusinessPartners
    {
        public class BusinessPartner
        {
            [JsonProperty("E_Mail")]
            public string E_Mail { get; set; }

            [JsonProperty("CardName")]
            public string CardName { get; set; }

            [JsonProperty("TaxId4")]
            public string TaxId4 { get; set; }

            [JsonProperty("U_datadenascimento")]
            public string U_datadenascimento { get; set; }

            [JsonProperty("Gender")]
            public string Gender { get; set; }

            [JsonProperty("CardCode")]
            public string CardCode { get; set; }

            [JsonProperty("Phone1")]
            public string Phone1 { get; set; }

            [JsonProperty("Phone2")]
            public string Phone2 { get; set; }

            [JsonProperty("Cellular")]
            public string Cellular { get; set; }

            [JsonProperty("Address")]
            public string Address { get; set; }

            [JsonProperty("State")]
            public string State { get; set; }

            [JsonProperty("Country")]
            public string Country { get; set; }

            [JsonProperty("AddrType")]
            public string AddrType { get; set; }

            [JsonProperty("Street")]
            public string Street { get; set; }

            [JsonProperty("StreetNo")]
            public string StreetNo { get; set; }

            [JsonProperty("Building")]
            public object Building { get; set; }

            [JsonProperty("Block")]
            public string Block { get; set; }

            [JsonProperty("ZipCode")]
            public string ZipCode { get; set; }

            [JsonProperty("City")]
            public string City { get; set; }

            [JsonProperty("cobAddress")]
            public string cobAddress { get; set; }

            [JsonProperty("cobState")]
            public string cobState { get; set; }

            [JsonProperty("cobCountry")]
            public string cobCountry { get; set; }

            [JsonProperty("cobAddrType")]
            public string cobAddrType { get; set; }

            [JsonProperty("cobStreet")]
            public string cobStreet { get; set; }

            [JsonProperty("cobStreetNo")]
            public string cobStreetNo { get; set; }

            [JsonProperty("cobBuilding")]
            public object cobBuilding { get; set; }

            [JsonProperty("cobBlock")]
            public string cobBlock { get; set; }

            [JsonProperty("cobZipCode")]
            public string cobZipCode { get; set; }

            [JsonProperty("cobCity")]
            public string cobCity { get; set; }

            [JsonProperty("Balance")]
            public double Balance { get; set; }

            [JsonProperty("MagentoId")]
            public string MagentoId { get; set; }

            [JsonProperty("Active")]
            public string Active { get; set; }

            [JsonProperty("CreateDate")]
            public string CreateDate { get; set; }

            [JsonProperty("CreateDateTime")]
            public string CreateDateTime { get; set; }

            [JsonProperty("UpdateDate")]
            public string UpdateDate { get; set; }

            [JsonProperty("UpdateDateTime")]
            public string UpdateDateTime { get; set; }

            [JsonProperty("id__")]
            public int id__ { get; set; }
        }

        public class Content
        {

            [JsonProperty("@odata.context")]
            public string odatacontext { get; set; }

            [JsonProperty("value")]
            public IList<BusinessPartner> value { get; set; }
        }
    }
}
