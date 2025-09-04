using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesOrder.Models.SAP
{
    public class Metadata_CreditCard
    {
        public class CreditCard
        {
            public int CreditCardCode { get; set; }
            public string CreditCardName { get; set; }
            public string GLAccount { get; set; }
            public string Telephone { get; set; }
            public string CompanyID { get; set; }
            public string CountryCode { get; set; }
        }

        public class Content
        {
            [JsonProperty("@odata.metadata")]
            public string odatametadata { get; set; }
            [JsonProperty("value")]
            public IList<CreditCard> value { get; set; }
        }
    }
}
