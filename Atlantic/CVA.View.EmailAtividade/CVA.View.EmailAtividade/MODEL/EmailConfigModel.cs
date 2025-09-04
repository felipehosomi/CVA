using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAtividade.MODEL
{
    public class EmailConfigModel
    {
        public string Code { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string SSL { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
