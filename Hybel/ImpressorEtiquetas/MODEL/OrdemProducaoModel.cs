using System;

namespace MODEL
{
    public class OrdemProducaoModel
    {
        public bool Imprimir { get; set; }
        public int OP { get; set; }
        public string Pedido { get; set; }
        public DateTime Data { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal Qtde { get; set; }
    }
}
