using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CVA.MonitorReplicacao.Helper
{
    [AttributeUsageAttribute(AttributeTargets.Method)]
    public class Authorize : Attribute
    {
        public Authorize(string method)
        {
            bool isAuthorized = false;

            if (method == "TESTE")
                isAuthorized = true;
            
            if (!isAuthorized)
                throw new SecurityException("You don't have the rights to perform this action");
        }
    }
}
