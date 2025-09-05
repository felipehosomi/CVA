using System;

namespace CVA.Hybel.Monitor.IntegSkaSap.MODEL
{
    public class IntegracaoModel
    {
        
        public bool Reprocessar { get; set; }

        public int ID { get; set; }
        public string OP { get; set; }
        public string TIPO { get; set; }
        public string Status { get; set; }
        public int ID_LOG { get; set; }
        public string BELPOS_ID { get; set; }
        public string POS_ID { get; set; }
        public string OPER { get; set; }
        public int QUANT { get; set; }
        public int REJ { get; set; }
        public string OPERADOR { get; set; }
        public string MAQ { get; set; }
        public string Description { get; set; }
        public string CODPECA { get; set; }
        public DateTime REPDATETIME { get; set; }
    }
}
