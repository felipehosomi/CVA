using AUXILIAR;
using DAO;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ItemBLL
    {
        public EtiquetaModel GetEtiqueta(string itemCode, string serie)
        {
            return new EtiquetaDAO().GetEtiqueta(itemCode, serie);
        }
    }
}
