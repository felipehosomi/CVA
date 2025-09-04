using CVA.Portal.Producao.Model.Configuracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVA.Portal.Producao.Web.Helper
{
    public class CvaSession
    {
        public static UsuarioModel Usuario
        {
            get { return HttpContext.Current.Session["CVAUSR"] as UsuarioModel; }
        }

        public static List<ViewModel> ViewList
        {
            get { return HttpContext.Current.Session["CVAVIEW"] as List<ViewModel>; }
        }

        public static ParametrosModel Parametros
        {
            get { return HttpContext.Current.Session["CVAPARAM"] as ParametrosModel; }
        }
    }
}