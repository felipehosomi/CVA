using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.Resources.Query;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;

namespace CVA.View.ControleQualidade.DAO
{
    public class OperadorDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }
        private SAPbobsCOM.Company _company { get; set; }

        public OperadorDAO(BusinessOneDAO businessOneDAO, SAPbobsCOM.Company company)
        {
            _businessOneDAO = businessOneDAO;
            _company = company;
        }

        public int GetMaxCode()
        {
            var query = String.Format(Select.GetMaxOperador);
            return _businessOneDAO.ExecuteSqlForObject<int>(query);
        }

        public List<Operador> GetOperadores()
        {
            return _businessOneDAO.ExecuteSqlForList<Operador>(Select.GetOperadores);
        }
    }
}
