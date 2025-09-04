using MODEL.Interface;

namespace MODEL.Classes
{
    public class OportunittyFinancialModel : IModel
    {
        public decimal OportunittyValue { get; set; }
        public decimal BusinessValue { get; set; }
        public int Temperature { get; set; }
    }
}
