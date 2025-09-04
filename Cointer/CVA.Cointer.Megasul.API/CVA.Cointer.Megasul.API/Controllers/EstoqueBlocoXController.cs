using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Cointer.Megasul.API.Controllers
{
    public class EstoqueBlocoXController : ApiController
    {
        HanaDAO dao = new HanaDAO();

        public EstoqueBlocoX Get(int pagina, int quantidade_registros, string data = null, string cnpj = null)
        {
            EstoqueBlocoX estoqueBlocoX = new EstoqueBlocoX();

            return estoqueBlocoX;
        }
    }
}
