using CVA.Portal.Producao.BLL.Estoque;
using CVA.Portal.Producao.Model.Estoque;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class GrupoItemController : ApiController
    {
        GrupoItemBLL BLL;

        public GrupoItemController()
        {
            BLL = new GrupoItemBLL();
        }

        // GET: api/GrupoItem
        public IEnumerable<GrupoItemModel> Get()
        {
            return BLL.Get();
        }
    }
}
