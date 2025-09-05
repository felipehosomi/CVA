using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conferencia.ConexaoSAP
{
    public struct ParametrosSAP
    {
        public string Company { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public override string ToString()
        {
            return string.Format("Empresa: {0}; Usuário: {1}; Senha: {2};", Company, Usuario, Senha);
        }

    }
}
