using CVA.Hub.SERVICE.OSTC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hub.BLL
{
    public class CodigoImpostoBLL
    {
        private CodigoImpostoDAO _CodigoImpostoDAO { get; }

        public CodigoImpostoBLL(CodigoImpostoDAO codigoImpostoDAO)
        {
            _CodigoImpostoDAO = codigoImpostoDAO;
        }

        public List<string> GetObsNF(List<string> codeList)
        {
            return _CodigoImpostoDAO.GetObsNFList(codeList);
        }
    }
}