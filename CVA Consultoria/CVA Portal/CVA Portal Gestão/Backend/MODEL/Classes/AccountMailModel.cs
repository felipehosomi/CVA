using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class AccountMailModel : IModel
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public string DeliveryMethod { get; set; }
        public bool UseDefaultCredentials { get; set; }
     
        public string Password { get; set; }
        public string AddressMail { get; set; }
    }
}
