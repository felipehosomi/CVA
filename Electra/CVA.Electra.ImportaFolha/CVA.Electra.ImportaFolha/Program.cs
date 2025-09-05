using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.Electra.ImportaFolha
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //  Creating an object
            ImportaFolha oImportaFolha = null;

            oImportaFolha = new ImportaFolha();

            //  Starting the Application
            System.Windows.Forms.Application.Run();
        }
    }
}
