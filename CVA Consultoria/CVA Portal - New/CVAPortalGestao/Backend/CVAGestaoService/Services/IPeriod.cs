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
          [OperationContract]
        MessageModel OpenPeriod();
     
        [OperationContract]
        MessageModel SetStatusSubPeriod(int periodId, int statusId);

        [OperationContract]
        MessageModel SetStatusSubPeriodList(string periodIdList, int statusId);
    }
}