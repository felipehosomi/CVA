using CVA.Portal.Producao.BLL.Estoque;
using CVA.Portal.Producao.Model.Estoque;
using CVA.Portal.Producao.Model.Producao;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class TransferenciaEstoqueController : ApiController
    {
        TransferenciaEstoqueBLL BLL;

        public TransferenciaEstoqueController()
        {
            BLL = new TransferenciaEstoqueBLL();
        }

        public async Task<string> Post(List<ProducaoModel> model, string usuario)
        {
            string retorno = await BLL.ExecutarTransferencia(model, usuario);
            return retorno;
        }

        public TransferenciaEstoqueModel GetByOPDocNumStageId(int opDocNum, int stageId)
        {
            return BLL.GetTransfByOPDocNumStageId(opDocNum, stageId);
        }
    }
}