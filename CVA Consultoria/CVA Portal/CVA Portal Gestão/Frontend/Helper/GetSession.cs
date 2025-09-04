using MODEL.Classes;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace CVAGestaoLayout.Helper
{
    public class GetSession : Controller
    {
        public static UserModel UserConnected;

        public GetSession()
        {
            UserConnected = (UserModel)System.Web.HttpContext.Current.Session["CVAUSR"];
        }
    }

    public class CvaAuthorize : AuthorizeAttribute
    {
        public string[] View { get; set; }
        public bool NoValidation { get; set; }

        public CvaAuthorize(params string[] Perfil)
        {
            this.View = Perfil;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!NoValidation)
            {
                if (VerifySession())
                    return false;

                var authorized = base.AuthorizeCore(httpContext);
                if (!authorized)
                    return false;

                string previlege;
                foreach (var item in GetViews())
                {
                    previlege = item.Name;
                    foreach (var p in View)
                        if (previlege.Equals(p.ToString()))
                            return true;
                }
                return false;
            }
            return true;
        }
        private string GetUserRights()
        {
            return ((UserModel)HttpContext.Current.Session["CVAUSR"]).Profile.Name;
        }

        private List<UserViewModel> GetViews()
        {
            return ((UserModel)HttpContext.Current.Session["CVAUSR"]).Profile.UserView;
        }
        private bool VerifySession()
        {
            return HttpContext.Current.Session["CVAUSR"] == null;
        }
    }
}