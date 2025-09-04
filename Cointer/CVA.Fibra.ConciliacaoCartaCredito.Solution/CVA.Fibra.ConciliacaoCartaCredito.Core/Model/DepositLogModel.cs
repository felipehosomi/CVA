using SBO.Hub.Attributes;

namespace CVA.Fibra.ConciliacaoCartaCredito.Core.Model
{
    public class DepositLogModel
    {
        [HubModel(ColumnName = "U_Id")]
        public string Id { get; set; }
        [HubModel(ColumnName = "U_Type")]
        public int Type { get; set; }
    }
}
