using CVA.Hub.SERVICE.Resource;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hub.SERVICE.OSTC
{
    public class CodigoImpostoDAO
    {
        private BusinessOneDAO _BusinessOneDAO { get; set; }

        public CodigoImpostoDAO(BusinessOneDAO businessOneDAO)
        {
            _BusinessOneDAO = businessOneDAO;
        }

        public List<string> GetObsNFList(List<string> codeList)
        {
            if (codeList.Count == 0)
            {
                return new List<string>();
            }

            string where = String.Empty;

            foreach (var item in codeList)
            {
                where += $", '{item}'";
            }
            where = where.Substring(2);

            List<string> list = _BusinessOneDAO.ExecuteSqlForList<string>(String.Format(Query.CodigoImposto_GetObsNF, where));
            return list;
        }
    }
}
