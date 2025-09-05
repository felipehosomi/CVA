using CVA.Hub.DAO.OSTC;
using System.Collections.Generic;

namespace CVA.Hub.BLL
{
    public class CodigoImpostoBLL
    {
        private CodigoImpostoDAO _CodigoImpostoDAO { get; }

        public CodigoImpostoBLL()
        {
            _CodigoImpostoDAO = new CodigoImpostoDAO();
        }

        public List<string> GetObsNF(List<string> codeList)
        {
            return _CodigoImpostoDAO.GetObsNFList(codeList);
        }
    }
}