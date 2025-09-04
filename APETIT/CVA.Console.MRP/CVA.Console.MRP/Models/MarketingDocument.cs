using System;
using System.Collections.Generic;

namespace CVA.Console.MRP.Models
{
    public class MarketingDocument
    {
        public int? DocEntry { get; set; }
        public int? DocNum { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime RequriedDate { get; set; }
        public string CardCode { get; set; }
        public int BPL_IDAssignedToInvoice { get; set; }
        public string Comments { get; set; }
        public List<DocumentLine> DocumentLines { get; set; }

        public MarketingDocument()
        {
            DocumentLines = new List<DocumentLine>();
        }
    }

    public class DocumentLine
    {
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int UoMEntry { get; set; }
        public string UoMCode { get; set; }
        public string WarehouseCode { get; set; }
        public DateTime ShipDate { get; set; }
        public DateTime RequiredDate { get; set; }
    }
}