using System;
using System.Collections.Generic;

namespace Escoteiro.Magento.Models
{
    public class DocumentMarketingReturnModel
    {
        public int? DocEntry { get; set; }
    }

    public class DocumentoMarketingModel
    {
        public string CardCode { get; set; }
        public int? DocEntry { get; set; }
        public string ObjectType { get; set; }
        public string DocDate { get; set; }
        public string TaxDate { get; set; }
        public string DocDueDate { get; set; }
        public string NumAtCard { get; set; }
        public int BPL_IDAssignedToInvoice { get; set; }
        public string DocumentStatus { get; set; }
        public double? DocTotal { get; set; }
        public string U_OrigemPedido { get; set; }
        public string ShipToCode { get; set; }
        public string PayToCode { get; set; }
        public float DiscountPercent { get; set; }
        public float PaidToDate { get; set; }
        public string PaymentMethod { get; set; }
        public int? SequenceCode { get; set; }
        public int? SequenceSerial { get; set; }

        public int? TransportationCode { get; set; }
        public string DownPaymentType { get; set; }

        // UDF
        public int? U_CVA_EntityId { get; set; }
        public int? U_CVA_Increment_id { get; set; }
        public string U_CVA_CCusto { get; set; }
        public string U_CVA_IntegratedCancellation { get; set; }
        public string U_nfe_tipoEnv { get; set; }

        public List<Documentline> DocumentLines { get; set; }
        public Taxextension TaxExtension { get; set; }
        public List<DocumentAdditionalExpenses> DocumentAdditionalExpenses { get; set; }
        public AddressExtension AddressExtension { get; set; }
    }

    public class DocumentAdditionalExpenses
    {
        public int ExpenseCode { get; set; }
        public object Remarks { get; set; }
        public float LineTotal { get; set; }
        public string TaxCode { get; set; }
    }

    public class Taxextension
    {
        public object NFRef { get; set; }
        public string Carrier { get; set; }
        public float GrossWeight { get; set; }
        public float NetWeight { get; set; }
        public string PackDescription { get; set; }
        public string Incoterms { get; set; }
    }

    public class Documentline
    {
        public double Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public double? DiscountPercent { get; set; }
        public int? LineNum { get; set; }
        public int Usage { get; set; }
        public string WarehouseCode { get; set; }

        public int? BaseType { get; set; }
        public int? BaseEntry { get; set; }
        public int? BaseLine { get; set; }

        public string ItemCode { get; set; }
        public string TaxCode { get; set; }

        public string CostingCode { get; set; }
        public string CostingCode2 { get; set; }
        public string CostingCode3 { get; set; }

        public string U_CVA_CCusto { get; set; }

        public List<Serialnumber> SerialNumbers { get; set; }
        public List<Batchnumber> BatchNumbers { get; set; }
    }

    public class Serialnumber
    {
        public int BaseLineNumber { get; set; }
        public string InternalSerialNumber { get; set; }
        public string ManufacturerSerialNumber { get; set; }
        public int SystemSerialNumber { get; set; }
        public double Quantity { get; set; }
        public DateTime? ManufactureDate { get; set; }
    }

    public class Batchnumber
    {
        public int BaseLineNumber { get; set; }
        public string BatchNumber { get; set; }
        public string ManufacturerSerialNumber { get; set; }
        public double Quantity { get; set; }
        public DateTime? ManufacturingDate { get; set; }
    }

    public class AddressExtension
    {
        public string BillToBlock { get; set; } 
        public string BillToBuilding { get; set; } 
        public string BillToCity { get; set; } 
        public string BillToCountry { get; set; } 
        public string BillToCounty { get; set; } 
        public string BillToState { get; set; } 
        public string BillToStreet { get; set; } 
        public string BillToStreetNo { get; set; } 
        public string BillToZipCode { get; set; } 
        public string BillToAddressType { get; set; } 
 
        public string ShipToAddressType { get; set; }
        public string ShipToBlock { get; set; } 
        public string ShipToBuilding { get; set; } 
        public string ShipToCity { get; set; } 
        public string ShipToCountry { get; set; } 
        public string ShipToCounty { get; set; } 
        public string ShipToState { get; set; } 
        public string ShipToStreet { get; set; } 
        public string ShipToStreetNo { get; set; }
        public string ShipToZipCode { get; set; } 
    }
}
