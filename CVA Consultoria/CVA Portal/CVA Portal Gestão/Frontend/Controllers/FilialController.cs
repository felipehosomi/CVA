using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    [CvaAuthorize("Filial")]
    public class FilialController : Controller
    {
        private CVAGestaoService.UfClient _ufClient {get;set;}
        private CVAGestaoService.ExpenseTypeClient _expenseTypeClient { get; set; }
        private GetSession GetSession = null;

        public FilialController()
        {
            this._ufClient =             new CVAGestaoService.UfClient();
            this._expenseTypeClient =   new CVAGestaoService.ExpenseTypeClient();

            LoadViewBags();
        }

        public ActionResult CadastrarFilial()
        {
            return View();
        }

        public string GetSpecificStatus()
        {
            return null;// JsonConvert.SerializeObject(_branchClient.GetBranchStatus());
        }

        public string GetUf()
        {
            return JsonConvert.SerializeObject(_ufClient.GetUf());
        }

        public string GetExpenseTypes()
        {
            return JsonConvert.SerializeObject(_expenseTypeClient.GetExpenseTypes());
        }

       

        public ActionResult Pesquisar()
        {
            return null;// View(_branchClient.GetBranchs());
        }

        public ActionResult Editar(int branchID)
        {
            return null;// View("CadastrarFilial", _branchClient.GetBranch_ById(branchID));
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
