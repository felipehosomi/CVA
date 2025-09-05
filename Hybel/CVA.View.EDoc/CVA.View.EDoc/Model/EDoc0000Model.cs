using CVA.AddOn.Common.Attributes;
using System;

namespace CVA.View.EDoc.Model
{
    public class EDoc0000Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; }

        [FileWriter(Position = 2, Size = 4)]
        public string LFPD { get; set; }

        [FileWriter(Position = 3, Size = 8, Format = "ddMMyyyy")]
        public DateTime DataInicial { get; set; }

        [FileWriter(Position = 4, Size = 8, Format = "ddMMyyyy")]
        public DateTime DataFinal { get; set; }

        [FileWriter(Position = 5, Size = 60)]
        public string Filial { get; set; }

        [FileWriter(Position = 6, Size = 14, OnylNumeric = true)]
        public string CNPJ { get; set; }

        [FileWriter(Position = 7, Size = 2)]
        public string UF { get; set; }

        [FileWriter(Position = 8, Size = 14, OnylNumeric = true)]
        public string IE { get; set; }

        [FileWriter(Position = 9, Size = 7)]
        public string CodMunicipio { get; set; }

        [FileWriter(Position = 10, Size = 14)]
        public string IM { get; set; }

        [FileWriter(Position = 11, Size = 0)]
        public string Vazio1 { get; set; }

        [FileWriter(Position = 12, Size = 0)]
        public string Suframa { get; set; }

        [FileWriter(Position = 13, Size = 4)]
        public string Layout { get; set; }

        [FileWriter(Position = 14, Size = 1)]
        public string Finalidade { get; set; }

        [FileWriter(Position = 15, Size = 2)]
        public string CodConteudo { get; set; }

        [FileWriter(Position = 16, Size = 6)]
        public string Pais { get; set; }

        [FileWriter(Position = 17, Size = 60)]
        public string NomeFantasia { get; set; }

        [FileWriter(Position = 18, Size = 11)]
        public string NIRE { get; set; }

        [FileWriter(Position = 19, Size = 11)]
        public string CPF { get; set; }

        [FileWriter(Position = 20, Size = 0)]
        public string Vazio2 { get; set; }
    }
}
