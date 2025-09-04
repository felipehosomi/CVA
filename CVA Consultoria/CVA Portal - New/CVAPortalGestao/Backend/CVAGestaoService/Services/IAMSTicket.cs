using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IAMSTicket
    {
        [OperationContract]
        List<AMSTicketModel> GetTicketsByProject(int projectId);

        [OperationContract]
        List<AMSTicketModel> GetTicketsByProjectAndDate(int projectId, DateTime date);

        [OperationContract]
        List<AMSTicketModel> GetTicketsByProjectAndDateAndTicket(int projectId, DateTime date, int numTicket);
    }
}