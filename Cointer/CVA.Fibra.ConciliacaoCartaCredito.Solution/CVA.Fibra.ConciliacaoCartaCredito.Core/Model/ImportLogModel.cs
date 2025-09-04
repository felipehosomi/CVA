using SBO.Hub.Attributes;
using System;
using System.Security.Cryptography;

namespace CVA.Fibra.ConciliacaoCartaCredito.Core.Model
{
    public class ImportLogModel
    {
        [HubModel(UIFieldName = "Linha")]
        public int Line { get; set; }
        [HubModel(UIIgnore = true)]
        public int CreditCardId { get; set; }
        [HubModel(UIIgnore = true)]
        public int BPLId { get; set; }
        [HubModel(UIIgnore = true)]
        public string Deposited { get; set; }
        [HubModel(UIFieldName = "Data")]
        public DateTime Date { get; set; }
        [HubModel(UIFieldName = "NSU")]
        public string NSU { get; set; }
        [HubModel(UIFieldName = "ID Depósito")]
        public int DepositId { get; set; }
        [HubModel(UIFieldName = "Observações")]
        public string Comments { get; set; }
    }
}
