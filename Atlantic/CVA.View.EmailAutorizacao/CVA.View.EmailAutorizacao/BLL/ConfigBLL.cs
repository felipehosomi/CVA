using CVA.View.EmailAutorizacao.MODEL;
using CVA.View.EmailAutorizacao.SERVICE.UserTables;

namespace CVA.View.EmailAutorizacao.BLL
{
    public class ConfigBLL
    {
        private ConfigDAO _configDAO { get; }

        public ConfigBLL(ConfigDAO configDAO)
        {
            _configDAO = configDAO;
        }

        public ConfigModel GetConfig(int tipo)
        {
            return _configDAO.GetConfig(tipo);
        }
    }
}
