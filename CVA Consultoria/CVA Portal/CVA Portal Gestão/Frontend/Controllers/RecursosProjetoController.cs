using CVAGestaoLayout.Helper;
using MODEL.Classes;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    [CvaAuthorize("Recursos")]
    public class RecursosProjetoController : Controller
    {
        private CVAGestaoService.CollaboratorClient _collaboratorClient { get; set; }
        private CVAGestaoService.ProjectClient _projectClient { get; set; }
        private GetSession GetSession = null;

        public RecursosProjetoController()
        {
            this._collaboratorClient = new CVAGestaoService.CollaboratorClient();
            this._projectClient = new CVAGestaoService.ProjectClient();
            LoadViewBags();
        }

        public ActionResult Adicionar()
        {
            return View();
        }

        public string GetProjetoPorColaborador(int colaboradorId)
        {
            try
            {
                return JsonConvert.SerializeObject(_projectClient.Get_ByUser(colaboradorId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetProjetos()
        {
            return JsonConvert.SerializeObject(_projectClient.GetActiveProjects());
        }

        public string Salvar(ProjectModel project)
        {
            throw new NotImplementedException();
            //project.User = GetSession.UserConnected.Id;
            //return JsonConvert.SerializeObject(_projectClient.AllocatedCollaborator(project));
        }

        public string GetColaboradoresPorProjeto(int projectId)
        {

            throw new NotImplementedException();
            //return JsonConvert.SerializeObject(_projectClient.GetCollaboratorsAllocatedInProject(projectId));
        }
        
        public string Inactive(ProjectModel project)
        {

            throw new NotImplementedException();
            //return JsonConvert.SerializeObject(_projectClient.InactiveAllocated(project.Id, project.Collaborators[0].Id, GetSession.UserConnected.Id));
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
