using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using SAPbobsCOM;
using System.Threading;

namespace CVA.Monitor.WBC.Excel.Files
{
    public class Program
    {
        //static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8G}");
        static void Main(string[] args)
        {
            //if (mutex.WaitOne(TimeSpan.Zero, true))
            //{
            var m = new Monitoramento();
            System.Environment.Exit(0);
            //Console.ReadKey(true);
            //mutex.ReleaseMutex();
            //}
        }
    }
}
