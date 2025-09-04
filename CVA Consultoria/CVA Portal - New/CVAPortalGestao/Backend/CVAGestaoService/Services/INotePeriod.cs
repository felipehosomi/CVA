using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface INotePeriod
    {
        [OperationContract]
        List<NotePeriod> GetAllPeriods();

        [OperationContract]
        NotePeriod GetPeriod(int year, int month);

        [OperationContract]
        void AddPeriod(int year, int month);

        [OperationContract]
        void DeletePeriod(int year, int month);
    }
}
