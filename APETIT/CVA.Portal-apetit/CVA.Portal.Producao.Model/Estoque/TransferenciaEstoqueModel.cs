using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model.Estoque
{
    public class TransferenciaEstoqueModel
    {
        public int? DocNum { get; set; }
        public string Status { get; set; }
        public DateTime? DocDate { get; set; }
        public string DocObjectCode { get; set; }
        public string CardCode { get; set; }
        public string U_CVA_ObsPortal { get; set; }
        public List<LinhaTransferenciaEstoqueModel> StockTransferLines;
    }

    public class LinhaTransferenciaEstoqueModel
    {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public double Quantity { get; set; }
        public double? QuantityMet { get; set; }
        public double? OpenCreQty { get; set; }
        public string FromWarehouseCode { get; set; }
        public string WarehouseCode { get; set; }
        public string U_CVA_Etapa { get; set; }
    }
}