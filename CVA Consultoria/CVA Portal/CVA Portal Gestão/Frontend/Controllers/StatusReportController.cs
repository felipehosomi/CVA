using CVAGestaoLayout.Helper;
using CVAGestaoLayout.Report.ClientReport;
using Microsoft.Reporting.WebForms;
using MODEL.Classes;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CVAGestaoLayout.Controllers
{
    public class StatusReportController : Controller
    {
        #region Atributos
        private CVAGestaoService.StatusReportClient _statusReportService { get; set; }
        private GetSession GetSession = null;
        #endregion

        #region Construtor
        public StatusReportController()
        {
            this._statusReportService = new CVAGestaoService.StatusReportClient();
            LoadViewBags();
        }
        #endregion

        public string Save(StatusReportModel model)
        {
            model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return JsonConvert.SerializeObject(_statusReportService.StatusReport_Save(model));
        }

        public string Get_All(int id)
        {
            return JsonConvert.SerializeObject(_statusReportService.StatusReport_Get_All(id));
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
