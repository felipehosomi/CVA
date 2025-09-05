using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDocC300Model
    {
        public int DocEntry { get; set; }
        public string ObjType { get; set; }
        public int UsageId { get; set; }

        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; }

        [FileWriter(Position = 2, Size = 4)]
        public int NumeroLinha { get; set; }

        [FileWriter(Position = 3, Size = 28)]
        public string ItemCode { get; set; }

        [FileWriter(Position = 4, Size = 20)]
        public string UN { get; set; }

        [FileWriter(Position = 5, Size = 50)]
        public double ValorUnitario { get; set; }

        [FileWriter(Position = 6, Size = 50)]
        public double Quantidade { get; set; }

        [FileWriter(Position = 7, Size = 50)]
        public double ValorDesconto { get; set; }

        [FileWriter(Position = 8, Size = 50)]
        public double ValorJuros { get; set; }

        [FileWriter(Position = 9, Size = 50)]
        public double ValorLiquido { get; set; }

        [FileWriter(Position = 10, Size = 8, OnylNumeric = true)]
        public string NCM { get; set; }

        [FileWriter(Position = 11, Size = 4, OnylNumeric = true)]
        public string CST { get; set; }

        [FileWriter(Position = 12, Size = 4)]
        public string CFOP { get; set; }

        [FileWriter(Position = 13, Size = 50)]
        public double BaseICMS { get; set; }

        [FileWriter(Position = 14, Size = 50)]
        public double AliquotaICMS { get; set; }

        [FileWriter(Position = 15, Size = 50)]
        public double ValorICMS { get; set; }

        [FileWriter(Position = 16, Size = 50)]
        public double BaseICMS_ST { get; set; }

        [FileWriter(Position = 17, Size = 50)]
        public double AliquotaICMS_ST { get; set; }

        [FileWriter(Position = 18, Size = 50)]
        public double ValorICMS_ST { get; set; }

        [FileWriter(Position = 19, Size = 50)]
        public double BaseIPI { get; set; }

        [FileWriter(Position = 20, Size = 50)]
        public double AliquotaIPI { get; set; }

        [FileWriter(Position = 21, Size = 50)]
        public double ValorIPI { get; set; }
    }
}
