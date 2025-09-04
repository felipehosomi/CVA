using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.Model.Configuracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class LoginFornecedorController : ApiController
    {
        UsuarioBLL BLL;

        public LoginFornecedorController()
        {
            BLL = new UsuarioBLL();
        }

        public string Post(UsuarioModel model)
        {
            return BLL.LoginFornecedor(model.Usuario, model.Senha);
        }
    }
}
