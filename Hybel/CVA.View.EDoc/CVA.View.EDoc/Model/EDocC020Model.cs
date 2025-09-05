using CVA.AddOn.Common.Attributes;
using System;

namespace CVA.View.EDoc.Model
{
    public class EDocC020Model
    {
        public int DocEntry { get; set; }
        public string ObjType { get; set; }

        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; }

        [FileWriter(Position = 2, Size = 1)]
        public int IndicadorOperacao { get; set; }

        [FileWriter(Position = 3, Size = 1)]
        public int IndicadorEmitente { get; set; }

        [FileWriter(Position = 4, Size = 28)]
        public string CardCode { get; set; }

        [FileWriter(Position = 5, Size = 2)]
        public string ModeloNF { get; set; }

        [FileWriter(Position = 6, Size = 2)]
        public string SituacaoNF { get; set; }

        [FileWriter(Position = 7, Size = 3)]
        public string Serie { get; set; }

        [FileWriter(Position = 8, Size = 9)]
        public int NrNF { get; set; }

        [FileWriter(Position = 9, Size = 44)]
        public string ChaveAcesso { get; set; }

        [FileWriter(Position = 10, Size = 8)]
        public DateTime? DataEmissao { get; set; }

        [FileWriter(Position = 11, Size = 8)]
        public DateTime DataDocumento { get; set; }

        [FileWriter(Position = 12, Size = 6)]
        public int UsageId { get; set; }

        [FileWriter(Position = 13, Size = 1)]
        public int IndicadorPagamento { get; set; }

        [FileWriter(Position = 14, Size = 50)]
        public double ValorDocumento { get; set; }

        [FileWriter(Position = 15, Size = 50)]
        public double ValorDesconto { get; set; }

        [FileWriter(Position = 16, Size = 50)]
        public double ValorJuros { get; set; }

        [FileWriter(Position = 17, Size = 50)]
        public double TotalLinha { get; set; }

        [FileWriter(Position = 18, Size = 50)]
        public double ValorFrete { get; set; }

        [FileWriter(Position = 19, Size = 50)]
        public double ValorSeguro { get; set; }

        [FileWriter(Position = 20, Size = 50)]
        public double ValorOutrasDespesas { get; set; }

        [FileWriter(Position = 21, Size = 50)]
        public double BaseISS { get; set; }

        [FileWriter(Position = 22, Size = 50)]
        public double BaseICMS { get; set; }

        [FileWriter(Position = 23, Size = 50)]
        public double ValorICMS { get; set; }

        [FileWriter(Position = 24, Size = 50)]
        public double BaseICMS_ST { get; set; }

        [FileWriter(Position = 25, Size = 50)]
        public double ValorICMS_ST { get; set; }

        [FileWriter(Position = 26, Size = 50)]
        public double ValorICMS_AT { get; set; }

        [FileWriter(Position = 27, Size = 50)]
        public double ValorIPI { get; set; }

        [FileWriter(Position = 28, Size = 9)]
        public string Observacoes { get; set; }
    }
}
