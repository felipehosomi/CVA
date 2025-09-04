using SBO.Hub.Attributes;
using System.Collections.Generic;

namespace CVA.Cointer.Core.Model
{
    [HubModel(TableName = "@CVA_CONSIGNMENT")]
    public class InvoiceItemModel
    {
        [HubModel(ColumnName = "U_DocEntry", UIFieldName = "DocEntry")]
        public int DocEntry { get; set; }
        [HubModel(UIFieldName = "DocNum", DataBaseFieldYN = false)]
        public int DocNum { get; set; }
        [HubModel(UIFieldName = "Nr NF", DataBaseFieldYN = false)]
        public int Serial { get; set; }

        [HubModel(UIFieldName = "CardCode", DataBaseFieldYN = false)]
        public string CardCode { get; set; }
        [HubModel(ColumnName = "U_LineNum", UIFieldName = "LineNum")]
        public int LineNum { get; set; }
        [HubModel(UIFieldName = "TaxCode", DataBaseFieldYN = false)]
        public string TaxCode { get; set; }

        [HubModel(UIFieldName = "BPLId", DataBaseFieldYN = false)]
        public int BPLId { get; set; }
        [HubModel(UIFieldName = "Item", DataBaseFieldYN = false)]
        public string ItemCode { get; set; }
        [HubModel(UIFieldName = "WhsCode", DataBaseFieldYN = false)]
        public string Warehouse { get; set; }
        [HubModel(UIFieldName = "Usage", DataBaseFieldYN = false)]
        public int Usage { get; set; }
        [HubModel(ColumnName = "U_Quantity", UIFieldName = "Quantidade")]
        public double Quantity { get; set; }
        [HubModel(UIFieldName = "Preço do Item", DataBaseFieldYN = false)]
        public double Price { get; set; }
        [HubModel(UIFieldName = "Custo do Item", DataBaseFieldYN = false)]
        public double StockPrice { get; set; }
        [HubModel(ColumnName = "U_BatchNum", UIFieldName = "Lote")]
        public string Batch { get; set; }
        [HubModel(ColumnName = "U_DraftEntry", UIIgnore = true)]
        public int DocEntryDraft { get; set; }

        [HubModel(UIFieldName = "Vencimento", DataBaseFieldYN = false)]
        public string ExpDate { get; set; }

        public string ImportComments { get; set; }
        public List<BatchModel> BatchList { get; set; }
    }
}
