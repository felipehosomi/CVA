using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.Resources.Query;
using Dover.Framework.DAO;
using System.Collections.Generic;
using System;

namespace CVA.View.ControleQualidade.DAO
{
    public partial class AtributoDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }
    }

    public partial class AtributoDAO
    {
        public AtributoDAO(BusinessOneDAO businessOneDAO)
        {
            _businessOneDAO = businessOneDAO;
        }

        public List<Atributo> GetAttributes()
        {
            var query = Select.GetAtributos;
            return _businessOneDAO.ExecuteSqlForList<Atributo>(query);
        }

        public string GetAttributeName(string code)
        {
            try
            {
                var query = String.Format(Select.GetNomeAtributo, code);
                return _businessOneDAO.ExecuteSqlForObject<string>(query);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int GetAttributeType(string code)
        {
            var query = String.Format(Select.GetTipoAtributo, code);
            return _businessOneDAO.ExecuteSqlForObject<int>(query);
        }
    }
}
