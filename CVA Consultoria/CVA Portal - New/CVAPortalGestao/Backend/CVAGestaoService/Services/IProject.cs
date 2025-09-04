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
        MessageModel Remove_Step(StepModel model);


        [OperationContract]
        List<ProjectModel> Project_Search(ProjectFilterModel model);

        [OperationContract]
        ProjectModel Get(int id);

        [OperationContract]
        List<ProjectModel> Get_ByUser(int id);

        [OperationContract]
        List<StepModel> Project_GetSteps();

        [OperationContract]
        List<ProjectModel> LoadCombo_Project();




        [OperationContract]
        MessageModel Project_Save(ProjectModel model);




        [OperationContract]
        MessageModel Project_Add_ColToProject(int ProjectId, int ColaboradorId);

        [OperationContract]
        MessageModel Project_Remove_ColToProject(int ProjectId, int ColaboradorId);


        [OperationContract]
        string Project_Generate_Number(int id);


        [OperationContract]
        ProjectModel Project_Get_StatusReportParcial(int id);
    }
}
