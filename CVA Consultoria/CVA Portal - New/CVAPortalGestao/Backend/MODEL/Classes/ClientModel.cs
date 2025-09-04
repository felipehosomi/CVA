using System.Collections.Generic;
using MODEL.Enumerators;
using MODEL.Interface;

namespace MODEL.Classes
{
    public class ClientModel : IModel
    {
        public static int oObjectType { get { return (int)ObjectType.Client; } }
        public string Name { get; set; }
        public string FantasyName { get; set; }
        public string CNPJ { get; set; }
        public string IE { get; set; }
        public string Description { get; set; }
        public string DescriptionAMS { get; set; }
        public List<AddressModel> Addresses { get; set; }
        public List<ContactModel> Contact { get; set; }
        public List<PoliticExpenseModel> PoliticExpense { get; set; }
        public int LocalPoliticExpense { get; set; }
        public string Tag { get; set; }
    }
}
