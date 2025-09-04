using CVA.Core.DIRF.MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DIRF.AUXILIAR
{
    public class Writer
    {
        #region Construtor
        public Writer()
        {
            if (!Directory.Exists($@"C:\CVA Consultoria\DIRF"))
            {
                Directory.CreateDirectory($@"C:\CVA Consultoria\DIRF");
            }
        }
        #endregion

        public bool Write(DirfModel model)
        {
            var data = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            StreamWriter log = new StreamWriter($@"C:\CVA Consultoria\DIRF\DIRF{data}.txt", true);

            try
            {
                log.WriteLine($@"{model.DIRF}");
                log.WriteLine($@"{model.RESPO}");
                log.WriteLine($@"{model.DECPJ}");

                foreach (var item_A in model.IDREC)
                {
                    log.WriteLine($@"{item_A.IDREC}");
                    foreach (var item_B in item_A.Info)
                    {
                        log.WriteLine($@"{item_B.BPJDEC}");
                        log.WriteLine($@"{item_B.RTRT}");
                        log.WriteLine($@"{item_B.RTIRF}");
                    }
                }

                log.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
