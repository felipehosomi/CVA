using CVA.Portal.Producao.BLL;
using CVA.Portal.Producao.BLL.Apetit;
using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class OrdemProducaoController : ApiController
    {
        OrdemProducaoBLL BLLOrdemProduca;

        public OrdemProducaoController()
        {
            BLLOrdemProduca = new OrdemProducaoBLL();
        }


        [HttpGet]
        public List<ComboBoxModelHANA> GetOpenOrders(string ordersBPLId, DateTime? ordersDate)
        {
            var retlist = BLLOrdemProduca.GetOpenOrders(ordersBPLId, ordersDate);
            return (retlist?.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }

        [HttpGet]
        public List<ComboBoxModelHANA> GetAllOrders(string ordersBPLId)
        {
            var retlist = BLLOrdemProduca.GetAllOrders(ordersBPLId);
            return (retlist?.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }

        [HttpGet]
        public string GetClienteBPLID(string clienteBPLID)
        {
            return BLLOrdemProduca.GetClienteBPLID(clienteBPLID);
        }
        

    }
}
