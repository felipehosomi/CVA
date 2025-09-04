using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model
{

    public class ProductionOrdersModel
    {
        public ProductionOrdersModel()
        {
            ProductionOrderLines = new List<ProductionOrderLineModel>();
        }
        
        public int? AbsoluteEntry { get; set; }
        public int? DocumentNumber { get; set; }
        public string ProductionOrderStatus { get; set; }     
        public float? PlannedQuantity { get; set; }
        public string PostingDate { get; set; }
        public string Remarks { get; set; }
        public float? U_CVA_RESTO { get; set; }
        public float? U_CVA_SOBRA { get; set; }
        public string U_CVA_APO_ZERO { get; set; }
        public List<ProductionOrderLineModel> ProductionOrderLines { get; set; }
        public string U_CVA_APO { get; set; }
    }

    public class ProductionOrderLineModel
    {
        public int? DocumentAbsoluteEntry { get; set; }
        public int LineNumber { get; set; }
        public string ItemNo { get; set; }
        public float? PlannedQuantity { get; set; }
        public string ProductionOrderIssueType { get; set; }
        public string Warehouse { get; set; }
        public int VisualOrder { get; set; }
        public string ItemType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string U_CVA_Substituto { get; set; }
        public string U_CVA_SubMotivo { get; set; }
        public string U_CVA_SubJust { get; set; }
    }
}
