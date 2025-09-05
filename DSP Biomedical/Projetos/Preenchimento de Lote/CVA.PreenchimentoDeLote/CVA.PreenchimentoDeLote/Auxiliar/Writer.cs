using System;
using System.IO;

namespace CVA.PreenchimentoDeLote.Auxiliar
{
    public class Writer
    {
        #region Construtor
        public Writer()
        {
            if (!Directory.Exists($@"C:\CVA Consultoria\CVA.PreenchimentoDeLote"))
            {
                Directory.CreateDirectory($@"C:\CVA Consultoria\CVA.PreenchimentoDeLote");
            }
        }
        #endregion

        public bool Write(string novolote)
        {


            if (File.Exists($@"C:\CVA Consultoria\CVA.PreenchimentoDeLote\NovoLote.txt")) 
            {
                File.Delete($@"C:\CVA Consultoria\CVA.PreenchimentoDeLote\NovoLote.txt");

            }
            if (!File.Exists($@"C:\CVA Consultoria\CVA.PreenchimentoDeLote\NovoLote.txt")) 
            {

                File.Create($@"C:\CVA Consultoria\CVA.PreenchimentoDeLote\NovoLote.txt").Close();
            }


            StreamWriter log = new StreamWriter($@"C:\CVA Consultoria\CVA.PreenchimentoDeLote\NovoLote.txt", true);


            try
            {
                log.WriteLine($@"{novolote}");             

                log.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Read()
        {
            return File.ReadAllText(@"C:\CVA Consultoria\CVA.PreenchimentoDeLote\NovoLote.txt");
        }

    }
}