using CVA.Hub.DAO.OUSG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hub.BLL
{
    public class UtilizacaoBLL
    {
        private UtilizacaoDAO _UtilizacaoDAO { get; }

        public UtilizacaoBLL()
        {
            _UtilizacaoDAO = new UtilizacaoDAO();
        }

        public string GetDeposito(string utilizacaoId)
        {
            return _UtilizacaoDAO.GetColumnValue<string>("U_EASY_WHS", utilizacaoId);
        }

        public string GetBloqueioQtde(string utilizacaoId)
        {
            return _UtilizacaoDAO.GetColumnValue<string>("U_BloqQtde", utilizacaoId);
        }
    }
}
