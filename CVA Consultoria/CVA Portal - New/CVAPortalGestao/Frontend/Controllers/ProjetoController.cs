using Newtonsoft.Json;
using System.Web.Mvc;
using MODEL.Classes;
using CVAGestaoLayout.Helper;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using CVAGestaoLayout.Report.ProjectReport;
using System;
using CVAGestaoLayout.Report.StatusReport;

namespace CVAGestaoLayout.Controllers
{
    public class ProjetoController : Controller
    {
        #region Atributos
        private GetSession GetSession;
        private CVAGestaoService.ClientClient _clientService { get; set; }
        private CVAGestaoService.ProjectClient _projectClient { get; set; }
        private CVAGestaoService.CollaboratorClient _collaboratorClient { get; set; }
        private CVAGestaoService.StatusReportClient _statusReportClient { get; set; }
        #endregion

        #region Construtor
        public ProjetoController()
        {
            this.GetSession = new GetSession();
            this._clientService = new CVAGestaoService.ClientClient();
            this._projectClient = new CVAGestaoService.ProjectClient();
            this._collaboratorClient = new CVAGestaoService.CollaboratorClient();
            this._statusReportClient = new CVAGestaoService.StatusReportClient();

            ViewBag.Perfil = GetSession.UserConnected;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.UserName = GetSession.UserConnected.Name;
            ViewBag.FinancialAccess = GetSession.UserConnected.Profile.FinancialAccess;
        }
        #endregion

        public ActionResult Pesquisar()
        {
            return View();
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        public ActionResult Get(int id)
        {
            return View("Cadastrar", _projectClient.Get(id));
        }

        public string GetSteps()
        {
            return JsonConvert.SerializeObject(_projectClient.Project_GetSteps());
        }

        public string Salvar(ProjectModel model)
        {
            model.User = new UserModel { Id = GetSession.UserConnected.Id };
            return JsonConvert.SerializeObject(_projectClient.Project_Save(model));
        }

        public string Add_ColToProject(int ProjectId, int ColaboradorId)
        {
            return JsonConvert.SerializeObject(_projectClient.Project_Add_ColToProject(ProjectId, ColaboradorId));
        }


        public string Remove_ColToProject(int ProjectId, int ColaboradorId)
        {
            return JsonConvert.SerializeObject(_projectClient.Project_Remove_ColToProject(ProjectId, ColaboradorId));
        }

        public string LoadCombo()
        {
            return JsonConvert.SerializeObject(_projectClient.LoadCombo_Project());
        }

        public string Remove_Step(StepModel model)
        {
            return JsonConvert.SerializeObject(_projectClient.Remove_Step(model));
        }

        public string Generate_Number(int id)
        {
            return _projectClient.Project_Generate_Number(id);
        }

        public string Search(int clientId, string code, string status)
        {
            if (String.IsNullOrEmpty(status) || status == "undefined")
            {
                status = "0";
            }

            var model = new ProjectFilterModel
            {
                ClientId = clientId,
                Code = code,
                Status = Convert.ToInt32(status)
            };
            return JsonConvert.SerializeObject(_projectClient.Project_Search(model));
        }

        //public ActionResult StatusReportParcial(int id)
        //{
        //    var statusReportDataSet = new StatusReportParcialDataSet();

        //    var result = _projectClient.Project_Get_StatusReportParcial(id);

        //    var concluido = new Double();
        //    foreach (var fase in result.StatusReport[0].Fases)
        //        concluido += Convert.ToDouble(fase.Concluido);
        //    concluido = concluido / result.StatusReport[0].Fases.Count;

        //    statusReportDataSet.dtProjeto.AdddtProjetoRow(
        //        result.Codigo
        //      , result.Tag
        //      , result.Nome
        //      , concluido.ToString()
        //      , result.StatusReport[0].Concluido.ToString()
        //    );

        //    foreach (var item in result.StatusReport)
        //        statusReportDataSet.dtStatusReport.AdddtStatusReportRow(
        //            item.Data.ToShortDateString()
        //          , item.Descricao
        //          , item.PontosAtencao
        //          , item.PlanoDeAcao
        //          , item.Conquistas
        //          , item.ProximosPassos
        //          , item.GerenteProjeto.Nome
        //          , item.HorasOrcadas
        //          , item.HorasConsumidas
        //          , item.Concluido
        //        );

        //    var aux = result.StatusReport[0].Fases;

        //    foreach (var item in aux)
        //    {
        //        var dias = Convert.ToInt32((item.DataPrevista - DateTime.Today).TotalDays);
        //        if (Convert.ToInt32(item.Concluido) < 100)
        //            statusReportDataSet.dtFase.AdddtFaseRow(
        //                item.Nome
        //              , item.Concluido
        //              , item.DataInicio.ToShortDateString()
        //              , item.DataPrevista.ToShortDateString()
        //              , dias.ToString()
        //            );
        //        else
        //            statusReportDataSet.dtFase.AdddtFaseRow(
        //                item.Nome
        //              , item.Concluido
        //              , item.DataInicio.ToShortDateString()
        //              , item.DataPrevista.ToShortDateString()
        //              , "-"
        //            );
        //    }

        //    var horas = _statusReportClient.StatusReport_Get_ParcialHours(result.Id, result.StatusReport[0].Data);
        //    statusReportDataSet.dtHoras.AdddtHorasRow(
        //            horas[0]
        //          , horas[1]
        //          , horas[2]
        //          , horas[3]
        //          , horas[4]
        //          , horas[5]
        //        );

        //    var viewer = new ReportViewer()
        //    {
        //        ProcessingMode = ProcessingMode.Local,
        //        SizeToReportContent = true,
        //        Width = Unit.Percentage(100),
        //        Height = Unit.Percentage(100)
        //    };

        //    viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\StatusReport\StatusReportParcial.rdlc";
        //    viewer.LocalReport.DataSources.Add(new ReportDataSource("ProjetoDataSet", (DataTable)statusReportDataSet.dtProjeto));
        //    viewer.LocalReport.DataSources.Add(new ReportDataSource("StatusReportDataSet", (DataTable)statusReportDataSet.dtStatusReport));
        //    viewer.LocalReport.DataSources.Add(new ReportDataSource("FaseDataSet", (DataTable)statusReportDataSet.dtFase));
        //    viewer.LocalReport.DataSources.Add(new ReportDataSource("HorasDataSet", (DataTable)statusReportDataSet.dtHoras));
        //    viewer.LocalReport.Refresh();

        //    byte[] bytes;
        //    bytes = viewer.LocalReport.Render("PDF", null, out string mimeType, out string encoding, out string filenameExtension, out string[] streamids, out Warning[] warnings);
        //    return new FileContentResult(bytes, mimeType);
        //}

        //public ActionResult RelatorioProjeto(int id)
        //{
        //    var projeto = _projectClient.Get(id);
        //    var projetoDataSet = new ProjetoCompletoDataSet();
        //    var recursosDataSet = new RecursosDataSet();
        //    var fasesDataSet = new FasesDataSet();

        //    projetoDataSet.dtProjeto.AdddtProjetoRow(
        //           projeto.Tag
        //         , projeto.Codigo
        //         , projeto.Nome
        //         , projeto.Descricao
        //         , projeto.DataInicial.ToShortDateString()
        //         , projeto.DataPrevista.ToShortDateString()
        //         , projeto.Cliente.Name
        //         , projeto.TipoProjeto.Nome
        //         , projeto.ValorProjeto
        //         , projeto.CustoOrcado
        //         , projeto.CustoReal
        //         , projeto.HorasOrcadas
        //         , projeto.HorasConsumidas
        //         , projeto.IngressoLiquido
        //         , projeto.RiscoGerenciavel
        //         , projeto.IngressoTotal
        //         , projeto.ResponsavelDespesa
        //    );

        //    foreach (var recurso in projeto.Recursos)
        //    {
        //        recursosDataSet.dtRecursos.AdddtRecursosRow(
        //          recurso.Nome
        //        , recurso.Insert.ToShortDateString()
        //        );
        //    }

        //    foreach (var fase in projeto.Fases)
        //    {
        //        fasesDataSet.dtFases.AdddtFasesRow(
        //          fase.Nome
        //        , fase.DataInicio.ToShortDateString()
        //        , fase.DataPrevista.ToShortDateString()
        //        );
        //    }

        //    var viewer = new ReportViewer()
        //    {
        //        ProcessingMode = ProcessingMode.Local,
        //        SizeToReportContent = true,
        //        Width = Unit.Percentage(100),
        //        Height = Unit.Percentage(100)
        //    };

        //    viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\ProjectReport\ProjetoCompleto.rdlc";
        //    viewer.LocalReport.DataSources.Add(new ReportDataSource("ProjetoCompletoDataSet", (DataTable)projetoDataSet.dtProjeto));
        //    viewer.LocalReport.DataSources.Add(new ReportDataSource("RecursosDataSet", (DataTable)recursosDataSet.dtRecursos));
        //    viewer.LocalReport.DataSources.Add(new ReportDataSource("FasesDataSet", (DataTable)fasesDataSet.dtFases));
        //    viewer.LocalReport.Refresh();

        //    byte[] bytes;
        //    bytes = viewer.LocalReport.Render("PDF", null, out string mimeType, out string encoding, out string filenameExtension, out string[] streamids, out Warning[] warnings);
        //    return new FileContentResult(bytes, mimeType);
        //}

        public ActionResult ExtrairRelatorio(int clientId, string code, int reportType)
        {
            var model = new ProjectFilterModel
            {
                ClientId = clientId,
                Code = code
            };
            var projetos = _projectClient.Project_Search(model);

            var projetoDataSet = new ProjetoDataSet();
            foreach (var item in projetos)
            {
                projetoDataSet.dtProjeto.AdddtProjetoRow(
                    item.Nome
                    , item.Cliente.Name
                    , item.DataInicial.ToShortDateString()
                );
            }

            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\ProjectReport\ProjetoReport.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ProjetoDataSet", (DataTable)projetoDataSet.dtProjeto));

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
            if (reportType == 1)
                bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            else
                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            return new FileContentResult(bytes, mimeType);
        }
    }
}
