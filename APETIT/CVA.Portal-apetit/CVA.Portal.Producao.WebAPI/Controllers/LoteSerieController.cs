using CVA.Portal.Producao.BLL.Estoque;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class LoteSerieController : ApiController
    {
        LoteSerieBLL BLL;

        public LoteSerieController()
        {
            BLL = new LoteSerieBLL();
        }

        // GET: api/LoteSerie
        public List<LoteSerieModel> Get(string itemCode, string controle, string deposito)
        {
            return BLL.GetDisponivel(itemCode, controle, deposito);
        }
    }
}
