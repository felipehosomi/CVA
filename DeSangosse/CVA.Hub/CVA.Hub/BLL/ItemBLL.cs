using CVA.Hub.HELPER;
using CVA.Hub.MODEL;
using CVA.Hub.SERVICE.OITM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hub.BLL
{
    public class ItemBLL
    {
        private ItemDAO _ItemDAO { get; }
        
        public ItemBLL(ItemDAO itemDAO)
        {
            _ItemDAO = itemDAO;
        }

        public string GetCentroCusto(string itemCode)
        {
            return _ItemDAO.GetColumnValue<string>("U_EASY_CCusto", itemCode);
        }

        public double GetQtdePorEmbalagem(string itemCode, TipoDoc tipoDoc)
        {
            if (tipoDoc == TipoDoc.Saida)
            {
                return _ItemDAO.GetColumnValue<double>("SalPackUn", itemCode);
            }
            else
            {
                return _ItemDAO.GetColumnValue<double>("PurPackUn", itemCode);
            }
        }

        public bool ItemExiste(string itemCode)
        {
            return _ItemDAO.ItemExiste(itemCode);
        }

        public string ItemGetObs(string itemCode)
        {
            return _ItemDAO.GetItemObs(itemCode);
        }
    }
}
