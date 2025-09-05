using CVA.AddOn.Common.Attributes;
using System;

namespace CVA.View.DIME.MODEL
{
    public class DimeContadorModel
    {
        [FileWriter(Position = 1, Size = 2)]
        public string Tipo { get; set; } = "20";

        [FileWriter(Position = 3, Size = 2)]
        public string Quadro { get; set; } = "  ";

        [FileWriter(Position = 5, Size = 11, OnylNumeric = true)]
        public string CPF { get; set; }

        [FileWriter(Position = 16, Size = 50)]
        public string Nome { get; set; }

        [FileWriter(Position = 66, Size = 14, Format = "yyyyMMddHHmmss")]
        public DateTime Data { get; set; } = DateTime.Now;
    }
}
