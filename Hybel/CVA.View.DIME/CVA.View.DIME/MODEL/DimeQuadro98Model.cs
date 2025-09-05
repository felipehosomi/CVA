using CVA.AddOn.Common.Attributes;

namespace CVA.View.DIME.MODEL
{
    public class DimeQuadro98Model
    {
        [FileWriter(Position = 1, Size = 2)]
        public string Tipo { get; set; } = "98";

        [FileWriter(Position = 3, Size = 2)]
        public string Quadro { get; set; } = "  ";

        [FileWriter(Position = 5, Size = 5)]
        public int Quantidade { get; set; }
    }
}
