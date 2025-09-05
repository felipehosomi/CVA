using System;

namespace CVA.Hybel.EtiquetaVerde.MODEL
{
    public class ItemModel
    {
        public bool Imprimir { get; set; } = true;
        public DateTime DataEmissao { get; set; }
        public int Linha { get; set; }
        public int NrNF { get; set; }
        public string Cliente { get; set; }
        public string Transportadora { get; set; }
        public string Endereco { get; set; }
        public int NrPedido { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UN { get; set; }
        public decimal Quantidade { get; set; }
        public int QtdeEtiq { get; set; }        
    }
}
