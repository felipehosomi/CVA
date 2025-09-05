using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;

namespace CVA.View.BancadaTeste.BLL
{
    public class ServiceBLL
    {
        private static System.Timers.Timer DeleteFileTimer;
        private static int DeleteFileSeconds;
        private static string ReturnFileName = ConfigurationManager.AppSettings["FileName"];

        public static void ExecuteService()
        {
            try
            {
                DeleteFileTimer = new System.Timers.Timer();
                DeleteFileSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["DeleteFileSeconds"]);

                DeleteFileTimer.Interval = (DeleteFileSeconds + 2) * 1000;
                DeleteFileTimer.Enabled = true;
                DeleteFileTimer.Elapsed += DeleteFileTimer_Elapsed;
                DeleteFileTimer.Start();

                int waitMilliSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["WaitMilliSeconds"]);

                int bancada = 1;

                string path = ConfigurationManager.AppSettings[bancada.ToString()];

                while (true)
                {
                    while (!String.IsNullOrEmpty(path))
                    {
                        try
                        {
                            string returnFile = Path.Combine(Path.GetDirectoryName(path), ReturnFileName);

                            if (File.Exists(path))
                            {
                                string op = POFileBLL.GetOP(path);

                                string error = ItemBLL.GenerateFile(returnFile, op);
                                if (!String.IsNullOrEmpty(error))
                                {
                                    LogBLL.GenerateLog("Erro ao gerar arquivo: " + error);
                                }
                                File.Delete(path);
                            } 
                        }
                        catch (Exception e)
                        {
                            LogBLL.GenerateLog("Erro: " + e.Message);
                            if (e.InnerException != null)
                            {
                                LogBLL.GenerateLog("Exceção interna: " + e.InnerException.Message);
                            }
                        }
                        finally
                        {
                            bancada++;
                            path = ConfigurationManager.AppSettings[bancada.ToString()];
                        }
                    }
                    bancada = 1;
                    path = ConfigurationManager.AppSettings[bancada.ToString()];
                    Thread.Sleep(TimeSpan.FromMilliseconds(waitMilliSeconds));
                }
            }
            catch (Exception e)
            {
                LogBLL.GenerateLog("Erro - serviço parado: " + e.Message);
                if (e.InnerException != null)
                {
                    LogBLL.GenerateLog("Exceção interna: " + e.InnerException.Message);
                }
            }
        }

        private static void DeleteFileTimer_Elapsed(object sender, EventArgs e)
        {
            int bancada = 1;

            string path = ConfigurationManager.AppSettings[bancada.ToString()];
            while (!String.IsNullOrEmpty(path))
            {
                try
                {
                    string returnFile = Path.Combine(Path.GetDirectoryName(path), ReturnFileName);

                    if (File.Exists(returnFile))
                    {
                        FileInfo fileInfo = new FileInfo(returnFile);
                        if (fileInfo.LastWriteTime.AddSeconds(DeleteFileSeconds) < DateTime.Now)
                        {
                            File.Delete(returnFile);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogBLL.GenerateLog("Erro: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        LogBLL.GenerateLog("Exceção interna: " + ex.InnerException.Message);
                    }
                }
                finally
                {
                    bancada++;
                    path = ConfigurationManager.AppSettings[bancada.ToString()];
                }
            }
        }
    }
}
