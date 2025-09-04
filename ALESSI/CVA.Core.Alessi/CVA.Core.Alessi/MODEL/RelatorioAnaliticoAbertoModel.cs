using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.MODEL
{
   public class RelatorioAnaliticoAbertoModel
    {
        public string CardCode { get; set; }

        public int NF { get; set; }
        public int Juros { get; set; }

        public DateTime DtDoc { get; set; }
        public DateTime DtVec { get; set; }

        public double Valor { get; set; }
        public double Total { get; set; }


    }
}
