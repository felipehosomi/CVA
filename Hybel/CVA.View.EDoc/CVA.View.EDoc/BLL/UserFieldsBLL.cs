using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.View.EDoc.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            userObjectController.InsertUserField("OHEM", "CVA_CEP", "EDoc: CEP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8);
            userObjectController.InsertUserField("OHEM", "CVA_Endereco", "EDoc: Endereço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("OHEM", "CVA_Numero", "EDoc: Número", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 6);
            userObjectController.InsertUserField("OHEM", "CVA_Complemento", "EDoc: Complemento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("OHEM", "CVA_Bairro", "EDoc: Bairro", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);
            userObjectController.InsertUserField("OHEM", "CVA_UF", "EDoc: UF", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2);
            userObjectController.InsertUserField("OHEM", "CVA_CodMunicipio", "EDoc: Cód. Município", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 7);
            userObjectController.InsertUserField("OHEM", "CVA_Telefone", "EDoc: Telefone", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 12);
            userObjectController.InsertUserField("OHEM", "CVA_Email", "EDoc: E-mail", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);

            userObjectController.InsertUserField("OUSG", "CVA_ICMS", "EDoc: Tabela ICMS", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);

            userObjectController.CreateUserTable("CVA_EDOC_COMPLEM", "CVA: [EDoc] Compl. contri.", BoUTBTableType.bott_MasterData);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "Filial", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 3, true);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "Nome", "Nome Responsável", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, true);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "CodAssin", "Código Assinante", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, true, "999");
            userObjectController.AddValidValueToUserField("@CVA_EDOC_COMPLEM", "U_CodAssin", "999", "999", true);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "CPF", "CPF", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, true);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "CEP", "CEP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8, true);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "Endereco", "Endereço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 40, true);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "Numero", "Nr. Endereço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 6, true);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "Complemento", "Complemento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "Bairro", "Bairro", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "Telefone", "Telefone", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 12);
            userObjectController.InsertUserField("@CVA_EDOC_COMPLEM", "Email", "Email", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);

            userObjectController.CreateUserObject("CVA_EDOC_COMPLEM", "CVA: [EDoc] Compl. contrib.", "@CVA_EDOC_COMPLEM", BoUDOObjType.boud_MasterData);
            userObjectController.MakeFieldsSearchable("@CVA_EDOC_COMPLEM");



        }
    }
}
