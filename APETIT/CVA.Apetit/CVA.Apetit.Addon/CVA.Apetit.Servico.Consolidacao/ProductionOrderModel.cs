using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Apetit.Servico.Consolidacao
{
    public class ProductionOrderModel
    {
        [JsonIgnore]
        public int PlanEntry { get; set; }
        [JsonIgnore]
        public string Day { get; set; }
        [JsonIgnore]
        public string TipoPrato { get; set; }
        public int AbsoluteEntry { get; set; }
        public string ItemNo { get; set; }
        public string ProductionOrderStatus { get; set; }
        public string ProductionOrderType { get; set; }
        public double PlannedQuantity { get; set; }
        public string PostingDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Warehouse { get; set; }
        public DateTime StartDate { get; set; }
        public string U_CVA_SERVICO { get; set; }
        public string U_CVA_Turno { get; set; }
        public string U_CVA_CONTRATO { get; set; }
        public int U_CVA_PlanCode { get; set; }
        public int U_CVA_LineId { get; set; }
        public List<Productionorderline> ProductionOrderLines { get; set; }
    }

    public class Productionorderline
    {
        public string ItemNo { get; set; }
        public double BaseQuantity { get; set; }
        public double PlannedQuantity { get; set; }
        public string ItemType { get; set; }
        public string ProductionOrderIssueType { get; set; }
        public string Warehouse { get; set; }
    }

}
