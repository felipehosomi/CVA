using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVA.IntegracaoMagento.SalesOrderStatus.Models.Magento
{
    public class SalesStatusDespachado
    {
        public class Item2
        {
            [JsonProperty("order_item_id")]
            public int order_item_id { get; set; }

            [JsonProperty("qty")]
            public double qty { get; set; }
        }

        public class Item
        {
            [JsonProperty("items")]
            public IList<Item2> items { get; set; }
        }

        [JsonProperty("items")]
        public Item items { get; set; }
    }

}
