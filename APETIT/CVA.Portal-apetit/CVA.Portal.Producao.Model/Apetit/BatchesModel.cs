namespace CVA.Portal.Producao.Model
{
    public class BatchesModel
    {
        public string ItemCode { get; set; }

        public string DistNumber { get; set; }

        public string MnfSerial { get; set; }

        public string LotNumber { get; set; }

        public string Notes { get; set; }

        public double Quantity { get; set; }

        public string WhsCode { get; set; }
    }

    public class BatchesControlModel
    {
        public string ManBtchNum { get; set; }
    }

    
}
