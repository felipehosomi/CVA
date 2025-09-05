using CVA.View.Comissionamento.Controllers;
using CVA.View.Comissionamento.Helpers;
using System;
using System.Windows.Forms;

namespace CVA.View.Comissionamento
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var container = new ContainerHelper().GetContainer();
            new AppController(container);
            Application.Run();
        }
    }
}
