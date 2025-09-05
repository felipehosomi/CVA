using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using System;
using SAPbouiCOM;

namespace CVA.View.Hybel.BLL
{
    public class ParceiroBLL
    {
        public static bool Check_BusinessPartnerStatus(string cardCode){
            return Convert.ToBoolean(CrudController.ExecuteScalar(String.Format(SQL.Parceiro_GetStatus, cardCode)));          
        }

        public static string Get_CardCode(string cardName)
        {
            return Convert.ToString(CrudController.ExecuteScalar(String.Format(SQL.Parceiro_GetCardCode, cardName)));
        }
    }
}
