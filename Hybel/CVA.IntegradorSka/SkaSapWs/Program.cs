using System.IO;
using Topshelf;

namespace SkaSapWs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));
                //x.UseLog4Net("log4net.config");
                x.Service<TimerService>(s =>
                {
                    s.ConstructUsing(name => new TimerService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.EnableServiceRecovery(r =>
                {
                    //r.OnCrashOnly();
                    r.RestartService(0);
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.SetDescription("[CVA] Integrador SKA X SAP");
                x.SetDisplayName("CVA.IntegradorSkaSap");
                x.SetServiceName("CVA.IntegradorSkaSap");
            });
        }
    }
}
