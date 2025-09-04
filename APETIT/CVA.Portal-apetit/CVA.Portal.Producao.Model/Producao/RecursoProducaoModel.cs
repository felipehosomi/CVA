using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model.Producao
{
    public class RecursoProducaoModel
    {
        public int DocEntry { get; set; }

        public int DocNum { get; set; }

        public DateTime DocDate { get; set; }

        public string DocDateStr { get; set; }

        public string VisResCode { get; set; }

        public string ResName { get; set; }

        public string MetodoBaixa { get; set; }

        public int LineNum { get; set; }

        public string Deposito { get; set; }

        public int DocEntryPedido { get; set; }

        public int NrPedido { get; set; }

        public int CodEtapa { get; set; }

        public string DescEtapa { get; set; }

        public string CodMP { get; set; }

        public string DescMP { get; set; }

        public double QtdeOP { get; set; }

        public double QtdeOriginal { get; set; }

        public double Quantidade { get; set; }

        public double QtdeSelecionada { get; set; }

        public double QtdeEmitida { get; set; }

        public double QtdeBase { get; set; }

        public double QtdeRealizada { get; set; }

        public int Sequencia { get; set; }

        public string CentroCusto { get; set; }

        public string Item
        {
            get
            {
                return VisResCode + " - " + ResName;
            }
        }
    }
}
