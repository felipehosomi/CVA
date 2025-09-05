using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.StockTransfer.MODEL
{
    public class BatchModel
    {
        public string ItemCode { get; set; }
        public string BatchNum { get; set; }
        public double Quantity { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
