using MODEL.Enumerators;
using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class ResourceSpecialtyModel : IModel
    {
        public static int oObjectType { get { return (int)ObjectType.Specialty; } }
        public int CollaboratorId { get; set; }
        public string CollaboratorName { get; set; }
        public int SpecialtyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }

    }
}
