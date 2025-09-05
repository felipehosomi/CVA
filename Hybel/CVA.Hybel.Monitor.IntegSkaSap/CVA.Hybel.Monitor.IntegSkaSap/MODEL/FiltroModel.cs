using System;

namespace CVA.Hybel.Monitor.IntegSkaSap.MODEL
{
    public class FiltroModel
    {
        public DateTime? DataDe { get; set; }
        public DateTime? DataAte { get; set; }
        public string OP { get; set; }
        public string Status { get; set; }
        public string BELPOS_ID { get; set; }
        public string POS_ID { get; set; }
        public string OPER { get; set; }
        public int QUANT { get; set; }
        public string OPERADOR { get; set; }
        public string MAQ { get; set; }
        public string Mensagem { get; set; }
        public string CODPECA { get; set; }
        public DateTime REPDATETIME { get; set; }
    }
}
