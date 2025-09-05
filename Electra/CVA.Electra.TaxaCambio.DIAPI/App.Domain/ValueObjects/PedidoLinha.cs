using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects
{
    public class PedidoLinha
    {
        public string ItemCode { get; set; }
        public double Price { get; set; }
        public string TaxCode { get; set; }
        public int List { get; set; }
        public int LineNum { get; set; }
        public double Quantity { get; set; }
        public string Dscription { get; set; }
        public double U_SD_CGM { get; set; }
        public double U_SD_CVD { get; set; }
        public double U_SD_CAT { get; set; }
        public bool AssTec { get; set; }
        public string ListName { get; set; }
        public double ListPrice { get; set; }
        public double U_SD_CMGM { get; set; }
        public double U_SD_CMVD { get; set; }
        public double U_SD_CMAT { get; set; }
        public string U_SD_PINF { get; set; }
    }
}
