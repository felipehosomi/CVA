using System;

namespace CVA.IntegracaoMagento.BusinessPartner
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(1000);
            var objIntegration = new Integration();
            objIntegration.SetIntegration().Wait();
        }
    }
}
