using System;
using System.Diagnostics;
using Quartz;
using Common.Logging;
using CVA_Rep_DAL;

namespace CVA_Rep_Timer
{
    public class TimerJob : IJob
    {
        private static Process _oProcess;
        private static readonly ILog _log = LogManager.GetLogger(typeof(TimerJob));
        
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                StartService();
            }
            catch (Exception)
            {
                StopService();
                throw;
            }
        }

        private static void StartService()
        {
            if (Process.GetProcessesByName("CVA_Rep_Service").Length <= 0)
                LaunchCommandLineApp();
        }

        private static void StopService()
        {
            if (IsRunning())
                _oProcess.Kill();
        }

        private static bool IsRunning()
        {
            try
            {
                Process.GetProcessById(_oProcess.Id);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static void LaunchCommandLineApp()
        {
            var _oUnitOfWork = new UnitOfWork();
            var _cvaTimer = _oUnitOfWork.CvaTimRepository.GetByID(1);

            if (_cvaTimer.STU == 2)
            {
                if (_cvaTimer.STU == 2)
                {
                    var startInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        ErrorDialog = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        FileName = $"{AppDomain.CurrentDomain.BaseDirectory}\\..\\Service\\CVA_Rep_Service.exe",
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    _oProcess = Process.Start(startInfo);
                    if (_oProcess != null)
                    {
                        _oProcess.EnableRaisingEvents = true;
                    }
                }
            }
        }
/*
        private static void VerificaErro()
        {
            var _lockTaken = false;
            Monitor.Enter(_object, ref _lockTaken);
            try
            {
                var oUnitOfWork = new UnitOfWork();
                var cvaTim = oUnitOfWork.CvaTimRepository.GetByID(1);

                EventLog log = new EventLog("Application");

                var entries = log.Entries.Cast<EventLogEntry>().Reverse().Where(x => x.InstanceId == 1000 && x.Source == "Application Error").Take(10).ToList();

                foreach (var entry in entries)
                {
                    var messageContains = entry.Message.Contains("CVA_Rep_Service.exe");

                    if (messageContains)
                    {
                        var iTimeGenerated = entry.TimeGenerated;
                        var sTimeGenerated = iTimeGenerated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        var cvaRegLog = oUnitOfWork.CvaRegLogRepository.Get().OrderByDescending(o => o.ID).FirstOrDefault();
                        var cvaRegs = oUnitOfWork.CvaRegRepository.Get(r => r.STU == 3).OrderBy(r => r.ID).Take((int)cvaTim.NUM_OBJ).ToList();

                        var cvaRegLogINS = cvaRegLog.INS.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        if (cvaRegLogINS == sTimeGenerated)
                        {
                            var cvaRegLogSTU = cvaRegLog.STU;

                            if (cvaRegLogSTU == 4)
                            {
                                var bas = cvaRegLog.BAS;
                                var reg = cvaRegLog.REG;

                                foreach (var cvaReg in cvaRegs)
                                {
                                    if (cvaReg.ID <= reg)
                                    {
                                        bas = oUnitOfWork.CvaBasRepository.Get(b => b.ID > bas).FirstOrDefault().ID;
                                        cvaReg.BAS_ERR = bas;
                                        //oUnitOfWork.CvaRegRepository.Update(cvaReg);
                                        oUnitOfWork.Save();
                                    }
                                    else
                                    {
                                        cvaReg.BAS_ERR = bas;
                                        //oUnitOfWork.CvaRegRepository.Update(cvaReg);
                                        oUnitOfWork.Save();
                                    }
                                }

                                cvaTim.STU = 2;
                                //oUnitOfWork.CvaTimRepository.Update(cvaTim);
                                oUnitOfWork.Save();
                                break;
                            }
                        }
                        else
                        {
                            var s2TimeGenerated = iTimeGenerated.AddMinutes(Convert.ToDouble(5));
                            s2TimeGenerated = s2TimeGenerated.AddSeconds(-s2TimeGenerated.Second);
                            s2TimeGenerated = s2TimeGenerated.AddMilliseconds(-s2TimeGenerated.Millisecond);

                            var i2TimeGenerated = iTimeGenerated.AddSeconds(-iTimeGenerated.Second);
                            i2TimeGenerated = i2TimeGenerated.AddMilliseconds(-i2TimeGenerated.Millisecond);

                            var cvaRegLogINSDT = cvaRegLog.INS.AddSeconds(-cvaRegLog.INS.Second);
                            cvaRegLogINSDT = cvaRegLogINSDT.AddMilliseconds(-cvaRegLogINSDT.Millisecond);

                            if (cvaRegLogINSDT >= i2TimeGenerated && cvaRegLogINSDT <= s2TimeGenerated)
                            {
                                if (cvaRegLog.STU == 4)
                                {
                                    var bas = cvaRegLog.BAS;
                                    var reg = cvaRegLog.REG;

                                    foreach (var cvaReg in cvaRegs)
                                    {
                                        if (cvaReg.ID <= reg)
                                        {
                                            bas = oUnitOfWork.CvaBasRepository.Get(b => b.ID > bas && b.STU == 2).FirstOrDefault().ID;
                                            cvaReg.BAS_ERR = bas;
                                            //oUnitOfWork.CvaRegRepository.Update(cvaReg);
                                            oUnitOfWork.Save();
                                        }
                                        else
                                        {
                                            cvaReg.BAS_ERR = bas;
                                            //oUnitOfWork.CvaRegRepository.Update(cvaReg);
                                            oUnitOfWork.Save();
                                        }
                                    }

                                    cvaTim.STU = 2;
                                    //oUnitOfWork.CvaTimRepository.Update(cvaTim);
                                    oUnitOfWork.Save();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (_lockTaken)
                    Monitor.Exit(_object);
            }
        }
*/
    }
}