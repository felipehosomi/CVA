using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesOrder.Models.SAP
{
    public class Metadata_BusinessPartner
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string PeymentMethodCode { get; set; }
        public string ShipToDefault { get; set; }
        public string BilltoDefault { get; set; }        
    }
}
