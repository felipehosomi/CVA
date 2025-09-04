using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model
{

    public class InventoryGenExitsModel
    {
        public InventoryGenExitsModel()
        {
            DocumentLines = new List<InventoryGenExitsModelDocumentline>();
        }

        public int? DocEntry { get; set; }
        public int? DocNum { get; set; }
        public string DocType { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string Comments { get; set; }
        public string JournalMemo { get; set; }
        public string DocObjectCode { get; set; }
        public int? BPL_IDAssignedToInvoice { get; set; }  
        public List<InventoryGenExitsModelDocumentline> DocumentLines { get; set; }
    }


    public class InventoryGenExitsModelDocumentline
    {
        public int? LineNum { get; set; }
        public float Quantity { get; set; }
        public int? BaseType { get; set; }
        public object BaseEntry { get; set; }
        public object BaseLine { get; set; }
    }




}
