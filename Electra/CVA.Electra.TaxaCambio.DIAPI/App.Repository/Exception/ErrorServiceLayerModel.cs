using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repository.Exception
{
    
    public class MessageServiceLayer
    {
        public string lang { get; set; }
        public string value { get; set; }
    }

    public class ErrorServiceLayer
    {
        public int code { get; set; }
        public MessageServiceLayer message { get; set; }
    }

    public class ErrorServiceLayerException
    {
        public ErrorServiceLayer error { get; set; }
    }
}
