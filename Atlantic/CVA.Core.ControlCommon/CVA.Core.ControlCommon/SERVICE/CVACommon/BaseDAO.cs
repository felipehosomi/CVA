using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL.CVACommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.SERVICE.CVACommon
{
    public class BaseDAO
    {
        public Base GetByName(string database)
        {
            SqlHelper sqlHelper = new SqlHelper(StaticKeys.ConnectionString);
            string query = String.Format(Resource.Query.Base_GetByName, database);
            return sqlHelper.FillModelAccordingToSql<Base>(query);
        }

        public int GetMaxId()
        {
            SqlHelper sqlHelper = new SqlHelper(StaticKeys.ConnectionString);
            string query = String.Format(Resource.Query.Base_GetMaxId);
            return Convert.ToInt32(sqlHelper.ExecuteScalar(query));
        }
    }
}
