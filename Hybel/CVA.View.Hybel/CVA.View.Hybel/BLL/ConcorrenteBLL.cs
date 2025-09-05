using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Hybel.BLL
{
    public class ConcorrenteBLL
    {
        public static int GetItemQuantity(string concorrente)
        {
            return Convert.ToInt32(CrudController.ExecuteScalar(String.Format(SQL.Concorrente_GetCount, concorrente)));
        }

        public static string GetItemCode(string concorrente)
        {
            return CrudController.ExecuteScalar(String.Format(SQL.Concorrente_GetItemCode, concorrente)).ToString();
        }
    }
}
