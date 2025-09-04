using CVA.Core.ControlCommon.SERVICE.BaseReplicadora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.BLL.BaseReplicadora
{
    public class GenericBLL
    {
        GenericDAO _GenericDAO { get; set; }

        public GenericBLL(GenericDAO genericDAO)
        {
            _GenericDAO = genericDAO;
        }

        public string GetMaxId(string columnName, string tableName)
        {
            return _GenericDAO.GetMaxId(columnName, tableName);
        }
    }
}
