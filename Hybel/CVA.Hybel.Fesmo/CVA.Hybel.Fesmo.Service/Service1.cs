using CVA.Hybel.Fesmo.BLL;
using System.ServiceProcess;
using System.Threading;

namespace CVA.Hybel.Fesmo.Service
{
    public partial class Service1 : ServiceBase
    {

        public static string ServiceId = "CVA.Hybek.Fesmo";
        public static string DisplayName = "CVA - Integração B1 X Fesmo";

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
