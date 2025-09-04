using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class AutorizacaoController : Controller
    {
        #region Atributos
        private GetSession _getSession = null;
        private CVAGestaoService.AuthorizationClient _authorizationClient { get; set; }
        private CVAGestaoService.NotePeriodClient _notePeriodClient { get; set; }
        #endregion

        #region Construtor
        public AutorizacaoController()
        {
            this._getSession = new GetSession();
            this._authorizationClient = new CVAGestaoService.AuthorizationClient();
            this._notePeriodClient = new CVAGestaoService.NotePeriodClient();

            ViewBag.Perfil = GetSession.UserConnected;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.UserName = GetSession.UserConnected.Name;
        }
        #endregion

        public string Get_DiasAutorizados(int idCol)
        {
            return JsonConvert.SerializeObject(_authorizationClient.Get_DiasAutorizados(idCol));
        }

        public ActionResult AddDiaAutorizado(AuthorizedDayModel model)
        {
            var periodFrom = _notePeriodClient.GetPeriod(model.De.Year, model.De.Month);
            if (periodFrom != null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"Não é mais possível adicionar autorizações dentro do período {periodFrom.Description} pois este período já foi fechado.");

            var periodTo = _notePeriodClient.GetPeriod(model.Ate.Year, model.Ate.Month);
            if (periodTo != null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"Não é mais possível adicionar autorizações dentro do período {periodTo.Description} pois este período já foi fechado.");

            model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return Json(_authorizationClient.AddDiaAutorizado(model));
        }

        public string RemoveDiaAutorizado(int id)
        {
            return JsonConvert.SerializeObject(_authorizationClient.RemoveDiaAutorizado(id));
        }

        public string Get_DespesasAutorizados(int idCol)
        {
            return JsonConvert.SerializeObject(_authorizationClient.Get_DespesasAutorizados(idCol));
        }

        public string AddDespesaAutorizado(AuthorizedDayModel model)
        {
            model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return JsonConvert.SerializeObject(_authorizationClient.AddDespesaAutorizado(model));
        }

        public string RemoveDespesaAutorizado(int id)
        {
            return JsonConvert.SerializeObject(_authorizationClient.RemoveDespesaAutorizado(id));
        }

        public string Get_HorasAutorizadas(int idCol)
        {
            return JsonConvert.SerializeObject(_authorizationClient.Get_HorasAutorizadas(idCol));
        }

        public ActionResult AddHorasAutorizadas(AuthorizedHoursModel model)
        {
            var period = _notePeriodClient.GetPeriod(model.Data.Year, model.Data.Month);
            if (period != null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"Não é mais possível adicionar autorizações dentro do período {period.Description} pois este período já foi fechado.");

            model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return Json(_authorizationClient.AddHorasAutorizadas(model));
        }

        public string RemoveHorasAutorizadas(int id)
        {
            return JsonConvert.SerializeObject(_authorizationClient.RemoveHorasAutorizadas(id));
        }

        public string Get_LimiteHoras(int idCol)
        {
            return JsonConvert.SerializeObject(_authorizationClient.Get_LimiteHoras(idCol));
        }

        public string AddLimiteHoras(HoursLimitModel model)
        {
            model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return JsonConvert.SerializeObject(_authorizationClient.AddLimiteHoras(model));
        }

        public string RemoveLimiteHoras(int id)
        {
            return JsonConvert.SerializeObject(_authorizationClient.RemoveLimiteHoras(id));
        }

        [CvaAuthorize("Tela de Autorizações")]
        public ActionResult AdministrarApontamento()
        {
            //return View("~/Views/Apontamento/Administrar/AdministrarApontamento.cshtml");
            return View();
        }
    }
}