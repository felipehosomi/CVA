using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDoc9990Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; } = "9990";

        [FileWriter(Position = 2, Size = 6)]
        public int QtdeLinhas { get; set; }
    }
}
