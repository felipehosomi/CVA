using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.SERVICE.OBPL
{
    public class BranchDAO
    {
        BusinessOneDAO _businessOneDAO { get; set; }

        public BranchDAO(BusinessOneDAO businessOneDAO)
        {
            _businessOneDAO = businessOneDAO;
        }

        public string GetNameById(int id)
        {
            var query = String.Format(SERVICE.Resource.Query.Branch_GetNameById, id);
            return _businessOneDAO.ExecuteSqlForObject<string>(query);
        }
    }
}
