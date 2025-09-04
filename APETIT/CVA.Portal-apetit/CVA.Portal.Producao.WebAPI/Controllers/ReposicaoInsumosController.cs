using CVA.Portal.Producao.BLL.Apetit;
using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Qualidade;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ReposicaoInsumosController : ApiController
    {
        SaidaMateriaisBLL BLLMateriais;
        ReposicaoInsumosBLL BLLReposicao;

        public ReposicaoInsumosController()
        {
            BLLMateriais = new SaidaMateriaisBLL();
            BLLReposicao = new ReposicaoInsumosBLL();
        }

        [HttpGet]
        public List<ComboBoxModelHANA> GetInsumos(string bplid)
        {
            var retlist = BLLMateriais.GetInsumosReposicao(bplid);
            return (retlist.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }

        [HttpGet]
        public List<ComboBoxModelHANA> GetMotivo(string motivoBplid)
        {
            var retlist = BLLMateriais.GetMotivo(motivoBplid);
            return (retlist.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }

        // POST: api/ReposicaoInsumos
        [HttpPost]
        public async Task<IHttpActionResult> Post(ReposicaoInsumoAPIModel model)
        {
            var ret = await BLLReposicao.PurchaseRequestSL(model);

            if (!string.IsNullOrEmpty(ret))
            {
                return BadRequest(ret);
            } else
            {
                return Ok();
            }
        }
    }
}
