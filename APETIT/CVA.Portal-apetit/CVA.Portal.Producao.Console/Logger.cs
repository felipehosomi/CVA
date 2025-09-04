using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Console
{
    public class Logger
    {
        public StringBuilder SB { get; set; }
        private CultureInfo culture = new CultureInfo("pt-BR");

        public Logger()
        {
            SB = new StringBuilder();
        }

        public void Log(string message)
        {
            message = $"{DateTime.Now.ToString(culture)} | {message}";
            System.Console.WriteLine(message);
            SB.AppendLine(message);
        }

        public void Save()
        {
            string fileName = $"log_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
            Directory.CreateDirectory("Logs");
            File.WriteAllText($"Logs\\{fileName}", SB.ToString());
        }
    }
}