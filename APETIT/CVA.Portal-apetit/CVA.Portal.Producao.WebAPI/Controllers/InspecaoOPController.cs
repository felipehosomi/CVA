using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class InspecaoOPController : ApiController
    {
        InspecaoOPBLL BLL;

        public InspecaoOPController()
        {
            BLL = new InspecaoOPBLL();
        }

        public IEnumerable<DocumentoModel> Get(DateTime? dataDe, DateTime? dataAte, string status, int? pedido, int? op, string codigoItem)
        {
            return BLL.GetOPs(dataDe, dataAte, status, pedido, op, codigoItem);
        }
    }
}
