namespace CVA.Escoteiro.Magento.Models.Magento
{
    public class OrderComment
    {
        public StatusHistory statusHistory { get; set; }
    }

    public class StatusHistory
    {
        public string comment { get; set; }
        public string created_at { get; set; }
        public int is_customer_notified { get; set; }
        public int is_visible_on_front { get; set; }
        public int parent_id { get; set; }
        public string status { get; set; }
    }
}