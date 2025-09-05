using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B1.WFN.API.Models
{
    public class DIAPIConnectionSettings
    {
        public string Language { get; set; }
        public string Server { get; set; }
        public string ServerInstance { get; set; }
        public string CompanyDB { get; set; }
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DbServerType { get; set; }
        public bool UseTrusted { get; set; }
        public string SLDServer { get; set; }
    }
}
