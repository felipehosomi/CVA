using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.Model.Configuracoes;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class LoginCartaoController : ApiController
    {
        UsuarioBLL BLL;

        public LoginCartaoController()
        {
            BLL = new UsuarioBLL();
        }

        public string Post(UsuarioModel model)
        {
            return BLL.LoginCartao(model.NumeroCartao);
        }
    }
}
