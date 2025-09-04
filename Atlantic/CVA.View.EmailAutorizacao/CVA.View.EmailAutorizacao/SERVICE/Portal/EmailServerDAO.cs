using CVA.View.EmailAutorizacao.HELPER;
using CVA.View.EmailAutorizacao.MODEL;
using CVA.View.EmailAutorizacao.SERVICE.Portal.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAutorizacao.SERVICE.Portal
{
    public class EmailServerDAO
    {
        private SqlHelper SqlHelper { get; }

        public EmailServerDAO()
        {
            SqlHelper = new SqlHelper();
        }

        public EmailServerModel GetEmailServer()
        {
            EmailServerModel model = SqlHelper.FillModel<EmailServerModel>(Query.EmailServer_Get);
            return model;
        }
    }
}
