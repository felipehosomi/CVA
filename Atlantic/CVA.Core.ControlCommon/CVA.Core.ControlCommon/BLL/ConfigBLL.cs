using CVA.Core.ControlCommon.MODEL;
using CVA.Core.ControlCommon.SERVICE.UserTables;

namespace CVA.Core.ControlCommon.BLL
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
