using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.DAO.CVA_DOC_FRETE
{
    public class DocFreteDAO
    {
        public int GetLastCode()
        {
            return Convert.ToInt32(CrudController.ExecuteScalar(String.Format(Query.DocFrete_GetLastCode)));
        }
    }
}
