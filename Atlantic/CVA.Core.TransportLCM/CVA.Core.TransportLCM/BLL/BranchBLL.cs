using CVA.Core.TransportLCM.SERVICE.OBPL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.BLL
{
    public class BranchBLL
    {
        BranchDAO _branchDAO { get; set; }

        public BranchBLL(BranchDAO branchDAO)
        {
            _branchDAO = branchDAO;
        }

        public string GetNameById(int id)
        {
            return _branchDAO.GetNameById(id);
        }
    }
}
