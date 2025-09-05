using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.StockTransfer.MODEL
{
    public class StockTransferItemModel
    {
        public string ItemCode { get; set; }
        public string Usage { get; set; }
        public double Quantity { get; set; }
        public double InvQty { get; set; }
        public string WhsCode { get; set; }
        public double PackQty { get; set; }
        public double Price { get; set; }

    }
}
