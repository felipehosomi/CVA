using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class TiposProjetoController : Controller
    {
        #region Atributos
        private CVAGestaoService.ProjectTypeClient _projectTypeClient { get; set; }
        private GetSession GetSession = null;
        #endregion

        #region Construtor
        public TiposProjetoController()
        {
            this._projectTypeClient = new CVAGestaoService.ProjectTypeClient();

            this.GetSession = new GetSession();
            ViewBag.UserName = GetSession.UserConnected.Name;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.Perfil = GetSession.UserConnected;
        }
        #endregion

        public ActionResult Pesquisar()
        {
            return View(_projectTypeClient.ProjectType_Get_All());
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        public ActionResult Editar(int id)
        {
            return View("Cadastrar", _projectTypeClient.ProjectType_Get(id));
        }

        public string Get(int id)
        {
            return JsonConvert.SerializeObject(_projectTypeClient.ProjectType_Get(id));
        }

        public string Get_All()
        {
            return JsonConvert.SerializeObject(_projectTypeClient.ProjectType_Get_All());
        }

        public string Insert(ProjectTypeModel model)
        {
            model.User = new UserModel()
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_projectTypeClient.ProjectType_Insert(model));
        }

        public string Update(ProjectTypeModel model)
        {
            model.User = new UserModel()
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_projectTypeClient.ProjectType_Update(model));
        }

        public string Remove(int id)
        {
            return JsonConvert.SerializeObject(_projectTypeClient.ProjectType_Remove(id));
        }
    }
}
