using MODEL.Enumerators;
using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class ActivityTypeModel : IModel
    {
        public static int oObjectType { get { return (int)ObjectType.ActivityType; } }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
