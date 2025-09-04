using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.StockItems.Models.Magento
{
    public class SourceItems
    {
        [JsonProperty("sourceItems")]
        public IList<SourceItem> sourceItems { get; set; }
        public class SourceItem
        {
            [JsonProperty("sku")]
            public string sku { get; set; }

            [JsonProperty("source_code")]
            public string source_code { get; set; }

            [JsonProperty("quantity")]
            public double quantity { get; set; }

            [JsonProperty("status")]
            public int status { get; set; }
        }
    }
}
