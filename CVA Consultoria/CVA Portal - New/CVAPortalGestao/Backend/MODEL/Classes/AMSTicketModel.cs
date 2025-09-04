using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class AMSTicketModel : IModel
    {
        public string StatusAMS { get; set; }
        public string Description { get; set; }
        public string NumTck { get; set; }
        public string ClientAMS { get; set; }
        public string Request { get; set; }
    }
}
