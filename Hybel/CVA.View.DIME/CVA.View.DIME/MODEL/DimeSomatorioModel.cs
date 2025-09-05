using CVA.AddOn.Common.Attributes;

namespace CVA.View.DIME.MODEL
{
    public class DimeSomatorioModel
    {
        [FileWriter(Position = 1, Size = 2)]
        public string Tipo { get; set; }

        [FileWriter(Position = 3, Size = 2)]
        public string Quadro { get; set; }

        [FileWriter(Position = 5, Size = 3)]
        public string Item { get; set; }

        public string Descricao { get; set; }

        [FileWriter(Position = 8, Size = 17)]
        public double Valor { get; set; }
    }
}
