using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesOrder.Models.SAP
{
    public class Metadata_DownPayments
    {
        public class DownPayments
        {
            public int DocEntry { get; set; }
            public int DocNum { get; set; }
            public string CardCode { get; set; }
            public string DownPaymentType { get; set; }
            public int BPL_IDAssignedToInvoice { get; set; }
            public DateTime DocDate { get; set; }
            public DateTime DocDueDate { get; set; }
            public DateTime TaxDate { get; set; }
            public double DocTotal { get; set; }
            public List<DocumentLine> DocumentLines { get; set; }
        }

        public class DocumentLine
        {
            public string ItemCode { get; set; }
            public string ItemDescription { get; set; }
            public int BaseEntry { get; set; }
            public int BaseType { get; set; }
            public int BaseLine { get; set; }
            public double UnitPrice { get; set; }
            public double Quantity { get; set; }
            public double DiscountPercent { get; set; }
            public int Usage { get; set; }
            public string WarehouseCode { get; set; }            
        }
    }
}
