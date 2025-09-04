using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class UsuarioController : Controller
    {
        private CVAGestaoService.UserClient _userClient { get; set; }
        private CVAGestaoService.CollaboratorClient _collaboratorClient { get; set; }
        private CVAGestaoService.ProfileClient _profileClient { get; set; }
        private GetSession GetSession = null;

        public UsuarioController()
        {
            this._userClient = new CVAGestaoService.UserClient();
            this._collaboratorClient = new CVAGestaoService.CollaboratorClient();
            this._profileClient = new CVAGestaoService.ProfileClient();

            LoadViewBags();
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        public ActionResult Pesquisar()
        {
            return View(_userClient.GetUsers());
        }

        public string GetAllUsers()
        {
            return JsonConvert.SerializeObject(_userClient.GetUsers());
        }
        public string GetSpecificStatus()
        {
            return JsonConvert.SerializeObject(_userClient.GetUserStatus());
        }

        public string GetCollaborators()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.GetCollaboratorNotUser());
        }

        public string GetProfile()
        {
            return JsonConvert.SerializeObject(_profileClient.GetProfiles());
        }

        public string GetBranch()
        {
            return null;// JsonConvert.SerializeObject(_branchClient.GetCombo());
        }

        public string Save(UserModel model)
        {
            model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return JsonConvert.SerializeObject(_userClient.SaveUser(model));
        }

        public ActionResult Editar(int userID)
        {
            return View("Cadastrar", _userClient.GetUser(userID));
        }

        [CvaAuthorize(new string[] { "Editar Conta" })]
        public ActionResult EditarConta()
        {
            return View(GetSession.UserConnected);
        }

        [CvaAuthorize(new string[] { "Editar Conta" })]
        public ActionResult AlterarCadastroUsuario(UserModel user)
        {
            try
            {
                var result = _userClient.UpdateUser_ByUser(user);
                if (result.Error == null)
                    return RedirectToAction("LogOut", "Login");
                else
                {
                    ViewBag.Error = result.Error.Message;
                    return View("EditarConta");
                }
            }
            catch (Exception)
            {
                throw;
            }
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