using CVA.AddOn.Common.Controllers;
using CVA.View.StockTransfer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.StockTransfer.BLL
{
    public class UsageBLL
    {
        public static bool ValidateTransfer(string usageId)
        {
            object validateTransfer = CrudController.ExecuteScalar(String.Format(Query.Usage_GetValidateTransfer, usageId));
            return validateTransfer != null && validateTransfer.ToString() == "Y";
        }
    }
}
