using CVAGestaoLayout.Helper;
using CVAGestaoLayout.Report.CollaboratorReport;
using Microsoft.Reporting.WebForms;
using MODEL.Classes;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CVAGestaoLayout.Controllers
{
    public class ColaboradorController : Controller
    {
        #region Atributos
        private CVAGestaoService.SpecialtyClient _specialtyClient { get; set; }
        private CVAGestaoService.CollaboratorClient _collaboratorClient { get; set; }
        private CVAGestaoService.GenreClient _genreClient { get; set; }
        private CVAGestaoService.MaritalStatusClient _maritalStatusClient { get; set; }
        private CVAGestaoService.UfClient UfClient { get; set; }
        private GetSession GetSession = null;
        #endregion

        #region Construtor
        public ColaboradorController()
        {
            this._specialtyClient = new CVAGestaoService.SpecialtyClient();
            this._collaboratorClient = new CVAGestaoService.CollaboratorClient();
            this._genreClient = new CVAGestaoService.GenreClient();
            this._maritalStatusClient = new CVAGestaoService.MaritalStatusClient();
            this.UfClient = new CVAGestaoService.UfClient();

            LoadViewBags();
        }
        #endregion


        public ActionResult Cadastrar()
        {
            ViewBag.FinancialAccess = GetSession.UserConnected.Profile.FinancialAccess;
            return View();
        }

        public string Insert(CollaboratorModel model)
        {
            model.User = new UserModel
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_collaboratorClient.Collaborator_Insert(model));
        }
        public string Update(CollaboratorModel model)
        {
            ViewBag.FinancialAccess = GetSession.UserConnected.Profile.FinancialAccess;
            model.User = new UserModel
            {
                Id = GetSession.UserConnected.Id
            };
            return JsonConvert.SerializeObject(_collaboratorClient.Collaborator_Update(model));
        }

        public string Get_Specialties()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.Collaborator_Get_Specialties(GetSession.UserConnected.Id));
        }

        public string Get_NotUser()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.Collaborator_Get_NotUser());
        }

        public string Remove_Specialty(SpecialtyModel model)
        {
            var idUser = GetSession.UserConnected.Id;
            return JsonConvert.SerializeObject(_collaboratorClient.Collaborator_Remove_Specialty(model, idUser));
        }

        public string GetSpecialtiesForCollaborator(int id)
        {
            return JsonConvert.SerializeObject(_collaboratorClient.GetSpecialtiesForCollaborator(id));
        }

        public string GetSpecificStatus()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.CollaboratorStatus());
        }




        public string GetGeneros()
        {
            return JsonConvert.SerializeObject(_genreClient.GetGenre());
        }

        public string GetEstadoCivil()
        {
            return JsonConvert.SerializeObject(_maritalStatusClient.GetMaritalStatus());
        }

        public string GetUf()
        {
            return JsonConvert.SerializeObject(UfClient.GetUf());
        }

        public string GetTypes()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.GetCollaboratorTypes());
        }

        public string CrateLink(CollaboratorModel colaborador)
        {
            return "";
        }

        [HttpGet]
        public ActionResult CadastroColaboradorExterno(string nomeColaborador)
        {
            return View();
        }

        public ActionResult Pesquisar()
        {
            return View(/*_collaboratorClient.GetCollaborator()*/);
        }

        [HttpPost]
        public ActionResult UploadExcel()
        {
            try
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    string folderPath = @"C:\CVA Consultoria\CVA Portal de Gestão\Colaboradores";
                    string fileName = "Dados_Colaborador.xlsx";

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    
                    if (System.IO.File.Exists(Path.Combine(folderPath, fileName)))
                    {
                        System.IO.File.Delete(Path.Combine(folderPath, fileName));
                    }

                    Request.Files[0].SaveAs(Path.Combine(folderPath, fileName));

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                return Json("Selecione um arquivo", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Erro geral ao salvar arquivo no servidor: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public string ImportarDadosColaborador()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.ImportarDadosColaborador());
        }


        public string Get_All()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.GetCollaborator());
        }
        public string LoadCombo()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.LoadCombo_Collaborator());
        }

        public string Get_Active()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.Collaborator_Get_Active());
        }

        public ActionResult Get(int id)
        {
            ViewBag.FinancialAccess = GetSession.UserConnected.Profile.FinancialAccess;
            return View("Cadastrar", _collaboratorClient.Collaborator_Get(id));
        }

        public string Filter_Collaborators(string nome, string cpf, string cnpj, int especialidade, int status)
        {
            return JsonConvert.SerializeObject(_collaboratorClient.GetCollaboratorByFilters(nome, cpf, cnpj, 0, especialidade, status));
        }

        public string Get_Managers()
        {
            return JsonConvert.SerializeObject(_collaboratorClient.GetPMs());
        }

        public string Get_CollaboratorBySpecialty(int id)
        {
            return JsonConvert.SerializeObject(_collaboratorClient.Get_CollaboratorBySpecialty(id));
        }
        
   

        public ActionResult RelatorioColaboradores(string nome, string cpf, string cnpj, int especialidade, int status, int reportType)
        {
            CollaboratorModel[] colaboradores = null;

            colaboradores = _collaboratorClient.GetCollaboratorByFilters(nome, cpf, cnpj, 0, especialidade, status);
            //int collaboratorId = 0;

            var colaboradorDataSet = new ColaboradorDataSet();
            foreach (var col in colaboradores)
            {
                // if (col.Id != clientId)
                // {

                colaboradorDataSet.dtColaborador.AdddtColaboradorRow(
                        col.Id.ToString()
                        , col.Nome
                        , col.CPF
                        , col.CNPJ
                        , col.Email
                        , "Setor"
                        , col.Especialidades != null && col.Especialidades.Count > 0 ? col.Especialidades[0].Name : ""
                        , col.Status.Id.ToString()
                    );
                //     clientId = cli.Id;
                // }
            }

            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\CollaboratorReport\ColaboradorReport.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ColaboradorDataSet", (DataTable)colaboradorDataSet.dtColaborador));

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


        private void LoadViewBags()
        {
            this.GetSession = new GetSession();
            ViewBag.UserName = GetSession.UserConnected.Name;
            ViewBag.Email = GetSession.UserConnected.Email;
            ViewBag.Perfil = GetSession.UserConnected;
        }
    }
}
