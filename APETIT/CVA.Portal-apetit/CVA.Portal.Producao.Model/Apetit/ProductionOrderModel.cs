using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model
{
    public class ProductionOrderModel
    {
        public ProductionOrderModel()
        {
            ProductionOrderLines = new List<Productionorderline>();
        }
        

        public int AbsoluteEntry { get; set; }
        public int DocumentNumber { get; set; }
        public int Series { get; set; }
        public string ItemNo { get; set; }
        public string ProductionOrderStatus { get; set; }
        public string ProductionOrderType { get; set; }
        public float PlannedQuantity { get; set; }
        public float CompletedQuantity { get; set; }
        public float RejectedQuantity { get; set; }
        public string PostingDate { get; set; }
        public string DueDate { get; set; }
        public object ProductionOrderOriginEntry { get; set; }
        public object ProductionOrderOriginNumber { get; set; }
        public string ProductionOrderOrigin { get; set; }
        public int UserSignature { get; set; }
        public object Remarks { get; set; }
        public object ClosingDate { get; set; }
        public object ReleaseDate { get; set; }
        public object CustomerCode { get; set; }
        public string Warehouse { get; set; }
        public string InventoryUOM { get; set; }
        public string JournalRemarks { get; set; }
        public object TransactionNumber { get; set; }
        public string CreationDate { get; set; }
        public string Printed { get; set; }
        public string DistributionRule { get; set; }
        public string Project { get; set; }
        public string DistributionRule2 { get; set; }
        public string DistributionRule3 { get; set; }
        public string DistributionRule4 { get; set; }
        public string DistributionRule5 { get; set; }
        public int UoMEntry { get; set; }
        public string StartDate { get; set; }
        public string ProductDescription { get; set; }
        public int Priority { get; set; }
        public string RoutingDateCalculation { get; set; }
        public string UpdateAllocation { get; set; }
        public object SAPPassport { get; set; }
        public List<Productionorderline> ProductionOrderLines { get; set; }
    }

    public class Productionorderline
    {
        public int DocumentAbsoluteEntry { get; set; }
        public int LineNumber { get; set; }
        public string ItemNo { get; set; }
        public float BaseQuantity { get; set; }
        public float PlannedQuantity { get; set; }
        public float IssuedQuantity { get; set; }
        public string ProductionOrderIssueType { get; set; }
        public string Warehouse { get; set; }
        public int VisualOrder { get; set; }
        public object DistributionRule { get; set; }
        public object LocationCode { get; set; }
        public object Project { get; set; }
        public object DistributionRule2 { get; set; }
        public object DistributionRule3 { get; set; }
        public object DistributionRule4 { get; set; }
        public object DistributionRule5 { get; set; }
        public int UoMEntry { get; set; }
        public int UoMCode { get; set; }
        public object WipAccount { get; set; }
        public string ItemType { get; set; }
        public object LineText { get; set; }
        public float AdditionalQuantity { get; set; }
        public object ResourceAllocation { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public object StageID { get; set; }
        public float RequiredDays { get; set; }
        public object U_CVA_Status { get; set; }
        public object[] SerialNumbers { get; set; }
        public object[] BatchNumbers { get; set; }
    }

}
