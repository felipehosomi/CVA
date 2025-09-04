using CVA.AddOn.Common.Controllers;
using CVA.View.EmailAtividade.MODEL;
using CVA.View.EmailAtividade.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAtividade.DAO
{
    public class EmailServerDAO
    {
        private CrudController CrudController { get; }

        public EmailServerDAO()
        {
            CrudController = new CrudController();
        }

        public EmailConfigModel GetEmail()
        {
            EmailConfigModel model = CrudController.FillModelAccordingToSql<EmailConfigModel>(Query.EmailConfig_Get);
            return model;
        }
    }
}
