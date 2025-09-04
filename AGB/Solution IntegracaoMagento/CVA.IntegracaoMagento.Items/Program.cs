using System;
using System.Diagnostics;

namespace CVA.IntegracaoMagento.Items
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Iniciando.");
            //System.Threading.Thread.Sleep(1000);

            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
                return;

            var objIntegration = new Integration();
            objIntegration.SetIntegration().Wait();
        }
    }
}
