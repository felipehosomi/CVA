using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesOrderStatus.Models.SAP
{
    public class Metadata_SalesOrderStatus
    {
        public class Items
        {
            [JsonProperty("U_CVA_Magento_Id")]
            public string U_CVA_Magento_Id { get; set; }

            [JsonProperty("U_CVA_Magento_Entity")]
            public string U_CVA_Magento_Entity { get; set; }

            [JsonProperty("U_CVA_Magento_ItemId")]
            public int U_CVA_Magento_ItemId { get; set; }

            [JsonProperty("Id_Pedido_SAP")]
            public int Id_Pedido_SAP { get; set; }

            [JsonProperty("Quantity")]
            public double Quantity { get; set; }

            [JsonProperty("CardCode")]
            public string CardCode { get; set; }

            [JsonProperty("CardFName")]
            public string CardFName { get; set; }

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

        public class Sales
        {
            [JsonProperty("U_CVA_Magento_Data")]
            public string U_CVA_Magento_Data { get; set; }

            [JsonProperty("U_CVA_Magento_Hora")]
            public int U_CVA_Magento_Hora { get; set; }
            [JsonProperty("U_CVA_Magento_Status")]
            public string U_CVA_Magento_Status { get; set; }

            [JsonProperty("U_CVA_Magento_Msg")]
            public string U_CVA_Magento_Msg { get; set; }
        }
    }
}
