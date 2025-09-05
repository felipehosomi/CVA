using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.Resources.Query;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.DAO
{
    public partial class UsuarioDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }
        private SAPbobsCOM.Company _company { get; set; }
    }

    public partial class UsuarioDAO
    {
        public UsuarioDAO(BusinessOneDAO businessOneDAO, SAPbobsCOM.Company company)
        {
            _businessOneDAO = businessOneDAO;
            _company = company;
        }

        public Usuario GetUsuario(string user)
        {
            var query = String.Format(Select.GetUsuario, user);
            return _businessOneDAO.ExecuteSqlForObject<Usuario>(query);
        }

        public List<Usuario> GetUsuariosAprovadores()
        {
            return _businessOneDAO.ExecuteSqlForList<Usuario>(Select.GetUsuariosAprovadores);
        }
    }
}
