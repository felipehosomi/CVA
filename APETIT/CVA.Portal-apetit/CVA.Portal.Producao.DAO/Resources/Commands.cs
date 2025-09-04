using System.Resources;

namespace CVA.Portal.Producao.DAO.Resources
{
    public class Commands
    {
        public static ResourceManager Resource;

        public static void SetResourceManager()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["ServerType"] == "9")
            {
                Resource = new ResourceManager("CVA.Portal.Producao.DAO.Resources.HanaCommands", typeof(HanaCommands).Assembly);
            }
            else
            {
                Resource = new ResourceManager("CVA.Portal.Producao.DAO.Resources.SqlCommands", typeof(SqlCommands).Assembly);
            }
        }
    }
}
