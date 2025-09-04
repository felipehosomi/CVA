using BLL.Classes;
using MODEL.Classes;
using System;
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

        public List<AMSTicketModel> GetTicketsByProjectAndDate(int clientId, DateTime date)
        {
            return AMSTicketBLL.GetTicketsByProject(clientId, date);
        }

        public List<AMSTicketModel> GetTicketsByProjectAndDateAndTicket(int clientId, DateTime date, int numTicket)
        {
            return AMSTicketBLL.GetTicketsByProject(clientId, date, numTicket);
        }
    }
}