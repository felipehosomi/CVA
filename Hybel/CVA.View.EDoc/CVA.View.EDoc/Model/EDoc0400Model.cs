using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDoc0400Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; }

        [FileWriter(Position = 2, Size = 6)]
        public int UsageId { get; set; }

        [FileWriter(Position = 3, Size = 60)]
        public string UsageDesc { get; set; }

        [FileWriter(Position = 4, Size = 4)]
        public string ICMS { get; set; }
    }
}
