using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.SERVICE.BaseReplicadora
{
    public class GenericDAO
    {
        BusinessOneDAO _businessOneDAO { get; set; }

        public GenericDAO(BusinessOneDAO businessOneDAO)
        {
            _businessOneDAO = businessOneDAO;
        }

        public string GetMaxId(string columnName, string tableName)
        {
            var query = String.Format(SERVICE.Resource.Query.Generic_GetMax, columnName, tableName);
            return _businessOneDAO.ExecuteSqlForObject<string>(query);
        }
    }
}
