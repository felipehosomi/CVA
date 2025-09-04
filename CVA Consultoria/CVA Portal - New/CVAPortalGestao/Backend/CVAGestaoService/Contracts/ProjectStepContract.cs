using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class ProjectStepContract
    {
        private ProjectStepBLL _projectStepBLL { get; set; }

        public ProjectStepContract()
        {
            this._projectStepBLL = new ProjectStepBLL();
        }

        public List<StepModel> Get_ProjectSteps(int id, int user)
        {
            return _projectStepBLL.Get_ProjectSteps(id, user);
        }

        public MessageModel Save(ProjectStepModel projectStep)
        {
            return _projectStepBLL.Save(projectStep);
        }

        public List<ProjectStepModel> Get(int isProject)
        {
            return _projectStepBLL.Get(isProject);
        }

        public List<StatusModel> GetSpecificStatus()
        {
            return _projectStepBLL.GetSpecificStatus();
        }

        public List<ProjectStepModel> GetAll()
        {
            return _projectStepBLL.Get();
        }

        public ProjectStepModel Get_ByID(int id)
        {
            return _projectStepBLL.Get_ByID(id);
        }
    }
}