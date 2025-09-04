using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.PriceListSpecial.Models.SAP
{
    public class Metadata_PriceListSpecial
    {
        public class Items
        {
            [JsonProperty("CodigoSAP")]
            public string ItemCode { get; set; }

            [JsonProperty("Price")]
            public double Price { get; set; }

            [JsonProperty("CodigoMagento")]
            public string U_CVA_Magento_Id { get; set; }

            [JsonProperty("ValidFrom")]
            public DateTime ValidFrom { get; set; }

            [JsonProperty("ValidTo")]
            public DateTime ValidTo { get; set; }

            [JsonProperty("id__")]
            public int id__ { get; set; }
        }

        public class Content
        {
            [JsonProperty("@odata.context")]
            public string odatacontext { get; set; }

            [JsonProperty("value")]
            public IList<Items> value { get; set; }
        }

    }
}
