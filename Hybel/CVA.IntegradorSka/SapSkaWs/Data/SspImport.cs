using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapSkaWs.Data
{
    public class SspImport
    {
        public string OP { get; set; }
        public string OPER { get; set; }
        public string CODPECA { get; set; }
        public string MAQ { get; set; }
        public DateTime? PLANDTINI { get; set; }
        public DateTime? PLANDTFIM { get; set; }
        public int? PLANQTY { get; set; }
        public int? CYCQTY { get; set; }
        public float? PLANTMUNIT { get; set; }
        public float? PLANTMSETUP { get; set; }
        public int? ACAO { get; set; }
        public int? STATUS { get; set; }
        public int? BELPOS_ID { get; set; }
        public int? POS_ID { get; set; }
        public DateTime? TSTAMP { get; set; }
    }
}
