using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVA.IntegracaoMagento.Stock.Models.SAP
{
    public class Metadata_Stock
    {
        public class Stock
        {
            [JsonProperty("stockItemCode")]
            public string stockItemCode { get; set; }

            [JsonProperty("stockCodeBars")]
            public string stockCodeBars { get; set; }

            [JsonProperty("stockWareHouse")]
            public string stockWareHouse { get; set; }

            [JsonProperty("stockOnHand")]
            public double stockOnHand { get; set; }

            [JsonProperty("stockMovOnHand")]
            public double stockMovOnHand { get; set; }

            [JsonProperty("id__")]
            public int id__ { get; set; }
        }

        public class Content
        {
            [JsonProperty("@odata.context")]
            public string odatacontext { get; set; }

            [JsonProperty("value")]
            public IList<Stock> value { get; set; }
        }
    }
}
