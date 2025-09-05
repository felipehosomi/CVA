using System;

namespace CVA.View.CRCP.Model
{
    public class CrcpModel
    {
        public int BPGroupCode { get; set; }
        public DateTime Vencimento { get; set; }
        public DateTime? Pagamento { get; set; }
        public double Valor { get; set; }
    }
}
