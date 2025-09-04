using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    [CvaAuthorize("Especialidade")]
    public class EspecialidadeController : Controller
    {
        private CVAGestaoService.SpecialtyClient _specialtyClient { get; set; }
        private GetSession GetSession = null;

        public EspecialidadeController()
        {
            this._specialtyClient = new CVAGestaoService.SpecialtyClient();

            LoadViewBags();
        }

        public ActionResult Cadastrar()
        {
            ViewBag.FinancialAccess = GetSession.UserConnected.Profile.FinancialAccess;
            return View();
        }

        public string Get_All()
        {
            return JsonConvert.SerializeObject(_specialtyClient.Specialty_Get_All());
        }

        public ActionResult Pesquisar()
        {
            return View(_specialtyClient.GetSpecialtys());
        }

        public ActionResult Editar(int id)
        {
            ViewBag.FinancialAccess = GetSession.UserConnected.Profile.FinancialAccess;
            return View("Cadastrar", _specialtyClient.GetSpecialty_ByID(id));
        }

        public string Salvar(SpecialtyModel specialty)
        {
            specialty.User = new UserModel{
               Id =  GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_specialtyClient.SaveSpecialty(specialty));
        }

        public string Get_TiposEspecialidade()
        {
            return JsonConvert.SerializeObject(_specialtyClient.Specialty_Get_TiposEspecialidade());
        }

        public string GetSpecificStatus()
        {
            return JsonConvert.SerializeObject(_specialtyClient.SpecialtyStatus());
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
