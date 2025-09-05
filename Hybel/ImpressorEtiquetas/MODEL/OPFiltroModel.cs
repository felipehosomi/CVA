using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class OPFiltroModel
    {
        public DateTime? DataDe { get; set; }
        public DateTime? DataAte { get; set; }
        public decimal OPDe { get; set; }
        public decimal OPAte { get; set; }
        public bool Reimpressao { get; set; }
    }
}
