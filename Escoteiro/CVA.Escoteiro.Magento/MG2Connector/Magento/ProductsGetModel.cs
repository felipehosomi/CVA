using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.Models.Magento
{
    public class CategoryLinkGet
    {
        public int position { get; set; }
        public string category_id { get; set; }
    }

    public class ExtensionAttributesGet
    {
        public List<int> website_ids { get; set; }
        public List<CategoryLinkGet> category_links { get; set; }
    }

    public class MediaGalleryEntryGet
    {
        public int id { get; set; }
        public string media_type { get; set; }
        public object label { get; set; }
        public int position { get; set; }
        public bool disabled { get; set; }
        public List<string> types { get; set; }
        public string file { get; set; }
    }

    public class CustomAttributeGet
    {
        public string attribute_code { get; set; }
        public object value { get; set; }
    }

    public class ItemGet
    {
        public int id { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public int attribute_set_id { get; set; }
        public double price { get; set; }
        public int status { get; set; }
        public int visibility { get; set; }
        public string type_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public double weight { get; set; }
        public ExtensionAttributesGet extension_attributes { get; set; }
        public List<object> product_links { get; set; }
        public List<object> options { get; set; }
        public List<MediaGalleryEntryGet> media_gallery_entries { get; set; }
        public List<object> tier_prices { get; set; }
        public List<CustomAttributeGet> custom_attributes { get; set; }
    }

    public class FilterGet
    {
        public string field { get; set; }
        public string value { get; set; }
        public string condition_type { get; set; }
    }

    public class FilterGroup
    {
        public List<FilterGet> filters { get; set; }
    }

    public class SearchCriteria
    {
        public List<FilterGroup> filter_groups { get; set; }
    }

    public class ProductsGetModel
    {
        public List<ItemGet> items { get; set; }
        public SearchCriteria search_criteria { get; set; }
        public int total_count { get; set; }
    }
}
