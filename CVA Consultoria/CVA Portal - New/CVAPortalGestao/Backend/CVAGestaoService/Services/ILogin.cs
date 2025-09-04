
using MODEL.Classes;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface ILogin
    {
        [OperationContract]
        UserModel LogIn(string email, string password);

        [OperationContract]
        MessageModel LogOff(int userId);

        [OperationContract]
        UserModel FirstAccess(UserModel user);

    }
}
