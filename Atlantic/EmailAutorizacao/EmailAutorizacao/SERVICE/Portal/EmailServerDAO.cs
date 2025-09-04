using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailAutorizacao.HELPER;
using EmailAutorizacao.MODEL;
using EmailAutorizacao.SERVICE.Portal.Resource;

namespace EmailAutorizacao.SERVICE.Portal
{
    public class EmailServerDAO
    {
        public static EmailServerModel GetEmailServer()
        {
            var SqlHelper = new SqlHelper();
            var model = SqlHelper.FillModel<EmailServerModel>(Query.EmailServer_Get);
            return model;
        }
    }
}
