using CVA.AddOn.Common.Attributes;

namespace CVA.View.DIME.MODEL
{
    public class DimeQuadro80Model
    {
        [FileWriter(Position = 1, Size = 2)]
        public int REG { get; set; }

        [FileWriter(Position = 2, Size = 2)]
        public int QUADRO { get; set; }

        [FileWriter(Position = 3, Size = 3)]
        public string ITEM { get; set; }

        [FileWriter(Position = 3, Size = 17)]
        public double VALOR { get; set; }
    }
}
