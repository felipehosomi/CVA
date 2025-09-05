using CVA.AddOn.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.DIME.MODEL
{
    public class DimeQuadro12Model
    {
        [FileWriter(Position = 1, Size = 2)]
        public string Tipo { get; set; } = "33";

        [FileWriter(Position = 3, Size = 2)]
        public string Quadro { get; set; } = "12";

        [FileWriter(Position = 5, Size = 1)]
        public int Origem { get; set; }

        [FileWriter(Position = 6, Size = 4)]
        public int Receita { get; set; }

        [FileWriter(Size = 8, Position = 10, Format = "ddMMyyyy")]
        public DateTime Vencimento { get; set; }

        [FileWriter(Size = 17, Position = 18)]
        public double Valor { get; set; }

        [FileWriter(Size = 5, Position = 35)]
        public int Classe { get; set; }

        [FileWriter(Size = 15, Position = 40, PaddingChar = "0", PaddingType = AddOn.Common.Enums.PaddingTypeEnum.Left)]
        public string NrAcordo { get; set; }
    }
}
