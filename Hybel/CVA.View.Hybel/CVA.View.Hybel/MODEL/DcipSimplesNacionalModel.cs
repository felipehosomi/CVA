using CVA.AddOn.Common.Attributes;
using System;

namespace CVA.View.Hybel.MODEL
{
    public class DcipSimplesNacionalModel
    {
        public string IEFilial { get; set; }

        [FileWriter(Size = 9, Position = 1, OnylNumeric = true)]
        public string IE { get; set; }

        [FileWriter(Size = 6, Position = 10, Format = "yyyyMM")]
        public DateTime Periodo { get; set; }

        [FileWriter(Size = 3, Position = 16)]
        public string Tipo { get; set; }

        [FileWriter(Size = 14, Position = 19, OnylNumeric = true)]
        public string CNPJ { get; set; }

        [FileWriter(Size = 2, Position = 33)]
        public string UF { get; set; }

        [FileWriter(Size = 3, Position = 35)]
        public string Serie { get; set; }

        [FileWriter(Size = 9, Position = 38)]
        public int NrNota { get; set; }

        [FileWriter(Size = 8, Position = 47, Format = "yyyyMMdd")]
        public DateTime Data { get; set; }

        [FileWriter(Size = 4, Position = 55)]
        public string CFOP { get; set; }

        [FileWriter(Size = 17, Position = 59, DecimalSeparator = "")]
        public double ValorTotal { get; set; }

        [FileWriter(Size = 17, Position = 76, DecimalSeparator = "")]
        public double BaseCalculo { get; set; }

        [FileWriter(Size = 5, Position = 93, DecimalSeparator = "")]
        public double AliqICMS { get; set; }

        [FileWriter(Size = 2, Position = 98)]
        public int ModeloNF { get; set; }
    }
}
