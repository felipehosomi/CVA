using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.Core.Cianet.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();
            userObjectController.InsertUserField("QUT1", "CVA_Preco_IPI", "Preço com IPI", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
            userObjectController.CreateUserTable("REGRA_FRETE", "[CVA] Regras de Frete", BoUTBTableType.bott_NoObjectAutoIncrement);
            userObjectController.InsertUserField("@REGRA_FRETE", "PRODUTO", "Cod. Produto", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@REGRA_FRETE", "UF", "Estado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@REGRA_FRETE", "QUANTIDADE", "Quantidade", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            userObjectController.InsertUserField("@REGRA_FRETE", "PORCENTAGEM", "Porcentagem", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 10);

            userObjectController.CreateUserTable("USUARIOS_MESTRE", "[CVA] Usuários Mestres", BoUTBTableType.bott_NoObjectAutoIncrement);

        }
    }
}
