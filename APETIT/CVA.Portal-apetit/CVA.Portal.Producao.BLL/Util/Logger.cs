using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Util
{
    public class Logger
    {
        //private static readonly string fileName = "\\log.txt";
        private static readonly string repos = "\\log";
        private static readonly string fileNameReposicaoInsumos = "\\logReposicaoInsumos.txt";
        private static readonly object lockObj = new object();

        public static void Log(string message)
        {
            try
            {
                StreamWriter sw;
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\LogApontamento\";
                string str3 = path + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (!File.Exists(str3))
                    sw = File.CreateText(str3);
                else
                    sw = File.AppendText(str3);

                if (message != "")
                    sw.WriteLine(String.Format(new CultureInfo("pt-BR"), "[" + DateTime.Now.ToString() + "] - " + message));
                else
                    sw.WriteLine();

                sw.Flush();
                sw.Close();

                //lock (lockObj)
                //{
                //    File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~") + repos + $"\\{DateTime.Now.ToString("yyyy_MM_dd")}.txt", $"[{DateTime.Now}] - {message}{Environment.NewLine}");
                //}
            }
            catch
            { }
        }

        public static void LogReposicaoInsumos(string message)
        {
            try
            {
                StreamWriter sw;
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\LogReposicaoInsumos\";
                string str3 = path + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (!File.Exists(str3))
                    sw = File.CreateText(str3);
                else
                    sw = File.AppendText(str3);

                if (message != "")
                    sw.WriteLine(String.Format(new CultureInfo("pt-BR"), "[" + DateTime.Now.ToString() + "] - " + message));
                else
                    sw.WriteLine();

                sw.Flush();
                sw.Close();

                //lock (lockObj)
                //{
                //    File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~") + fileNameReposicaoInsumos, $"{DateTime.Now} - {message}{Environment.NewLine}");
                //}
            }
            catch
            { }
        }
    }
}
