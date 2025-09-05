namespace Escoteiro.Magento.Models
{
    public class CVA_MAGENTO_STOCK_Model
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string U_BarCode { get; set; }
        public string U_ItemCode { get; set; }
        public string U_ItemName { get; set; }
        public double U_WhsQty { get; set; }
        public object U_LastSyncDt { get; set; }
        public object U_LastSyncHr { get; set; }
    }
}