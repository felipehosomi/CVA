using System;

namespace CVA.Cointer.BoePrinter.Model
{
    public class BoeModel
    {
        public Int64 IntegrationId { get; set; }
        public int DocNum { get; set; }
        public int Serial { get; set; }
        public int Bancada { get; set; }
        public int ContractBank { get; set; }
        public string OurNumber { get; set; }

        public DateTime DateCreate { get; set; }
    }
}
