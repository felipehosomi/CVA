using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DSP.Controle.BLL
{
    public partial class ProdutoAcabadoBLL
    {
        private ItemBLL _itemBLL { get; set; }
        private FerramentasBLL _ferramentaBLL { get; set; }
    }

    public partial class ProdutoAcabadoBLL
    {
        public ProdutoAcabadoBLL(ItemBLL itemBLL,FerramentasBLL ferramentaBLL)
        {
            _itemBLL = itemBLL;
            _ferramentaBLL = ferramentaBLL;
        }

        public bool UpdateCounter(string itemCode, int quantity)
        {
            try
            {
                var oitm = _itemBLL.GetCounter(itemCode);
                var quantity_total = quantity * oitm.Quantity;
                
                return _ferramentaBLL.Update(oitm.Tool, quantity_total);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
