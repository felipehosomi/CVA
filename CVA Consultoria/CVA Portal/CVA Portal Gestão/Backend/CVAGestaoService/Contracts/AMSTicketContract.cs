using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class AMSTicketContract
    {
        public AMSTicketBLL AMSTicketBLL { get; set; }
        public AMSTicketContract()
        {
            this.AMSTicketBLL = new AMSTicketBLL();
        }

        public List<AMSTicketModel> GetTicketsByProject(int clientId)
        {
            return AMSTicketBLL.GetTicketsByProject(clientId);
        }
    }
}