using System;
using System.Configuration;
using System.IO;

namespace SkaSapWs.BLL
{
    public class LogBLL
    {
        public static void GenerateLog(string log)
        {
            string logFile = ConfigurationManager.AppSettings["LogFile"];

            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - {log}");
            sw.Close();
        }
    }
}
