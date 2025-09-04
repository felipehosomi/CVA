using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IProjectStep
    {
        [OperationContract]
        MessageModel SaveProjectStep(ProjectStepModel projectStep);

        [OperationContract]
        List<ProjectStepModel> GetProjectStep(int isProject);

        [OperationContract]
        List<StatusModel> GetProjectStepStatus();

        [OperationContract]
        List<StepModel> Get_ProjectSteps(int id);

        [OperationContract]
        List<ProjectStepModel> GetAllProjectStep();

        [OperationContract]
        ProjectStepModel GetProjectStep_ByID(int id);
    }
}