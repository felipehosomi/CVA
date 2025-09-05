using CVA.Hub.SERVICE.OPRC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hub.BLL
{
    public class CentroCustoBLL
    {
        private CentroCustoDAO _CentroCustoDAO { get; }

        public CentroCustoBLL(CentroCustoDAO centroCustoDAO)
        {
            _CentroCustoDAO = centroCustoDAO;
        }

        public string GetDesc(string code)
        {
            return _CentroCustoDAO.GetDesc(code);
        }
    }
}
