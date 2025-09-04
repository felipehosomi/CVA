using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model.Qualidade;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ModeloFichaController : ApiController
    {
        ModeloFichaBLL BLL;

        public ModeloFichaController()
        {
            BLL = new ModeloFichaBLL();
        }

        // GET: api/ModeloFicha
        public IEnumerable<ModeloFichaModel> Get()
        {
            return BLL.Get();
        }

        // GET: api/ModeloFicha/5
        public ModeloFichaModel Get(string id)
        {
            return BLL.Get(id);
        }

        // GET: api/ModeloFicha/5
        public ModeloFichaModel GetByItem(string codItem)
        {
            return BLL.Get(codItem);
        }

        // POST: api/ModeloFicha
        public void Post(ModeloFichaModel model)
        {
            BLL.Create(model);
        }

        // PUT: api/ModeloFicha/5
        public void Put(ModeloFichaModel model)
        {
            BLL.Update(model);
        }
    }
}
