using CVA.Hub.SERVICE.Resource;
using Dover.Framework.DAO;
using System;

namespace CVA.Hub.SERVICE.OPRC
{
    public class CentroCustoDAO
    {
        private BusinessOneDAO _BusinessOneDAO { get; set; }

        public CentroCustoDAO(BusinessOneDAO businessOneDAO)
        {
            _BusinessOneDAO = businessOneDAO;
        }

        public string GetDesc(string code)
        {
            return _BusinessOneDAO.ExecuteSqlForObject<string>(String.Format(Query.CentroCusto_GetDesc, code));
        }
    }
}
