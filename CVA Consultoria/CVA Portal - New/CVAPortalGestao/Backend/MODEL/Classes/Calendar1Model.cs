using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class Calendar1Model : IModel
    {
        public int CalendarId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
