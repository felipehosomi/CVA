using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using CVA.AddOn.Common.Controllers;
using System;

namespace CVA.AddOn.Control.Logic.DAO.CVACommon
{
    public class BaseDAO
    {
        public Base GetByName(string database)
        {
            SqlController sqlHelper = new SqlController(StaticKeys.ConnectionString);
            string query = String.Format(Resources.Query.Base_GetByName, database);
            return sqlHelper.FillModelAccordingToSql<Base>(query);
        }

        public int GetMaxId()
        {
            SqlController sqlHelper = new SqlController(StaticKeys.ConnectionString);
            string query = String.Format(Resources.Query.Base_GetMaxId);
            return Convert.ToInt32(sqlHelper.ExecuteScalar(query));
        }
    }
}
