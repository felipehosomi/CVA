using CVA.AddOn.Common.Attributes;

namespace CVA.View.DIME.MODEL
{
    public class DimeDocumentModel
    {
        [FileWriter(Position = 1, Size = 2)]
        public string Tipo { get; set; }

        [FileWriter(Position = 3, Size = 2)]
        public string Quadro { get; set; } = "01";

        [FileWriter(Position = 5, Size = 5, PaddingChar = "0", PaddingType = AddOn.Common.Enums.PaddingTypeEnum.Left)]
        public string CFOP { get; set; }

        [FileWriter(Position = 10, Size = 17, DecimalPlaces = 2)]
        public double ValorContabil { get; set; }

        [FileWriter(Position = 27, Size = 17, DecimalPlaces = 2)]
        public double BaseCalculo { get; set; }

        [FileWriter(Position = 44, Size = 17, DecimalPlaces = 2)]
        public double ImpostoCreditado { get; set; }

        [FileWriter(Position = 61, Size = 17, DecimalPlaces = 2)]
        public double Isentas { get; set; }

        [FileWriter(Position = 78, Size = 17, DecimalPlaces = 2)]
        public double Outras { get; set; }

        [FileWriter(Position = 95, Size = 17, DecimalPlaces = 2)]
        public double BaseCalculoImpostoRetido { get; set; }

        [FileWriter(Position = 112, Size = 17, DecimalPlaces = 2)]
        public double ImpostoRetido { get; set; }

        [FileWriter(Position = 129, Size = 17, DecimalPlaces = 2)]
        public double DiferencaAliquota { get; set; }
    }
}
