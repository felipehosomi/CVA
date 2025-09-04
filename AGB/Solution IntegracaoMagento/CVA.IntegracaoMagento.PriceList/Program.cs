using System;
using System.Diagnostics;

namespace CVA.IntegracaoMagento.PriceList
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
                return;

            var objIntegration = new Integration();
            objIntegration.SetIntegration().Wait();
        }
    }
}
