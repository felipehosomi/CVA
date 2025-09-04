using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    [CvaAuthorize("Calendário")]
    public class CalendarioController : Controller
    {
        private CVAGestaoService.CalendarClient _calendarClient { get; set; }
        private GetSession GetSession = null;

        public CalendarioController()
        {
            LoadViewBags(); 
            this._calendarClient =  new CVAGestaoService.CalendarClient();
            ViewBag.Perfil =        GetSession.UserConnected;
        }

        public ActionResult Consultar()
        {
            return View();
        }

        public ActionResult Pesquisar()
        {
            return View(_calendarClient.GetCalendar());
        }

        public ActionResult CriarCalendario()
        {
            return View();
        }

        public ActionResult Editar(int calendarID)
        {
            CalendarModel model = _calendarClient.GetCalendar_ById(calendarID);
            return View("CriarCalendario", model);
        }

        public string Salvar(CalendarModel calendario)
        {
            calendario.User = new UserModel
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_calendarClient.SaveCalendarHeader(calendario));
        }

        public string GetSpecificStatus()
        {
            return JsonConvert.SerializeObject(_calendarClient.CalendarStatus());
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
