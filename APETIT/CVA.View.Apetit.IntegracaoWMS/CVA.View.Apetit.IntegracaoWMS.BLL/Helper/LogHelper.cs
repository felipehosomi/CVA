using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace CVA.View.Apetit.IntegracaoWMS.BLL
{
    public static class Helper
    {
        public static void LogInfo(string texto)
        {
            try
            {
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory); //or set executing Assembly location path in param

                var path = Path.Combine(Directory.GetCurrentDirectory(), "LOGS");
                Directory.CreateDirectory(path);
                File.AppendAllText(Path.Combine(path, "LogIntegra.txt"), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " :=> " + texto);
            }
            catch (Exception)
            {
            }
        }

        public static string Aspas(this string texto)
        {
            return $@"""{texto}""";
        }

        public static string LimparCNPJ(this string texto)
        {
            var txt = texto.Replace(".","").Replace(".", "").Replace("/", "").Replace("-", "");
            return txt;
        }

        public static int ToInt(this JValue val)
        {
            return int.Parse(val.ToString());
        }
    }
}
