using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Escoteiros.CriadorDeCampos.BLL
{
    public class InitializeBLL
    {

        public static void Initialize()
        {            
            UserFieldsBLL.CreateUserFields();
            UserFieldsBLL.CreateLogin();            

        }
    }
}
