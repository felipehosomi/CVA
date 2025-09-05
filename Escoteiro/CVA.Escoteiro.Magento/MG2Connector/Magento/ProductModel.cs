using System.Collections.Generic;

namespace CVA.Escoteiro.Magento.Models.Magento
{
    public class CategoryLink
    {
        public int position { get; set; }
        public string category_id { get; set; }
    }

    public class ExtensionAttributes3
    {
    }

    public class StockItem
    {
        public int item_id { get; set; }
        public int product_id { get; set; }
        public int stock_id { get; set; }
        public int qty { get; set; }
        public bool is_in_stock { get; set; }
        public bool is_qty_decimal { get; set; }
        public bool show_default_notification_message { get; set; }
        public bool use_config_min_qty { get; set; }
        public int min_qty { get; set; }
        public int use_config_min_sale_qty { get; set; }
        public int min_sale_qty { get; set; }
        public bool use_config_max_sale_qty { get; set; }
        public int max_sale_qty { get; set; }
        public bool use_config_backorders { get; set; }
        public int backorders { get; set; }
        public bool use_config_notify_stock_qty { get; set; }
        public int notify_stock_qty { get; set; }
        public bool use_config_qty_increments { get; set; }
        public int qty_increments { get; set; }
        public bool use_config_enable_qty_inc { get; set; }
        public bool enable_qty_increments { get; set; }
        public bool use_config_manage_stock { get; set; }
        public bool manage_stock { get; set; }
        public string low_stock_date { get; set; }
        public bool is_decimal_divided { get; set; }
        public int stock_status_changed_auto { get; set; }
        public ExtensionAttributes3 extension_attributes { get; set; }
    }

    public class ExtensionAttributes4
    {
    }

    public class ProductLink
    {
        public string id { get; set; }
        public string sku { get; set; }
        public int option_id { get; set; }
        public int qty { get; set; }
        public int position { get; set; }
        public bool is_default { get; set; }
        public int price { get; set; }
        public int price_type { get; set; }
        public int can_change_quantity { get; set; }
        public ExtensionAttributes4 extension_attributes { get; set; }
    }

    public class ExtensionAttributes5
    {
    }

    public class BundleProductOption
    {
        public int option_id { get; set; }
        public string title { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
        public int position { get; set; }
        public string sku { get; set; }
        public List<ProductLink> product_links { get; set; }
        public ExtensionAttributes5 extension_attributes { get; set; }
    }

    public class ExtensionAttributes6
    {
    }

    public class Value
    {
        public int value_index { get; set; }
        public ExtensionAttributes6 extension_attributes { get; set; }
    }

    public class ExtensionAttributes7
    {
    }

    public class ConfigurableProductOption
    {
        public int id { get; set; }
        public string attribute_id { get; set; }
        public string label { get; set; }
        public int position { get; set; }
        public bool is_use_default { get; set; }
        public List<Value> values { get; set; }
        public ExtensionAttributes7 extension_attributes { get; set; }
        public int product_id { get; set; }
    }

    public class ExtensionAttributes8
    {
    }

    public class LinkFileContent
    {
        public string file_data { get; set; }
        public string name { get; set; }
        public ExtensionAttributes8 extension_attributes { get; set; }
    }

    public class ExtensionAttributes9
    {
    }

    public class SampleFileContent
    {
        public string file_data { get; set; }
        public string name { get; set; }
        public ExtensionAttributes9 extension_attributes { get; set; }
    }

    public class ExtensionAttributes10
    {
    }

    public class DownloadableProductLink
    {
        public int id { get; set; }
        public string title { get; set; }
        public int sort_order { get; set; }
        public int is_shareable { get; set; }
        public int price { get; set; }
        public int number_of_downloads { get; set; }
        public string link_type { get; set; }
        public string link_file { get; set; }
        public LinkFileContent link_file_content { get; set; }
        public string link_url { get; set; }
        public string sample_type { get; set; }
        public string sample_file { get; set; }
        public SampleFileContent sample_file_content { get; set; }
        public string sample_url { get; set; }
        public ExtensionAttributes10 extension_attributes { get; set; }
    }

    public class ExtensionAttributes11
    {
    }

    public class SampleFileContent2
    {
        public string file_data { get; set; }
        public string name { get; set; }
        public ExtensionAttributes11 extension_attributes { get; set; }
    }

    public class ExtensionAttributes12
    {
    }

    public class DownloadableProductSample
    {
        public int id { get; set; }
        public string title { get; set; }
        public int sort_order { get; set; }
        public string sample_type { get; set; }
        public string sample_file { get; set; }
        public SampleFileContent2 sample_file_content { get; set; }
        public string sample_url { get; set; }
        public ExtensionAttributes12 extension_attributes { get; set; }
    }

    public class ExtensionAttributes13
    {
    }

    public class GiftcardAmount
    {
        public int attribute_id { get; set; }
        public int website_id { get; set; }
        public int value { get; set; }
        public int website_value { get; set; }
        public ExtensionAttributes13 extension_attributes { get; set; }
    }

    public class ExtensionAttributes
    {
        //public List<int> website_ids { get; set; }
        public List<CategoryLink> category_links { get; set; }
        //public StockItem stock_item { get; set; }
        //public List<BundleProductOption> bundle_product_options { get; set; }
        //public List<ConfigurableProductOption> configurable_product_options { get; set; }
        //public List<int> configurable_product_links { get; set; }
        //public List<DownloadableProductLink> downloadable_product_links { get; set; }
        //public List<DownloadableProductSample> downloadable_product_samples { get; set; }
        //public List<GiftcardAmount> giftcard_amounts { get; set; }
    }

    public class ExtensionAttributes14
    {
        public int qty { get; set; }
    }

    public class ProductLink2
    {
        public string sku { get; set; }
        public string link_type { get; set; }
        public string linked_product_sku { get; set; }
        public string linked_product_type { get; set; }
        public int position { get; set; }
        public ExtensionAttributes14 extension_attributes { get; set; }
    }

    public class Value2
    {
        public string title { get; set; }
        public int sort_order { get; set; }
        public int price { get; set; }
        public string price_type { get; set; }
        public string sku { get; set; }
        public int option_type_id { get; set; }
    }

    public class ExtensionAttributes15
    {
        public string vertex_flex_field { get; set; }
    }

    public class Option
    {
        public string product_sku { get; set; }
        public int option_id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public int sort_order { get; set; }
        public bool is_require { get; set; }
        public int price { get; set; }
        public string price_type { get; set; }
        public string sku { get; set; }
        public string file_extension { get; set; }
        public int max_characters { get; set; }
        public int image_size_x { get; set; }
        public int image_size_y { get; set; }
        public List<Value2> values { get; set; }
        public ExtensionAttributes15 extension_attributes { get; set; }
    }

    public class Content
    {
        public string base64_encoded_data { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }

    public class VideoContent
    {
        public string media_type { get; set; }
        public string video_provider { get; set; }
        public string video_url { get; set; }
        public string video_title { get; set; }
        public string video_description { get; set; }
        public string video_metadata { get; set; }
    }

    public class ExtensionAttributes16
    {
        public VideoContent video_content { get; set; }
    }

    public class MediaGalleryEntry
    {
        public int id { get; set; }
        public string media_type { get; set; }
        public string label { get; set; }
        public int position { get; set; }
        public bool disabled { get; set; }
        public List<string> types { get; set; }
        public string file { get; set; }
        public Content content { get; set; }
        public ExtensionAttributes16 extension_attributes { get; set; }
    }

    public class ExtensionAttributes17
    {
        public int percentage_value { get; set; }
        public int website_id { get; set; }
    }

    public class TierPrice
    {
        public int customer_group_id { get; set; }
        public int qty { get; set; }
        public int value { get; set; }
        public ExtensionAttributes17 extension_attributes { get; set; }
    }

    public class CustomAttribute
    {
        public string attribute_code { get; set; }
        public object value { get; set; }
    }

    public class Product
    {
        public int id { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public int attribute_set_id { get; set; } = 4;
        public object price { get; set; }
        public int status { get; set; } = 1;
        public int visibility { get; set; }
        public string type_id { get; set; } = "simple";
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object weight { get; set; }
        public ExtensionAttributes extension_attributes { get; set; }
        //public List<ProductLink2> product_links { get; set; }
        //public List<Option> options { get; set; }
        //public List<MediaGalleryEntry> media_gallery_entries { get; set; }
        //public List<TierPrice> tier_prices { get; set; }
        public List<CustomAttribute> custom_attributes { get; set; }
    }

    public class ProductModel
    {
        public Product product { get; set; }
    }
}
