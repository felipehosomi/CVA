using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmissorEtiqueta.MODEL
{
    public class PRINT
    {
        public string Dimensao { get; set; }
        public string Descricao { get; set; }
        public string DescricaoEstrangeira { get; set; }
        public string Model { get; set; }
        public string Altura { get; set; }
        public string Largura { get; set; }
        public string Ref { get; set; }
        public DateTime Fabricacao { get; set; }
        public DateTime Validade { get; set; }
        public string Anvisa { get; set; }
        public string Esteril { get; set; }
        public string Material { get; set; }
        public string Fixo { get { return "CE 00860"; } set { } }
    }
}
