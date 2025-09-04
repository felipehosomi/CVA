using System;
using System.Windows.Forms;

namespace EnvioNfLote
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new AppController();
            Application.Run();
        }
    }
}
