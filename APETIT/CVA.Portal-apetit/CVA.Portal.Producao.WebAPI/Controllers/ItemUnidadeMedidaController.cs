using CVA.Portal.Producao.BLL.Compras;
using CVA.Portal.Producao.Model.Compras;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ItemUnidadeMedidaController : ApiController
    {
        OfertaCompraBLL BLL;

        public ItemUnidadeMedidaController()
        {
            BLL = new OfertaCompraBLL();
        }

        [HttpGet]
        public List<OfertaCompraItemUMModel> Get(string itemCode)
        {
            return BLL.GetItensUM(itemCode);
        }
    }
}
