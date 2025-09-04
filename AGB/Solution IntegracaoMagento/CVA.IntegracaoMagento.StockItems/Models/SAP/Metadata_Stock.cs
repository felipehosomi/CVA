using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVA.IntegracaoMagento.StockItems.Models.SAP
{
    public class Metadata_Stock
    {        
        public class Stock
        {
            [JsonProperty("ItemCode")]
            public string ItemCode { get; set; }

            [JsonProperty("CodeBars")]
            public string CodeBars { get; set; }

            [JsonProperty("WareHouse")]
            public string WareHouse { get; set; }

            [JsonProperty("FilialSAP")]
            public string FilialSAP { get; set; }

            [JsonProperty("FilialMagento")]
            public string FilialMagento { get; set; }

            [JsonProperty("DepositoMagento")]
            public string DepositoMagento { get; set; }

            [JsonProperty("OnHand")]
            public double OnHand { get; set; }

            [JsonProperty("ItemMagento")]
            public string ItemMagento { get; set; }

            [JsonProperty("OnHandMagento")]
            public double OnHandMagento { get; set; }

            [JsonProperty("Novo")]
            public string Novo { get; set; }

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
