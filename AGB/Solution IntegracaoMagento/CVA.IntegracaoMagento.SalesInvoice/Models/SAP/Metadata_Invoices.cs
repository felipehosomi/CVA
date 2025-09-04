using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesInvoice.Models.SAP
{
    public class Metadata_Invoices
    {
        public class Invoice
        {
            [JsonProperty("DocEntry")]
            public int DocEntry { get; set; }

            [JsonProperty("DocNum")]
            public int DocNum { get; set; }

            [JsonProperty("BPLId")]
            public int BPLId { get; set; }

            [JsonProperty("ObjType")]
            public int ObjType { get; set; }

            [JsonProperty("CardCode")]
            public string CardCode { get; set; }

            [JsonProperty("U_CVA_SourceChannel")]
            public string U_CVA_SourceChannel { get; set; }

            [JsonProperty("U_CVA_Vcto", NullValueHandling = NullValueHandling.Ignore)]
            public DateTime U_CVA_Vcto { get; set; }

            [JsonProperty("U_Adiant")]
            public string U_Adiant { get; set; }

            [JsonProperty("DocEntryAD")]
            public int DocEntryAD { get; set; }

            [JsonProperty("DocEntryCR")]
            public int DocEntryCR { get; set; }
        }
        public class Example
        {
            [JsonProperty("@odata.context")]
            public string odatacontext { get; set; }

            [JsonProperty("value")]
            public IList<Invoice> value { get; set; }
        }
    }
}
