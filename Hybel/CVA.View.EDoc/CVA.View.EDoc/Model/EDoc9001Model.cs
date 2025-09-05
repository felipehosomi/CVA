using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDoc9001Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; } = "9001";

        [FileWriter(Position = 2, Size = 1)]
        public string Indicador { get; set; } = "0";
    }
}
