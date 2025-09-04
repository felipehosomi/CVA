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
    public class UsuarioEtapaController : ApiController
    {
        UsuarioEtapaBLL BLL;

        public UsuarioEtapaController()
        {
            BLL = new UsuarioEtapaBLL();
        }

        // GET: api/UsuarioEtapa/5
        public IEnumerable<UsuarioEtapaModel> Get(string codUsuario)
        {
            return BLL.GetByUsuario(codUsuario);
        }

    }
}
