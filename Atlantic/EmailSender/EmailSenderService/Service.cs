using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmailSenderService
{
    public partial class Service : ServiceBase
    {
        public static string ServiceId = "CVAEmailSenderService";
        public static string DisplayName = "CVA - Serviço de envio de e-mails";

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread thread = new Thread(new ThreadStart(ServiceHelper.ExecuteService));
            thread.Start();
        }

        protected override void OnStop()
        {
            foreach (Process proc in Process.GetProcessesByName("EmailSenderService"))
            {
                proc.Kill();
            }
        }
    }
}
