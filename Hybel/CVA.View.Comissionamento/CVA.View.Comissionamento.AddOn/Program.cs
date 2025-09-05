using CVA.View.Comissionamento.Controllers;
using CVA.View.Comissionamento.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.View.Comissionamento.AddOn
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = new ContainerHelper().GetContainer();
            new AppController(container);
            Application.Run();
        }
    }
}
