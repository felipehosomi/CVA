using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Comissionamento.Models
{
    public class CalculoComissaoFiltroModel
    {
        public string DataMetaInicial { get; set; }
        public string DataMetaFinal { get; set; }
        public string DataComissaoInicial { get; set; }
        public string DataComissaoFinal { get; set; }
        public int Filial { get; set; }
        public int DiasUteis { get; set; }
        public int Feriados { get; set; }
        public bool Todas { get; set; }
        public bool Pagas { get; set; }
        public bool NaoPagas { get; set; }
    }
}
