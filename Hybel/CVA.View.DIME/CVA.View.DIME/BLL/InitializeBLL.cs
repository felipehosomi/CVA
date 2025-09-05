using CVA.AddOn.Common.Controllers;

namespace CVA.View.DIME.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            EventFilterController.SetDefaultEvents();
            UserFieldsBLL.CreateUserFields();
            MenuController.LoadFromXML(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Menu.xml");
        }
    }
}
