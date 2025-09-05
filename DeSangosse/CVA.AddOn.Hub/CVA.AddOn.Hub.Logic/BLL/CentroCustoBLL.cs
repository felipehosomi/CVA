using CVA.Hub.DAO.OPRC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hub.BLL
{
    public class CentroCustoBLL
    {
        public string GetDesc(string code)
        {
            return CentroCustoDAO.GetDesc(code);
        }
    }
}
