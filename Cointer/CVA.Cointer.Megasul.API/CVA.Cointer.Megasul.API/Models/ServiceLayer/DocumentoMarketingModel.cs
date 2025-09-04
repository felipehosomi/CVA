using System;
using System.Collections.Generic;

namespace CVA.Cointer.Megasul.API.Models.ServiceLayer
{
    public class DocumentoMarketingModel
    {
        public int? DocEntry { get; set; }
        public int? DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string DocumentStatus { get; set; }
        //public string PaymentMethod { get; set; }
        public int SequenceCode { get; set; }
        public int SequenceSerial { get; set; }
        public int SequenceModel { get; set; }
        public string SeriesString { get; set; }
        public int BPL_IDAssignedToInvoice { get; set; }
        public int SalesPersonCode { get; set; }
        public double DiscountPercent { get; set; }
        public int U_CVA_DocNumCancelado { get; set; }
        public int U_CVA_CF_EF { get; set; }
        public int U_CVA_CF_SI { get; set; }
        public int U_CVA_CF_TR { get; set; }
        public string U_ChaveAcesso { get; set; }
        public TaxExtension TaxExtension { get; set; }
        public List<Documentline> DocumentLines { get; set; }
        public List<Documentreference> DocumentReferences { get; set; }
        public List<DocumentAdditionalExpense> DocumentAdditionalExpenses { get; set; }
        public List<DownPaymentsToDraw> DownPaymentsToDraw { get; set; }
    }

    public class Documentline
    {
        public double Quantity { get; set; }
        public int Usage { get; set; }
        public double UnitPrice { get; set; }
        //public double DiscountPercent { get; set; }
        public string ItemCode { get; set; }
        public string WarehouseCode { get; set; }
        public double LineTotal { get; set; }
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

    public class TaxExtension
    {
        public int MainUsage { get; set; }
    }

    public class Documentreference
    {
        //public int LineNumber { get; set; }
        public int RefDocEntr { get; set; }
        //public int RefDocNum { get; set; }
        public string RefObjType { get; set; } = "rot_SalesInvoice";
        //public object AccessKey { get; set; }
        //public string IssueDate { get; set; }
        //public object IssuerCNPJ { get; set; }
        //public string IssuerCode { get; set; }
        //public string Model { get; set; }
        //public string Series { get; set; }
        //public int Number { get; set; }
        //public string RefAccKey { get; set; }
        //public float RefAmount { get; set; }
        //public object SubSeries { get; set; }
        //public object Remark { get; set; }
        //public string LinkRefTyp { get; set; }
    }

    public class DocumentAdditionalExpense
    {
        public int ExpenseCode { get; set; }
        public double LineTotal { get; set; }
    }

    public class DownPaymentsToDraw
    {
        public int DocEntry { get; set; }
        public double AmountToDraw { get; set; }
    }
}