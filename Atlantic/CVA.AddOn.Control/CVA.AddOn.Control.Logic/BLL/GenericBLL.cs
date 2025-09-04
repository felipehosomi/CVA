using CVA.AddOn.Control.Logic.DAO.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.BLL
{
    public class GenericBLL
    {
        public string GetMaxId(string columnName, string tableName)
        {
            GenericDAO genericDAO = new GenericDAO();
            return genericDAO.GetMaxId(columnName, tableName);
        }
    }
}
