using CVA.NF.Import.BLL;
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

namespace CVA.NF.Import.Service
{
    public partial class Service : ServiceBase
    {
        public static string ServiceId = "CVANFImport";
        public static string DisplayName = "CVA - Importação Notas Fiscais";

        public Service()
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
            foreach (Process proc in Process.GetProcessesByName("CVA.NF.Import.Service"))
            {
                proc.Kill();
            }
        }
    }
}
