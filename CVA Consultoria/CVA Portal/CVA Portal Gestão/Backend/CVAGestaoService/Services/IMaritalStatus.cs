using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IMaritalStatus
    {
        [OperationContract]
        List<MaritalStatusModel> GetMaritalStatus();
    }
}