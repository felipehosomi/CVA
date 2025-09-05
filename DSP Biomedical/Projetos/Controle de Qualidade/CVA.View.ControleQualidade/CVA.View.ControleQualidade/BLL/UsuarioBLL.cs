using CVA.View.ControleQualidade.DAO;
using CVA.View.ControleQualidade.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.BLL
{
    public partial class UsuarioBLL
    {
        private UsuarioDAO _usuarioDAO { get; set; }
    }

    public partial class UsuarioBLL
    {
        public UsuarioBLL(UsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        public Usuario GetUsuario(string user)
        {
            return _usuarioDAO.GetUsuario(user);
        }
    }
}
