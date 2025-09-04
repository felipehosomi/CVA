using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System.Threading.Tasks;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class SaidaInsumosController : ApiController
    {
        SaidaInsumosBLL BLL = new SaidaInsumosBLL();

        public async Task<string> Post(ProducaoModel model)
        {
            string retorno = await BLL.ExecutaSaidaInsumos(model);
            return retorno;
        }
    }
}
