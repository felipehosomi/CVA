using CVAGestaoLayout.Helper;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class PortalController : Controller
    {
        private GetSession GetSession = null;
        public PortalController()
        {
            LoadViewBags();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AcessoNegado()
        {
            return View("~/Views/Shared/AcessoNegado.cshtml");
        }

        private void LoadViewBags()
        {
            this.GetSession = new GetSession();
            ViewBag.UserName = GetSession.UserConnected.Name;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.Perfil = GetSession.UserConnected;
        }
    }
}
