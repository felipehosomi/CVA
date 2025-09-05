using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDoc0200Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; }

        [FileWriter(Position = 2, Size = 28)]
        public string ItemCode { get; set; }

        [FileWriter(Position = 3, Size = 80)]
        public string ItemName { get; set; }

        [FileWriter(Position = 4, Size = 2)]
        public string CodGenero { get; set; }

        [FileWriter(Position = 5, Size = 4)]
        public string CodLST { get; set; }
    }
}
