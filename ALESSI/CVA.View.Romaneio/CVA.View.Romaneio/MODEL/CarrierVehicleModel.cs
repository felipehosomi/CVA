using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Romaneio.MODEL
{
    public class CarrierVehicleModel
    {
        [ModelController(UIFieldName = "Veículo", ColumnName = "U_CodeVehicle")]
        public string Vehicle { get; set; }

        [ModelController(ColumnName = "U_CarrierCode")]
        public string CarrierCode { get; set; }
    }
}
