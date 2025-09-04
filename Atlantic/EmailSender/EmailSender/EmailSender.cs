using EmailSender.BLL;
using Quartz;
using Quartz.Impl;
using System;
using System.Diagnostics;


namespace EmailSender
{
    public class EmailSender :IJob
    {
        static ISchedulerFactory schedFact = new StdSchedulerFactory();
        static IScheduler sched;

        static IJobDetail check = JobBuilder.Create<EmailSender>()
                .WithIdentity("Fluxo", "1")
                .Build();

        static ITrigger trigger = TriggerBuilder.Create()
          .WithIdentity("Gatilho", "1")
          .StartNow()
          .WithSimpleSchedule(x => x
              .WithIntervalInSeconds(15)
              .RepeatForever())
          .Build();

        public static EmailSenderBLL _emailSenderBLL; 

        public static void Main(string[] args)
        {
            _emailSenderBLL = new EmailSenderBLL();
            sched = schedFact.GetScheduler();
            sched.Start();
            sched.ScheduleJob(check, trigger);
            
        }

        public void Execute(IJobExecutionContext context)
        {
            _emailSenderBLL.CheckREP();
            _emailSenderBLL.CheckCON();
        }
    }
}

