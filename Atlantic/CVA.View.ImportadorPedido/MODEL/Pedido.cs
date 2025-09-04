using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class Pedido
    {
        public int DocEntry { get; set; }
        public string TaxIdNum { get; set; }
        public string CardCode { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string CondicaoPagamento { get; set; }
        public string FormaPagamento { get; set; }
        public string NumeroNota { get; set; }
        public string SerieNota { get; set; }
        public string Modelo { get; set; }
        public string Projeto { get; set; }
        public string Observacao { get; set; }
        public List<PedidoLinha> Linhas { get; set; }

        public Pedido()
        {
            Linhas = new List<PedidoLinha>();
        }

    }
}
