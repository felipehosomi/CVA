using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDoc9900Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; } = "9900";

        [FileWriter(Position = 2, Size = 4)]
        public string SiglaLinha { get; set; }

        [FileWriter(Position = 3, Size = 6)]
        public int QtdeLinhas { get; set; }
    }
}
