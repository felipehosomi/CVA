using System;
using System.Collections.Generic;
using System.IO;

namespace AUXILIAR
{
    public class LogWriter
    {
        public void WriteLog(List<string> logList)
        {
            string fileName = @"C:\CVA Consultoria\CVA Importador Tomticket\Log\[LOG]_Importador" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            StreamWriter writer = new StreamWriter(fileName);

            foreach (var l in logList)
                writer.WriteLine(l);
            
            writer.Close();
        }
    }
}
