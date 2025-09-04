using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DIRF.MODEL
{
    public class DirfModel
    {
        public string DIRF { get; set; }
        public string RESPO { get; set; }
        public string DECPJ { get; set; }
        public List<IdrecModel> IDREC { get; set; }
    }

    public class IdrecModel
    {
        public string IDREC { get; set; }
        public List<InfoModel> Info { get; set; }
    }

    public class InfoModel
    {
        public string BPJDEC { get; set; }
        public string RTRT { get; set; }
        public string RTIRF { get; set; }
    }
}
