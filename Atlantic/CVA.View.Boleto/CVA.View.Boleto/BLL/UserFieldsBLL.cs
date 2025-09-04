using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.View.Boleto.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            userObjectController.InsertUserField("OPYM", "CVA_Bol_Mesmo_Banco", "Boleto mesmo banco", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OPYM", "CVA_Bol_Mesmo_Banco", "N", "Não", true);
            userObjectController.AddValidValueToUserField("OPYM", "CVA_Bol_Mesmo_Banco", "Y", "Sim");

            userObjectController.InsertUserField("OPYM", "CVA_Bol_Outro_Banco", "Boleto outro banco", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OPYM", "CVA_Bol_Outro_Banco", "N", "Não", true);
            userObjectController.AddValidValueToUserField("OPYM", "CVA_Bol_Outro_Banco", "Y", "Sim");

            userObjectController.InsertUserField("OPYM", "CVA_Bol_Conces", "Boleto concessionária", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OPYM", "CVA_Bol_Conces", "N", "Não", true);
            userObjectController.AddValidValueToUserField("OPYM", "CVA_Bol_Conces", "Y", "Sim");

            userObjectController.InsertUserField("OPYM", "CVA_Valida_Linha", "Valida linha digitável", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OPYM", "CVA_Valida_Linha", "N", "Não", true);
            userObjectController.AddValidValueToUserField("OPYM", "CVA_Valida_Linha", "Y", "Sim");
        }
    }
}
