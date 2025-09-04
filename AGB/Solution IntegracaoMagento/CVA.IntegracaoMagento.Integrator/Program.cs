using System;

namespace CVA.IntegracaoMagento.Integrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var objIntegration = new Integration();
            objIntegration.SetIntegration().Wait();
        }
    }
}
