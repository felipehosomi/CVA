using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.MODEL
{
    public class DocumentItemModel
    {
        public string OcrCode2 { get; set; }

        public string Validacao { get; set; }

        public double LineTotal { get; set; }

        public double VatSum { get; set; }

        public double DocTotal { get; set; }
    }
}
