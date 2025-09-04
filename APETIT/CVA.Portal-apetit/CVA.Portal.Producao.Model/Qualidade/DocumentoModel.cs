using System;

namespace CVA.Portal.Producao.Model.Qualidade
{
    public class DocumentoModel
    {
        public string CodFicha { get; set; }
        public string StatusFicha { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public int Serial { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public int Sequencia { get; set; }

        public string CodEtapa { get; set; }
        public string DescEtapa { get; set; }
    }
}
