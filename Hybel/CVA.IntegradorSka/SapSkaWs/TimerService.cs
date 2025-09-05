using System;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Quartz.Impl;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Specialized;
using SapSkaWs.Data;

namespace SapSkaWs
{
    public class TimerService
    {
        //private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CancellationTokenSource _cancellatonTokenSource;
        private readonly Task _task;
        static readonly object _object = new object();

        public TimerService()
        {
            _cancellatonTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellatonTokenSource.Token;
            _task = new Task(DoWork, cancellationToken);
        }

        private void DoWork()
        {
            while (!_cancellatonTokenSource.IsCancellationRequested)
            {
                try
                {
                    //Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter
                    //{
                    //    Level = Common.Logging.LogLevel.Warn
                    //};

                    var scheduler = StdSchedulerFactory.GetDefaultScheduler();

                    scheduler.Start();

                    var rand = new Random(999999999);
                    var sjob = $"job_{rand.Next()}";
                    var sgroup = $"group_{rand.Next()}";
                    var strigger = $"trigger_{rand.Next()}";

                    var job = JobBuilder.Create<TimerJob>()
                        .WithIdentity(sjob, sgroup)
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity(strigger, sgroup)
                        .StartNow()
                        .WithSimpleSchedule(x =>
                        {
                            x.WithIntervalInMinutes(5);
                        })
                        .Build();

                    scheduler.ScheduleJob(job, trigger);
                }
                catch (SchedulerException se)
                {
                    if(!se.Message.Contains("Unable to store Job"))
                        Logger.Log.Error(GetInnerException(se), se);
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("Unable to store Job"))
                        Logger.Log.Fatal(GetInnerException(ex), ex);
                }
            }
        }

        private static string GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
                return $"{ex.InnerException.Message} > {GetInnerException(ex.InnerException)} ";
            return string.Empty;
        }

        public void Start()
        {
            Logger.Log.Info("CVA.IntegradorSapSka is started.");
            _task.Start();
        }

        public void Stop()
        {
            Logger.Log.Info("CVA.IntegradorSapSka is stopped.");
            _cancellatonTokenSource.Cancel();
            _task.Wait();
        }
    }
}
