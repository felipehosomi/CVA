using CVA.Portal.Producao.BLL.Compras;
using CVA.Portal.Producao.Model.Compras;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class OfertaCompraController : ApiController
    {
        OfertaCompraBLL BLL;

        public OfertaCompraController()
        {
            BLL = new OfertaCompraBLL();
        }

        public List<OfertaCompraListModel> Get(string sFornecedor)
        {
            return BLL.GetList(sFornecedor);
        }

        public OfertaCompraModel GetById(int id)
        {
            return BLL.GetById(id);
        }

        [HttpPut]
        public async Task<string> Put(int DocEntry, [FromBody] OfertaCompraModel model)
        {
            return await BLL.Update(DocEntry, model);
        }

        [HttpGet]
        public async Task<string> SendEmail(TiposEmail tipoEmail, string sCotacaoCompra, string sNomeContato, string sEmailContato, string sObsRevisao, string sOrdemCompra)
        {
            return await BLL.SendEmail(tipoEmail, sCotacaoCompra, sNomeContato, sEmailContato, sOrdemCompra, sObsRevisao);
        }
    }
}