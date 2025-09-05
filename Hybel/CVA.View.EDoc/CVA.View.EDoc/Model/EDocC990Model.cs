using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDocC990Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; } = "C990";

        [FileWriter(Position = 2, Size = 6)]
        public int QtdeLinhas { get; set; }
    }
}
