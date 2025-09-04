using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.Core.Cianet.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();


            userObjectController.InsertUserField("ORDR", "CVA_CalculoFrete", "Calculo Frete", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("ORDR", "CVA_ClaculoFrete", "Y","Sim");
            //userObjectController.AddValidValueToUserField("ORDR", "CVA_ClaculoFrete", "N","Não");




            userObjectController.InsertUserField("QUT1", "CVA_Preco_IPI", "Preço com IPI", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
            userObjectController.CreateUserTable("REGRA_FRETE", "[CVA] Regras de Frete", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@REGRA_FRETE", "PRODUTO", "Cod. Produto", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@REGRA_FRETE", "UF", "Estado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@REGRA_FRETE", "QUANTIDADE", "Quantidade", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            userObjectController.InsertUserField("@REGRA_FRETE", "PORCENTAGEM", "Porcentagem", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 10);
            userObjectController.InsertUserField("@REGRA_FRETE", "TRANSP", "Transportadora", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.CreateUserTable("USUARIOS_MESTRE", "[CVA] Usuários Mestres", BoUTBTableType.bott_NoObject);

        }
    }
}
