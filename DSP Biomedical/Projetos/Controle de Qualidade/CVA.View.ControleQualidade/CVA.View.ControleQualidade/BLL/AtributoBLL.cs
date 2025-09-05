using CVA.View.ControleQualidade.DAO;
using CVA.View.ControleQualidade.MODEL;
using System;
using System.Collections.Generic;

namespace CVA.View.ControleQualidade.BLL
{
    public partial class AtributoBLL
    {
        private AtributoDAO _atributoDAO { get; set; }
    }
    public partial class AtributoBLL
    {
        public AtributoBLL(AtributoDAO atributoDAO)
        {
            _atributoDAO = atributoDAO;
        }

        public List<Atributo> Get()
        {
            try
            {
                return _atributoDAO.GetAttributes();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetAtributoNome(string code)
        {
            try
            {
                return _atributoDAO.GetAttributeName(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAtributoTipo(string code)
        {
            try
            {
                return _atributoDAO.GetAttributeType(code);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
