using System;
using System.IO;
using System.Text;

namespace CVA.IntegracaoMagento.Invoices
{
    public class Util
    {
        #region [ Gravar LOG ]

        public static void GravarLog(string sCaminho, string sMensagem)
        {
            try
            {
                StreamWriter objArquivo = new StreamWriter(sCaminho, true, Encoding.UTF8); //ASCII
                objArquivo.WriteLine("\n" + DateTime.Now + " - " + sMensagem);
                objArquivo.Close();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
