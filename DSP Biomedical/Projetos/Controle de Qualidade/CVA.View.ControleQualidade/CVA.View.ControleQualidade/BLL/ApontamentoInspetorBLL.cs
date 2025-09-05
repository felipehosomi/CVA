using CVA.View.ControleQualidade.DAO;
using CVA.View.ControleQualidade.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.BLL
{
    public class ApontamentoInspetorBLL
    {
        private ApontamentoInspetorDAO _apontamentoInspetorDAO { get; set; }

        public ApontamentoInspetorBLL(ApontamentoInspetorDAO apontamentoInspetorDAO)
        {
            _apontamentoInspetorDAO = apontamentoInspetorDAO;
        }

        public string GetNextCode()
        {
            return _apontamentoInspetorDAO.GetNextCode();
        }

        public ApontamentoInspetor Get(string user, string date, string op)
        {
            ApontamentoInspetor apontamento = _apontamentoInspetorDAO.Get(user, date, op);
            return apontamento;
        }

        public string Save(ApontamentoInspetor apontamento)
        {
            string msg = String.Empty;
            msg = _apontamentoInspetorDAO.Save(apontamento);

            return msg;
        }
    }
}
