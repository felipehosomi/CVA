using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IPeriod
    {
        //[OperationContract]
        //MessageModel ClosingPeriod();

        [OperationContract]
        MessageModel OpenPeriod();

        [OperationContract]
        MessageModel SaveSubPeriod(SubPeriodModel model);

        [OperationContract]
        List<SubPeriodModel> GetSubPeriods(int? colId, int? clientId, int? projectId, DateTime? dateFrom, DateTime? dateTo);

        [OperationContract]
        MessageModel SetStatusSubPeriod(int periodId, int statusId);

        [OperationContract]
        MessageModel SetStatusSubPeriodList(string periodIdList, int statusId);
    }
}