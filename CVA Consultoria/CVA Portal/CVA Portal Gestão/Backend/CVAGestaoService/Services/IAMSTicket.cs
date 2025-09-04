using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IAMSTicket
    {
        [OperationContract]
        List<AMSTicketModel> GetTicketsByProject(int projectId);
    }
}