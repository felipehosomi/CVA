using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.Model.Configuracoes;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ViewController : ApiController
    {
        ViewBLL BLL;

        public ViewController()
        {
            BLL = new ViewBLL();
        }

        // GET: api/View
        public IEnumerable<ViewModel> Get()
        {
            return BLL.Get();
        }
    }
}
