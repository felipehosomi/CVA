using CVA.Core.DSP.Controle.MODEL;
using CVA.Core.DSP.Controle.Resources.Select;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;

namespace CVA.Core.DSP.Controle.DAO
{
    public class RecursoDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }

        public RecursoDAO(BusinessOneDAO businessOneDAO)
        {
            _businessOneDAO = businessOneDAO;
        }

        public List<Recurso> GetRecursoFixo(string itemCode)
        {
            try
            {
                var query = String.Format(Query.GetRecursoValorFixo, itemCode);
                return _businessOneDAO.ExecuteSqlForList<Recurso>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
