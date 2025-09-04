using MODEL.Classes;

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IStatusReport
    {
        [OperationContract]
        string[] StatusReport_Get_ParcialHours(int id, DateTime data);

        [OperationContract]
        MessageModel StatusReport_Save(StatusReportModel model);

        [OperationContract]
        List<StatusReportModel> StatusReport_Get_All(int id);
    }
}