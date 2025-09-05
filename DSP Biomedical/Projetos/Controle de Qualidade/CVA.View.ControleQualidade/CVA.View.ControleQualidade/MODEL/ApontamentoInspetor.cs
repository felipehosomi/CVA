using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.MODEL
{
    public class ApontamentoInspetor
    {
        public string Code { get; set; }
        public string Usuario { get; set; }
        public string PlanoInsp { get; set; }
        public DateTime Data { get; set; }
        public int OP { get; set; }
        public string ItemCode { get; set; }
        public string PecaPiloto { get; set; }

        public List<ApontamentoInspetorLinha> Linhas { get; set; }
    }

    public class ApontamentoInspetorLinha
    {
        public string Code { get; set; }
        public int InspLinha { get; set; }
        public string Hora { get; set; }
        public string Valor { get; set; }
        public string Nome { get; set; }
    }
}
