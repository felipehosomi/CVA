using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.TransferenciaFiliais.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            EventFilterBLL.SetDefaultEvents();
        }
    }
}
