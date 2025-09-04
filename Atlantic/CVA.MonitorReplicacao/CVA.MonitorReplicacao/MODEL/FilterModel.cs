using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.MonitorReplicacao.MODEL
{
    public class FilterModel
    {
        public string IdFrom { get; set; }
        public string IdTo { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
        public string Object { get; set; }
        public string Function { get; set; }
        public string ErrorBase { get; set; }
    }
}
