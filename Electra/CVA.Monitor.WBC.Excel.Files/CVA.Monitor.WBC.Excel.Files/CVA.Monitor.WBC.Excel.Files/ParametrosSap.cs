using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Monitor.WBC.Excel.Files
{
    public struct ParametrosSap
    {
        public string Company { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public int Tempo { get; set; }

        public override string ToString()
        {
            return string.Format("Empresa: {0}; Usuário: {1}; Senha: {2};", Company, Usuario, Senha);
        }

    }
}
