using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Hybel.MODEL
{
    public class ItemModel
    {
        public int Linha { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Imposto { get; set; }

        public double Quantidade { get; set; }
        public double Peso { get; set; }
        public double PrecoUnitarioUSD { get; set; }
        public double TaxaMoeda { get; set; }
        public double ValorTotal { get; set; }
        public double ValorFrete { get; set; }

        public double EstoqueFisico { get; set; }
        public double EstoqueEncomendado { get; set; }
        public double EstoqueReservado { get; set; }
        public double EstoqueDisponivel { get; set; }
        public double EstoqueMinimo { get; set; }
    }
}
