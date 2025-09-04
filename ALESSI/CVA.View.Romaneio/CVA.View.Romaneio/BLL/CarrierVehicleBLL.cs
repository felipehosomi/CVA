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
    public class CarrierVehicleBLL
    {
        public string AddList(string carrierCode, List<CarrierVehicleModel> list)
        {
            if (list.Count > 0)
            {
                string deleteSql = String.Format(Query.CarrierVehicle_Delete, list[0].CarrierCode);
                CrudController.ExecuteNonQuery(deleteSql);

                CrudController crudController = new CrudController("@CVA_CAR_VEHICLE");
                try
                {
                    foreach (var item in list)
                    {
                        if (String.IsNullOrEmpty(item.Vehicle.Trim()))
                        {
                            continue;
                        }
                        item.CarrierCode = carrierCode;

                        crudController.Model = item;
                        crudController.UserTableType = SAPbobsCOM.BoUTBTableType.bott_NoObject;
                        crudController.CreateModel();
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return String.Empty;
        }

        public static string GetCarVehicleSql(string carrierCode)
        {
            return String.Format(Query.Vehicle_GetByCarrierCode, carrierCode);
        }

        public static string GetCarVehicleComboboxSql(string carrierCode)
        {
            return String.Format(Query.Vehicle_GetForCombobox, carrierCode);
        }
    }
}
