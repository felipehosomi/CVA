using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model.Producao
{
    public class EstruturaProducaoModel
    {
        public int DocEntry { get; set; }

        public int DocNum { get; set; }

        public DateTime DocDate { get; set; }

        public string DocDateStr { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

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

        public string ControlePorLote { get; set; }

        public string ControlePorSerie { get; set; }

        public string Item
        {
            get
            {
                return ItemCode + " - " + ItemName;
            }
        }

        public List<LoteSerieModel> LoteSerie { get; set; }

        public string DecimalPlacesUI
        {
            get
            {
                int count = BitConverter.GetBytes(decimal.GetBits((decimal)QtdeRealizada)[3])[2];
                if (count > 0)
                {
                    return "0." + "1".PadLeft(count, '0');
                }
                else
                {
                    return "1";
                }
            }
        }
    }
}
