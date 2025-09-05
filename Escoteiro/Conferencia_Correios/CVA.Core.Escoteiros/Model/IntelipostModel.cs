using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.Model
{
    public class IntelipostModel
    {
        public string order_number                  { get; set; }
        public float custumer_shipping_costs        { get; set; }
        public string sales_channel                 { get; set; }
        public string scheduled                     { get; set; }
        public DateTime created                     { get; set; }
        public DateTime shipped_date                { get; set; }
        public string shipped_order_type            { get; set; }
        public int delivery_method_id               { get; set; }
        public int delivery_method_External_id      { get; set; }
        public string first_name                    { get; set; }
        public string last_name                     { get; set; }
        public string email                         { get; set; }
        public string phone                         { get; set; }
        public string cellphone                     { get; set; }
        public string is_company                    { get; set; }
        public string federal_tax_payer_id          { get; set; }
        public string shipping_country              { get; set; }
        public string shipping_state                { get; set; }
        public string shipping_city                 { get; set; }
        public string shipping_address              { get; set; }
        public string shipping_number               { get; set; }
        public string shipping_quarter              { get; set; }
        public string shipping_reference            { get; set; }
        public string shipping_additional           { get; set; }
        public string shipping_zip_code             { get; set; }
        public string origin_zip_code               { get; set; }
        public string origin_warehouse_code         { get; set; }
        public string invoice_serie                 { get; set; }
        public int invoice_number                   { get; set; }
        public string invoice_key                   { get; set; }
        public DateTime invoice_date                { get; set; }
        public double invoice_total_value           { get; set; }
        public double invoice_products_value        { get; set; }
        public string invoice_cfop                  { get; set; }
        public string tracking_code                 { get; set; }
        public DateTime estimate_delivery_date      { get; set; }
        public int erp                              { get; set; }
    }

    public class IntelipostVolume
    {
        public string name                          { get; set; }
        public int shipment_order_volume_number     { get; set; }
        public string volume_type_code              { get; set; }
        public float weight                         { get; set; }
        public int widht                            { get; set; }
        public int height                           { get; set; }
        public int lenght                           { get; set; }
        public int prdoducts_quantity               { get; set; }
        public string products_name                 { get; set; }
    }
}
