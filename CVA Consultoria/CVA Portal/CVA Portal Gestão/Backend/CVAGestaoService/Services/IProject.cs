using MODEL.Classes;

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IProject
    {
        [OperationContract]
        List<StatusModel> ProjectStatus();

        [OperationContract]
        List<ProjectModel> GetActiveProjects();

     
        [OperationContract]
        MessageModel Remove_Step(StepModel model);


        [OperationContract]
        List<ProjectModel> Filter_Projects(int clientId, string code);

        [OperationContract]
        ProjectModel Get(int id);

        [OperationContract]
        List<ProjectModel> Get_ByUser(int id);

        [OperationContract]
        List<StepModel> Project_GetSteps();

        [OperationContract]
        List<ProjectModel> LoadCombo_Project();

        [OperationContract]
        List<ProjectModel> Project_Get_All();

        [OperationContract]
        List<ProjectModel> Project_Get_ByClient(int id);

        [OperationContract]
        List<ProjectModel> Project_Get_ByCollaborator(int id);

        [OperationContract]
        List<ProjectModel> Project_Get_ByClientAndCollaborator(int idClient, int idCollaborator);

 

        [OperationContract]
        MessageModel Project_Save(ProjectModel model);

        [OperationContract]
        string Project_Generate_Number(int id);


        [OperationContract]
        ProjectModel Project_Get_StatusReportParcial(int id);
    }
}
