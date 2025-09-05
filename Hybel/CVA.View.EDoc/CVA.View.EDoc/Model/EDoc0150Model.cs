using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDoc0150Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; }

        [FileWriter(Position = 2, Size = 28)]
        public string CardCode { get; set; }

        [FileWriter(Position = 3, Size = 60)]
        public string CardName { get; set; }

        [FileWriter(Position = 4, Size = 5)]
        public string Pais { get; set; }

        [FileWriter(Position = 5, Size = 14, OnylNumeric = true)]
        public string CNPJ { get; set; }

        [FileWriter(Position = 6, Size = 11, OnylNumeric = true)]
        public string CPF { get; set; }

        [FileWriter(Position = 7, Size = 0)]
        public string Vazio { get; set; }

        [FileWriter(Position = 8, Size = 2)]
        public string UF { get; set; }

        [FileWriter(Position = 9, Size = 14, OnylNumeric = true)]
        public string IE { get; set; }

        [FileWriter(Position = 10, Size = 14, OnylNumeric = true)]
        public string IE_ST { get; set; }

        [FileWriter(Position = 11, Size = 7)]
        public string CodMunicipio { get; set; }

        [FileWriter(Position = 12, Size = 14)]
        public string IM { get; set; }

        [FileWriter(Position = 13, Size = 9)]
        public string Suframa { get; set; }
    }
}
