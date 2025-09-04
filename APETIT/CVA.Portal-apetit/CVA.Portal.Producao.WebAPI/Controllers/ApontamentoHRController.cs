using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ApontamentoHRController : ApiController
    {
        ApontamentoHRBLL BLL;

        public ApontamentoHRController()
        {
            BLL = new ApontamentoHRBLL();
        }

        public void Post(ApontamentoHRModel model)
        {
            BLL.Create(model);
        }

        public void Put(ApontamentoHRModel model)
        {
            BLL.Update(model);
        }

        [HttpGet]
        public List<ApontamentoHRModel> GetApontamentosAbertos(string usuario)
        {
            return BLL.GetApontamentosAbertos(usuario);
        }

        [HttpGet]
        public ApontamentoHRModel GetApontamentoAberto(string usuario, int opDocNum, int codEtapa, int recursoLineNum)
        {
            return BLL.GetApontamentoAberto(usuario, opDocNum, codEtapa, recursoLineNum);
        }
    }
}