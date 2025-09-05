using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using System;

namespace CVA.Hub.DAO.OUSG
{
    public class UtilizacaoDAO
    {
        public T GetColumnValue<T>(string columnName, string utilizacaoId)
        {
            return CrudController.GetColumnValue<T>(String.Format(Query.Utilizacao_GetGenericColumn, columnName, utilizacaoId));
        }
    }
}
