using CVA.AddOn.Control.Logic.DAO.CVACommon;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.BLL.CVACommon
{
    public class BaseBLL
    {
        BaseDAO _baseDAO { get; set; }

        public BaseBLL()
        {
            _baseDAO = new BaseDAO();
        }

        public Base GetByName(string database)
        {
            return _baseDAO.GetByName(database);
        }

        public int GetMaxId()
        {
            return _baseDAO.GetMaxId();
        }
    }
}
