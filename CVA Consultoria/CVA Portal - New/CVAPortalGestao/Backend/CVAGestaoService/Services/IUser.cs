
using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IUser
    {
        [OperationContract]
        UserModel GetUser(int id);

        [OperationContract]
        MessageModel SaveUser(UserModel user);

        [OperationContract]
        List<StatusModel> GetUserStatus();

        [OperationContract]
        List<UserModel> GetUsers();

        [OperationContract]
        MessageModel UpdateUser_ByUser(UserModel user);
    }
}