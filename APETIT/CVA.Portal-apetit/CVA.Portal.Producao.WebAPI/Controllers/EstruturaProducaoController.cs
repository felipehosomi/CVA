using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class RecursoProducaoController : ApiController
    {
        ProducaoBLL BLL;

        public RecursoProducaoController()
        {
            BLL = new ProducaoBLL();
        }

        public IEnumerable<RecursoProducaoModel> Get(int nrOP, int codEtapa)
        {
            return BLL.GetRecursoProducao(nrOP, codEtapa);
        }
    }
}
