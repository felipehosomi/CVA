using CVA.Portal.Producao.BLL.Apetit;
using CVA.Portal.Producao.BLL.Qualidade;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Qualidade;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class FilialController : ApiController
    {
        FilialBLL BLL;

        public FilialController()
        {
            BLL = new FilialBLL();
        }

        [HttpGet]
        public List<ComboBoxModel> Get()
        {
            return BLL.Get();
        }

        [HttpGet]
        public List<ComboBoxModelHANA> GetFilial(string userId)
        {
            var retlist = BLL.GetFilial(userId);
            return (retlist.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }

        [HttpGet]
        public SaidaInsumoModelGetOBPL GetSaidaInsumoOBPL(string BPLId)
        {
            return BLL.GetSaidaInsumoOBPL(BPLId);
        }

    }
}
