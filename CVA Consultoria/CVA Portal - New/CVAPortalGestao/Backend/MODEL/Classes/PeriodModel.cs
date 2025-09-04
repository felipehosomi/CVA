using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class PeriodModel : IModel
    {
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
    }
}
