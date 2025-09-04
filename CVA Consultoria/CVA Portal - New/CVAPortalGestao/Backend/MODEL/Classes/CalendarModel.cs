using MODEL.Enumerators;
using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class CalendarModel : IModel
    {
        public static int oObjectType { get {return (int)ObjectType.Calendar_Header;} }
        public DateTime InitialDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        
        public List<Calendar1Model> Holidays { get; set; }
    }
}
