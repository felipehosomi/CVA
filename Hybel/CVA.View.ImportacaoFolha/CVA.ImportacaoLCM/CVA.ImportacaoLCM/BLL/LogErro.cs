using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CVA.ImportacaoLCM.BLL
{
    public class LogErro
    {       
        public string GetTempPath()
        {
            string path = Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";


           

            return path;
        }

        public void FileExists()
        {
            if (File.Exists(GetTempPath() + "CVA.LogImportação.txt"))
            {
                File.Delete(GetTempPath() + "CVA.LogImportação.txt");
            }
            if (!File.Exists(GetTempPath() + "CVA.LogImportação.txt"))
            {
                File.Create(GetTempPath() + "CVA.LogImportação.txt").Close();
            }
        }
        public void Log(string msg)
        {
            StreamWriter sw = File.AppendText(GetTempPath() + "CVA.LogImportação.txt");
            try
            {
                string logLine = String.Format("{0:G}: {1}.", DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
