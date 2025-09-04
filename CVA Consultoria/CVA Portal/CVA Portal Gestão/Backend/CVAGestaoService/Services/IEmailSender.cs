
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IEmailSender
    {
        [OperationContract]
        MessageModel SendEmail(string emailAddress);
    }
}
