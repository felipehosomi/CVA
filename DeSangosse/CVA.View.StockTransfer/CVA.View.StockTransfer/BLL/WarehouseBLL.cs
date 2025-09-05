using CVA.AddOn.Common.Controllers;
using CVA.View.StockTransfer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.StockTransfer.BLL
{
    public class WarehouseBLL
    {
        public static void UpdateWhsTransferDefault(string whsCode)
        {
            CrudController.ExecuteNonQuery(String.Format(Query.Warehouse_UpdateTransferDefault, whsCode));
        }
    }
}
