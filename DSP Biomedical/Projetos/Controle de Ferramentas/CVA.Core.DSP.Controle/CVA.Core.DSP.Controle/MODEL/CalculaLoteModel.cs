using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DSP.Controle.MODEL
{
    public class CalculaLoteModel
    {
        public string selecionaLote { get; set; }

        public string calculaSubLote { get; set; }
        public string lote { get; set; }

    }


    public class NextLote
    {
        public string code { get; set; }
    }

    public class NewLote
    {
        public string newlote { get; set; }
    }
}
