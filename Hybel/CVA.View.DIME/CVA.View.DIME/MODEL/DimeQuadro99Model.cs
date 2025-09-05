using CVA.AddOn.Common.Attributes;

namespace CVA.View.DIME.MODEL
{
    public class DimeQuadro99Model
    {
        [FileWriter(Position = 1, Size = 2)]
        public string Tipo { get; set; } = "99";

        [FileWriter(Position = 3, Size = 2)]
        public string Quadro { get; set; } = "  ";

        [FileWriter(Position = 5, Size = 5)]
        public int Quantidade { get; set; }

        [FileWriter(Position = 10, Size = 5)]
        public int QuantidadeIcms { get; set; } = 1;
    }
}
