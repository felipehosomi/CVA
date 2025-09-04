using CVA.Portal.Producao.Model.Configuracoes;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Helper
{
    public class CvaAuthorize : AuthorizeAttribute
    {
        public CvaAuthorize(string controller)
        {
            Controller = controller;
        }

        public string Controller { get; set; }

        public bool NoValidation { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!NoValidation)
            {
                if (VerifySession())
                    return false;

                var authorized = base.AuthorizeCore(httpContext);
                if (!authorized && !httpContext.User.Identity.IsAuthenticated)
                    return false;

                if (GetViews().Any(v => v.Controller == Controller))
                    return true;
                return false;
            }
            return true;
        }

        private List<ViewModel> GetViews()
        {
            return HttpContext.Current.Session["CVAVIEW"] as List<ViewModel>;
        }

        private bool VerifySession()
        {
            return HttpContext.Current.Session["CVAUSR"] == null;
        }
    }
}