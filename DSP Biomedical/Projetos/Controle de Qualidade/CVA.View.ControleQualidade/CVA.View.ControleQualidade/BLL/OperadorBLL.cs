using CVA.View.ControleQualidade.DAO;
using CVA.View.ControleQualidade.MODEL;
using System;
using System.Collections.Generic;

namespace CVA.View.ControleQualidade.BLL
{
    public partial class OperadorBLL
    {
        private OperadorDAO _operadorDAO;

        public OperadorBLL(OperadorDAO operadorDAO)
        {
            _operadorDAO = operadorDAO;
        }

        public string GetNextCode()
        {
            int nextCode = _operadorDAO.GetMaxCode() + 1;
            return nextCode.ToString().PadLeft(4, '0');
        }

        public List<Operador> GetOperadores()
        {
            return _operadorDAO.GetOperadores();
        }
    }
}
