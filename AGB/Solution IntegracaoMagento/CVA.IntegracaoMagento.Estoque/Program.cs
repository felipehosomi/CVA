using CVA.IntegracaoMagento.Integrator;
using System;

namespace CVA.IntegracaoMagento.Estoque
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
