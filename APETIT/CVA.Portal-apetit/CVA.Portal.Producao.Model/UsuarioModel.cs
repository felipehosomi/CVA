using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.Model
{
    public class UsuarioModel
    {
        public string Code { get; set; }
        public string Senha { get; set; }
        public int Ativo { get; set; }
        public string CodPerfil { get; set; }
    }
}
