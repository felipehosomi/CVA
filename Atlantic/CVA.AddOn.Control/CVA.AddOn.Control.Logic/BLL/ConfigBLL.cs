using CVA.AddOn.Control.Logic.DAO.UserTables;
using CVA.AddOn.Control.Logic.MODEL;

namespace CVA.AddOn.Control.Logic.BLL
{
    public class ConfigBLL
    {
        private ConfigDAO _configDAO { get; }

        public ConfigBLL()
        {
            _configDAO = new ConfigDAO();
        }

        public ConfigModel GetConfig(int tipo)
        {
            return _configDAO.GetConfig(tipo);
        }
    }
}
