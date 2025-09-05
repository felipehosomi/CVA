using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.View.TransferenciaFiliais.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();
            userObjectController.InsertUserField("ODRF", "cva_docentry_custo", "Cliente transferência", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);


        }
    }
}
