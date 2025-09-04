using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.DataBase;
using System.IO;
using System.Reflection;

namespace CVA.Core.ObrigacoesFiscais.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            try
            {
                string applicationPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                UserFieldsBLL.CreateUserFields();
                EventFilterBLL.CreateDefaultEvents();
                MenuController.LoadFromXML(Path.Combine(applicationPath, "Menu.xml"));
                //StoredProcedureController.CreateSprocs(Path.Combine(applicationPath, "DAO", "Sproc"));
            }
            catch { }
        }
    }
}
