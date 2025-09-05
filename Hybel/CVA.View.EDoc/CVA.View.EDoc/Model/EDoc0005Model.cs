using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDoc0005Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; }

        [FileWriter(Position = 2, Size = 60)]
        public string Nome { get; set; }

        [FileWriter(Position = 3, Size = 3)]
        public string CodAssinante { get; set; }

        [FileWriter(Position = 4, Size = 11, OnylNumeric = true)]
        public string CPF { get; set; }

        [FileWriter(Position = 5, Size = 8, OnylNumeric = true)]
        public string CEP { get; set; }

        [FileWriter(Position = 6, Size = 40)]
        public string Endereco { get; set; }

        [FileWriter(Position = 7, Size = 6)]
        public string Numero  { get; set; }

        [FileWriter(Position = 8, Size = 60)]
        public string Complemento { get; set; }

        [FileWriter(Position = 9, Size = 20)]
        public string Bairro { get; set; }

        [FileWriter(Position = 10, Size = 8)]
        public string CEP_CP { get; set; }

        [FileWriter(Position = 11, Size = 5)]
        public string CP { get; set; }

        [FileWriter(Position = 12, Size = 12)]
        public string Telefone { get; set; }

        [FileWriter(Position = 13, Size = 12)]
        public string Fax { get; set; }

        [FileWriter(Position = 14, Size = 60)]
        public string Email { get; set; }
    }
}
