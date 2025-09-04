using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.Model.Configuracoes;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        UsuarioBLL BLL;

        public LoginController()
        {
            BLL = new UsuarioBLL();
        }

        public string Post(UsuarioModel model)
        {
            return BLL.Login(model.Usuario, model.Senha);
        }
    }
}
