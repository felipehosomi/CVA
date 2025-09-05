using CVA.AddOn.Common.Attributes;

namespace CVA.View.EDoc.Model
{
    public class EDoc0030Model
    {
        [FileWriter(Position = 1, Size = 4)]
        public string Linha { get; set; } = "0030";

        [FileWriter(Position = 2, Size = 1)]
        public int IndicadorEntrada { get; set; } = 1;

        [FileWriter(Position = 3, Size = 1)]
        public int IndicadorArquivo { get; set; } = 9;

        [FileWriter(Position = 4, Size = 1)]
        public int PerfilISS { get; set; } = 9;

        [FileWriter(Position = 5, Size = 1)]
        public int PerfilICMS { get; set; } = 2;

        [FileWriter(Position = 6, Size = 1)]
        public int PerfilRIDF { get; set; } = 1;

        [FileWriter(Position = 7, Size = 1)]
        public int PerfilRUDF { get; set; } = 1;

        [FileWriter(Position = 8, Size = 1)]
        public int PerfilLMC { get; set; } = 1;

        [FileWriter(Position = 9, Size = 1)]
        public int PerfilRV { get; set; } = 1;

        [FileWriter(Position = 10, Size = 1)]
        public int PerfilRI { get; set; } = 0;

        [FileWriter(Position = 11, Size = 1)]
        public int IndicadorEC { get; set; } = 9;

        [FileWriter(Position = 12, Size = 1)]
        public int ISS { get; set; } = 1;

        [FileWriter(Position = 13, Size = 1)]
        public int RT { get; set; } = 1;

        [FileWriter(Position = 14, Size = 1)]
        public int ICMS { get; set; } = 0;

        [FileWriter(Position = 15, Size = 1)]
        public int ST { get; set; } = 0;

        [FileWriter(Position = 16, Size = 1)]
        public int AT { get; set; } = 1;

        [FileWriter(Position = 17, Size = 1)]
        public int IPI { get; set; } = 0;

        [FileWriter(Position = 18, Size = 1)]
        public string RI { get; set; } = "";
    }
}
