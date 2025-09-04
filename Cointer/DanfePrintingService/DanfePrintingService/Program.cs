using DanfePrintingService.Controller;
using System;
using System.Windows.Forms;

namespace DanfePrintingService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            PrintingController.Print();

            Application.Exit();
            return;
        }
    }
}
