using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model.Qualidade;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class TipoEspecificacaoController : ApiController
    {
        TipoEspecificacaoBLL BLL;

        public TipoEspecificacaoController()
        {
            BLL = new TipoEspecificacaoBLL();
        }

        // GET: api/TipoEspecificacao
        public IEnumerable<TipoEspecificacaoModel> Get()
        {
            return BLL.Get();
        }

        // GET: api/TipoEspecificacao/5
        public TipoEspecificacaoModel Get(string id)
        {
            return BLL.Get(id);
        }

        // POST: api/TipoEspecificacao
        public void Post(TipoEspecificacaoModel model)
        {
            BLL.Create(model);
        }

        // PUT: api/TipoEspecificacao/5
        public void Put(TipoEspecificacaoModel model)
        {
            BLL.Update(model);
        }

        public void Delete(string id)
        {
            BLL.Delete(id);
        }
    }
}
