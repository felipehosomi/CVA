using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.View.CRCP.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            #region SystemTables
            //userObjectController.InsertUserField("ORDR", "NumeroLicitacao", "NumeroLicitacao", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            //userObjectController.InsertUserField("ORDR", "TipoLicitacao", "TipoLicitacao", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            //userObjectController.InsertUserField("ORDR", "NumeroContrato", "NumeroContrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            //userObjectController.InsertUserField("ORDR", "TipoContrato", "TipoContrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            //userObjectController.InsertUserField("ORDR", "NumeroDocumento", "NumeroDocumento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            //userObjectController.InsertUserField("ORDR", "TipoAutorizacao", "TipoAutorizacao", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            //userObjectController.AddValidValueToUserField("ORDR", "U_TipoAutorizacao", "1", "Autorização de Compra");
            //userObjectController.AddValidValueToUserField("ORDR", "U_TipoAutorizacao", "2", "Autorização de Despesa");
            //userObjectController.AddValidValueToUserField("ORDR", "U_TipoAutorizacao", "3", "Autorização de Fornecimento");

            userObjectController.InsertUserField("INV6", "CVA_Status", "Status Cobrança", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
            userObjectController.AddValidValueFromTable("INV6", "CVA_Status", "@CVA_STATUS_COBRANCA");

            userObjectController.InsertUserField("OJDT", "CVA_Status", "Status Cobrança", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
            userObjectController.AddValidValueFromTable("OJDT", "CVA_Status", "@CVA_STATUS_COBRANCA");
            #endregion

        }
    }
}
