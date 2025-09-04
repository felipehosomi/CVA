using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.Model.Configuracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ColaboradorController : ApiController
    {
        ColaboradorBLL BLL;

        public ColaboradorController()
        {
            BLL = new ColaboradorBLL();
        }

        public IEnumerable<ColaboradorModel> Get()
        {
            return BLL.Get();
        }
    }
}
