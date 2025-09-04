using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesInvoice.Models.SAP
{
    public class Invoices
    {        
        [JsonProperty("CardCode")]
        public string CardCode { get; set; }

        [JsonProperty("DocType")]
        public string DocType { get; set; }

        [JsonProperty("DocCurrency")]
        public string DocCurrency { get; set; }

        [JsonProperty("DocRate")]
        public double DocRate { get; set; }

        [JsonProperty("BPL_IDAssignedToInvoice")]
        public int BPL_IDAssignedToInvoice { get; set; }

        [JsonProperty("U_CVA_SourceChannel")]
        public string U_CVA_SourceChannel { get; set; }

        [JsonProperty("U_nfe_lib_env")]
        public string U_nfe_lib_env { get; set; }

        [JsonProperty("U_nfe_indPres")]
        public int U_nfe_indPres { get; set; }

        [JsonProperty("DocDueDate")]
        public DateTime DocDueDate { get; set; }

        [JsonProperty("DocumentLines")]
        public List<DocumentLine> DocumentLines { get; set; }

        [JsonProperty("DownPaymentsToDraw")]
        public List<DownPaymentsToDraw> DownPaymentsToDraws { get; set; }

        public partial class DocumentLine
        {
            [JsonProperty("BaseType")]
            public int BaseType { get; set; }

            [JsonProperty("BaseEntry")]
            public int BaseEntry { get; set; }

            [JsonProperty("BaseLine")]
            public int BaseLine { get; set; }

            [JsonProperty("ItemCode")]
            public string ItemCode { get; set; }

            [JsonProperty("Quantity")]
            public double Quantity { get; set; }

            [JsonProperty("UnitPrice")]
            public double UnitPrice { get; set; }
        }

        public partial class DownPaymentsToDraw
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
    }
}
