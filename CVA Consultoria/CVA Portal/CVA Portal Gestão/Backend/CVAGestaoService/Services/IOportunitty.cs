using MODEL.Classes;

using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IOpportunity
    {
        [OperationContract]
        MessageModel SaveOportunitty(OpportunityModel oportunitty);

        [OperationContract]
        MessageModel ConvertOportunittyToProject(OpportunityModel oportunitty);

        [OperationContract]
        List<OpportunityModel> GetOpportunities();

        [OperationContract]
        OpportunityModel GetOportunittyById(int id);

        [OperationContract]
        List<StepModel> Oportunitty_GetSteps();

        [OperationContract]
        List<OpportunityModel> Search(string code, int clientId);

        [OperationContract]
        string NewCodeGenerator();
    }
}