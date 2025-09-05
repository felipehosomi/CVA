using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.Model
{

    public class Shipment_OrderModel
    {
        public string order_number { get; set; }
        public string parent_shipment_order_number { get; set; }
        public float customer_shipping_costs { get; set; }
        public string sales_channel { get; set; }
        public bool scheduled { get; set; }
        public DateTime created { get; set; }
        public DateTime shipped_date { get; set; }
        public string shipment_order_type { get; set; }
        public int delivery_method_id { get; set; }
        public int delivery_method_external_id { get; set; }    
        public End_Customer end_customer { get; set; }
        public string origin_zip_code { get; set; }
        public string origin_warehouse_code { get; set; }
        public Shipment_Order_Volume_Array[] shipment_order_volume_array { get; set; }
        public DateTime estimated_delivery_date { get; set; }
        public External_Order_Numbers external_order_numbers { get; set; }
    }

    public class End_Customer
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string cellphone { get; set; }
        public bool is_company { get; set; }
        public string federal_tax_payer_id { get; set; }
        public string shipping_country { get; set; }
        public string shipping_state { get; set; }
        public string shipping_city { get; set; }
        public string shipping_address { get; set; }
        public string shipping_number { get; set; }
        public string shipping_quarter { get; set; }
        public string shipping_reference { get; set; }
        public string shipping_additional { get; set; }
        public string shipping_zip_code { get; set; }
    }

    public class External_Order_Numbers
    {
        public string erp { get; set; }
    }

    public class Shipment_Order_Volume_Array
    {
        public string name { get; set; }
        public int shipment_order_volume_number { get; set; }
        public string volume_type_code { get; set; }
        public float weight { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int length { get; set; }
        public int products_quantity { get; set; }
        public string products_nature { get; set; }
        public Shipment_Order_Volume_Invoice shipment_order_volume_invoice { get; set; }
        public string tracking_code { get; set; }
        public string client_pre_shipment_list { get; set; }
    }

    public class Shipment_Order_Volume_Invoice
    {
        public string invoice_series { get; set; }
        public string invoice_number { get; set; }
        public string invoice_key { get; set; }
        public DateTime invoice_date { get; set; }
        public string invoice_total_value { get; set; }
        public string invoice_products_value { get; set; }
        public string invoice_cfop { get; set; }
    }

    public class CancelOrder
    {
        public string order_number { get; set; }
    }















    //public class Shipment_OrderModel
    //{
    //    public string order_number { get; set; }
    //    public double custumer_shipping_costs { get; set; }
    //    public string sales_channel { get; set; }
    //    public DateTime create_date { get; set; }
    //    public DateTime shipped_date { get; set; }
    //    public string shipment_order_type { get; set; }
    //    public int delivery_method_id { get; set; }
    //    public int delivery_method_external_id { get; set; }

    //    public end_costumer _end_costumer { get; set; }
    //}

    //public class end_costumer
    //{
    //    public string firts_name { get; set; }
    //    public string last_name { get; set; }
    //    public string email { get; set; }
    //    public string phone { get; set; }
    //    public string cellphone { get; set; }
    //    public string is_company { get; set; }
    //    public int federal_tax_payer_id { get; set; }


    //}



}
