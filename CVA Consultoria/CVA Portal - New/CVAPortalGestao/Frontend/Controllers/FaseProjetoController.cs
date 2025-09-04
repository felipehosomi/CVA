using CVAGestaoLayout.Helper;
using MODEL.Enumerators;
using MODEL.Classes;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class FaseProjetoController : Controller
    {
        private CVAGestaoService.ProjectStepClient _projectStepClient { get; set; }
        private GetSession GetSession = null;

        public FaseProjetoController()
        {
            this._projectStepClient = new CVAGestaoService.ProjectStepClient();

            LoadViewBags();
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        public string Salvar(ProjectStepModel projectStep)
        {
            projectStep.User = new UserModel
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_projectStepClient.SaveProjectStep(projectStep));
        }

        public string GetSpecificStatus()
        {
            return JsonConvert.SerializeObject(_projectStepClient.GetProjectStepStatus());
        }

        public string Get()
        {
            return JsonConvert.SerializeObject(_projectStepClient.GetProjectStep((int)oProjectStepType.ProjectStep));
        }

        public ActionResult Pesquisar()
        {
            return View(_projectStepClient.GetAllProjectStep());
        }

        public ActionResult Editar(int id)
        {
            return View("Cadastrar", _projectStepClient.GetProjectStep_ByID(id));
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
