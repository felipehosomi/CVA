using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class ClientContract
    {
        private ClientBLL _clientBLL { get; set; }
        public ClientContract()
        {
            this._clientBLL = new ClientBLL();
        }

        public MessageModel Save(ClientModel client)
        {
            return _clientBLL.Save(client);
        }

        public List<StatusModel> GetSpecificStatus()
        {
            return _clientBLL.GetSpecificStatus();
        }

        //public List<ClientModel> Get()
        //{
        //    return _clientBLL.Get();
        //}

        public List<ClientModel> LoadCombo_Client()
        {
            return _clientBLL.LoadCombo();
        }

        public List<ClientModel> Search(string name)
        {
            return _clientBLL.Search(name);
        }


        public List<ContactModel> GetContacts(int id)
        {
            return _clientBLL.GetContacts(id);
        }

        public ClientModel GetClientBy_ID(int clientId)
        {
            return _clientBLL.Get(clientId);
        }
    }
}