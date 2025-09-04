
using MODEL.Enumerators;
using MODEL.Interface;

namespace MODEL.Classes
{
    public class ExpenseTypeModel : IModel
    {
        public static int oObjectType { get { return (int)ObjectType.ExpenseType; } }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UnitMeter { get; set; }
    }
}
