using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IStatus
    {
        [OperationContract]
        List<StatusModel> GetStatus(int objectId);
    }
}