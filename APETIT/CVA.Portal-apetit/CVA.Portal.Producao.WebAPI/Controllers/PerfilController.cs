using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.Model.Configuracoes;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class PerfilController : ApiController
    {
        PerfilBLL BLL;

        public PerfilController()
        {
            BLL = new PerfilBLL();
        }

        // GET: api/Perfil
        public IEnumerable<PerfilModel> Get()
        {
            return BLL.Get();
        }

        // GET: api/Perfil/5
        public PerfilModel Get(string id)
        {
            return BLL.Get(id);
        }

        // POST: api/Perfil
        public void Post(PerfilModel model)
        {
            BLL.Create(model);
        }

        // PUT: api/Perfil/5
        public void Put(PerfilModel model)
        {
            BLL.Update(model);
        }


    }
}
