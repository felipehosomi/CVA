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
    [CvaAuthorize("Cliente")]
    public class ClienteController : Controller
    {
        #region Instances
        private CVAGestaoService.ClientClient _clientService { get; set; }
        private CVAGestaoService.UfClient _ufClient { get; set; }
        private CVAGestaoService.ExpenseTypeClient _expenseTypeClient { get; set; }
        private GetSession GetSession = null;
        #endregion

        public ClienteController()
        {
            this._clientService = new CVAGestaoService.ClientClient();
            this._ufClient = new CVAGestaoService.UfClient();
            this._expenseTypeClient = new CVAGestaoService.ExpenseTypeClient();
            LoadViewBags();
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        public string Salvar(ClientModel client)
        {
            client.User = new UserModel
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_clientService.SaveClient(client));
        }

        public string GetSpecificStatus()
        {
            return JsonConvert.SerializeObject(_clientService.GetClientStatus());
        }

        public string GetUf()
        {
            return JsonConvert.SerializeObject(_ufClient.GetUf());
        }


        public string LoadCombo()
        {
            return JsonConvert.SerializeObject(_clientService.LoadCombo_Client());
        }


     
        public string GetExpenseTypes()
        {
            return JsonConvert.SerializeObject(_expenseTypeClient.GetExpenseTypes());
        }

        public ActionResult Pesquisar()
        {
            return View();
        }

        public ActionResult Editar(int clienteId)
        {
            try
            {
                return View("Cadastrar", _clientService.GetClientBy_ID(clienteId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Search(string name)
        {
            return JsonConvert.SerializeObject(_clientService.Client_Search(name));
        }

        public ActionResult RelatorioClientes(string name, int reportType)
        {
            ClientModel[] clientes = null;

            clientes = _clientService.Client_Search(name);
            int clientId = 0;

            var clienteDataSet = new ClienteDataSet();
            foreach (var cli in clientes)
            {
                if (cli.Id != clientId)
                {
                    clienteDataSet.dtCliente.AdddtClienteRow(
                        cli.Id.ToString()
                        , ""
                        , cli.Name
                        , cli.CNPJ
                        , cli.Status.Id.ToString()
                        , " "
                        , ""
                    );
                    clientId = cli.Id;
                }
            }

            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\ClientReport\ClienteReport.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ClienteDataSet", (DataTable)clienteDataSet.dtCliente));

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

        public ActionResult RelatorioIndividual(int clienteId)
        {
            ClientModel cliente = _clientService.GetClientBy_ID(clienteId);

            var clienteInfoDataSet = new ClienteInfoDataSet();

            clienteInfoDataSet.dtClienteInfo.AdddtClienteInfoRow(
                        cliente.Id.ToString()
                        , ""
                        , cliente.Name
                        , cliente.CNPJ
                        , cliente.Status.Id.ToString()
                        , ""
                        , ""
                    );

            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\ClientReport\ClienteInfoReport.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ClienteInfoDataSet", (DataTable)clienteInfoDataSet.dtClienteInfo));

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
