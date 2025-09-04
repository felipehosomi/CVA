using CVAGestaoLayout.Helper;
using CVAGestaoLayout.Report.ExpenseReport;
using Microsoft.Reporting.WebForms;
using MODEL.Classes;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CVAGestaoLayout.Controllers
{
    [CvaAuthorize("Despesa")]
    public class DespesaController : Controller
    {
        #region Atributos
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
            var model = _expenseClient.Expense_Get(expenseId);
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

        public string GetProjects()
        {
            return JsonConvert.SerializeObject(_projectClient.Get_ByUser(GetSession.UserConnected.Id));
        }

        public string GetClients()
        {
            return JsonConvert.SerializeObject(_clientService.LoadCombo_Client());
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

                /* Produção */
                string directory = $"c:\\CVA Consultoria\\CVA Portal de Gestão\\Despesas\\{DateTime.Today.ToString("MM-yyyy")}\\{userModel.Name}\\{expense.Projeto.Cliente.Name}\\{DateTime.Today.ToString("dd-MM-yyyy")}";

                /* Teste */
                //string directory = $"c:\\CVA Consultoria\\CVA Portal de Gestão - Teste\\Despesas\\{DateTime.Today.ToString("MM-yyyy")}\\{userModel.Name}\\{expense.Projeto.Cliente.Name}\\{DateTime.Today.ToString("dd-MM-yyyy")}";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                else
                {
                    if (System.IO.File.Exists($"{directory}\\{file.FileName}"))
                    {
                        MessageModel message = new MessageModel();
                        message.Error = new ErrorMessage();
                        message.Error.Code = 99;
                        message.Error.Message = "Já existe um arquivo com o nome informado, por favor altere o nome do arquivo e tente novamente";
                        return JsonConvert.SerializeObject(message);
                    }
                }

                file.SaveAs($"{directory}\\{file.FileName}");
            }

            expense.User = new UserModel
            {
                Id = GetSession.UserConnected.Id,
                Name = this._userClient.GetUser(GetSession.UserConnected.Id).Name
            };
            return JsonConvert.SerializeObject(_expenseClient.Expense_Save(expense));
        }

        public string Remove(int id)
        {
            return JsonConvert.SerializeObject(_expenseClient.Expense_Remove(id));
        }

        public ActionResult GetAttachedFile(DateTime date, int projectId, string filename)
        {
            try
            {
                if (!String.IsNullOrEmpty(filename))
                {
                    UserModel userModel = this._userClient.GetUser(GetSession.UserConnected.Id);
                    ProjectModel projModel = this._projectClient.Get(projectId);
                    string directory = $"c:\\CVA Consultoria\\Despesas\\{date.ToString("MM-yyyy")}\\{userModel.Name}\\{projModel.Cliente.Name}\\{date.ToString("dd-MM-yyyy")}";

                    FileStream fs = new FileStream(directory + "\\" + filename, FileMode.Open, FileAccess.Read);
                    return File(fs, "application/" + filename.Split('.')[0], filename);
                }
                return new HttpNotFoundResult("Arquivo não encontrado no servidor.");
            }
            catch (Exception ex)
            {
                return new HttpNotFoundResult("Erro geral: " + ex.Message);
            }
        }

        public string Filtrar(int col, int cli, int prj, DateTime? de, DateTime? ate, bool interno)
        {
            if (interno)
                return JsonConvert.SerializeObject(_expenseClient.Expense_Search(GetSession.UserConnected.Collaborator.Id, cli, prj, de, ate));
            else
                return JsonConvert.SerializeObject(_expenseClient.Expense_Search(col, cli, prj, de, ate));
        }

        public ActionResult Extrair_Relatorio(int col, int cli, int prj, DateTime? de, DateTime? ate, string tipo, bool interno)
        {
            #region Variáveis
            ExpenseModel[] despesas = null;
            var Total = 0.0;
            var despesaDataSet = new ExpenseDataSet();
            var viewer = new ReportViewer();
            Warning[] warnings;
            string mimeType;
            string[] streamids;
            string encoding;
            string filenameExtension;
            byte[] bytes;
            #endregion

            if (interno)
                despesas = _expenseClient.Expense_Search(GetSession.UserConnected.Collaborator.Id, cli, prj, de, ate);
            else
                despesas = _expenseClient.Expense_Search(col, cli, prj, de, ate);


            foreach (var item in despesas)
            {
                Total += Math.Round(Convert.ToDouble(item.ValorReembolso), 2);
            }

            foreach (var item in despesas)
            {             
                despesaDataSet.dtDespesa.AdddtDespesaRow(
                      item.Data.ToShortDateString()
                    , item.User.Name
                    , item.Projeto.Cliente.Name
                    , item.Projeto.Nome
                    , item.Projeto.ResponsavelDespesa
                    , item.Descricao
                    , item.TipoDespesa.Name
                    , item.NumNota
                    , "R$ " + item.ValorNota
                    , "R$ " + item.Projeto.Cliente.PoliticExpense[0].Value
                    , "R$ " + item.ValorReembolso
                    , "R$ " + Total.ToString()
                );
            }

            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\ExpenseReport\DespesaReport.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ExpenseDataSet", (DataTable)despesaDataSet.dtDespesa));

            viewer.SizeToReportContent = true;
            viewer.Width = Unit.Percentage(100);
            viewer.Height = Unit.Percentage(100);
            viewer.LocalReport.Refresh();



            if (tipo.Equals("PDF"))
                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            else
                bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            return new FileContentResult(bytes, mimeType);
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