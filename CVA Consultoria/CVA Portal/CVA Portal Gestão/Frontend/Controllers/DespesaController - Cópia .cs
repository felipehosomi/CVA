using AUXILIAR;
using CVAGestaoLayout.Helper;
using CVAGestaoLayout.Report.ExpenseReport;
using DAO.Classes;
using Microsoft.Reporting.WebForms;
using MODEL.Classes;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CVAGestaoLayout.Controllers
{
    [CvaAuthorize("Despesa")]
    public class DespesaController : Controller
    {
        #region Properties
        private CVAGestaoService.ProjectClient _projectClient { get; set; }
        private CVAGestaoService.PoliticExpenseClient _politicExpenseClient { get; set; }
        private CVAGestaoService.ExpenseClient _expenseClient { get; set; }
        private CVAGestaoService.CollaboratorClient _collaboratorClient { get; set; }
        private CVAGestaoService.OpportunityClient _oportunittyClient { get; set; }
        private CVAGestaoService.ClientClient _clientService { get; set; }
        private CVAGestaoService.UserClient _userClient { get; set; }

        private GetSession GetSession = null;
        #endregion

        #region Construtor
        public DespesaController()
        {
            this._projectClient = new CVAGestaoService.ProjectClient();
            this._politicExpenseClient = new CVAGestaoService.PoliticExpenseClient();
            this._expenseClient = new CVAGestaoService.ExpenseClient();
            this._collaboratorClient = new CVAGestaoService.CollaboratorClient();
            this._oportunittyClient = new CVAGestaoService.OpportunityClient();
            this._clientService = new CVAGestaoService.ClientClient();
            this._userClient = new CVAGestaoService.UserClient();

            LoadViewBags();
        }
        #endregion

        #region Views
        public ActionResult Despesa()
        {
            return View();
        }

        public ActionResult Consultar()
        {
            return View(_expenseClient.GetExpense_ByUserID(GetSession.UserConnected.Id));
        }

        public ActionResult Editar(int expenseId)
        {
            var model = _expenseClient.GetExpense_ByID(expenseId);
            return View("Despesa", model);
        }

        public ActionResult Extrair()
        {
            return View();
        }

        public ActionResult ExtrairInterno()
        {
            return View();
        }
        #endregion

        #region Get
        public string GetProjects()
        {
            return JsonConvert.SerializeObject(_projectClient.Get_ByUser(GetSession.UserConnected.Id));
        }

        public string GetClients()
        {
            return JsonConvert.SerializeObject(_clientService.GetClient());
        }

        public string Get()
        {
            return JsonConvert.SerializeObject(_expenseClient.GetExpense_ByUserID(GetSession.UserConnected.Id));
        }

        public string GetExpenseByProject(int projectId)
        {
            return JsonConvert.SerializeObject(_politicExpenseClient.GetPoliticExpenseByProject(projectId, GetSession.UserConnected.Id));
        }

        public string GetProject()
        {
            return JsonConvert.SerializeObject(_projectClient.GetActiveProjects());
        }

        public void OpenExpenseContract()
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            XMLReader reader = new XMLReader();
            process.StartInfo.FileName = reader.GetPDFPath();
            process.Start();
        }

        public string GetOportunitty()
        {
            return JsonConvert.SerializeObject(_oportunittyClient.GetOpportunities());
        }
        #endregion

        #region Post
        public string Save()
        {
            var httpRequest = System.Web.HttpContext.Current.Request;

            var expenseJson = httpRequest.Form["Expense"];
            var expense = JsonConvert.DeserializeObject<ExpenseModel>(expenseJson);

            if (httpRequest.Files.Count > 0)
            {
                var file = httpRequest.Files[0];

                UserModel userModel = this._userClient.GetUser(GetSession.UserConnected.Id);
                expense.Projeto = this._projectClient.Get(expense.Projeto.Id);

                string directory = $"c:\\CVA Consultoria\\Despesas\\{DateTime.Today.ToString("MM-yyyy")}\\{userModel.Name}\\{expense.Projeto.Cliente.Name}\\{DateTime.Today.ToString("dd-MM-yyyy")}";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                else
                {
                    if (System.IO.File.Exists($"{directory}\\{file.FileName}"))
                    {
                        MODEL.SystemMessage.Message message = new MODEL.SystemMessage.Message();
                        message.Error = new MODEL.SystemMessage.ErrorMessage();
                        message.Error.Code = 99;
                        message.Error.Message = "Já existe um arquivo com o nome informado, por favor altere o nome do arquivo e tente novamente";
                        return JsonConvert.SerializeObject(message);
                    }
                }

                file.SaveAs($"{directory}\\{file.FileName}");

                //using (var binaryReader = new BinaryReader(file.InputStream))
                //{
                //    expenseModel.Picture = binaryReader.ReadBytes(file.ContentLength);
                //}
            }

            expense.User = new UserModel
            {
                Id = GetSession.UserConnected.Id,
                Name = this._userClient.GetUser(GetSession.UserConnected.Id).Name
            };


            return JsonConvert.SerializeObject(_expenseClient.SaveExpense(expense));
        }

        public string Remove(int id)
        {
            return JsonConvert.SerializeObject(_expenseClient.Remove(id));
        }
        #endregion

        public ActionResult GetAttachedFile(DateTime date, int projectId, string filename)
        {
            try
            {
                if (!String.IsNullOrEmpty(filename))
                {
                    //DateTime fileDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.CurrentCulture);

                    UserModel userModel = this._userClient.GetUser(GetSession.UserConnected.Id);
                    ProjectModel projModel = this._projectClient.Get(projectId);
                    string directory = $"c:\\CVA Consultoria\\Despesas\\{date.ToString("MM-yyyy")}\\{userModel.Name}\\{projModel.Cliente.Name}\\{date.ToString("dd-MM-yyyy")}";

                    FileStream fs = new FileStream(directory + "\\" + filename, FileMode.Open, FileAccess.Read);
                    return File(fs, "application/" + filename.Split('.')[0], filename);
                }
                return new HttpNotFoundResult("Arquivo não encontrado no sevidor!");
            }
            catch (Exception ex)
            {
                return new HttpNotFoundResult("Erro geral: " + ex.Message);
            }
        }

        #region RelatorioDespesa
        public ActionResult RelatorioDespesa(int collaboratorID, int projectID, int oportunittyID, DateTime? initialDate, DateTime? finishDate, int clientId)
        {
            var despesas = _expenseClient.FilterExpense(collaboratorID, projectID, oportunittyID, initialDate, finishDate, clientId);
            return View();
        }

        public ActionResult RelatorioDespesas(int collaboratorID, int projectID, int oportunittyID, DateTime? initialDate, DateTime? finishDate, int clientId, string type, bool interno)
        {
            ExpenseModel[] despesas = null;
            if (interno)
                despesas = _expenseClient.FilterExpense(GetSession.UserConnected.Collaborator.Id, projectID, oportunittyID, initialDate, finishDate, clientId);
            else
                despesas = _expenseClient.FilterExpense(collaboratorID, projectID, oportunittyID, initialDate, finishDate, clientId);

            var despesaDataSet = new ExpenseDataSet();
            foreach (var desp in despesas)
            {
                despesaDataSet.dtDespesa.AdddtDespesaRow(
                    desp.Data.ToShortDateString()
                    , desp.User.Name
                    , desp.Projeto.Nome
                    , desp.Projeto.Nome
                    , desp.Descricao
                    , desp.NumNota
                    , desp.TipoDespesa.Name
                    , desp.ValorNota
                    , desp.ValorReembolso
                    , desp.ValorDespesa
                    , desp.Projeto.Cliente.Name
                );
            }

            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\ExpenseReport\DespesaReport.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ExpenseDataSet", (DataTable)despesaDataSet.dtDespesa));

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

        #region Filtro
        public string Filtrar(int collaboratorID, int projectID, int oportunittyID, DateTime? initialDate, DateTime? finishDate, int clientId)
        {
            return JsonConvert.SerializeObject(_expenseClient.FilterExpense(collaboratorID, projectID, oportunittyID, initialDate, finishDate, clientId));
        }

        public string FiltrarInterno(int projectID, int oportunittyID, DateTime? initialDate, DateTime? finishDate, int clientId)
        {
            if (GetSession.UserConnected.Collaborator.Id == 0)
                return JsonConvert.SerializeObject(null);
            return JsonConvert.SerializeObject(_expenseClient.FilterExpense(GetSession.UserConnected.Collaborator.Id, projectID, oportunittyID, initialDate, finishDate, clientId));
        }
        #endregion

        private void LoadViewBags()
        {
            this.GetSession = new GetSession();
            ViewBag.UserName = GetSession.UserConnected.Name;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.Perfil = GetSession.UserConnected;
        }
    }
}