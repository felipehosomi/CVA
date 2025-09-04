using Newtonsoft.Json;
using System;

namespace CVA.IntegracaoMagento.Integrator.Models.SAP
{
    public partial class PriceLists_SAP
    {
        [JsonProperty("odata.metadata")]
        public Uri OdataMetadata { get; set; }

        [JsonProperty("RoundingMethod")]
        public string RoundingMethod { get; set; }

        [JsonProperty("GroupNum")]
        public string GroupNum { get; set; }

        [JsonProperty("BasePriceList")]
        public long? BasePriceList { get; set; }

        [JsonProperty("Factor")]
        public long? Factor { get; set; }

        [JsonProperty("PriceListNo")]
        public long? PriceListNo { get; set; }

        [JsonProperty("PriceListName")]
        public string PriceListName { get; set; }

        [JsonProperty("IsGrossPrice")]
        public string IsGrossPrice { get; set; }

        [JsonProperty("Active")]
        public string Active { get; set; }

        [JsonProperty("ValidFrom")]
        public object ValidFrom { get; set; }

        [JsonProperty("ValidTo")]
        public object ValidTo { get; set; }

        [JsonProperty("DefaultPrimeCurrency")]
        public string DefaultPrimeCurrency { get; set; }

        [JsonProperty("DefaultAdditionalCurrency1")]
        public string DefaultAdditionalCurrency1 { get; set; }

        [JsonProperty("DefaultAdditionalCurrency2")]
        public string DefaultAdditionalCurrency2 { get; set; }

        [JsonProperty("RoundingRule")]
        public string RoundingRule { get; set; }

        [JsonProperty("FixedAmount")]
        public long? FixedAmount { get; set; }

    }
}
