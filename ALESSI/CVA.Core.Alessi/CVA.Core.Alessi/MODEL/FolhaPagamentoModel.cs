using System;
using System.Collections.Generic;

namespace CVA.Core.Alessi.MODEL
{
    public class FolhaPagamentoModel
    {
        public int BPlId { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DueDate { get; set; }
        public List<FolhaPagamentoLineModel> Lines { get; set; }
    }

    public class FolhaPagamentoLineModel
    {
        public string ContaContabil { get; set; }
        public double Credito { get; set; }
        public double Debito { get; set; }
        public string ParceiroNegocio { get; set; }
        public string CentroCusto01 { get; set; }
        public string Projeto { get; set; }
        public string Observacao { get; set; }
    }
}
