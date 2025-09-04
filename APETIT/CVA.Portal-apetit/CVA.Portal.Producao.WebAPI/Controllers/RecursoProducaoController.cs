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
    public class EstruturaProducaoController : ApiController
    {
        ProducaoBLL BLL;

        public EstruturaProducaoController()
        {
            BLL = new ProducaoBLL();
        }

        public IEnumerable<EstruturaProducaoModel> Get(int nrOP, int codEtapa)
        {
            return BLL.GetEstruturaProducao(nrOP, codEtapa);
        }
    }
}
