using Common.Logging;
using Quartz;
using System;
using System.Diagnostics;
using System.Linq;

namespace SkaSapWs
{
    [DisallowConcurrentExecution]
    public class TimerJob : IJob
    {
        private static Process _oProcess;
        //private static readonly ILog _log = LogManager.GetLogger(typeof(TimerJob));

        public void Execute(IJobExecutionContext context)
        {
            //try
            //{
            //    StartService();
            //}
            //catch (Exception)
            //{
            //    StopService();
            //    throw;
            //}
        }

        private static void StartService()
        {
            InitSvc();
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

        private static void InitSvc()
        {
            try
            {
                Logger.Log.Info("Iniciando processo de integração de apontamentos");
                var init = new Init();
                var lst = init.ExportData();
                if (lst.Count > 0)
                {
                    Logger.Log.Info($"Total de {lst.Count} apontamentos para integrar.");
                    init.ImportData(lst);
                    init.ReturnData(lst);
                    Logger.Log.Info($"Processo concluído. {lst.Count} apontamentos integrados."); 
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"Erro no processo de integração de apontamentos: {ex.Message}", ex);
                throw;
            }
        }
    }
}
