using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Hybel.MODEL
{
    public class TaxaConversaoVendaModel
    {
        public int CanceladosCount { get; set; }
        public int FaturadosCount { get; set; }
        public int AtrasadosCount { get; set; }
        public int NaoFinalizadosCount { get; set; }
        public int TotalCount { get; set; }
        public double PorcCount { get; set; }

        public double Cancelados { get; set; }
        public double Faturados { get; set; }
        public double Atrasados { get; set; }
        public double NaoFinalizados { get; set; }
        public double Total { get; set; }
        public double Porc { get; set; }
    }
}
