using Topshelf;

namespace CVA_Rep_Timer
{
    public class Program
    {
        private static void Main()
        {
            HostFactory.Run(x =>
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.config"));
                x.UseLog4Net("log4net.config");
                x.Service<TimerService>(s =>
                {
                    s.ConstructUsing(name => new TimerService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.EnableServiceRecovery(r =>
                {
                    r.OnCrashOnly();
                    r.RestartService(0);
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.SetDescription("[CVA] Replication Timer Service");
                x.SetDisplayName("CVA_Rep_Timer");
                x.SetServiceName("CVA_Rep_Timer");
                
            });
        }
    }
}