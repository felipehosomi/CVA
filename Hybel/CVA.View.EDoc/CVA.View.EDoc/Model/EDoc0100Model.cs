using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDoc0100Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; }

        [FileWriter(Position = 2, Size = 60)]
        public string Nome { get; set; }

        [FileWriter(Position = 3, Size = 3)]
        public string CodAssinante { get; set; }

        [FileWriter(Position = 4, Size = 14, OnylNumeric = true)]
        public string CNPJ { get; set; }

        [FileWriter(Position = 5, Size = 11, OnylNumeric = true)]
        public string CPF { get; set; }

        [FileWriter(Position = 6, Size = 10)]
        public string CRC { get; set; }

        [FileWriter(Position = 7, Size = 8, OnylNumeric = true)]
        public string CEP { get; set; }

        [FileWriter(Position = 8, Size = 60)]
        public string Endereco { get; set; }

        [FileWriter(Position = 9, Size = 6)]
        public string Numero { get; set; }

        [FileWriter(Position = 10, Size = 50)]
        public string Complemento { get; set; }

        [FileWriter(Position = 11, Size = 20)]
        public string Bairro { get; set; }

        [FileWriter(Position = 12, Size = 2)]
        public string UF { get; set; }

        [FileWriter(Position = 13, Size = 7)]
        public string CodigoMunicipio { get; set; }

        [FileWriter(Position = 14, Size = 8)]
        public string CEP_CP { get; set; }

        [FileWriter(Position = 15, Size = 5)]
        public string CP { get; set; }

        [FileWriter(Position = 16, Size = 12)]
        public string Telefone { get; set; }

        [FileWriter(Position = 17, Size = 12)]
        public string Fax { get; set; }

        [FileWriter(Position = 18, Size = 60)]
        public string Email { get; set; }
    }
}
