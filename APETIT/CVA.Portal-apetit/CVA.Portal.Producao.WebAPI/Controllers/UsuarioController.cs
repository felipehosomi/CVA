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
    public class UsuarioController : ApiController
    {
        UsuarioBLL BLL;

        public UsuarioController()
        {
            BLL = new UsuarioBLL();
        }

        // GET: api/Usuario
        public IEnumerable<UsuarioModel> Get()
        {
            return BLL.Get();
        }

        // GET: api/Usuario/5
        public UsuarioModel Get(string id)
        {
            return BLL.Get(id);
        }

        // POST: api/Usuario
        public void Post(UsuarioModel model)
        {
            BLL.Create(model);
        }

        // PUT: api/Usuario/5
        public void Put(UsuarioModel model)
        {
            BLL.Update(model);
        }

       
    }
}
