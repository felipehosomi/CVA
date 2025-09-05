using System;

namespace B1.WFN.API.Models
{
    public class JournalEntryItemModel
    {
        public string AccountCode { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public DateTime? DueDate { get; set; }
        public string LineMemo { get; set; }
        public string CostingCode { get; set; }
        public int? BPLID { get; set; }
    }
}
