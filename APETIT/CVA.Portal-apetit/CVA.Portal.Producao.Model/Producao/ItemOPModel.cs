using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model.Producao
{
    public class ItemOPModel
    {
        public int DocEntry { get; set; }
	    public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int LineNum { get; set; }
        public int StageId { get; set; }
        public string Etapa { get; set; }
        public double BaseQty { get; set; }
        public double PlannedQty { get; set; }
        public double IssuedQty { get; set; }
        public string wareHouse { get; set; }
        public string DefaultWareHouse { get; set; }
        public string OcrCode { get; set; }
    }
}
