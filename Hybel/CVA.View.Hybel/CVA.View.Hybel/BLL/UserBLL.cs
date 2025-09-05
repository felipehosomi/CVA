using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using System;

namespace CVA.View.Hybel.BLL
{
    public class UserBLL
    {
        public static int GetUserId()
        {
            return Convert.ToInt32(CrudController.ExecuteScalar(String.Format(SQL.User_GetId, SBOApp.Company.UserName)));
        }
    }
}
