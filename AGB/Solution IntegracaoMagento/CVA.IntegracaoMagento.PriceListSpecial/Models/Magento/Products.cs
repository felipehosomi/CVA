using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVA.IntegracaoMagento.PriceListSpecial.Models.Magento
{
    public class Products
    {
        public class Product
        {
            public class Custom_Attributes
            {
                [JsonProperty("attribute_code")]
                public string special_price = "special_price";

                [JsonProperty("value")]
                public string special_price_value { get; set; }

                [JsonProperty("attribute_code")]
                public string special_from_date = "special_from_date";

                [JsonProperty("value")]
                public string special_from_date_value { get; set; }

                [JsonProperty("attribute_code")]
                public string special_to_date = "special_to_date";

                [JsonProperty("value")]
                public string special_to_date_value { get; set; }
            }

            [JsonProperty("custom_attributes")]
            public IList<Custom_Attributes> custom_attributes { get; set; }
        }

        [JsonProperty("product")]
        public Product product { get; set; }
        
    }
}
