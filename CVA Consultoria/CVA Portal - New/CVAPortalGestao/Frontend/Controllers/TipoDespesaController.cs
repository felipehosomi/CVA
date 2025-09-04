using CVAGestaoLayout.CVAGestaoService;
using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class TipoDespesaController : Controller
    {
        private ExpenseTypeClient _expenseTypeClient { get; set; }
        private UnitMeterClient _unitMeterClient { get; set; }
        private GetSession GetSession = null;

        public TipoDespesaController()
        {
            this._expenseTypeClient =   new ExpenseTypeClient();
            this._unitMeterClient =     new UnitMeterClient();

            LoadViewBags();
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        public ActionResult Pesquisar()
        {
            return View(_expenseTypeClient.GetAllExpenseTypes());
        }

        public ActionResult Editar(int id)
        {
            return View("Cadastrar", _expenseTypeClient.GetExpenseType(id));
        }

        public string GetSpecificStatus()
        {
            return JsonConvert.SerializeObject(_expenseTypeClient.ExpenseTypeStatus());
        }

        public string Salvar(ExpenseTypeModel model)
        {
            try
            {
                model.User = new UserModel
                {
                    Id = GetSession.UserConnected.Id,
                };
                return JsonConvert.SerializeObject(_expenseTypeClient.SaveExpenseType(model));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetUnitMeters()
        {
            return JsonConvert.SerializeObject(_unitMeterClient.GetUnitMeter());
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