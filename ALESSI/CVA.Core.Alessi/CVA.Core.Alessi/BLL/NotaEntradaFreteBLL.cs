using CVA.AddOn.Common.Controllers;
using CVA.Core.Alessi.DAO.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.BLL
{
    public class NotaEntradaFreteBLL
    {
        public static bool Exists(int docEntry)
        {
            object hasRows = CrudController.ExecuteScalar(String.Format(Query.NotaEntradaFrete_ExistsByDocEntry, docEntry));
            return hasRows != null;
        }
    }
}
