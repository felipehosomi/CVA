using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.Model.Producao
{
    public class ProducaoModel
    {
        public int DocEntry { get; set; }
        public int DocEntryPedido { get; set; }
        public int NrPedido { get; set; }
        public int NrOP { get; set; }
        public DateTime DataOP { get; set; }
        public DateTime VencimentoOP { get; set; }
        public int CodEtapa { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Modelo { get; set; }
        public double Comprimento { get; set; }
        public double Largura { get; set; }
        public double Altura { get; set; }
        public double Quantidade { get; set; }
        public double QuantidadePlanejada { get; set; }
        public double QuantidadeApontada { get; set; }
        public double QuantidadeBase { get; set; }
        public double QtdeRealizada { get; set; }
        public double SaldoEtapa { get; set; }
        public string Etapa { get; set; }
        public bool Concluir { get; set; }
        public string ObsOP { get; set; }
        public string ObsPedido { get; set; }
        public string Medidas { get; set; }
        public string MedidasMaxflex { get; set; }
        public string BarCode { get; set; }
        public string RecursoCode { get; set; }
        public string RecursoName { get; set; }
        public int RecursoLineNum { get; set; }
        public bool Checked { get; set; }

        public string Item
        {
            get
            {
                return ItemCode + " - " + ItemName;
            }
        }

        public List<EstruturaProducaoModel> Estrutura { get; set; }
        public List<RecursoProducaoModel> Recursos { get; set; }
        public List<HistoricoProducaoModel> Historico { get; set; }
        public List<ItemOPModel> Itens { get; set; }
    }
}
