using CVA.Portal.Producao.BLL.Estoque;
using CVA.Portal.Producao.Model.Estoque;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ItemController : ApiController
    {
        ItemBLL BLL;

        public ItemController()
        {
            BLL = new ItemBLL();
        }

        // GET: api/Item
        public IEnumerable<ItemModel> Get()
        {
            return BLL.Get();
        }
    }
}
