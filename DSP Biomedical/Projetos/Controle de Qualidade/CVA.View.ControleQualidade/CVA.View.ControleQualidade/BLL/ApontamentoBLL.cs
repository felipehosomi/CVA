using CVA.View.ControleQualidade.DAO;
using CVA.View.ControleQualidade.MODEL;
using System;
using System.Collections.Generic;

namespace CVA.View.ControleQualidade.BLL
{
    public partial class ApontamentoBLL
    {
        private ApontamentoDAO _apontamentoDAO { get; set; }
    }


    public partial class ApontamentoBLL
    {
        public ApontamentoBLL(ApontamentoDAO apontamentoDAO)
        {
            _apontamentoDAO = apontamentoDAO;
        }

        public List<string> GetItems(string code)
        {
            return _apontamentoDAO.GetItems(code);
        }

        public string GetNextCode()
        {
            return (_apontamentoDAO.GetUltimoCodigo() + 1).ToString();
        }

        public int VerificaApontamentosRestantes(string docNum)
        {
            return _apontamentoDAO.GetQuantidadeApontamentosRestantes(docNum);
        }

        public bool ExistsQualidade(string docNumOP)
        {
            return _apontamentoDAO.ExistsQualidade(docNumOP);
        }
    }
}
