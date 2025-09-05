using CVA.Core.DSP.Controle.DAO;
using CVA.Core.DSP.Controle.HELPER;
using CVA.Core.DSP.Controle.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DSP.Controle.BLL
{
    public partial class ItemBLL
    {
        private ItemDAO _itemDAO { get; set; }
    }
    public partial class ItemBLL
    {
        public ItemBLL(ItemDAO itemDAO)
        {
            _itemDAO = itemDAO;
        }

        public OITM GetCounter(string itemCode)
        {
            BusinessOneLog.Add("Iniciando processo de recuperação de dados mestres por item");
            if (string.IsNullOrEmpty(itemCode))
                throw new ArgumentNullException("ItemCode inválido");

            return _itemDAO.GetCounter(itemCode);
        }
    }
}
