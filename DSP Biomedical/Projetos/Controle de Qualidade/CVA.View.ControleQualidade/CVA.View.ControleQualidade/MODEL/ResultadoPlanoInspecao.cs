using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.MODEL
{
    public class ResultadoPlanoInspecao
    {
        public string QuantidadeAmostra { get; set; }
        public string ValorMedicao { get; set; }
        public string Resultado { get; set; }
        public string CodigoOrdemProducao { get; set; }
        public string Lote { get; set; }
        public string CodigoPlanoInspecao { get; set; }
    }
}
