using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkaSapWs.Data
{
    public class SspExportProd
    {
        public int Id { get; set; }
        public string OP { get; set; }
        public string OPER { get; set; }
        public string CODPECA { get; set; }
        public string MAQ { get; set; }
        public int TURNO { get; set; }
        public string OPERADOR { get; set; }
        public DateTime DATAINI { get; set; }
        public DateTime DATAFIM { get; set; }
        public int QUANT { get; set; }
        public int REJ { get; set; }
        public string UNID { get; set; }
        public float TMLIQPROD { get; set; }
        public float TMINTPROD { get; set; }
        public int CNTREP { get; set; }
        public int ENCERR { get; set; }
        public int STATUS { get; set; }
        public int REPTYPE { get; set; }
        public DateTime REPDATETIME { get; set; }
        public int BELPOS_ID { get; set; }
        public int POS_ID { get; set; }
    }
}
