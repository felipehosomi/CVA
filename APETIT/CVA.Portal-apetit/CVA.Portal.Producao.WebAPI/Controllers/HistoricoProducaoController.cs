using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class HistoricoProducaoController : ApiController
    {
        ProducaoBLL BLL;

        public HistoricoProducaoController()
        {
            BLL = new ProducaoBLL();
        }

        public IEnumerable<HistoricoProducaoModel> Get(int docEntry)
        {
            return BLL.GetHistorico(docEntry);
        }
    }
}
