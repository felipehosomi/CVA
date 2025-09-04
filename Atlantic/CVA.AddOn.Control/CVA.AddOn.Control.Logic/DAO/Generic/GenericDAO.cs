using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.DAO.Generic
{
    public class GenericDAO
    {
        public string GetMaxId(string columnName, string tableName)
        {
            var query = String.Format(Resources.Query.Generic_GetMax, columnName, tableName);
            return CrudController.ExecuteScalar(query).ToString();
        }
    }
}
