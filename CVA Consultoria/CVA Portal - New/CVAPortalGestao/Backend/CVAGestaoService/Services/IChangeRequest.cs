
using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IChangeRequest
    {
        [OperationContract]
        ChangeRequestModel ChangeRequest_Get(int id);

        [OperationContract]
        List<ChangeRequestModel> ChangeRequest_Get_for_Project(int id);

        [OperationContract]
        MessageModel ChangeRequest_Save(ChangeRequestModel model);
    }
}