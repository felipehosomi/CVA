using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.Core.Escoteiros.BLL
{
    public class InitializeBLL
    {

        public static void Initialize()
        {
            //EventFilterBLL.SetDefaultEvents();
            UserFieldsBLL.CreateUserFields();
            //UserFieldsBLL.CreateLogin();

            MenuController.LoadFromXML(Application.StartupPath + "\\Menu\\Menu.xml");

        }
    }
}
