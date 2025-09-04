using CVA.Portal.Producao.BLL.Apetit;
using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Qualidade;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ServicoExtraController : ApiController
    {
        ServicoExtraBLL BLL;

        public ServicoExtraController()
        {
            BLL = new ServicoExtraBLL();
        }

        public string GetByClient(string client)
        {
            return BLL.GetByClient(client);
        }

        [HttpGet]
        public List<ComboBoxModelHANA> GetInsumos(string bplid)
        {
            var retlist = BLL.GetInsumos(bplid);
            return (retlist.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }

        // POST: api/ServicoExtra
        public async Task PostAsync(ServicoExtraAPIModel model)
        {           
            await BLL.SaveSL(model);
        }
    }
}
