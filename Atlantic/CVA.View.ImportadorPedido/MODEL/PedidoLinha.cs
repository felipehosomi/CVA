using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class PedidoLinha
    {
        public string CodigoItem { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Utilizacao { get; set; }
        public string Dimensao01 { get; set; }
        public string Dimensao02 { get; set; }
        public string Dimensao03 { get; set; }
        public string Dimensao04 { get; set; }
        public string Dimensao05 { get; set; }
    }
}
