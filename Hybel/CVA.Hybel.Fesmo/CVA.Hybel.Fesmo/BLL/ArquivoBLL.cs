using System;
using System.IO;

namespace CVA.Hybel.Fesmo.BLL
{
    public class ArquivoBLL
    {
        public string RemoveArquivos(string diretorio, int quantidadeDias)
        {
            try
            {
                string[] files = Directory.GetFiles(diretorio);

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.CreationTime < DateTime.Now.AddDays(quantidadeDias * (-1)))
                    {
                        fi.Delete();
                    }
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
