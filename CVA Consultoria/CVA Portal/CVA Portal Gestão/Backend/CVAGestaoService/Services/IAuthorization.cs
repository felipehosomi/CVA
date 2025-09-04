using MODEL.Classes;

using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IAuthorization
    {
        [OperationContract]
        List<AuthorizedDayModel> Get_DiasAutorizados(int idCol);

        [OperationContract]
        MessageModel AddDiaAutorizado(AuthorizedDayModel model);

        [OperationContract]
        MessageModel RemoveDiaAutorizado(int id);

        [OperationContract]
        List<AuthorizedHoursModel> Get_HorasAutorizadas(int idCol);

        [OperationContract]
        MessageModel AddHorasAutorizadas(AuthorizedHoursModel model);

        [OperationContract]
        MessageModel RemoveHorasAutorizadas(int id);
        
        [OperationContract]
        List<HoursLimitModel> Get_LimiteHoras(int idCol);

        [OperationContract]
        MessageModel AddLimiteHoras(HoursLimitModel model);

        [OperationContract]
        MessageModel RemoveLimiteHoras(int id);       
    }
}