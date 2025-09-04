using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class PerfilController : Controller
    {
        private CVAGestaoService.ProfileClient _profileClient { get; set; }
        private CVAGestaoService.UserViewClient _userViewClient { get; set; }
        private GetSession GetSession = null;

        public PerfilController()
        {
            this._profileClient = new CVAGestaoService.ProfileClient();
            this._userViewClient = new CVAGestaoService.UserViewClient();

            LoadViewBags();
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        public ActionResult Pesquisar()
        {
            return View(_profileClient.GetProfiles());
        }

        public string Salvar(ProfileModel model)
        {
            model.User = new UserModel
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_profileClient.SaveProfile(model));
        }

        public string GetViews()
        {
            try
            {
                return JsonConvert.SerializeObject(_userViewClient.GetUserView());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetSpecificStatus()
        {
            return JsonConvert.SerializeObject(_profileClient.ProfileGetStatus());
        }

        public ActionResult Editar(int profileID)
        {
            return View("Cadastrar", _profileClient.GetProfile_ByID(profileID));
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