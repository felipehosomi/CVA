using System;
using System.IO;

namespace CVA.Core.TransportLCM.HELPER
{
    public class BusinessOneLog
    {
        public static void Add(string message, bool IsError = false)
        {
            string path = @"c:\\temp\\dover_log";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var arquivo = path + DateTime.Now.ToString("yyyyMMddHH") + ((IsError) ? "logerror.txt" : "log.txt");
            StreamWriter sr = new StreamWriter(arquivo, true);
            var msg = DateTime.Now.ToString("yyyyMMdd HHmmss") + " - " + message;
            Console.WriteLine(msg);
            sr.WriteLine(msg);

            sr.Close();
        }
    }
}
