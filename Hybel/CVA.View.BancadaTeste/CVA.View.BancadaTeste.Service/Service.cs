using CVA.View.BancadaTeste.BLL;
using System.ServiceProcess;
using System.Threading;

namespace CVA.View.BancadaTeste.Service
{
    public partial class Service : ServiceBase
    {
        public static string ServiceId = "CVA.BancadaTeste";
        public static string DisplayName = "CVA - Arquivo Bancada Teste";

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
        }
    }
}
