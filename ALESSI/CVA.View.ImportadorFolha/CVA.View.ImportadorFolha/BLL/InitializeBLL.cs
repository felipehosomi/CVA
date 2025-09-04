using CVA.AddOn.Common.Controllers;
using System.IO;
using System.Reflection;

namespace CVA.View.ImportadorFolha.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            EventFilterController.SetDefaultEvents();
            UserFieldsBLL.CreateUserFields();
            MenuController.LoadFromXML(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Menu.xml");
            //FormattedSearchBLL.CreateFormattedSeaches();
        }
    }
}
