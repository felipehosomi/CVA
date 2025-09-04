using CVA.AddOn.Common.Controllers;
using CVA.View.Romaneio.DAO.Resources;
using CVA.View.Romaneio.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Romaneio.BLL
{
    public class VehicleBLL
    {
        public static VehicleModel Get(string code)
        {
            string sql = String.Format(Query.Vehicle_Get, code);
            CrudController controller = new CrudController();
            return controller.FillModelAccordingToSql<VehicleModel>(sql);
        }
    }
}
