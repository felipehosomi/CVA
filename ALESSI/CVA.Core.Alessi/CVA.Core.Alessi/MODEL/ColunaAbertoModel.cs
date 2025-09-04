using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.MODEL
{
    public class ColunaAbertoModel
    {
        public int QtdeAberto { get; set; }
        public int QtdeEmCarteira { get; set; }
        public int QtdeBoleto { get; set; }
        public int QtdeCheque { get; set; }
        public int QtdeTotal { get; set; }
        public int QtdeAtrasado { get; set; }

        public string ValorAberto { get; set; }
        public string ValorEmCarteira { get; set; }
        public string ValorBoleto { get; set; }
        public string ValorCheque { get; set; }
        public string ValorTotal { get; set; }
        public string ValorAtrasado { get; set; }
    }
}
