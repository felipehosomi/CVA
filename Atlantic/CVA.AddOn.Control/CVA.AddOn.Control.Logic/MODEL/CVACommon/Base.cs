using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.MODEL.CVACommon
{
    public class Base
    {
        public int ID { get; set; }
        public System.DateTime INS { get; set; }
        public Nullable<System.DateTime> UPD { get; set; }
        public Nullable<int> STU { get; set; }
        public string SRVR { get; set; }
        public string PAS { get; set; }
        public string COMP { get; set; }
        public Nullable<int> USE_TRU { get; set; }
        public string UNAME { get; set; }
        public string DB_UNAME { get; set; }
        public string DB_PAS { get; set; }
        public Nullable<int> DB_TYP { get; set; }
        public string DB_SRVR { get; set; }
        public TipoBaseEnum TipoBase { get; set; }
    }
}
