namespace TaxDeterminationLoader
{
    using System;
    using System.IO;

    public sealed class Log
    {
        private static string fileDefault = "C:\\CVA Consultoria\\Log Determinação Código Imposto.txt";
        private static StreamWriter sw;

        public static void writeLog(string logLine)
        {
            string folder = Path.GetDirectoryName(fileDefault);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (File.Exists(fileDefault))
            {
                sw = File.AppendText(fileDefault);
                sw.WriteLine(DateTime.Now.ToString() + ";" + logLine);
                sw.Close();
            }
            else
            {
                sw = new StreamWriter(fileDefault);
                sw.WriteLine(DateTime.Now.ToString() + ";" + logLine);
                sw.Close();
            }
        }
    }
}

