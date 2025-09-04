using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fibra.ItemPricing.Models
{
    public partial class ItemMasterData
    {
        [JsonProperty("odata.metadata")]
        public Uri OdataMetadata { get; set; }

        [JsonProperty("ItemCode")]
        public string ItemCode { get; set; }

        [JsonProperty("ItemPrices")]
        public List<ItemPrice> ItemPrices { get; set; }
    }

    public partial class ItemPrice
    {
        [JsonProperty("PriceList")]
        public long? PriceList { get; set; }

        [JsonProperty("PriceListName")]
        public string PriceListName { get; set; }

        [JsonProperty("Price")]
        public double? Price { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("AdditionalPrice1")]
        public long? AdditionalPrice1 { get; set; }

        [JsonProperty("AdditionalCurrency1")]
        public string AdditionalCurrency1 { get; set; }

        [JsonProperty("AdditionalPrice2")]
        public long? AdditionalPrice2 { get; set; }

        [JsonProperty("AdditionalCurrency2")]
        public string AdditionalCurrency2 { get; set; }

        [JsonProperty("BasePriceList")]
        public long? BasePriceList { get; set; }

        [JsonProperty("Factor")]
        public long? Factor { get; set; }

        [JsonProperty("UoMPrices")]
        public object[] UoMPrices { get; set; }
    }
}