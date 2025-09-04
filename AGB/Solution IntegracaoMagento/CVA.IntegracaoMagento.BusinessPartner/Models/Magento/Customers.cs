using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVA.IntegracaoMagento.BusinessPartner.Models.Magento
{
    public class Customers
    {
        public class Customer
        {
            [JsonProperty("id")]
            public int? id { get; set; }

            [JsonProperty("dob")]
            public string dob { get; set; }

            [JsonProperty("email")]
            public string email { get; set; }

            [JsonProperty("firstname")]
            public string firstname { get; set; }

            [JsonProperty("lastname")]
            public string lastname { get; set; }

            [JsonProperty("gender")]
            public int gender { get; set; }

            [JsonProperty("taxvat")]
            public string taxvat { get; set; }

            [JsonProperty("codigo_cliente")]
            public string codigo_cliente { get; set; }

            [JsonProperty("group_id")]
            public int group_id { get; set; }

            [JsonProperty("website_id")]
            public int website_id { get; set; }

            [JsonProperty("addresses")]
            public IList<Address> addresses { get; set; }

            [JsonProperty("custom_attributes")]
            public IList<Custom_Attribute> custom_attributes { get; set; }

            public class Address
            {
                [JsonProperty("defaultShipping")]
                public bool defaultShipping { get; set; }

                [JsonProperty("defaultBilling")]
                public bool defaultBilling { get; set; }

                [JsonProperty("firstname")]
                public string firstname { get; set; }

                [JsonProperty("lastname")]
                public string lastname { get; set; }

                [JsonProperty("country_id")]
                public string country_id { get; set; }

                [JsonProperty("region")]
                public Region region { get; set; }

                [JsonProperty("postcode")]
                public string postcode { get; set; }

                [JsonProperty("street")]
                public IList<string> street { get; set; }

                [JsonProperty("city")]
                public string city { get; set; }

                [JsonProperty("telephone")]
                public string telephone { get; set; }

                [JsonProperty("fax")]
                public string fax { get; set; }

                [JsonProperty("countryId")]
                public string countryId { get; set; }

                [JsonProperty("vat_id")]
                public string vat_id { get; set; }
            }

            public class Region
            {
                [JsonProperty("regionCode")]
                public string regionCode { get; set; }

                [JsonProperty("region")]
                public string region { get; set; }

                [JsonProperty("regionId")]
                public int regionId { get; set; }
            }

            public class Custom_Attribute
            {
                [JsonProperty("attribute_code")]
                public string attribute_code { get; set; }

                [JsonProperty("value")]
                public string value { get; set; }
            }
        }

        [JsonProperty("customer")]
        public Customer customer { get; set; }
    }
}
