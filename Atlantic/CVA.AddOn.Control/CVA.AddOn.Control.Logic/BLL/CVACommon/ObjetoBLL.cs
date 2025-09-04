using CVA.AddOn.Control.Logic.DAO.CVACommon;
using CVA.AddOn.Control.Logic.MODEL;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.BLL.CVACommon
{
    public class ObjetoBLL
    {
        ObjetoDAO _objetoDAO { get; }

        public ObjetoBLL()
        {
            _objetoDAO = new ObjetoDAO();
        }

        public Objeto GetObjeto(CVAObjectEnum objectType)
        {
            return _objetoDAO.Get(objectType);
        }
    }
}
