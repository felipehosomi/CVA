using CVAGestaoLayout.Helper;
using Microsoft.Reporting.WebForms;
using MODEL.Classes;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CVAGestaoLayout.Report.NoteReport;
using System.Net;

namespace CVAGestaoLayout.Controllers
{
    public class ApontamentoController : Controller
    {
        #region Properties
        private GetSession _getSession = null;
        private CVAGestaoService.NoteClient _noteClient { get; set; }
        private CVAGestaoService.NotePeriodClient _notePeriodClient { get; set; }
        private CVAGestaoService.ProjectClient _projectClient { get; set; }
        private CVAGestaoService.SpecialtyClient _specialtyClient { get; set; }
        private CVAGestaoService.AMSTicketClient _AMSTicketClient { get; set; }
        private CVAGestaoService.ProjectStepClient _projectStepClient { get; set; }
        #endregion

        #region Construtor
        public ApontamentoController()
        {
            this._getSession = new GetSession();
            this._noteClient = new CVAGestaoService.NoteClient();
            this._notePeriodClient = new CVAGestaoService.NotePeriodClient();
            this._projectClient = new CVAGestaoService.ProjectClient();
            this._specialtyClient = new CVAGestaoService.SpecialtyClient();
            this._AMSTicketClient = new CVAGestaoService.AMSTicketClient();
            this._projectStepClient = new CVAGestaoService.ProjectStepClient();

            ViewBag.Perfil = GetSession.UserConnected;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.UserName = GetSession.UserConnected.Name;
        }
        #endregion


        #region Views
        [CvaAuthorize("Lançar Apontamentos")]
        public ActionResult Apontamento()
        {
            return View();
        }

        [CvaAuthorize("Consultar Apontamentos")]
        public ActionResult ConsultaApontamento()
        {
            return View();
        }

        [CvaAuthorize("Gerenciar Apontamentos")]
        public ActionResult Gerenciar()
        {
            return View();
        }
        #endregion

        public string GetApontamentos(int month, int year)
        {
            return JsonConvert.SerializeObject(_noteClient.Get_UserNotes(GetSession.UserConnected.Id, month, year));
        }

        public string GetProjects()
        {
            return JsonConvert.SerializeObject(_projectClient.Get_ByUser(GetSession.UserConnected.Id));
        }

        public string Get_Specialty(int projectId, int StepId)
        {
            return JsonConvert.SerializeObject(_specialtyClient.Get_Specialty(projectId, StepId, GetSession.UserConnected.Id));
        }

        public string GetTicketByProject(int projectId)
        {
            return JsonConvert.SerializeObject(_AMSTicketClient.GetTicketsByProject(projectId));
        }

        public string GetTicketByProjectAndDate(int projectId, DateTime date)
        {
            return JsonConvert.SerializeObject(_AMSTicketClient.GetTicketsByProjectAndDate(projectId, date));
        }

        public string GetTicketByProjectAndDateAndTicket(int projectId, DateTime date, int numTicket)
        {
            return JsonConvert.SerializeObject(_AMSTicketClient.GetTicketsByProjectAndDateAndTicket(projectId, date, numTicket));
        }

        public string Get_ProjectSteps(int projectId)
        {
            return JsonConvert.SerializeObject(_projectStepClient.Get_ProjectSteps(projectId, GetSession.UserConnected.Id));
        }
        
        public ActionResult Salvar(NoteModel model)
        {
            var period = _notePeriodClient.GetPeriod(model.Date.Year, model.Date.Month);
            if (period != null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"Não é mais possível realizar apontamentos dentro do período {period.Description} pois este período já foi fechado.");

            //model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return Json(_noteClient.Note_Save(model));
        }

        public string Remove(NoteModel model)
        {
            return JsonConvert.SerializeObject(_noteClient.Note_Remove(model));
        }

        public string Filtrar(int user, DateTime? initialDate, DateTime? finishDate, int projectId, int clientId, int Chamado, bool interno)
        {
            if (interno)
                user = GetSession.UserConnected.Id;

            var model = new NoteFilterModel
            {
                UserId = user,
                InitialDate = initialDate,
                FinishDate = finishDate,
                ProjectId = projectId,
                ClientId = clientId,
                Chamado = Chamado,
                interno = interno
            };

            return JsonConvert.SerializeObject(_noteClient.Note_Search(model));
        }

        public ActionResult RelatorioApontamento(int user, DateTime? initialDate, DateTime? finishDate, int projectId, int statusID, string type, int clientId, bool interno)
        {
            if (interno)
                user = GetSession.UserConnected.Collaborator.Id;

            var model = new NoteFilterModel
            {
                UserId = user,
                InitialDate = initialDate,
                FinishDate = finishDate,
                ProjectId = projectId,
                ClientId = clientId,
            };

            NoteModel[] apontamentos = _noteClient.Note_Search(model);

            var apontamentoDataSet = new ApontamentoDataSet();
            foreach (var item in apontamentos)
            {
                try
                {
                    apontamentoDataSet.dtApontamento.AdddtApontamentoRow(
                      item.Date.ToShortDateString()
                    , item.User.Name
                    , item.Project.Codigo + item.Project.Tag + item.Project.Nome
                    , item.Project.Cliente.Name
                    , item.InitHour.Value.ToString("HH:mm")
                    , item.IntervalHour.Value.ToString("HH:mm")
                    , item.FinishHour.Value.ToString("HH:mm")
                    , item.TotalLine
                    , item.Description
                    , item.Ticket.Code.ToString()
                    , item.Requester
                    , item.Specialty.Name
                    , item.Project.TipoProjeto.Nome
                    , item.Project.TipoProjeto.Nome
                    , TimeSpan.Parse(item.TotalLine).TotalHours.ToString()
                    , item.TotalHours
                    );
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\NoteReport\ApontamentoReport.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ApontamentoDataSet", (DataTable)apontamentoDataSet.dtApontamento));

            viewer.SizeToReportContent = true;
            viewer.Width = Unit.Percentage(100);
            viewer.Height = Unit.Percentage(100);
            viewer.LocalReport.Refresh();

            Warning[] warnings;
            string mimeType;
            string[] streamids;
            string encoding;
            string filenameExtension;

            byte[] bytes;
            if (type.Equals("PDF"))
                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            else
                bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            return new FileContentResult(bytes, mimeType);
        }
    }
}