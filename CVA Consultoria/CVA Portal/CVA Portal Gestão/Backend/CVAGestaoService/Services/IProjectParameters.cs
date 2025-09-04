using MODEL.Classes;

using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IProjectParameters
    {
        [OperationContract]
        List<ProjectParametersModel> ProjectParameters_Get_All();

        [OperationContract]
        MessageModel ProjectParameters_Save(ProjectParametersModel model);
    }
}
