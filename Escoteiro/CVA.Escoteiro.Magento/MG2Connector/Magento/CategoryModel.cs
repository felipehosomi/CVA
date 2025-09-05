using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVA.Escoteiro.Magento.Models.Magento
{

    public class Category
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("children")]
        public string Children { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("available_sort_by")]
        public IList<string> AvailableSortBy { get; set; }

        [JsonProperty("include_in_menu")]
        public bool IncludeInMenu { get; set; }

    }

    public class ProductCategory
    {

        [JsonProperty("category")]
        public Category Category { get; set; }
    }
}
