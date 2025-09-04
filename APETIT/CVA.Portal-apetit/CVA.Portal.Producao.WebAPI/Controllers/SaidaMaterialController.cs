using CVA.Portal.Producao.BLL.Apetit;
using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Qualidade;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class SaidaMaterialController : ApiController
    {
        SaidaMateriaisBLL BLL;
        ContratosBLL BLLContratos;
        SaidaInsumosBLL BLLInsumos;

        public SaidaMaterialController()
        {
            BLL = new SaidaMateriaisBLL();
            BLLContratos = new ContratosBLL();
            BLLInsumos = new SaidaInsumosBLL();
        }

        [System.Web.Http.HttpGet]
        public List<ComboBoxModelHANA> GetInsumos(string bplid)
        {
            var retlist = BLL.GetInsumos(bplid);
            return (retlist.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }

        [System.Web.Http.HttpGet]
        public List<ComboBoxModelHANA> GetInsumosInvSell(string bplidSell)
        {
            var retlist = BLL.GetInsumosInvSell(bplidSell);
            return (retlist.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }

        [System.Web.Http.HttpGet]
        public List<SelectListItem> GetTiposSaida()
        {
            var retlist = BLLInsumos.GetTiposSaida();
            return (retlist.Count == 0 ? new List<SelectListItem>() : retlist);
        }

        [System.Web.Http.HttpGet]
        public SaidaInsumoModelGetItemOnHand CheckItemOnHand(string itemCode, string whs)
        {
            return BLL.GetItemOnHand(itemCode, whs);
        }

        [System.Web.Http.HttpGet]
        public SaidaInsumoModelGetItemOnHand CheckItemOnHandBPLId(string itemCode, string bplId)
        {
            return BLL.GetItemOnHandBPLId(itemCode, bplId);
        }

        [System.Web.Http.HttpGet]
        public List<SaidaInsumoItensModel02> ReportPosicaoEstoque(string RptBPLId)
        {
            return BLL.ReportPosicaoEstoque(RptBPLId);
        }

        // POST: api/SaidaMaterial
        public async Task<IHttpActionResult> PostAsync(SaidaInsumoModel model)
        {
            string retorno = await BLL.SaveSL(model);
            
            if (string.IsNullOrEmpty(retorno))
            {
                return Ok();
            } else
            {
                return BadRequest(retorno);
            }
        }
    }
}
