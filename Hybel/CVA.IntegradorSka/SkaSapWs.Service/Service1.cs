using SkaSapWs.BLL;
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

namespace SkaSapWs.Service
{
    public partial class Service1 : ServiceBase
    {
        public static string ServiceId = "CVA.SKAxSAP";
        public static string DisplayName = "CVA - Integração SKA -> SAP";

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
