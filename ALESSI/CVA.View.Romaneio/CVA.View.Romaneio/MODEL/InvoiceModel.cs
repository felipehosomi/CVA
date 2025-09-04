using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Romaneio.MODEL
{
    public class InvoiceModel
    {
        [ModelController(UIFieldName = "Nr. Doc")]
        public int DocNum { get; set; }

        [ModelController(UIFieldName = "Nr. NF")]
        public int Invoice { get; set; }

        [ModelController(UIFieldName = "Cód. PN")]
        public string CardCode { get; set; }

        [ModelController(UIFieldName = "Nome PN")]
        public string CardName { get; set; }

        [ModelController(UIFieldName = "Data")]
        public DateTime DocDate { get; set; }

        [ModelController(UIFieldName = "Transp.")]
        public string CarrierCode { get; set; }

        [ModelController(UIFieldName = "Nome Transp.")]
        public string CarrierName { get; set; }

        [ModelController(UIFieldName = "Peso Bruto")]
        public double GrossWeight { get; set; }

        [ModelController(UIFieldName = "Valor Total")]
        public double DocTotal { get; set; }

        [ModelController(UIFieldName = "UF")]
        public string State { get; set; }

        [ModelController(UIFieldName = "Cidade")]
        public string City { get; set; }
    }
}
