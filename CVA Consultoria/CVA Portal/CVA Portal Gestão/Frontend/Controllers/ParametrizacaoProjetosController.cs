using Newtonsoft.Json;
using System.Web.Mvc;
using CVAGestaoLayout.Helper;
using MODEL.Classes;

namespace CVAGestaoLayout.Controllers
{
    [CvaAuthorize("Parametrizacao Projetos")]
    public class ParametrizacaoProjetosController : Controller
    {
        #region Atributos
        private GetSession GetSession;
        private CVAGestaoService.ProjectParametersClient _projectParametersService { get; set; }
        #endregion

        #region Construtor
        public ParametrizacaoProjetosController()
        {
            this.GetSession = new GetSession();
            this._projectParametersService = new CVAGestaoService.ProjectParametersClient();

            ViewBag.Perfil = GetSession.UserConnected;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.UserName = GetSession.UserConnected.Name;
        }
        #endregion

        public ActionResult Parametros()
        {
            return View();
        }

        public string Get_All()
        {
            return JsonConvert.SerializeObject(_projectParametersService.ProjectParameters_Get_All());
        }

        public string Save(ProjectParametersModel model)
        {
            model.User = new UserModel()
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_projectParametersService.ProjectParameters_Save(model));
        }
    }
}
