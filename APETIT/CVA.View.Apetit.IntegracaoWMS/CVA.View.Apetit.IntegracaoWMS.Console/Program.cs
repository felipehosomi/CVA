using CVA.View.Apetit.IntegracaoWMS.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CVA.View.Apetit.IntegracaoWMS.Console
{
    class Program
    {
        static Mutex mutex = new Mutex(true, "{9F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8I}");
        static void Main(string[] args)
        {
            ParametrosConexao.param = new ParametrosConexao(Properties.Settings.Default);

            var integrador = new IntegracaoBLL();

            var lstTablesParams = new Dictionary<string, string[]> {
                 { "OPDN", new[] { "PDN1", "20" } }
                ,{ "OPCH", new[] { "PCH1", "18" } }
            };

            foreach (var item in lstTablesParams)
            {
                integrador.ProcessoDocumentoCompra(item.Key, item.Value[0], item.Value[1]);
            }

            return;
        }
    }
}
