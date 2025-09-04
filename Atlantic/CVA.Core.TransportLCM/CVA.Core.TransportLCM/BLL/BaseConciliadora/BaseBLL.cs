using CVA.Core.TransportLCM.MODEL.BaseConciliadora;
using CVA.Core.TransportLCM.SERVICE.BaseConciliadora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.BLL.BaseConciliadora
{
    public class BaseBLL
    {
        BaseDAO _baseDAO { get; set; }
        BaseDeParaDAO _baseDeParaDAO { get; set; }

        public BaseBLL(BaseDAO baseDAO, BaseDeParaDAO baseDeParaDAO)
        {
            _baseDAO = baseDAO;
            _baseDeParaDAO = baseDeParaDAO;
        }

        public Base Get(int id)
        {
            return _baseDAO.Get(id);
        }

        public BaseDePara GetByCnpj(string cnpj)
        {
            return _baseDeParaDAO.GetByCnpj(cnpj);
        }

        public List<BaseDePara> GetFilialList()
        {
            return _baseDeParaDAO.GetFilialList();
        }
    }
}
