using Newtonsoft.Json;

namespace Fibra.ItemPricing.Models
{
    class ItemCostJournal
    {
        [JsonProperty("Code")]
        public long Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("U_CreateDate")]
        public object CreateDate { get; set; }

        [JsonProperty("U_UpdateDate")]
        public object UpdateDate { get; set; }

        [JsonProperty("U_DocEntry")]
        public int DocEntry { get; set; }

        [JsonProperty("U_DocType")]
        public string DocType { get; set; }

        [JsonProperty("U_ItemCode")]
        public string ItemCode { get; set; }

        [JsonProperty("U_WhsCode")]
        public string WhsCode { get; set; }

        [JsonProperty("U_PriceList")]
        public int PriceList { get; set; }

        [JsonProperty("U_Cost")]
        public double Cost { get; set; }

        [JsonProperty("U_Status")]
        public string Status { get; set; }

        [JsonProperty("U_ErrorMsg")]
        public string ErrorMessage { get; set; }
    }

    public enum PricingStatus
    {
        Open = 'O',
        Close = 'C',
        Error = 'E'
    }
}
