using CVA.AddOn.Common.Controllers;
using System.IO;
using System.Reflection;

namespace CVA.View.Hybel.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            EventFilterBLL.SetDefaultEvents();
            UserFieldsBLL.CreateUserFields();
            FormattedSearchBLL.CreateFormattedSeaches();
            MenuController.LoadFromXML(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Menu.xml");
        }
    }
}
