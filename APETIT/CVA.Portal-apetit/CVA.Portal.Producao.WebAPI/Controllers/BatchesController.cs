using CVA.Portal.Producao.BLL.Apetit;
using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using CVA.Portal.Producao.Model.Qualidade;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class BatchesController : ApiController
    {
        BatchesBLL BLL;

        public BatchesController()
        {
            BLL = new BatchesBLL();
        }

        [HttpGet]
        public List<BatchesModel> GetBatches(string itemCode, string whs)
        {
            return BLL.GetItemBatches(itemCode, whs);
        }

        [HttpGet]
        public BatchesControlModel GetBatchesControl(string itemCode)
        {
            return BLL.BatchControl(itemCode);
        }
    }
}
