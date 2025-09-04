using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.Invoices.Models.SAP
{
    public class Invoices
    {
        public class DocumentLine
        {
            [JsonProperty("BaseType")]
            public int BaseType { get; set; }

            [JsonProperty("BaseEntry")]
            public int BaseEntry { get; set; }

            [JsonProperty("BaseLine")]
            public int BaseLine { get; set; }
        }

        public class DownPaymentsToDraw
        {
            [JsonProperty("DocEntry")]
            public int DocEntry { get; set; }

            [JsonProperty("AmountToDraw")]
            public double AmountToDraw { get; set; }

            [JsonProperty("DownPaymentType")]
            public string DownPaymentType { get; set; }

            [JsonProperty("DocNumber")]
            public int DocNumber { get; set; }
        }

        public class Invoice
        {
            [JsonProperty("CardCode")]
            public string CardCode { get; set; }

            [JsonProperty("DocDueDate")]
            public string DocDueDate { get; set; }

            [JsonProperty("DocType")]
            public string DocType { get; set; }

            [JsonProperty("DocCurrency")]
            public string DocCurrency { get; set; }

            [JsonProperty("DocRate")]
            public double DocRate { get; set; }

            [JsonProperty("BPL_IDAssignedToInvoice")]
            public int BPL_IDAssignedToInvoice { get; set; }

            [JsonProperty("DocumentLines")]
            public IList<DocumentLine> DocumentLines { get; set; }

            [JsonProperty("DownPaymentsToDraw")]
            public IList<DownPaymentsToDraw> DownPaymentsToDraw { get; set; }
        }
    }
}
