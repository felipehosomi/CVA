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
    public class UsuarioFornecedorController : ApiController
    {
        UsuarioBLL BLL;

        public UsuarioFornecedorController()
        {
            BLL = new UsuarioBLL();
        }

        public UsuarioModel Get(string usuario, string senha)
        {
            return BLL.GetByFornecedor(usuario, senha);
        }
    }
}
