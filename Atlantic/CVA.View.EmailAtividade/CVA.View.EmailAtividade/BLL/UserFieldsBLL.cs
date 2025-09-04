using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.View.EmailAtividade.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();
            userObjectController.CreateUserTable("CVA_EMAIL_ACTIVITY", "Cadastro E-mail Atividades", BoUTBTableType.bott_MasterData);
            userObjectController.CreateUserObject("CVA_EMAIL_ACTIVITY", "Cadastro E-mail Atividades", "CVA_EMAIL_ACTIVITY", BoUDOObjType.boud_MasterData);

            //userObjectController.InsertUserField("@CVA_EMAIL_ACTIVITY", "BPlId", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_EMAIL_ACTIVITY", "Server", "Servidor de e-mail", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_EMAIL_ACTIVITY", "Port", "Porta", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_EMAIL_ACTIVITY", "SSL", "Usa SSL", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_EMAIL_ACTIVITY", "SSL", "N", "Não", true);
            userObjectController.AddValidValueToUserField("@CVA_EMAIL_ACTIVITY", "SSL", "Y", "Sim");
            userObjectController.InsertUserField("@CVA_EMAIL_ACTIVITY", "User", "Usuário (E-mail)", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_EMAIL_ACTIVITY", "Password", "Senha", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_EMAIL_ACTIVITY", "Subject", "Assunto do E-mail", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_EMAIL_ACTIVITY", "Message", "Mensagem do E-mail", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 254);

        }
    }
}
