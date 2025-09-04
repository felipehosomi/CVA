
using System.Collections.Generic;
using System.ServiceModel;
using MODEL.Classes;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IProjectType
    {
        [OperationContract]
        ProjectTypeModel ProjectType_Get(int id);

        [OperationContract]
        List<ProjectTypeModel> ProjectType_Get_All();

        [OperationContract]
        MessageModel ProjectType_Insert(ProjectTypeModel model);

        [OperationContract]
        MessageModel ProjectType_Update(ProjectTypeModel model);

        [OperationContract]
        MessageModel ProjectType_Remove(int id);
    }
}