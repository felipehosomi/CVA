using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Hybel.MODEL
{
    public class TaxaConversaoFiltroModel
    {
        public string CodMotivo { get; set; }
        public string DescMotivo { get; set; }
        public int CodFilial { get; set; }
        public string Filial { get; set; }
        public DateTime DataDe { get; set; }
        public DateTime DataAte { get; set; }
        public string Diretorio { get; set; }
    }
}
