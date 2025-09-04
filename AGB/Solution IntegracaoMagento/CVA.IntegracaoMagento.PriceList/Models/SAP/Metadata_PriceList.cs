using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.PriceList.Models.SAP
{
    public class Metadata_PriceList
    {
        public class Items
        {
            [JsonProperty("CodigoSAP")]
            public string ItemCode { get; set; }

            [JsonProperty("Price")]
            public double Price { get; set; }

            [JsonProperty("CodigoMagento")]
            public string U_CVA_Magento_Id { get; set; }

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
            public IList<Items> value { get; set; }
        }

        public class ItemSAP
        {            
            [JsonProperty("U_CVA_Magento_Data")]
            public DateTime U_CVA_Magento_Data { get; set; }

            [JsonProperty("U_CVA_Magento_Hora")]
            public int U_CVA_Magento_Hora { get; set; }

            [JsonProperty("U_CVA_Magento_Msg")]
            public string U_CVA_Magento_Msg { get; set; }
        }

    }
}
