using SapKsaWs.BLL;
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

namespace SapSkaWs.Service.Teste
{
    public partial class Service1 : ServiceBase
    {
        public static string ServiceId = "CVA.Service.SAPxSKA.TESTE";
        public static string DisplayName = "CVA - SAP x SKA V2.1 - TESTE";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread thread = new Thread(new ThreadStart(ServiceBLL.ExecuteService));
            thread.Start();
        }

        protected override void OnStop()
        {
        }
    }
}
