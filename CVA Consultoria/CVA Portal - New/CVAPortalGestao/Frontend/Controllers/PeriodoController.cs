using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;

namespace CVAGestaoLayout.Controllers
{
    public class PeriodoController : Controller
    {
        private GetSession _getSession = null;
        private CVAGestaoService.NotePeriodClient _notePeriodClient { get; set; }

        public PeriodoController()
        {
            _notePeriodClient = new CVAGestaoService.NotePeriodClient();
            _getSession = new GetSession();

            ViewBag.Perfil = GetSession.UserConnected;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.UserName = GetSession.UserConnected.Name;
        }

        [CvaAuthorize("Períodos Fechados")]
        public ActionResult Periodo()
        {
            return View();
        }

        [CvaAuthorize("Períodos Fechados")]
        public ActionResult Cadastrar()
        {
            return View();
        }

        public ActionResult Incluir(NotePeriod period)
        {
            try
            {
                _notePeriodClient.AddPeriod(period.Year, period.Month);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"Erro ao fechar o período {period.Description}. Verifique se o período já está fechado.");
            }
        }

        public ActionResult Excluir(NotePeriod period)
        {
            try
            {
                _notePeriodClient.DeletePeriod(period.Year, period.Month);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public string GetAllPeriods()
        {
            var periods = _notePeriodClient.GetAllPeriods();
            return JsonConvert.SerializeObject(periods);
        }
    }
}