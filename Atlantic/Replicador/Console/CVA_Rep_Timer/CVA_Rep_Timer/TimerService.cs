using System;
using System.Threading;
using System.Threading.Tasks;
using CVA_Rep_DAL;
using log4net;
using Quartz;
using Quartz.Impl;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace CVA_Rep_Timer
{
    public class TimerService
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(TimerService));

        private static UnitOfWork _oUnitOfWork;
        private static CVA_TIM _cvaTimer;
        private readonly CancellationTokenSource _cancellatonTokenSource;
        private readonly Task _task;
        static readonly object _object = new object();

        public TimerService()
        {
            //System.IO.File.AppendAllText("log.txt", $"{DateTime.Now} - Iniciando o serviço do Timer.{Environment.NewLine}");
            _cancellatonTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellatonTokenSource.Token;
            _task = new Task(DoWork, cancellationToken);
        }

        private void DoWork()
        {
            //System.IO.File.AppendAllText("log.txt", $"{DateTime.Now} - Iniciando o processamento do Timer.{Environment.NewLine}");

            while (!_cancellatonTokenSource.IsCancellationRequested)
            {
                try
                {
                    Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter
                    {
                        Level = Common.Logging.LogLevel.Warn
                    };
                    _oUnitOfWork = new UnitOfWork();
                    _cvaTimer = _oUnitOfWork.CvaTimRepository.GetByID(1);

                    if (_cvaTimer.STU == 2)
                    {
                        // Grab the Scheduler instance from the Factory 
                        var scheduler = StdSchedulerFactory.GetDefaultScheduler();

                        // and start it off
                        scheduler.Start();

                        // define the job and tie it to our HelloJob class
                        var rand = new Random(999999999);
                        var rnd = rand.Next();
                        IJobDetail job = JobBuilder.Create<TimerJob>()
                            .WithIdentity($"job{rnd}", $"group{rnd}")
                            .Build();

                        // Trigger the job to run now, and then repeat every 10 seconds
                        ITrigger trigger = TriggerBuilder.Create()
                            .WithIdentity($"trigger{rnd}", $"group{rnd}")
                            .StartNow()
                            .WithSimpleSchedule(x =>
                            {
                                if (_cvaTimer.TIM != null)
                                    x
                                        .WithIntervalInMinutes((int)_cvaTimer.TIM)
                                        .RepeatForever();
                            })
                            .Build();

                        // Tell quartz to schedule the job using our trigger
                        scheduler.ScheduleJob(job, trigger);

                        // some sleep to show what's happening
                        //Thread.Sleep(TimeSpan.FromSeconds(60));

                        // and last shut down the scheduler when you are ready to close your program
                        //scheduler.Shutdown();  
                    }
                    else
                    {
                        if (Process.GetProcessesByName("CVA_Rep_Service").Length <= 0)
                            VerificaErro(_oUnitOfWork, _cvaTimer);                        
                    }
                }
                catch (SchedulerException se)
                {
                    //System.IO.File.AppendAllText("log.txt", $"{DateTime.Now} - {GetInnerException(se)}{Environment.NewLine}");
                    _log.Error(GetInnerException(se), se);
                }
                catch (Exception ex)
                {
                    //System.IO.File.AppendAllText("log.txt", $"{DateTime.Now} - {GetInnerException(ex)}{Environment.NewLine}");
                    _log.Fatal(GetInnerException(ex), ex);
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
            _log.Info("TimerSerivce is started.");
            _task.Start();
        }

        public void Stop()
        {
            _log.Info("TimerService is stopped.");
            _cancellatonTokenSource.Cancel();
            _task.Wait();
        }

        private void VerificaErro(UnitOfWork UoW, CVA_TIM timer)
        {
            var _lockTaken = false;
            Monitor.Enter(_object, ref _lockTaken);
            try
            {
                var log = new EventLog("Application");
                var entries = log.Entries.Cast<EventLogEntry>()
                    .Where(l =>
                        l.EntryType == EventLogEntryType.Error
                        && l.InstanceId == 1000
                        && l.Source == "Application Error"
                        && l.Message.Contains("CVA_Rep_Service.exe"))
                    .Reverse()
                    .Take((int)timer.NUM_OBJ)
                    .Reverse()
                    .ToList();
                var stack = new Stack<EventLogEntry>();

                for (var i = 0; i < entries.Count; i++)
                    stack.Push(entries[i]);

                var entry = stack.Pop();

                var timeGenerated = entry.TimeGenerated;
                var stringTimeGenerared = timeGenerated.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                var cvaRegLog = UoW.CvaRegLogRepository.Get()
                    .OrderByDescending(o => o.ID)
                    .FirstOrDefault();
                var cvaRegs = UoW.CvaRegRepository.Get(r => r.STU == 3)
                    .OrderBy(r => r.ID)
                    .Take((int)timer.NUM_OBJ)
                    .ToList();

                var inserted = cvaRegLog.INS;
                var stringInserted = inserted.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                if (stringInserted == stringTimeGenerared)
                {
                    var stu = cvaRegLog.STU;

                    if (stu == 4)
                    {
                        var bas = cvaRegLog.BAS;
                        var reg = cvaRegLog.REG;
                        var cvaBas = UoW.CvaBasRepository.Get(b => b.ID > bas && b.STU == 2)
                                        .FirstOrDefault();

                        foreach (var cvaReg in cvaRegs)
                        {
                            if (cvaReg.ID <= reg)
                            {
                                if (cvaBas != null)
                                {
                                    cvaReg.BAS_ERR = cvaBas.ID;
                                    UoW.CvaRegRepository.Update(cvaReg);
                                    UoW.Save();                                    
                                }
                            }
                            else
                            {
                                cvaReg.BAS_ERR = bas;
                                UoW.CvaRegRepository.Update(cvaReg);
                                UoW.Save();                                
                            }
                        }

                        timer.STU = 2;
                        UoW.CvaTimRepository.Update(timer);
                        UoW.Save();
                    }
                }
                else
                {
                    var timeGeneratedMaior = timeGenerated.AddMinutes(Convert.ToDouble(10))
                        .AddSeconds(-timeGenerated.Second)
                        .AddMilliseconds(-timeGenerated.Millisecond);

                    var timeGeneratedMenor = timeGenerated.AddMinutes(-Convert.ToDouble(10))
                        .AddSeconds(-timeGenerated.Second)
                        .AddMilliseconds(-timeGenerated.Millisecond);

                    var insertedZerado = cvaRegLog.INS.AddSeconds(-inserted.Second)
                        .AddMilliseconds(-inserted.Millisecond);

                    if (insertedZerado >= timeGeneratedMenor && insertedZerado <= timeGeneratedMaior)
                    {
                        var stu = cvaRegLog.STU;

                        if (stu == 4)
                        {
                            var bas = cvaRegLog.BAS;
                            var reg = cvaRegLog.REG;
                            var cvaBas = UoW.CvaBasRepository.Get(b => b.ID > bas && b.STU == 2)
                                            .FirstOrDefault();

                            foreach (var cvaReg in cvaRegs)
                            {
                                if (cvaReg.ID <= reg)
                                {
                                    if (cvaBas != null)
                                    {
                                        cvaReg.BAS_ERR = cvaBas.ID;
                                        UoW.CvaRegRepository.Update(cvaReg);
                                        UoW.Save();
                                    }
                                }
                                else
                                {
                                    cvaReg.BAS_ERR = bas;
                                    UoW.CvaRegRepository.Update(cvaReg);
                                    UoW.Save();
                                }
                            }

                            timer.STU = 2;
                            UoW.CvaTimRepository.Update(timer);
                            UoW.Save();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //System.IO.File.AppendAllText("log.txt", $"{DateTime.Now} - {GetInnerException(ex)}{Environment.NewLine}");
            }
            finally
            {
                if (_lockTaken)
                    Monitor.Exit(_object);
            }
        }
    }
}