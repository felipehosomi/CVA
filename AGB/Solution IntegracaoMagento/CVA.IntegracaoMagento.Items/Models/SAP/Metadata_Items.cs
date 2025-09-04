using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.Items.Models.SAP
{
    public class Metadata_Items
    {
        public class Items
        {
            [JsonProperty("ItemName")]
            public string ItemName { get; set; }

            [JsonProperty("ItemCode")]
            public string ItemCode { get; set; }

            [JsonProperty("SWeight1")]
            public string Peso { get; set; }

            [JsonProperty("SLength1")]
            public string Comprimento { get; set; }

            [JsonProperty("SHeight1")]
            public string Altura { get; set; }

            [JsonProperty("SWidth1")]
            public string Largura { get; set; }

            [JsonProperty("Status")]
            public int Status { get; set; }

            [JsonProperty("Price", NullValueHandling = NullValueHandling.Ignore)]
            public double Price { get; set; }

            [JsonProperty("CodeBars")]
            public string CodeBars { get; set; }

            [JsonProperty("U_CVA_Magento_Id")]
            public string U_CVA_Magento_Id { get; set; }

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
            [JsonProperty("U_CVA_Magento_Id")]
            public string U_CVA_Magento_Id { get; set; }

            [JsonProperty("U_CVA_Magento_Data")]
            public DateTime U_CVA_Magento_Data { get; set; }

            [JsonProperty("U_CVA_Magento_Hora")]
            public int U_CVA_Magento_Hora { get; set; }

            [JsonProperty("U_CVA_Magento_Msg")]
            public string U_CVA_Magento_Msg { get; set; }
        }
    }
}
