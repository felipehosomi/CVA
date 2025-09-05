namespace MarketingDocTestSL
{
    public class OrderModel
    {
        public int? DocEntry { get; set; }
        public int? DocNum { get; set; }
        public string DocDueDate { get; set; }
        public string CardCode { get; set; }
        public DocumentLine[] DocumentLines { get; set; }
    }

    public class DocumentLine
    {
        public string ItemCode { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
    }
}