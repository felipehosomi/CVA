using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model
{
    public class PurchaseRequestModel : DocumentoMarketingModel
    {
        public int ReqType { get { return 12; } }
        public string Requester { get; set; }
        public string RequriedDate { get; set; }
        public string CardCode { get; set; }
        public new List<PurchaseRequestLineModel> DocumentLines { get; set; }
        public string U_CVA_ObsPortal { get; set; }
        public int? AttachmentEntry { get; set; }
    }

    public class PurchaseRequestLineModel : Documentline
    {
        public string RequiredDate { get; set; }
    }

}
