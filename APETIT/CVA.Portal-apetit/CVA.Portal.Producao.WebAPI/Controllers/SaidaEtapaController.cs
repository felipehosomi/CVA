using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class SaidaEtapaController : ApiController
    {
        SaidaInsumosBLL BLL = new SaidaInsumosBLL();

        public async Task<string> Post(List<ProducaoModel> model)
        {
            string retorno = await BLL.ExecutaSaidaEtapa(model);
            return retorno;
        }
    }
}
