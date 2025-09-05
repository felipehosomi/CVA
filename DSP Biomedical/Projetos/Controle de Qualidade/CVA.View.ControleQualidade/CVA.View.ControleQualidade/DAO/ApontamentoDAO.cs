using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.Resources.Query;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;

namespace CVA.View.ControleQualidade.DAO
{
    public partial class ApontamentoDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }
        private SAPbobsCOM.Company _company { get; set; }
    }

    public partial class ApontamentoDAO
    {
        public ApontamentoDAO(BusinessOneDAO businessOneDAO, SAPbobsCOM.Company company)
        {
            _businessOneDAO = businessOneDAO;
            _company = company;
        }

        public List<string> GetItems(string code)
        {
            List<string> list = _businessOneDAO.ExecuteSqlForList<string>(String.Format(Select.GetItensApontamento, code));
            return list;
        }

        public int GetUltimoCodigo()
        {
            var query = Select.GetMaxCodeApontamento;
            return _businessOneDAO.ExecuteSqlForObject<int>(query);
        }

        public int GetQuantidadeApontamentosRestantes(string docNum)
        {
            var query = String.Format(Select.GetApontamentosRestantes, docNum);
            return _businessOneDAO.ExecuteSqlForObject<int>(query);
        }

        public bool ExistsQualidade(string docNumOP)
        {
            int exists = _businessOneDAO.ExecuteSqlForObject<int>(String.Format(Select.ExistsApontamentoQualidade, docNumOP));
            return exists > 0;
        }
    }
}
