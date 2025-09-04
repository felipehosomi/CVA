using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL.CVACommon;
using CVA.Core.ControlCommon.SERVICE.CVACommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.BLL.CVACommon
{
    public class BaseBLL
    {
        BaseDAO _baseDAO { get; set; }

        public BaseBLL(BaseDAO baseDAO)
        {
            _baseDAO = baseDAO;
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
