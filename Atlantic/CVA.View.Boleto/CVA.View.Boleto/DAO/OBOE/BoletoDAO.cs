using CVA.AddOn.Common.Controllers;
using CVA.View.Boleto.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Boleto.DAO.OBOE
{
    public class BoletoDAO
    {
        public static int GetLastCode()
        {
             return Convert.ToInt32(CrudController.ExecuteScalar(Query.Boleto_GetLastCode));
        }
    }
}
