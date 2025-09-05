using CVA.AddOn.Common.Controllers;
using CVA.View.DIME.DAO.Resources;
using System;

namespace CVA.View.DIME.BLL
{
    public class DimeConfigBLL
    {
        public static bool Exists(int filial, string periodo)
        {
            object exists = CrudController.ExecuteScalar(String.Format(SQL.DimeConfig_Exists, filial, periodo));
            return exists != null;
        }
    }
}
