using CVA.AddOn.Common.Controllers;
using System.IO;
using System.Reflection;

namespace CVA.View.ObservacoesFiscais.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            UserFieldsBLL.CreateUserFields();
            FormattedSearchBLL.CreateFormattedSeaches();
            MenuController.LoadFromXML(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Menu.xml");
        }
    }
}
