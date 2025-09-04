using CVA.Portal.Producao.BLL.Compras;
using CVA.Portal.Producao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class CondicaoPagamentoController : ApiController
    {
        OfertaCompraBLL BLL;

        public CondicaoPagamentoController()
        {
            BLL = new OfertaCompraBLL();
        }

        public IEnumerable<ComboBoxModel> Get()
        {
            return BLL.GetCondicaoPagamento();
        }
    }
}
