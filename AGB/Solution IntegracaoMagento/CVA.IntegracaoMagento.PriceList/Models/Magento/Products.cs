using Newtonsoft.Json;

namespace CVA.IntegracaoMagento.PriceList.Models.Magento
{
    public class Products
    {
        public class Product
        {
            [JsonProperty("Price")]
            public double price { get; set; }
        }

        [JsonProperty("product")]
        public Product product { get; set; }
        
    }
}
