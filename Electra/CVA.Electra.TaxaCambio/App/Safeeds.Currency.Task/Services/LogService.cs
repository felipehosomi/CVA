using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electra.Currency.Task
{
    class LogService
    {
        public static void CreateLog()
        {
            var filestream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory+"/Log","Log--" +DateTime.Now.ToString("dd-MM-yyyy"))+".txt", FileMode.Create);
            var streamwriter = new StreamWriter(filestream) {AutoFlush = true};
            System.Console.SetOut(streamwriter);
            System.Console.SetError(streamwriter);
        }

        public static void GravarException(string sException)
        {
            try
            {
                var filestream = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/Log", "Exception--" + DateTime.Now.ToString("dd-MM-yyyy")) + ".txt";
                var file = new StreamWriter(filestream, true);
                var sMensagem = DateTime.Now + "    -   " + sException;
                file.WriteLine(sMensagem);
                file.Close();
            }
            catch { }
        }


    }
}
