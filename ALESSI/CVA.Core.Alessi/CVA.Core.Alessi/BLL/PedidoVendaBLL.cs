using CVA.AddOn.Common.Controllers;
using CVA.Core.Alessi.DAO.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.BLL
{
    public class PedidoVendaBLL
    {
        public static string GetRegra(string docEntry)
        {
            object regra = CrudController.ExecuteScalar(String.Format(Query.BuscaRegraPedido, docEntry));
            if (regra != null)
            {
                return regra.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
