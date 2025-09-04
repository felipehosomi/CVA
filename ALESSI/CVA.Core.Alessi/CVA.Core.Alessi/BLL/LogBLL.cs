using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.BLL
{
    public class LogBLL
    {
        public static log4net.ILog Logger;

        public static void StartLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
