using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class InspecaoMPController : ApiController
    {
        InspecaoMPBLL BLL;

        public InspecaoMPController()
        {
            BLL = new InspecaoMPBLL();
        }

        public IEnumerable<DocumentoModel> Get(DateTime? dataDe, DateTime? dataAte, string status, int? nf, string itemCode)
        {
            return BLL.GetRecebimentos(dataDe, dataAte, status, nf, itemCode);
        }
    }
}
