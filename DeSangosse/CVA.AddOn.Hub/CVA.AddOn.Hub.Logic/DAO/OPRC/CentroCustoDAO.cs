using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using System;

namespace CVA.Hub.DAO.OPRC
{
    public class CentroCustoDAO
    {
        public static string GetDesc(string code)
        {
            return CrudController.GetColumnValue<string>(String.Format(Query.CentroCusto_GetDesc, code));
        }
    }
}
