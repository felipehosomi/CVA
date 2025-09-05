using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EscoteirosShip.Models.Magento
{
    public class Orders
    {
        [JsonProperty("base_currency_code")]
        public string base_currency_code { get; set; }

        [JsonProperty("base_discount_amount")]
        public double base_discount_amount { get; set; }

        [JsonProperty("base_discount_invoiced")]
        public double base_discount_invoiced { get; set; }

        [JsonProperty("base_grand_total")]
        public double base_grand_total { get; set; }

        [JsonProperty("base_shipping_amount")]
        public double base_shipping_amount { get; set; }

        [JsonProperty("base_discount_tax_compensation_amount")]
        public double base_discount_tax_compensation_amount { get; set; }

        [JsonProperty("base_discount_tax_compensation_invoiced")]
        public double base_discount_tax_compensation_invoiced { get; set; }

        [JsonProperty("base_shipping_discount_amount")]
        public double base_shipping_discount_amount { get; set; }

        [JsonProperty("base_shipping_discount_tax_compensation_amnt")]
        public double base_shipping_discount_tax_compensation_amnt { get; set; }

        [JsonProperty("base_shipping_incl_tax")]
        public double base_shipping_incl_tax { get; set; }

        [JsonProperty("base_shipping_invoiced")]
        public double base_shipping_invoiced { get; set; }

        [JsonProperty("base_shipping_tax_amount")]
        public double base_shipping_tax_amount { get; set; }

        [JsonProperty("base_subtotal")]
        public double base_subtotal { get; set; }

        [JsonProperty("base_subtotal_incl_tax")]
        public double base_subtotal_incl_tax { get; set; }

        [JsonProperty("base_subtotal_invoiced")]
        public double base_subtotal_invoiced { get; set; }

        [JsonProperty("base_tax_amount")]
        public double base_tax_amount { get; set; }

        [JsonProperty("base_tax_invoiced")]
        public double base_tax_invoiced { get; set; }

        [JsonProperty("base_total_due")]
        public double base_total_due { get; set; }

        [JsonProperty("base_total_invoiced")]
        public double base_total_invoiced { get; set; }

        [JsonProperty("base_total_invoiced_cost")]
        public int base_total_invoiced_cost { get; set; }

        [JsonProperty("base_total_paid")]
        public double base_total_paid { get; set; }

        [JsonProperty("base_to_global_rate")]
        public double base_to_global_rate { get; set; }

        [JsonProperty("base_to_order_rate")]
        public double base_to_order_rate { get; set; }

        [JsonProperty("billing_address_id")]
        public int billing_address_id { get; set; }

        [JsonProperty("created_at")]
        public string created_at { get; set; }

        [JsonProperty("customer_dob")]
        public string customer_dob { get; set; }

        [JsonProperty("customer_email")]
        public string customer_email { get; set; }

        [JsonProperty("customer_firstname")]
        public string customer_firstname { get; set; }

        [JsonProperty("customer_group_id")]
        public int customer_group_id { get; set; }

        [JsonProperty("customer_id")]
        public int customer_id { get; set; }

        [JsonProperty("customer_is_guest")]
        public int customer_is_guest { get; set; }

        [JsonProperty("customer_lastname")]
        public string customer_lastname { get; set; }

        [JsonProperty("customer_note_notify")]
        public int customer_note_notify { get; set; }

        [JsonProperty("customer_taxvat")]
        public string customer_taxvat { get; set; }

        [JsonProperty("discount_amount")]
        public double discount_amount { get; set; }

        [JsonProperty("discount_invoiced")]
        public double discount_invoiced { get; set; }

        [JsonProperty("email_sent")]
        public int email_sent { get; set; }

        [JsonProperty("entity_id")]
        public int entity_id { get; set; }

        [JsonProperty("global_currency_code")]
        public string global_currency_code { get; set; }

        [JsonProperty("grand_total")]
        public double grand_total { get; set; }

        [JsonProperty("discount_tax_compensation_amount")]
        public int discount_tax_compensation_amount { get; set; }

        [JsonProperty("discount_tax_compensation_invoiced")]
        public int discount_tax_compensation_invoiced { get; set; }

        [JsonProperty("increment_id")]
        public string increment_id { get; set; }

        [JsonProperty("is_virtual")]
        public int is_virtual { get; set; }

        [JsonProperty("order_currency_code")]
        public string order_currency_code { get; set; }

        [JsonProperty("protect_code")]
        public string protect_code { get; set; }

        [JsonProperty("quote_id")]
        public int quote_id { get; set; }

        [JsonProperty("remote_ip")]
        public string remote_ip { get; set; }

        [JsonProperty("shipping_amount")]
        public double shipping_amount { get; set; }

        [JsonProperty("shipping_description")]
        public string shipping_description { get; set; }

        [JsonProperty("shipping_discount_amount")]
        public double shipping_discount_amount { get; set; }

        [JsonProperty("shipping_discount_tax_compensation_amount")]
        public double shipping_discount_tax_compensation_amount { get; set; }

        [JsonProperty("shipping_incl_tax")]
        public double shipping_incl_tax { get; set; }

        [JsonProperty("shipping_invoiced")]
        public double shipping_invoiced { get; set; }

        [JsonProperty("shipping_tax_amount")]
        public double shipping_tax_amount { get; set; }

        [JsonProperty("state")]
        public string state { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("store_currency_code")]
        public string store_currency_code { get; set; }

        [JsonProperty("store_id")]
        public int store_id { get; set; }

        [JsonProperty("store_name")]
        public string store_name { get; set; }

        [JsonProperty("store_to_base_rate")]
        public int store_to_base_rate { get; set; }

        [JsonProperty("store_to_order_rate")]
        public int store_to_order_rate { get; set; }

        [JsonProperty("subtotal")]
        public double subtotal { get; set; }

        [JsonProperty("subtotal_incl_tax")]
        public double subtotal_incl_tax { get; set; }

        [JsonProperty("subtotal_invoiced")]
        public double subtotal_invoiced { get; set; }

        [JsonProperty("tax_amount")]
        public double tax_amount { get; set; }

        [JsonProperty("tax_invoiced")]
        public double tax_invoiced { get; set; }

        [JsonProperty("total_due")]
        public double total_due { get; set; }

        [JsonProperty("total_invoiced")]
        public double total_invoiced { get; set; }

        [JsonProperty("total_item_count")]
        public int total_item_count { get; set; }

        [JsonProperty("total_paid")]
        public double total_paid { get; set; }

        [JsonProperty("total_qty_ordered")]
        public int total_qty_ordered { get; set; }

        [JsonProperty("updated_at")]
        public string updated_at { get; set; }

        [JsonProperty("weight")]
        public double weight { get; set; }

        [JsonProperty("items")]
        public IList<Item> items { get; set; }

        public class Item
        {

            [JsonProperty("amount_refunded")]
            public double amount_refunded { get; set; }

            [JsonProperty("base_amount_refunded")]
            public double base_amount_refunded { get; set; }

            [JsonProperty("base_discount_amount")]
            public double base_discount_amount { get; set; }

            [JsonProperty("base_discount_invoiced")]
            public double base_discount_invoiced { get; set; }

            [JsonProperty("base_discount_tax_compensation_amount")]
            public int base_discount_tax_compensation_amount { get; set; }

            [JsonProperty("base_discount_tax_compensation_invoiced")]
            public int base_discount_tax_compensation_invoiced { get; set; }

            [JsonProperty("base_original_price")]
            public double base_original_price { get; set; }

            [JsonProperty("base_price")]
            public double base_price { get; set; }

            [JsonProperty("base_price_incl_tax")]
            public double base_price_incl_tax { get; set; }

            [JsonProperty("base_row_invoiced")]
            public double base_row_invoiced { get; set; }

            [JsonProperty("base_row_total")]
            public double base_row_total { get; set; }

            [JsonProperty("base_row_total_incl_tax")]
            public double base_row_total_incl_tax { get; set; }

            [JsonProperty("base_tax_amount")]
            public int base_tax_amount { get; set; }

            [JsonProperty("base_tax_invoiced")]
            public int base_tax_invoiced { get; set; }

            [JsonProperty("base_weee_tax_applied_amount")]
            public int base_weee_tax_applied_amount { get; set; }

            [JsonProperty("base_weee_tax_applied_row_amnt")]
            public int base_weee_tax_applied_row_amnt { get; set; }

            [JsonProperty("base_weee_tax_disposition")]
            public int base_weee_tax_disposition { get; set; }

            [JsonProperty("base_weee_tax_row_disposition")]
            public int base_weee_tax_row_disposition { get; set; }

            [JsonProperty("created_at")]
            public string created_at { get; set; }

            [JsonProperty("discount_amount")]
            public double discount_amount { get; set; }

            [JsonProperty("discount_invoiced")]
            public double discount_invoiced { get; set; }

            [JsonProperty("discount_percent")]
            public double discount_percent { get; set; }

            [JsonProperty("free_shipping")]
            public int free_shipping { get; set; }

            [JsonProperty("discount_tax_compensation_amount")]
            public int discount_tax_compensation_amount { get; set; }

            [JsonProperty("discount_tax_compensation_invoiced")]
            public int discount_tax_compensation_invoiced { get; set; }

            [JsonProperty("is_qty_decimal")]
            public int is_qty_decimal { get; set; }

            [JsonProperty("is_virtual")]
            public int is_virtual { get; set; }

            [JsonProperty("item_id")]
            public int item_id { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("no_discount")]
            public int no_discount { get; set; }

            [JsonProperty("order_id")]
            public int order_id { get; set; }

            [JsonProperty("original_price")]
            public double original_price { get; set; }

            [JsonProperty("price")]
            public double price { get; set; }

            [JsonProperty("price_incl_tax")]
            public double price_incl_tax { get; set; }

            [JsonProperty("product_id")]
            public int product_id { get; set; }

            [JsonProperty("product_type")]
            public string product_type { get; set; }

            [JsonProperty("qty_canceled")]
            public int qty_canceled { get; set; }

            [JsonProperty("qty_invoiced")]
            public int qty_invoiced { get; set; }

            [JsonProperty("qty_ordered")]
            public int qty_ordered { get; set; }

            [JsonProperty("qty_refunded")]
            public int qty_refunded { get; set; }

            [JsonProperty("qty_shipped")]
            public int qty_shipped { get; set; }

            [JsonProperty("quote_item_id")]
            public int quote_item_id { get; set; }

            [JsonProperty("row_invoiced")]
            public double row_invoiced { get; set; }

            [JsonProperty("row_total")]
            public string row_total { get; set; }

            [JsonProperty("row_total_incl_tax")]
            public double row_total_incl_tax { get; set; }

            [JsonProperty("row_weight")]
            public double row_weight { get; set; }

            [JsonProperty("sku")]
            public string sku { get; set; }

            [JsonProperty("store_id")]
            public int store_id { get; set; }

            [JsonProperty("tax_amount")]
            public int tax_amount { get; set; }

            [JsonProperty("tax_invoiced")]
            public int tax_invoiced { get; set; }

            [JsonProperty("tax_percent")]
            public int tax_percent { get; set; }

            [JsonProperty("updated_at")]
            public string updated_at { get; set; }

            [JsonProperty("weee_tax_applied")]
            public string weee_tax_applied { get; set; }

            [JsonProperty("weee_tax_applied_amount")]
            public int weee_tax_applied_amount { get; set; }

            [JsonProperty("weee_tax_applied_row_amount")]
            public int weee_tax_applied_row_amount { get; set; }

            [JsonProperty("weee_tax_disposition")]
            public int weee_tax_disposition { get; set; }

            [JsonProperty("weee_tax_row_disposition")]
            public int weee_tax_row_disposition { get; set; }

            [JsonProperty("weight")]
            public double weight { get; set; }

            [JsonProperty("parent_item_id")]
            public int? parent_item_id { get; set; }
        }

        public class ItemsResponse
        {
            [JsonProperty("items")]
            public IList<Orders> items { get; set; }
        }

        public static async Task<ItemsResponse> read_ORDER(Token model, int entity_id)
        {
            try
            {
                ItemsResponse item = new ItemsResponse();
                Uri geturi = new Uri("http://homolog.lojaescoteira.com.br/index.php/rest/V1/orders?searchCriteria[filterGroups][0][filters][0][field]=entity_id&searchCriteria[filterGroups][0][filters][0][value]=" + entity_id + "&searchCriteria[sortOrders][0][field]=entity_id&searchCriteria[sortOrders][0][direction]=DESC");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.bearerTolken.Replace('"', ' ').Trim());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.GetAsync(geturi).Result;
                if (message.IsSuccessStatusCode)
                {
                    string result = message.Content.ReadAsStringAsync().Result;
                    item = JsonConvert.DeserializeObject<ItemsResponse>(result);
                }
                return item;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
