using MODEL.Classes;

using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IClient
    {
        [OperationContract]
        MessageModel SaveClient(ClientModel client);

        [OperationContract]
        List<StatusModel> GetClientStatus();

        //[OperationContract]
        //List<ClientModel> GetClient();

        [OperationContract]
        List<ClientModel> LoadCombo_Client();


        [OperationContract]
        List<ClientModel> Client_Search(string name);


        [OperationContract]
        List<ContactModel> GetClientContacts(int id);

        [OperationContract]
        ClientModel GetClientBy_ID(int clientId);
       }
}