using System;
using System.Diagnostics;
using System.Text;

namespace CVA_Rep_Service
{
    public class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (Process.GetProcessesByName("CVA_Rep_Service.exe").Length > 1) Environment.Exit(0);
            
            try
            {
                new ReplicadorSvc();
            }
            catch (Exception ex)
            {
                var builder = new StringBuilder();
                builder.AppendLine(ex.Message);
                builder.AppendLine(ex.StackTrace);

                if (ex.InnerException != null)
                {
                    builder.AppendLine(ex.InnerException.Message);
                    builder.AppendLine(ex.InnerException.StackTrace);
                }

                Console.WriteLine(builder.ToString());
            }
        }
    }
}