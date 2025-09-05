using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.StockTransfer.MODEL
{
    public class StockTransferModel
    {
        public BoObjectTypes DocType { get; set; }
        public string CardCode { get; set; }
        public int BaseDocEntry { get; set; }
        public List<StockTransferItemModel> Items { get; set; }
    }
}
