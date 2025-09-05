namespace CVA.Escoteiro.Magento.Models.Magento
{

    public class OrdersListModel
    {
        public Item[] items { get; set; }
        public Search_Criteria search_criteria { get; set; }
        public float total_count { get; set; }
    }

    public class Search_Criteria
    {
        public Filter_Groups[] filter_groups { get; set; }
        public float page_size { get; set; }
        public float current_page { get; set; }
    }

    public class Filter_Groups
    {
        public Filter[] filters { get; set; }
    }

    public class Filter
    {
        public string field { get; set; }
        public string value { get; set; }
        public string condition_type { get; set; }
    }

    public class Item
    {
        //public string applied_rule_ids { get; set; }
        //public string base_currency_code { get; set; }
        //public float  base_discount_amount { get; set; }
        //public float base_grand_total { get; set; }
        //public float base_discount_tax_compensation_amount { get; set; }
        //public float base_shipping_amount { get; set; }
        //public float base_shipping_discount_amount { get; set; }
        //public float base_shipping_discount_tax_compensation_amnt { get; set; }
        //public float base_shipping_incl_tax { get; set; }
        //public float base_shipping_tax_amount { get; set; }
        //public float base_subtotal { get; set; }
        //public float base_subtotal_incl_tax { get; set; }
        //public float base_tax_amount { get; set; }
        //public float base_total_due { get; set; }
        //public float  base_to_global_rate { get; set; }
        //public float  base_to_order_rate { get; set; }
        public float billing_address_id { get; set; }
        public string created_at { get; set; }
        public string customer_email { get; set; }
        public string customer_firstname { get; set; }
        //public float  customer_group_id { get; set; }
        public float customer_id { get; set; }
        //public float  customer_is_guest { get; set; }
        public string customer_lastname { get; set; }
        //public float  customer_note_notify { get; set; }
        public string customer_taxvat { get; set; }
        public float discount_percent { get; set; }
        //public float  discount_amount { get; set; }
        //public string discount_description { get; set; }
        //public float  email_sent { get; set; }
        public int entity_id { get; set; }
        //public string global_currency_code { get; set; }
        //public float grand_total { get; set; }
        //public float  discount_tax_compensation_amount { get; set; }
        public string increment_id { get; set; }
        //public float  is_virtual { get; set; }
        //public string order_currency_code { get; set; }
        //public string protect_code { get; set; }
        //public float  quote_id { get; set; }
        //public string remote_ip { get; set; }
        //public float shipping_amount { get; set; }
        public string shipping_description { get; set; }
        //public float shipping_discount_amount { get; set; }
        //public float shipping_discount_tax_compensation_amount { get; set; }
        //public float shipping_incl_tax { get; set; }
        //public float shipping_tax_amount { get; set; }
        public string state { get; set; }
        public string status { get; set; }
        //public string store_currency_code { get; set; }
        //public float  store_id { get; set; }
        //public string store_name { get; set; }
        //public float  store_to_base_rate { get; set; }
        //public float  store_to_order_rate { get; set; }
        //public float subtotal { get; set; }
        //public float subtotal_incl_tax { get; set; }
        //public float  tax_amount { get; set; }
        //public float total_due { get; set; }
        //public float  total_item_count { get; set; }
        //public float  total_qty_ordered { get; set; }
        public string updated_at { get; set; }
        //public float weight { get; set; }
        public Item2[] items { get; set; }
        public Billing_Address billing_address { get; set; }
        public Payment payment { get; set; }
        //public Status_Histories[] status_histories { get; set; }
        public Extension_Attributes extension_attributes { get; set; }
    }

    public class Billing_Address
    {
        public string address_type { get; set; }
        public string city { get; set; }
        public string country_id { get; set; }
        public float customer_address_id { get; set; }
        public float customer_id { get; set; }
        public string email { get; set; }
        public float entity_id { get; set; }
        public string fax { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public float parent_id { get; set; }
        public string postcode { get; set; }
        public string region { get; set; }
        public string region_code { get; set; }
        public float region_id { get; set; }
        public string[] street { get; set; }
        public string telephone { get; set; }
    }

    public class Address2
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public Region region { get; set; }
        public int region_id { get; set; }
        public string country_id { get; set; }
        public string[] street { get; set; }
        public string telephone { get; set; }
        public string fax { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public bool default_shipping { get; set; }
        public bool default_billing { get; set; }
    }

    public class Region
    {
        public string region_code { get; set; }
        public string region { get; set; }
        public int region_id { get; set; }
    }

    public class Payment
    {
        public object account_status { get; set; }
        public string[] additional_information { get; set; }
        public float amount_authorized { get; set; }
        public float amount_ordered { get; set; }
        public float base_amount_authorized { get; set; }
        public float base_amount_ordered { get; set; }
        public float base_shipping_amount { get; set; }
        public string cc_exp_month { get; set; }
        public string cc_exp_year { get; set; }
        public string cc_last4 { get; set; }
        public string cc_owner { get; set; }
        public string cc_type { get; set; }
        public float entity_id { get; set; }
        public string method { get; set; }
        public float parent_id { get; set; }
        public float shipping_amount { get; set; }
    }

    public class Extension_Attributes
    {
        public Shipping_Assignments[] shipping_assignments { get; set; }
        public Payment_Additional_Info[] payment_additional_info { get; set; }
        //public object[] applied_taxes { get; set; }
        //public object[] item_applied_taxes { get; set; }
    }

    public class Shipping_Assignments
    {
        public Shipping shipping { get; set; }
        //public Item1[] items { get; set; }
    }

    public class Shipping
    {
        public Address address { get; set; }
        public string method { get; set; }
        public Total total { get; set; }
    }

    public class Address
    {
        public string address_type { get; set; }
        public string city { get; set; }
        public string country_id { get; set; }
        public float customer_address_id { get; set; }
        public float customer_id { get; set; }
        public string email { get; set; }
        public float entity_id { get; set; }
        public string fax { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public float parent_id { get; set; }
        public string postcode { get; set; }
        public string region { get; set; }
        public string region_code { get; set; }
        public float region_id { get; set; }
        public string[] street { get; set; }
        public string telephone { get; set; }
        public string vat_id { get; set; }
    }

    public class Total
    {
        //public float base_shipping_amount { get; set; }
        //public float base_shipping_discount_amount { get; set; }
        //public float base_shipping_discount_tax_compensation_amnt { get; set; }
        public float base_shipping_incl_tax { get; set; }
        //public float base_shipping_tax_amount { get; set; }
        //public float shipping_amount { get; set; }
        //public float shipping_discount_amount { get; set; }
        //public float shipping_discount_tax_compensation_amount { get; set; }
        public float shipping_incl_tax { get; set; }
        //public float shipping_tax_amount { get; set; }
    }

    public class Item1
    {
        public float amount_refunded { get; set; }
        public string applied_rule_ids { get; set; }
        public float base_amount_refunded { get; set; }
        public float base_discount_amount { get; set; }
        public float base_discount_invoiced { get; set; }
        public float base_discount_tax_compensation_amount { get; set; }
        public float base_original_price { get; set; }
        public float base_price { get; set; }
        public float base_price_incl_tax { get; set; }
        public float base_row_invoiced { get; set; }
        public float base_row_total { get; set; }
        public float base_row_total_incl_tax { get; set; }
        public float base_tax_amount { get; set; }
        public float base_tax_invoiced { get; set; }
        public float base_weee_tax_applied_amount { get; set; }
        public float base_weee_tax_applied_row_amnt { get; set; }
        public float base_weee_tax_disposition { get; set; }
        public float base_weee_tax_row_disposition { get; set; }
        public string created_at { get; set; }
        public float discount_amount { get; set; }
        public float discount_invoiced { get; set; }
        public float discount_percent { get; set; }
        public float free_shipping { get; set; }
        public float discount_tax_compensation_amount { get; set; }
        public float is_qty_decimal { get; set; }
        public float is_virtual { get; set; }
        public float item_id { get; set; }
        public string name { get; set; }
        public float no_discount { get; set; }
        public float order_id { get; set; }
        public float original_price { get; set; }
        public float price { get; set; }
        public float price_incl_tax { get; set; }
        public float product_id { get; set; }
        public string product_type { get; set; }
        public float qty_canceled { get; set; }
        public float qty_invoiced { get; set; }
        public float qty_ordered { get; set; }
        public float qty_refunded { get; set; }
        public float qty_shipped { get; set; }
        public float quote_item_id { get; set; }
        public float row_invoiced { get; set; }
        public float row_total { get; set; }
        public float row_total_incl_tax { get; set; }
        public float row_weight { get; set; }
        public string sku { get; set; }
        public float store_id { get; set; }
        public float tax_amount { get; set; }
        public float tax_invoiced { get; set; }
        public float tax_percent { get; set; }
        public string updated_at { get; set; }
        public string weee_tax_applied { get; set; }
        public float weee_tax_applied_amount { get; set; }
        public float weee_tax_applied_row_amount { get; set; }
        public float weee_tax_disposition { get; set; }
        public float weee_tax_row_disposition { get; set; }
        public float weight { get; set; }
        public Product_Option product_option { get; set; }
        public float parent_item_id { get; set; }
        public Parent_Item parent_item { get; set; }
    }

    public class Product_Option
    {
        public Extension_Attributes1 extension_attributes { get; set; }
    }

    public class Extension_Attributes1
    {
        public Configurable_Item_Options[] configurable_item_options { get; set; }
    }

    public class Configurable_Item_Options
    {
        public string option_id { get; set; }
        public float option_value { get; set; }
    }

    public class Parent_Item
    {
        public float amount_refunded { get; set; }
        public string applied_rule_ids { get; set; }
        public float base_amount_refunded { get; set; }
        public float base_discount_amount { get; set; }
        public float base_discount_invoiced { get; set; }
        public float base_discount_tax_compensation_amount { get; set; }
        public float base_original_price { get; set; }
        public float base_price { get; set; }
        public float base_price_incl_tax { get; set; }
        public float base_row_invoiced { get; set; }
        public float base_row_total { get; set; }
        public float base_row_total_incl_tax { get; set; }
        public float base_tax_amount { get; set; }
        public float base_tax_invoiced { get; set; }
        public float base_weee_tax_applied_amount { get; set; }
        public float base_weee_tax_applied_row_amnt { get; set; }
        public float base_weee_tax_disposition { get; set; }
        public float base_weee_tax_row_disposition { get; set; }
        public string created_at { get; set; }
        public float discount_amount { get; set; }
        public float discount_invoiced { get; set; }
        public float discount_percent { get; set; }
        public float free_shipping { get; set; }
        public float discount_tax_compensation_amount { get; set; }
        public float is_qty_decimal { get; set; }
        public float is_virtual { get; set; }
        public float item_id { get; set; }
        public string name { get; set; }
        public float no_discount { get; set; }
        public float order_id { get; set; }
        public float original_price { get; set; }
        public float price { get; set; }
        public float price_incl_tax { get; set; }
        public float product_id { get; set; }
        public string product_type { get; set; }
        public float qty_canceled { get; set; }
        public float qty_invoiced { get; set; }
        public float qty_ordered { get; set; }
        public float qty_refunded { get; set; }
        public float qty_shipped { get; set; }
        public float quote_item_id { get; set; }
        public float row_invoiced { get; set; }
        public float row_total { get; set; }
        public float row_total_incl_tax { get; set; }
        public float row_weight { get; set; }
        public string sku { get; set; }
        public float store_id { get; set; }
        public float tax_amount { get; set; }
        public float tax_invoiced { get; set; }
        public float tax_percent { get; set; }
        public string updated_at { get; set; }
        public string weee_tax_applied { get; set; }
        public float weee_tax_applied_amount { get; set; }
        public float weee_tax_applied_row_amount { get; set; }
        public float weee_tax_disposition { get; set; }
        public float weee_tax_row_disposition { get; set; }
        public float weight { get; set; }
        public Product_Option1 product_option { get; set; }
    }

    public class Product_Option1
    {
        public Extension_Attributes2 extension_attributes { get; set; }
    }

    public class Extension_Attributes2
    {
        public Configurable_Item_Options1[] configurable_item_options { get; set; }
    }

    public class Configurable_Item_Options1
    {
        public string option_id { get; set; }
        public float option_value { get; set; }
    }

    public class Payment_Additional_Info
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class Item2
    {
        //public float  amount_refunded { get; set; }
        //public string applied_rule_ids { get; set; }
        //public float  base_amount_refunded { get; set; }
        //public float  base_discount_amount { get; set; }
        //public float  base_discount_invoiced { get; set; }
        //public float  base_discount_tax_compensation_amount { get; set; }
        //public float base_original_price { get; set; }
        //public float base_price { get; set; }
        //public float base_price_incl_tax { get; set; }
        //public float  base_row_invoiced { get; set; }
        //public float base_row_total { get; set; }
        //public float base_row_total_incl_tax { get; set; }
        //public float  base_tax_amount { get; set; }
        //public float  base_tax_invoiced { get; set; }
        //public float  base_weee_tax_applied_amount { get; set; }
        //public float  base_weee_tax_applied_row_amnt { get; set; }
        //public float  base_weee_tax_disposition { get; set; }
        //public float  base_weee_tax_row_disposition { get; set; }
        //public string created_at { get; set; }
        //public float  discount_amount { get; set; }
        //public float  discount_invoiced { get; set; }
        public float  discount_percent { get; set; }
        //public float  free_shipping { get; set; }
        //public float  discount_tax_compensation_amount { get; set; }
        //public float  is_qty_decimal { get; set; }
        //public float  is_virtual { get; set; }
        //public float  item_id { get; set; }
        //public string name { get; set; }
        //public float  no_discount { get; set; }
        //public float  order_id { get; set; }
        //public float original_price { get; set; }
        public float? price { get; set; }
        //public float price_incl_tax { get; set; }
        //public float  product_id { get; set; }
        public string product_type { get; set; }
        //public float  qty_canceled { get; set; }
        //public float  qty_invoiced { get; set; }
        public float qty_ordered { get; set; }
        //public float  qty_refunded { get; set; }
        //public float  qty_shipped { get; set; }
        //public float  quote_item_id { get; set; }
        //public float  row_invoiced { get; set; }
        //public float row_total { get; set; }
        //public float row_total_incl_tax { get; set; }
        //public float  row_weight { get; set; }
        public string sku { get; set; }
        //public float  store_id { get; set; }
        //public float  tax_amount { get; set; }
        //public float  tax_invoiced { get; set; }
        //public float  tax_percent { get; set; }
        //public string updated_at { get; set; }
        //public string weee_tax_applied { get; set; }
        //public float  weee_tax_applied_amount { get; set; }
        //public float  weee_tax_applied_row_amount { get; set; }
        //public float  weee_tax_disposition { get; set; }
        //public float  weee_tax_row_disposition { get; set; }
        public float weight { get; set; }
        //public Product_Option2 product_option { get; set; }
        //public float  parent_item_id { get; set; }
        //public Parent_Item1 parent_item { get; set; }
    }

    public class Product_Option2
    {
        public Extension_Attributes3 extension_attributes { get; set; }
    }

    public class Extension_Attributes3
    {
        public Configurable_Item_Options2[] configurable_item_options { get; set; }
    }

    public class Configurable_Item_Options2
    {
        public string option_id { get; set; }
        public float option_value { get; set; }
    }

    public class Parent_Item1
    {
        public float amount_refunded { get; set; }
        public string applied_rule_ids { get; set; }
        public float base_amount_refunded { get; set; }
        public float base_discount_amount { get; set; }
        public float base_discount_invoiced { get; set; }
        public float base_discount_tax_compensation_amount { get; set; }
        public float base_original_price { get; set; }
        public float base_price { get; set; }
        public float base_price_incl_tax { get; set; }
        public float base_row_invoiced { get; set; }
        public float base_row_total { get; set; }
        public float base_row_total_incl_tax { get; set; }
        public float base_tax_amount { get; set; }
        public float base_tax_invoiced { get; set; }
        public float base_weee_tax_applied_amount { get; set; }
        public float base_weee_tax_applied_row_amnt { get; set; }
        public float base_weee_tax_disposition { get; set; }
        public float base_weee_tax_row_disposition { get; set; }
        public string created_at { get; set; }
        public float discount_amount { get; set; }
        public float discount_invoiced { get; set; }
        public float discount_percent { get; set; }
        public float free_shipping { get; set; }
        public float discount_tax_compensation_amount { get; set; }
        public float is_qty_decimal { get; set; }
        public float is_virtual { get; set; }
        public float item_id { get; set; }
        public string name { get; set; }
        public float no_discount { get; set; }
        public float order_id { get; set; }
        public float original_price { get; set; }
        public float price { get; set; }
        public float price_incl_tax { get; set; }
        public float product_id { get; set; }
        public string product_type { get; set; }
        public float qty_canceled { get; set; }
        public float qty_invoiced { get; set; }
        public float qty_ordered { get; set; }
        public float qty_refunded { get; set; }
        public float qty_shipped { get; set; }
        public float quote_item_id { get; set; }
        public float row_invoiced { get; set; }
        public float row_total { get; set; }
        public float row_total_incl_tax { get; set; }
        public float row_weight { get; set; }
        public string sku { get; set; }
        public float store_id { get; set; }
        public float tax_amount { get; set; }
        public float tax_invoiced { get; set; }
        public float tax_percent { get; set; }
        public string updated_at { get; set; }
        public string weee_tax_applied { get; set; }
        public float weee_tax_applied_amount { get; set; }
        public float weee_tax_applied_row_amount { get; set; }
        public float weee_tax_disposition { get; set; }
        public float weee_tax_row_disposition { get; set; }
        public float weight { get; set; }
        public Product_Option3 product_option { get; set; }
    }

    public class Product_Option3
    {
        public Extension_Attributes4 extension_attributes { get; set; }
    }

    public class Extension_Attributes4
    {
        public Configurable_Item_Options3[] configurable_item_options { get; set; }
    }

    public class Configurable_Item_Options3
    {
        public string option_id { get; set; }
        public float option_value { get; set; }
    }

    public class Status_Histories
    {
        public string comment { get; set; }
        public string created_at { get; set; }
        public float entity_id { get; set; }
        public int? is_customer_notified { get; set; }
        public float is_visible_on_front { get; set; }
        public float parent_id { get; set; }
        public string status { get; set; }
        public string entity_name { get; set; }
    }

}
