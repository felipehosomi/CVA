using CVAGestaoLayout.Helper;
using Microsoft.Reporting.WebForms;
using MODEL.Classes;
using MODEL.Status;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using CVAGestaoLayout.Report.NoteReport;

namespace CVAGestaoLayout.Controllers
{
    [CvaAuthorize(new string[] { "Apontamento" })]
    public class ApontamentoController : Controller
    {
        #region Properties
        private GetSession _getSession = null;
        private CVAGestaoService.NoteClient _noteClient { get; set; }
        private CVAGestaoService.PeriodClient _periodClient { get; set; }
        private CVAGestaoService.ClientClient _clientService { get; set; }
        private CVAGestaoService.ProjectClient _projectClient { get; set; }
        private CVAGestaoService.SpecialtyClient _specialtyClient { get; set; }
        private CVAGestaoService.CollaboratorClient _collaboratorClient { get; set; }
        private CVAGestaoService.AMSTicketClient _AMSTicketClient { get; set; }
        private CVAGestaoService.ProjectStepClient _projectStepClient { get; set; }
        private CVAGestaoService.UserClient _userClient { get; set; }
        #endregion

        #region Construtor
        public ApontamentoController()
        {
            this._getSession = new GetSession();
            this._noteClient = new CVAGestaoService.NoteClient();
            this._periodClient = new CVAGestaoService.PeriodClient();
            this._clientService = new CVAGestaoService.ClientClient();
            this._projectClient = new CVAGestaoService.ProjectClient();
            this._specialtyClient = new CVAGestaoService.SpecialtyClient();
            this._collaboratorClient = new CVAGestaoService.CollaboratorClient();
            this._AMSTicketClient = new CVAGestaoService.AMSTicketClient();
            this._projectStepClient = new CVAGestaoService.ProjectStepClient();
            this._userClient = new CVAGestaoService.UserClient();

            ViewBag.Perfil = GetSession.UserConnected;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.UserName = GetSession.UserConnected.Name;
        }
        #endregion

        #region Gets
        public string GetActiveProject()
        {
            return JsonConvert.SerializeObject(_projectClient.GetActiveProjects());
        }
        public string GetApontamentos()
        {
            return JsonConvert.SerializeObject(_noteClient.Get_UserNotes(GetSession.UserConnected.Id));
        }
        public string GetAuthorizedHours()
        {
            return JsonConvert.SerializeObject(_noteClient.GetAuthorizedHours());
        }
        public string GetAuthorizedHoursByCollaborator(int collaboratorId)
        {
            return JsonConvert.SerializeObject(_noteClient.GetAuthorizedHoursByCollaborator(collaboratorId));
        }
        public string GetClients()
        {
            return JsonConvert.SerializeObject(_clientService.LoadCombo_Client());
        }

        public string GetProjects()
        {
            return JsonConvert.SerializeObject(_projectClient.Get_ByUser(GetSession.UserConnected.Id));
        }
        public string GetSpecialties()
        {
            var collaborator = new CollaboratorModel();
            collaborator.User = new UserModel
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_specialtyClient.GetSpecialtyByColaborator(collaborator));
        }

        public string GetTicketByProject(int projectId)
        {
            return JsonConvert.SerializeObject(_AMSTicketClient.GetTicketsByProject(projectId));
        }

        public string Get_ProjectSteps(int projectId)
        {
            return JsonConvert.SerializeObject(_projectStepClient.Get_ProjectSteps(projectId));
        }


        #endregion

        #region Periodo
        public string AbrirPeriodo()
        {
            return JsonConvert.SerializeObject(_periodClient.OpenPeriod());
        }

        #endregion


        public string Salvar(NoteModel model)
        {
            model.User = new UserModel
            {
                Id = GetSession.UserConnected.Id,
                Name = this._userClient.GetUser(GetSession.UserConnected.Id).Name
            };
            return JsonConvert.SerializeObject(_noteClient.Save(model));
        }






        public string Remove(int id)
        {
            return JsonConvert.SerializeObject(_noteClient.Note_Remove(id));
        }

        #region Fechamento/Abertura de Subperíodos
        public string GetProjectsByCollaborator(int collaboratorId)
        {
            return JsonConvert.SerializeObject(_projectClient.Get_ByUser(collaboratorId));
        }
        public string SalvarRangeData(SubPeriodModel model)
        {
            model.User = new UserModel
            {
                Id = GetSession.UserConnected.Id
            };

            model.Status = new StatusModel
            {
                Id = (int)StatusEnum.Ativo
            };
            return JsonConvert.SerializeObject(_periodClient.SaveSubPeriod(model));
        }
        public string TravarRangeData(int periodId)
        {
            return JsonConvert.SerializeObject(_periodClient.SetStatusSubPeriod(periodId, (int)StatusEnum.Bloqueado));
        }
        public string TravarTodosRangesData(SubPeriodModel[] ranges)
        {
            List<int> periodIds = ranges.Select(r => r.Id).ToList();
            string periodList = string.Join(",", periodIds.Select(n => n.ToString()));

            return JsonConvert.SerializeObject(_periodClient.SetStatusSubPeriodList(periodList, (int)StatusEnum.Bloqueado));
        }
        public string DestravarTodosRangesData(SubPeriodModel[] ranges)
        {
            List<int> periodIds = ranges.Select(r => r.Id).ToList();
            string periodList = string.Join(",", periodIds.Select(n => n.ToString()));

            return JsonConvert.SerializeObject(_periodClient.SetStatusSubPeriodList(periodList, (int)StatusEnum.Ativo));
        }
        public string DestravarRangeData(int periodId)
        {
            return JsonConvert.SerializeObject(_periodClient.SetStatusSubPeriod(periodId, (int)StatusEnum.Ativo));
        }
        #endregion

        #region Filtros
        public string FiltrarInterno(DateTime? initialDate, DateTime? finishDate, int projectId, int clientId)
        {
            var model = new NoteFilterModel
            {
                UserId = GetSession.UserConnected.Id,
                InitialDate = initialDate,
                FinishDate = finishDate,
                ProjectId = projectId,
                ClientId = clientId,
            };
            return JsonConvert.SerializeObject(_noteClient.Note_Search(model));
        }

        public string FiltrarForUser(DateTime? initialDate, DateTime? finishDate, int projectId, int clientId)
        {
            var model = new NoteFilterModel
            {
                UserId = GetSession.UserConnected.Id,
                InitialDate = initialDate,
                FinishDate = finishDate,
                ProjectId = projectId,
                ClientId = clientId,
            };
            return JsonConvert.SerializeObject(_noteClient.Note_Search(model));
        }

        public string Filtrar(int user, DateTime? initialDate, DateTime? finishDate, int projectId, int statusID, int clientId)
        {
            var model = new NoteFilterModel
            {
                UserId = user,
                InitialDate = initialDate,
                FinishDate = finishDate,
                ProjectId = projectId,
                ClientId = clientId,
            };

            return JsonConvert.SerializeObject(_noteClient.Note_Search(model));
        }
        public string FiltrarAdministracao(int user, DateTime? initialDate, DateTime? finishDate, int projectId, int statusID, int clientId)
        {
            return Filtrar(user, initialDate, finishDate, projectId, statusID, clientId);
        }

        public string FiltrarRangeData(int? colId, int? clientId, int? projectId, DateTime? dateFrom, DateTime? dateTo)
        {
            if (clientId == 0)
            {
                clientId = null;
            }
            if (projectId == 0)
            {
                projectId = null;
            }

            return JsonConvert.SerializeObject(_periodClient.GetSubPeriods(colId, clientId, projectId, dateFrom, dateTo));
        }
        #endregion

        #region Views
        public ActionResult Apontar()
        {
            return View();
        }
        public ActionResult Consultar()
        {
            return View();
        }
        public ActionResult Apontamento()
        {
            return View();
        }
        [CvaAuthorize(new string[] { "Gerenciar Apontamento" })]
        public ActionResult Gerenciar()
        {
            return View();
        }
        [CvaAuthorize(new string[] { "Consulta de Apontamento por Consultor" })]
        public ActionResult ConsultaApontamento()
        {
            return View();
        }
        [CvaAuthorize(new string[] { "Administrar Apontamentos" })]
        public ActionResult AdministrarApontamento()
        {
            return View("~/Views/Apontamento/Administrar/AdministrarApontamento.cshtml");
        }
        #endregion

        #region RelatorioApontamento
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
                , item.Ticket.Id.ToString()
                , item.Requester
                , item.Specialty.Name
                , item.Project.TipoProjeto.Nome
                , item.Project.TipoProjeto.Nome
                , TimeSpan.Parse(item.TotalLine).TotalHours.ToString()
                , item.TotalHours
                );
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
        #endregion
    }
}
