using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model.Qualidade;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class FichaProdutoController : ApiController
    {
        FichaProdutoBLL BLL;

        public FichaProdutoController()
        {
            BLL = new FichaProdutoBLL();
        }

        // GET: api/FichaProduto
        public IEnumerable<FichaProdutoModel> Get()
        {
            return BLL.Get();
        }

        // GET: api/FichaProduto/5
        public FichaProdutoModel Get(string id)
        {
            return BLL.Get(id);
        }

        public List<FichaProdutoModel> GetByOP(int nrOP)
        {
            return BLL.GetByOP(nrOP);
        }

        public FichaProdutoModel GetObrigatorio(int docEntryOP, string codItem, string codEtapa, double quantidade)
        {
            return BLL.GetObrigatorio(docEntryOP, codItem, codEtapa, quantidade);
        }

        // POST: api/FichaProduto
        public void Post(FichaProdutoModel model)
        {
            BLL.Create(model);
        }

        // PUT: api/FichaProduto/5
        public void Put(FichaProdutoModel model)
        {
            BLL.Update(model);
        }

        public void Delete(string id)
        {
        }
    }
}
