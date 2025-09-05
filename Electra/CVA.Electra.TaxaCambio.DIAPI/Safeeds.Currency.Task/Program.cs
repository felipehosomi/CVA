using Electra.Currency.Task.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Electra.Currency.Task
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            LogService.CreateLog();
            try
            {
                var version = ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(
                         Assembly.GetExecutingAssembly(),
                         typeof(AssemblyFileVersionAttribute), false)).Version;
                Console.WriteLine(string.Format("Atualização Taxa de Câmbio- Version:{0} ", version));
                Console.WriteLine("Processo iniciado");
                CurrencyTaskHelper objTask = new CurrencyTaskHelper();
                objTask.Execute();
                Console.WriteLine("Processo finalizado");
            }
            catch (Exception ex)
            {
                EmailService.SendingMail(ex.Message);
                Console.WriteLine("Processo finalizado com erro");
                LogService.GravarException(ex.Message);
            }
        }
    }
}


