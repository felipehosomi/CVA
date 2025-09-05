using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects
{
    public class DocLinhaTextoFiscal
    {
        public string ItemCode { get; set; }
        public int U_SD_AA { get; set; }
        public string CFOPCode { get; set; }
        public string NCM { get; set; }
        public string CardCode { get; set; }
        public string UF { get; set; }
        public string ProductSrc { get; set; }
    }
}
