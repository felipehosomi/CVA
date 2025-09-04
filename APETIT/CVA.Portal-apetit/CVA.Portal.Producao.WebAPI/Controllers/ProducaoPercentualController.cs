using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ProducaoPercentualController : ApiController
    {
        ProducaoBLL BLL;

        public ProducaoPercentualController()
        {
            BLL = new ProducaoBLL();
        }
        
        public string Get(DateTime? dataDe, DateTime? dataAte, int? nrOP, int? nrPedido)
        {
            return BLL.GetPercentualProducao(dataDe, dataAte, nrOP, nrPedido);
        }
    }
}
