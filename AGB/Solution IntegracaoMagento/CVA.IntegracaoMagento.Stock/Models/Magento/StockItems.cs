using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CVA.IntegracaoMagento.Stock.Models.Magento
{
    public class StockItems
    {
        [JsonProperty("sku")]
        public string sku { get; set; }

        [JsonProperty("qty")]
        public string qty { get; set; }
    }
}
