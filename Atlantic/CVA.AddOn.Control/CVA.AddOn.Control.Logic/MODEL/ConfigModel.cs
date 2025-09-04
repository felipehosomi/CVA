using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.MODEL
{
    public class ConfigModel
    {
        /// <summary>
        /// 1 - Replicadora / 2 - Consolidadora / 3 - Portal
        /// </summary>
        public int Tipo { get; set; }
        public string Servidor { get; set; }
        public string Banco { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
    }
}
