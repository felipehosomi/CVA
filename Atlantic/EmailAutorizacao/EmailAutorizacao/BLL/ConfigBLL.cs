using EmailAutorizacao.MODEL;
using EmailAutorizacao.SERVICE.UserTables;

namespace EmailAutorizacao.BLL
{
    public class ConfigBLL
    {
        public static ConfigModel GetConfig(int tipo)
        {
            return ConfigDAO.GetConfig(tipo);
        }
    }
}
