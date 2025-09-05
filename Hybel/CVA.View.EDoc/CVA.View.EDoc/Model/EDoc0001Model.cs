using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDoc0001Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; } = "0001";

        [FileWriter(Position = 2, Size = 1)]
        public string Indicador { get; set; } = "0";
    }
}
