using CVA.AddOn.Common.Controllers;
using System.IO;
using System.Reflection;

namespace CVA.View.EDoc.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            UserFieldsBLL.CreateUserFields();
            //SprocBLL.CreateSprocs();
            EventFilterBLL.CreateDefaultEvents();
            MenuController.LoadFromXML(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Menu\\Menu.xml");
        }
    }
}
