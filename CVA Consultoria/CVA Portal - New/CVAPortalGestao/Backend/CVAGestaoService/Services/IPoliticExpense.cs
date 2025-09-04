using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IPoliticExpense
    {
        [OperationContract]
        List<PoliticExpenseModel> GetPoliticExpenseByProject(int projectId, int user);
    }
}