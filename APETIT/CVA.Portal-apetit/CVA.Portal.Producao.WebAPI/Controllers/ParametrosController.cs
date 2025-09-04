using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.Model.Configuracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ParametrosController : ApiController
    {
        ParametrosBLL BLL;

        public ParametrosController()
        {
            BLL = new ParametrosBLL();
        }

        // GET: api/Usuario/5
        public ParametrosModel Get(string id)
        {
            return BLL.Get(id);
        }

        // POST: api/Usuario
        public void Post(ParametrosModel model)
        {
            BLL.Create(model);
        }

        // PUT: api/Usuario/5
        public void Put(ParametrosModel model)
        {
            BLL.Update(model);
        }
    }
}