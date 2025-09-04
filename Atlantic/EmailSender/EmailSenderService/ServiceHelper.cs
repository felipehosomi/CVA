using EmailSender.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmailSenderService
{
    public class ServiceHelper
    {
        public static void ExecuteService()
        {
            var emailSenderBLL = new EmailSenderBLL();

            int wait = Convert.ToInt32(ConfigurationManager.AppSettings["WaitSeconds"]);
            
            while (true)
            {
                try
                {
                    emailSenderBLL.Check();
                }
                catch (Exception ex)
                {
                    string path = System.IO.Path.GetDirectoryName(typeof(ServiceHelper).Assembly.Location) + "\\log\\erro.txt";
                    StreamWriter writer = new StreamWriter(path);
                    writer.WriteLine("Erro geral: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        writer.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                    writer.Close();
                }
                Thread.Sleep(TimeSpan.FromSeconds(wait));
            }
        }
    }
}
