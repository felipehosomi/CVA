using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.Controller
{
    public static class MetodoExtensao
    {
        public static string ExtrairValorDaLinha(this string conteudoLinha, int de, int ate)
        {
            int inicio = de - 1;
            return conteudoLinha.Substring(inicio, ate - inicio);
        }
    }
}
