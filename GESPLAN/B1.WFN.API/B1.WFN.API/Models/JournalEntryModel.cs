using System;
using System.Collections.Generic;

namespace B1.WFN.API.Models
{
    public class JournalEntryModel
    {
        public DateTime? ReferenceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? TaxDate { get; set; }
        public IEnumerable<JournalEntryItemModel> JournalEntryLines { get; set; }
        
    }
}
