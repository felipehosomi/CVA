using MODEL.Interface;

namespace MODEL.Classes
{
    public class PoliticExpenseModel : IModel
    {
        public ExpenseTypeModel Expense { get; set; }
        public string Value { get; set; }
    }
}
