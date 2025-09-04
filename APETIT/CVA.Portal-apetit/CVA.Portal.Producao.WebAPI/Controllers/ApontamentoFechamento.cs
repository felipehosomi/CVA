using CVA.Portal.Producao.BLL;
using CVA.Portal.Producao.Model;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ApontamentoFechamento : ApiController
    {
        ApontamentoBLL BLL;

        public ApontamentoFechamento()
        {
            BLL = new ApontamentoBLL();
        }
        
        [HttpPost]
        public async System.Threading.Tasks.Task<string> SaveAsync(ApontamentoEncerramentoModel model)
        {
            return await BLL.ProductionOrderAsync(model);
        }
        
    }
}