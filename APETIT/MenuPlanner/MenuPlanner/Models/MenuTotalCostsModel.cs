using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPlanner.Models
{
    public class MenuTotalCostsModel
    {
        public int LineId { get; set; }
        public int Day { get; set; }
        public string ItemCode { get; set; }
        public double RawMaterial { get; set; }
        public double Goal { get; set; }
    }
}
