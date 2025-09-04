using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesOrder.Models.SAP
{
    public class Metadata_SalesOrder
    {
        public class Orders
        {
            public int DocEntry { get; set; }
            public int DocNum { get; set; }
            public int BPL_IDAssignedToInvoice { get; set; }
            public DateTime DocDate { get; set; }
            public DateTime DocDueDate { get; set; }
            public DateTime TaxDate { get; set; }
            //public double DocTotal { get; set; }
            public string CardCode { get; set; }
            public string CardName { get; set; }
            [JsonProperty("ShipToCode", NullValueHandling = NullValueHandling.Ignore)]
            public string ShipToCode { get; set; }
            [JsonProperty("PayToCode", NullValueHandling = NullValueHandling.Ignore)]
            public string PayToCode { get; set; }
            public int GroupNumber { get; set; }
            public string PaymentMethod { get; set; }
            public int TransportationCode { get; set; }
            //public string Incoterms { get; set; }
            public string U_CVA_Magento_Id { get; set; }
            public DateTime U_CVA_Magento_Data { get; set; }
            public int U_CVA_Magento_Hora { get; set; }
            public string U_CVA_Magento_Status { get; set; }
            public string U_CVA_Magento_Msg { get; set; }
            public int U_CVA_Magento_Entity { get; set; }
            public string U_CVA_SourceChannel { get; set; }
            public DateTime U_CVA_Vcto { get; set; }
            public List<DocumentLine> DocumentLines { get; set; }
            public List<DocumentAdditionalExpens> DocumentAdditionalExpenses { get; set; }
            public TaxExtension TaxExtension { get; set; }
        }

        public class DocumentLine
        {
            public string ItemCode { get; set; }
            public string ItemDescription { get; set; }
            public int LineNum { get; set; }
            public string BarCode { get; set; }
            public double Quantity { get; set; }
            public double Price { get; set; }
            public double DiscountPercent { get; set; }
            public int Usage { get; set; }
            public string WarehouseCode { get; set; }
            public double UnitPrice { get; set; }
            public string U_CVA_Magento_ItemId { get; set; }
        }

        public class DocumentAdditionalExpens
        {
            public int ExpenseCode { get; set; }
            public double LineTotal { get; set; }
            public double LineTotalFC { get; set; }
        }

        public class TaxExtension
        {
            public string TaxId0 { get; set; }
            public string TaxId4 { get; set; }
            public string Incoterms { get; set; }
        }

        public class UpdateOrder
        {
            public List<UpdateOrderLines> DocumentLines { get; set; }
        }

        public class UpdateOrderLines
        {
            public int LineNum { get; set; }
            public double UnitPrice { get; set; }
        }

    }
}
