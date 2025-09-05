using CVA.Hub.SERVICE.Resource;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hub.SERVICE.OUSG
{
    public class UtilizacaoDAO
    {
        private BusinessOneDAO _BusinessOneDAO { get; set; }

        public UtilizacaoDAO(BusinessOneDAO businessOneDAO)
        {
            _BusinessOneDAO = businessOneDAO;
        }

        public T GetColumnValue<T>(string columnName, string utilizacaoId)
        {
            return _BusinessOneDAO.ExecuteSqlForObject<T>(String.Format(Query.Utilizacao_GetGenericColumn, columnName, utilizacaoId));
        }
    }
}
