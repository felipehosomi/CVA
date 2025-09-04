using System;

namespace CVA.Console.MRP.Models
{
    public class Recommendation
    {
        public string OrderType { get; set; }
        public int BPLid { get; set; }
        public string Warehouse { get; set; }
        public DateTime DueDate { get; set; }
        public string CardCode { get; set; }
        public int UomEntry { get; set; }
        public string UomCode { get; set; }
        public string ItemCode { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public int ObjAbs { get; set; }
    }
}
