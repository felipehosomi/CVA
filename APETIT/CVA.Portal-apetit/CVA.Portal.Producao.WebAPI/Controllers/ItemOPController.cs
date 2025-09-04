using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ItemOPController : ApiController
    {
        ProducaoBLL BLL;

        public ItemOPController()
        {
            BLL = new ProducaoBLL();
        }

        public IEnumerable<ItemOPModel> Get(int opDocEntry, int stageId = -1)
        {
            return BLL.GetItensEstoque(opDocEntry, stageId);
        }
    }
}
