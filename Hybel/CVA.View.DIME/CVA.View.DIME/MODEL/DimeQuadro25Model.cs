using CVA.AddOn.Common.Attributes;

namespace CVA.View.DIME.MODEL
{
    public class DimeQuadro25Model
    {
        [FileWriter(Position = 1, Size = 2)]
        public string Tipo { get; set; } = "46";

        [FileWriter(Position = 3, Size = 2)]
        public string Quadro { get; set; } = "25";

        [FileWriter(Position = 5, Size = 3)]
        public int Sequencia { get; set; }

        [FileWriter(Position = 8, Size = 15, PaddingChar = "0", PaddingType = AddOn.Common.Enums.PaddingTypeEnum.Left)]
        public int Identificacao { get; set; }

        [FileWriter(Position = 23, Size = 17)]
        public double Valor { get; set; }

        [FileWriter(Position = 40, Size = 2)]
        public int Origem { get; set; }
    }
}
