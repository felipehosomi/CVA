using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class RecursoController : ApiController
    {
        RecursoBLL BLL;

        public RecursoController()
        {
            BLL = new RecursoBLL();
        }

        // GET: api/Recurso
        public IEnumerable<RecursoModel> Get(string tipo)
        {
            return BLL.Get(tipo);
        }
    }
}
