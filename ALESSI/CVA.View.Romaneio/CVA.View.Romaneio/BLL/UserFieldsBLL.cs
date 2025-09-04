using CVA.AddOn.Common;
using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;
using System;

namespace CVA.View.Romaneio.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();
            try
            {
                userObjectController.CreateUserTable("CVA_VEH_TYPE", "CVA - Tipo Veículo", BoUTBTableType.bott_MasterData);
                userObjectController.CreateUserObject("CVA_VEH_TYPE", "CVA - Tipo Veículo", "@CVA_VEH_TYPE", BoUDOObjType.boud_MasterData);

                userObjectController.CreateUserTable("CVA_VEHICLE", "CVA - Veículo", BoUTBTableType.bott_MasterData);
                userObjectController.InsertUserField("@CVA_VEHICLE", "VehicleType", "Cód. Tipo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
                userObjectController.InsertUserField("@CVA_VEHICLE", "Plate", "Placa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
                userObjectController.InsertUserField("@CVA_VEHICLE", "Color", "Placa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_VEHICLE", "Driver", "Motorista", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

                userObjectController.CreateUserObject("CVA_VEHICLE", "CVA - Veículo", "@CVA_VEHICLE", BoUDOObjType.boud_MasterData);
                userObjectController.MakeFieldsSearchable("CVA_VEHICLE");

                userObjectController.CreateUserTable("CVA_CARRIER", "CVA - Transp. X Veículo", BoUTBTableType.bott_MasterData);
                userObjectController.InsertUserField("@CVA_CARRIER", "CarrierName", "Nome Transportadora", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);

                userObjectController.CreateUserObject("CVA_CARRIER", "CVA - Transp. X Veículo", "@CVA_CARRIER", BoUDOObjType.boud_MasterData);
                userObjectController.MakeFieldsSearchable("CVA_CARRIER");

                userObjectController.CreateUserTable("CVA_CAR_VEHICLE", "CVA - Transp. X Veículo", BoUTBTableType.bott_NoObject);
                userObjectController.InsertUserField("@CVA_CAR_VEHICLE", "CarrierCode", "Cód. Transp.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
                userObjectController.InsertUserField("@CVA_CAR_VEHICLE", "CodeVehicle", "Cód. Veículo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);

                userObjectController.CreateUserTable("CVA_WAYBILL", "CVA - Romaneio", BoUTBTableType.bott_MasterData);

                userObjectController.InsertUserField("@CVA_WAYBILL", "Status", "Status", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObjectController.AddValidValueToUserField("@CVA_WAYBILL", "Status", "1", "Gerado", true);
                userObjectController.AddValidValueToUserField("@CVA_WAYBILL", "Status", "2", "Cancelado");

                userObjectController.InsertUserField("@CVA_WAYBILL", "Date", "Data Embarque", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);

                userObjectController.InsertUserField("@CVA_WAYBILL", "Carrier", "Transportadora", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
                userObjectController.InsertUserField("@CVA_WAYBILL", "Comments", "Observação", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_WAYBILL", "Plate", "Placa Veículo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_WAYBILL", "Vehicle", "Veículo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_WAYBILL", "VehType", "Tipo Veículo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
                //userObjectController.InsertUserField("@CVA_WAYBILL", "Brand", "Marca", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                //userObjectController.InsertUserField("@CVA_WAYBILL", "Model", "Modelo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_WAYBILL", "Color", "Cor", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_WAYBILL", "Driver", "Motorista", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

                userObjectController.CreateUserObject("CVA_WAYBILL", "CVA - Romaneio", "@CVA_WAYBILL", BoUDOObjType.boud_MasterData, true);
                userObjectController.MakeFieldsSearchable("@CVA_WAYBILL");

                userObjectController.CreateUserTable("CVA_WAYBILL_INV", "CVA - Romaneio - NF's", BoUTBTableType.bott_MasterDataLines);
                userObjectController.AddChildTableToUserObject("CVA_WAYBILL", "CVA_WAYBILL_INV");

                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "DocNum", "Nr. Doc", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, true);
                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "Invoice", "Nr. NF", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, true);
                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "CardCode", "Cód. PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, true);
                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "CardName", "Nome PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "Date", "Data", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "CarrierCode", "Cód. Transportadora", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "CarrierName", "Nome Transportadora", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "DocTotal", "Total Doc", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "GrossWeight", "Peso Bruto", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "State", "Estado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2);
                userObjectController.InsertUserField("@CVA_WAYBILL_INV", "City", "Cidade", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage("Erro ao criar campos de usuário: " + ex.Message);
            }
        }
    }
}
