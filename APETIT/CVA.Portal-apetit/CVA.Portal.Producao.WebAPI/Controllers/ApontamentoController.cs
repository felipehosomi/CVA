using CVA.Portal.Producao.BLL;
using CVA.Portal.Producao.BLL.Apetit;
using CVA.Portal.Producao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using static CVA.Portal.Producao.Model.PainelApontamentoModel;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ApontamentoController : ApiController
    {
        ApontamentoBLL BLL;

        public ApontamentoController()
        {
            BLL = new ApontamentoBLL();
        }

        [HttpGet]
        public ApontamentoGetClienteModel GetCliente(string clienteBPLID)
        {
            return BLL.GetCliente(clienteBPLID);
        }

        [HttpGet]
        public List<ApontamentoGetServicoModel> GetServico(string contratoIdGrupo)
        {
            return BLL.GetServico(contratoIdGrupo);
        }

        [HttpGet]
        public ApontamentoGetInfoContratoModel GetInfoPlanejamento(string contratoIdInfo, DateTime? contratoDate)
        {
            //return BLL.GetInfoPlanejamento(contratoIdInfo, contratoDate);
            return null;
        }

        [HttpGet]
        public ApontamentoGetInfoContratoModel GetInfoContrato(string contratoIdInfoContrato)
        {
            return BLL.GetInfoContrato(contratoIdInfoContrato);
        }

        [HttpGet]
        public List<ApontamentoGetInfoItensContratoModel> GetInfoPlanejamentoItens(string contratoIdInfoItens, string servicoIdInfoItens)
        {
            return BLL.GetInfoPlanejamentoItens(contratoIdInfoItens, servicoIdInfoItens);
        }
        [Route("api/Apontamento/GetInfoPlanejamentoItensFechado")]
        [HttpGet]
        public List<ApontamentoGetInfoItensContratoModel> GetInfoPlanejamentoItensFechado(string contratoIdInfoItens, string servicoIdInfoItens)
        {
            return BLL.GetInfoPlanejamentoItensFechado(contratoIdInfoItens, servicoIdInfoItens);
        }

        [HttpGet]
        public List<ComboBoxModelHANA> GetItensList(string itemCodeList, string BPLIdList)
        {
            var retlist = BLL.GetItens(BPLIdList);
            return (retlist.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }

        [HttpGet]
        public ApontamentoGetQtyModel GetQty(string qtySERVICO, string qtyTURNO, DateTime date, string qtyBPLID)
        {
            return BLL.GetQty(qtySERVICO, qtyTURNO, date, qtyBPLID);
        }

        [HttpGet]
        public ApontamentoItemInfo GetItemInfo(string infoItem, string infoBPLID)
        {
            return BLL.GetItemInfo(infoItem, infoBPLID);
        }

        [HttpGet]
        public ApontamentoCheckDay CheckDay(DateTime date)
        {
            return BLL.CheckDay(date);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<string> Complementacao(string cpFilial, string cpCONTRATO, string cpCliente, string cpSERVICO, DateTime date, string cpUser)
        {
            return await BLL.SendEmailComplementacao(cpFilial, cpCONTRATO, cpCliente, cpSERVICO, date, cpUser);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IHttpActionResult> SaveAsync(ApontamentoModel model)
        {
            string retorno = await BLL.SaveAsync(model);
            if (string.IsNullOrEmpty(retorno))
            {
                return Ok();
            } else
            {
                return BadRequest(retorno);
            }
        }

        [HttpGet]
        public List<PainelContratoItens> GetPainel(string pnBplId, DateTime dateDe, DateTime dateAte)
        {
            return BLL.GetPainel(pnBplId, dateDe, dateAte);
        }

        [HttpGet]
        public CVA_APTO_TERCEIROSModel.APIModel GetTerceiros(string tBPLID, DateTime? tData, string tTurno, string tServico)
        {
            return BLL.GetTerceiros(tBPLID, tData, tTurno, tServico);
        }

        [HttpGet]
        public List<CVA_APTO_TERCEIROSModel.CVA_APTO_TerceirosSAP> GetTerceirosBP(string bBPLID)
        {
            return BLL.GetTerceirosBP(bBPLID);
        }

        [HttpGet]
        public ApontamentoGetClienteModel GetTerceirosBPCode(string bCardCode)
        {
            return BLL.GetTerceirosBPCode(bCardCode);
        }

        [HttpGet]
        public ApontamentoGetItemOnHand CheckItemOnHandBPLId(string itemCode, string bplId)
        {
            return BLL.GetItemOnHandBPLId(itemCode, bplId);
        }

    }
}