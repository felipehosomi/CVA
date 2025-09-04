using BLL.Classes;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class ProjectContract
    {
        private ProjectBLL _projectBLL { get; set; }
        public ProjectContract()
        {
            this._projectBLL = new ProjectBLL();
        }



        public MessageModel Remove_Step(StepModel model)
        {
            return _projectBLL.Remove_Step(model);
        }

        public List<StatusModel> GetSpecificStatus()
        {
            return null;
        }
        
        public List<ProjectModel> GetActiveProjects()
        {
            return _projectBLL.GetActiveProjects();
        }

        public List<ProjectModel> LoadCombo_Project()
        {
            return _projectBLL.LoadCombo_Project();
        }
        public List<ProjectModel> Get_ByClient(int id)
        {
            return _projectBLL.Get_ByClient(id);
        }

        public List<ProjectModel> Get_ByCollaborator(int id)
        {
            return _projectBLL.Get_ByCollaborator(id);
        }

        public List<ProjectModel> Get_ByClientAndCollaborator(int idClient, int idCollaborator)
        {
            return _projectBLL.Get_ByClientAndCollaborator(idClient, idCollaborator);
        }

        public List<StepModel> GetSteps()
        {
            return _projectBLL.GetSteps();
        }

        public List<ProjectModel> Filter_Projects(int clientId, string code)
        {
            return _projectBLL.Filter_Projects(clientId, code);
        }
        public ProjectModel Get(int id)
        {
            return _projectBLL.Get(id);
        }

        public List<ProjectModel> Get_ByUser(int id)
        {
            return _projectBLL.Get_ByUser(id);
        }

        public string Generate_Number(int id)
        {
            return _projectBLL.Generate_Number(id);
        }

        public List<ProjectModel> Get_All()
        {
            return _projectBLL.Get_All();
        }

        public MessageModel Project_Save(ProjectModel model)
        {
            return _projectBLL.Save(model);
        }


        public ProjectModel Get_StatusReportParcial(int id)
        {
            return _projectBLL.Get_StatusReportParcial(id);
        }
        
        public MessageModel RemoveSpecialtyRule(int idProject, int idSpecialty, int idCollaborator)
        {
            return _projectBLL.Upd_ProjectResourcesSpecialties(idProject, idSpecialty, idCollaborator);
        }
    }
}
