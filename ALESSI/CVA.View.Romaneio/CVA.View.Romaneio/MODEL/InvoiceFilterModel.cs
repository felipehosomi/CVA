using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Romaneio.MODEL
{
    public class InvoiceFilterModel
    {
        [ModelController(UIFieldName = "et_Branch")]
        public string Branches { get; set; }
        [ModelController(UIFieldName = "et_Carrier")]
        public string CarrierCode { get; set; }

        [ModelController(UIFieldName = "et_DtFrom")]
        public DateTime? DateFrom { get; set; }

        [ModelController(UIFieldName = "et_DtTo")]
        public DateTime? DateTo { get; set; }

        [ModelController(UIFieldName = "et_NFFrom")]
        public int? NFFrom{ get; set; }

        [ModelController(UIFieldName = "et_NFTo")]
        public int? NFTo { get; set; }

        [ModelController(UIFieldName = "cb_State")]
        public string State { get; set; }

        [ModelController(UIFieldName = "et_City")]
        public string City { get; set; }

        public string WaybillCode { get; set; }
    }
}
