using CVA.Portal.Producao.Model.Estoque;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Estoque
{
    public class ItemBLL : BaseBLL
    {
        public ItemBLL()
        {
            DAO.TableName = "OITM";
        }

        public List<ItemModel> Get()
        {
            return DAO.RetrieveModelList<ItemModel>("\"InvntItem\" = 'Y'", "\"ItemCode\"");
        }
    }
}
