using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesInvoice.Models.SAP
{
    public class Orders
    {
        public class Order
        {
            [JsonProperty("odata.metadata")]
            public string odatametadata { get; set; }

            [JsonProperty("DocEntry")]
            public int DocEntry { get; set; }

            [JsonProperty("DocNum")]
            public int DocNum { get; set; }

            [JsonProperty("CardCode")]
            public string CardCode { get; set; }

            [JsonProperty("DocTotal")]
            public double DocTotal { get; set; }

            [JsonProperty("DocumentLines")]
            public List<DocumentLine> DocumentLines { get; set; }
        }

        public partial class DocumentLine
        {
            [JsonProperty("LineNum")]
            public int LineNum { get; set; }

            [JsonProperty("ItemCode")]
            public string ItemCode { get; set; }

            [JsonProperty("Quantity")]
            public double Quantity { get; set; }

            [JsonProperty("UnitPrice")]
            public double UnitPrice { get; set; }
        }
    }
}