using CVA.Core.TransportLCM.MODEL;
using CVA.Core.TransportLCM.SERVICE.Resource;
using Dover.Framework.DAO;
using System;

namespace CVA.Core.TransportLCM.SERVICE.UserTables
{
    public class ConfigDAO
    {
        private BusinessOneDAO _businessOneDAO { get; }

        public ConfigDAO(BusinessOneDAO businessOneDAO)
        {
            _businessOneDAO = businessOneDAO;
        }

        public ConfigModel GetConfig(int tipo)
        {
            return _businessOneDAO.ExecuteSqlForObject<ConfigModel>(String.Format(Query.Config_Get, tipo));
        }
    }
}
