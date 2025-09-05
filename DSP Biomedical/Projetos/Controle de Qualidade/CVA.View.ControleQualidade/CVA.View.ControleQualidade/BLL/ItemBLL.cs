using CVA.View.ControleQualidade.DAO;
using CVA.View.ControleQualidade.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.BLL
{
    public class ItemBLL
    {
        private ItemDAO _itemDAO { get; set; }

        public ItemBLL(ItemDAO itemDAO)
        {
            _itemDAO = itemDAO;
        }

        public Item GetInspecaoPorItem(string itemCode)
        {
            return _itemDAO.GetCodigoInspecaoPorItem(itemCode);
        }
    }
}
