using CVA.AddOn.Common.Attributes;

namespace CVA.View.DIME.MODEL
{
    public class DimeQuadro50Model
    {
        [FileWriter(Position = 1, Size = 2)]
        public string Tipo { get; set; }

        [FileWriter(Position = 3, Size = 2)]
        public string Quadro { get; set; }

        [FileWriter(Position = 5, Size = 2)]
        public string UF { get; set; }

        [FileWriter(Position = 7, Size = 17)]
        public double ValorNaoContribuinte { get; set; }

        [FileWriter(Position = 24, Size = 17)]
        public double ValorContribuinte { get; set; }

        [FileWriter(Position = 41, Size = 17)]
        public double BaseNaoContribuinte { get; set; }

        [FileWriter(Position = 58, Size = 17)]
        public double BaseContribuinte { get; set; }

        [FileWriter(Position = 75, Size = 17)]
        public double Outras { get; set; }

        [FileWriter(Position = 92, Size = 17)]
        public double IcmsST { get; set; }
    }
}
