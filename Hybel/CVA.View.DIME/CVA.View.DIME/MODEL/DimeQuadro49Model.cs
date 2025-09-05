using CVA.AddOn.Common.Attributes;

namespace CVA.View.DIME.MODEL
{
    public class DimeQuadro49Model
    {
        [FileWriter(Position = 1, Size = 2)]
        public string Tipo { get; set; }

        [FileWriter(Position = 3, Size = 2)]
        public string Quadro { get; set; }

        [FileWriter(Position = 5, Size = 2)]
        public string UF { get; set; }

        [FileWriter(Position = 7, Size = 17)]
        public double Valor { get; set; }

        [FileWriter(Position = 24, Size = 17)]
        public double BaseCalculo { get; set; }

        [FileWriter(Position = 41, Size = 17)]
        public double Outras { get; set; }

        [FileWriter(Position = 58, Size = 17)]
        public double Petroleo { get; set; }

        [FileWriter(Position = 75, Size = 17)]
        public double OutrosProdutos { get; set; }
    }
}
