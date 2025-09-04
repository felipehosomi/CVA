using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System.Threading.Tasks;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class SaidaRecursoController : ApiController
    {
        SaidaInsumosBLL BLL = new SaidaInsumosBLL();

        public async Task<string> Post(ApontamentoHRModel model)
        {
            string retorno = await BLL.ExecutaSaidaRecurso(model);
            return retorno;
        }
    }
}
