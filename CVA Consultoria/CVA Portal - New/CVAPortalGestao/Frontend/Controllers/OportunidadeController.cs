using CVAGestaoLayout.Helper;
using CVAGestaoLayout.Report.OportunittyReport;
using Microsoft.Reporting.WebForms;
using MODEL.Classes;
using MODEL.Enumerators;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace CVAGestaoLayout.Controllers
{
    public class OportunidadeController : Controller
    {
        #region Instances
        private CVAGestaoService.OpportunityClient _oportunittyClient { get; set; }
        private CVAGestaoService.ClientClient _clientService { get; set; }
        private CVAGestaoService.CollaboratorClient _collaboratorClient { get; set; }
        private CVAGestaoService.ProjectStepClient _projectStepClient { get; set; }
        private CVAGestaoService.UserClient _userClient { get; set; }
        private CVAGestaoService.PercentProjectClient _percentProjectClient { get; set; }
        private GetSession GetSession = null;
        #endregion

        public OportunidadeController()
        {
            this._oportunittyClient = new CVAGestaoService.OpportunityClient();
            this._clientService = new CVAGestaoService.ClientClient();
            this._collaboratorClient = new CVAGestaoService.CollaboratorClient();
            this._projectStepClient = new CVAGestaoService.ProjectStepClient();
            this._percentProjectClient = new CVAGestaoService.PercentProjectClient();
            this._userClient = new CVAGestaoService.UserClient();

            LoadViewBags();
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        public string Salvar(OpportunityModel model)
        {
            model.User = new UserModel
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_oportunittyClient.SaveOportunitty(model));
        }

        public string GetName()
        {
            return ViewBag.UserName;
        }

        public ActionResult Oportunidades()
        {
            return View();
        }

        public string Generate_NewCode(int id)
        {
            return _oportunittyClient.Generate_NewCode(id);
        }

      

        public string GetClients()
        {
            return JsonConvert.SerializeObject(_clientService.LoadCombo_Client());
        }

        public string GetContactsFromClient(int id)
        {
            return JsonConvert.SerializeObject(_clientService.GetClientContacts(id));
        }

        public string GetProjectManagers()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.GetPMs());
        }

        public string GetProjectStep()
        {
            return JsonConvert.SerializeObject(_projectStepClient.GetProjectStep((int)oProjectStepType.oOportunitty));
        }

        public string CopyToProject(OpportunityModel model)
        {
            try
            {
                model.User = new UserModel
                {
                    Id = GetSession.UserConnected.Id
                };
                return JsonConvert.SerializeObject(_oportunittyClient.ConvertOportunittyToProject(model));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetPercents()
        {
            try
            {
                return JsonConvert.SerializeObject(_percentProjectClient.GetPercentProject());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Editar(int id)
        {
            try
            {
                return View("Cadastrar", _oportunittyClient.GetOportunittyById(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

    

        public string Search(string Codigo, int IdCliente)
        {
            return JsonConvert.SerializeObject(_oportunittyClient.Search(Codigo, IdCliente));
        }

        public ActionResult GerarRelatorio(string Codigo, int IdCliente, int Tipo)
        {
            OpportunityModel[] oportunidades = null;
            oportunidades = _oportunittyClient.Search(Codigo, IdCliente);

            var oportunidadeDataSet = new OportunidadeDataSet();
            foreach (var oprt in oportunidades)
            {
                oportunidadeDataSet.dtOportunidade.AdddtOportunidadeRow(
                    oprt.Codigo
                    , oprt.Cliente.Name
                    , ""
                    , "R$ " + oprt.ValorOportunidade.ToString()
                    , oprt.Insert.ToShortDateString()
                    , oprt.DataPrevista.ToShortDateString()
                );
            }

            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\OportunittyReport\OportunidadeReport.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("OportunidadeDataSet", (DataTable)oportunidadeDataSet.dtOportunidade));

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
            if (Tipo == 1)
                bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            else
                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            return new FileContentResult(bytes, mimeType);
        }

        private void LoadViewBags()
        {
            this.GetSession = new GetSession();
            ViewBag.UserName = _userClient.GetUser(GetSession.UserConnected.Id).Name;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.Perfil = GetSession.UserConnected;

            ViewBag.FinancialAccess = GetSession.UserConnected.Profile.FinancialAccess;
        }
    }
}
