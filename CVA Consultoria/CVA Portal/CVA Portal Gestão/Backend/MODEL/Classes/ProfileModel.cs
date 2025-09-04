using MODEL.Enumerators;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.Interface;

namespace MODEL.Classes
{
    public class ProfileModel : IModel
    {
        public static int oObjectType { get { return (int)ObjectType.Profile; } }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Initials { get; set; }
        public List<UserViewModel> UserView { get; set; }
        public string FinancialAccess { get; set; }
    }
}
