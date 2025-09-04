using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.AdicionaTabelaUsuario
{
    public class DocsModel
    {
        public string Banco { get; set; }
        public string BPLName { get; set; }
        public int DocEntry { get; set; }
        public DateTime DocDate { get; set; }
        public string ItemCode { get; set; }
        public string TaxIdNum { get; set; }
        public string CardCode { get; set; }
        public decimal DocTotal { get; set; }
    }
}
