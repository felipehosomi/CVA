using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.Model.Configuracoes;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class UsuarioCartaoController : ApiController
    {
        UsuarioBLL BLL;

        public UsuarioCartaoController()
        {
            BLL = new UsuarioBLL();
        }

        // GET: api/UsuarioCartao/1234
        public UsuarioModel Get(string id)
        {
            return BLL.GetByCartao(id);
        }       
    }
}
