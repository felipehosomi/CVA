using System;
using System.Collections.Generic;

namespace PackIndicator.Models
{
    public enum LineType
    {
        New,
        Old,
        Normal,
        Balance,
        Remove
    }

    public class PickingData
    {
        public bool Suggested { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int LineNum { get; set; }
        public int VisOrder { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string AddressName { get; set; }
        public LineType DocLineType { get; set; }
        public int DocVisOrder { get; set; }
        public int DocLineNum { get; set; }
        public int OriginalDocLineNum { get; set; }
        public DateTime ShipDate { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
        public string OriginalUom { get; set; }
        public string SuggestedUom { get; set; }
        public double OriginalQty { get; set; }
        public double BalanceQty { get; set; }
        public int BPPriority { get; set; }
        public int DueDaysLimit { get; set; }
        public List<Packages> Packages { get; set; }
        public bool ReleasePicking { get; set; }

        public PickingData()
        {
            Packages = new List<Packages>();
        }
    }
}
