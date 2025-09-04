using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.Items.Models.Magento
{
    public class Products
    {
        public class Product
        {

            public class Custom_Attributes
            {
                [JsonProperty("attribute_code")]
                public string tax_class_id = "tax_class_id";

                [JsonProperty("value")]
                public string tax_class_id_value { get; set; }

                [JsonProperty("attribute_code")]
                public string volume_lenght = "volume_lenght";

                [JsonProperty("value")]
                public string volume_lenght_value { get; set; }

                [JsonProperty("attribute_code")]
                public string volume_height = "volume_height";

                [JsonProperty("value")]
                public string volume_height_value { get; set; }

                [JsonProperty("attribute_code")]
                public string volume_width = "volume_width";

                [JsonProperty("value")]
                public string volume_width_value { get; set; }

                [JsonProperty("attribute_code")]
                public string ean = "ean";

                [JsonProperty("value")]
                public string ean_value { get; set; }
            }

            [JsonProperty("attribute_set_id")]
            public int attribute_set_id = 4;

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("sku")]
            public string sku { get; set; }

            [JsonProperty("visibility")]
            public int visibility = 4;

            [JsonProperty("type_id")]
            public string type_id { get; set; }

            [JsonProperty("weight")]
            public string weight { get; set; }

            [JsonProperty("Price")]
            public double price = 0.00;

            [JsonProperty("status")]
            public int status { get; set; }

            [JsonProperty("custom_attributes")]
            public IList<Custom_Attributes> custom_attributes { get; set; }
        }

        [JsonProperty("product")]
        public Product product { get; set; }
        
    }
}
