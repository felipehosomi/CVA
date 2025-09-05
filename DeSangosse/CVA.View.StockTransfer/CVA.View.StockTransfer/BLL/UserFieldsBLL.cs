using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.View.StockTransfer.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            userObjectController.InsertUserField("OCRD", "CVA_Whs_Transf", "Depósito Transferência", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("OUSG", "CVA_Transfer", "Gera Transferência", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OUSG", "CVA_Transfer", "N", "Não", true);
            userObjectController.AddValidValueToUserField("OUSG", "CVA_Transfer", "Y", "Sim");

            userObjectController.InsertUserField("OWHS", "CVA_Transfer_Out", "Depósito saída transf.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OWHS", "CVA_Transfer_Out", "N", "Não", true);
            userObjectController.AddValidValueToUserField("OWHS", "CVA_Transfer_Out", "Y", "Sim");

            //userObjectController.InsertUserField("ORDR", "CVA_Transf", "Nº Transferência", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            userObjectController.InsertUserField("OWTQ", "CVA_Base_Doc", "Documento Origem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("OWTQ", "CVA_Base_DocEntry", "DocEntry Base", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("OWTQ", "CVA_Base_Type", "Tipo Base", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            userObjectController.InsertUserField("OITM", "CVA_Densidade", "CVA: Densidade", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("OITM", "CVA_PesoEmbalagem", "CVA: Peso Embalagem", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("OITM", "CVA_PesoPalete", "CVA: Peso Palete", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("ORDR", "CVA_Calcular", "CVA: Cálcular Peso", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, false, "S");
            userObjectController.AddValidValueToUserField("ORDR", "CVA_Calcular", "S", "SIM");
            userObjectController.AddValidValueToUserField("ORDR", "CVA_Calcular", "N", "NÃO");

        }
    }
}
