using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System.Collections.Generic;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class EtapaItinerarioController : ApiController
    {
        EtapaItinerarioBLL BLL;

        public EtapaItinerarioController()
        {
            BLL = new EtapaItinerarioBLL();
        }

        // GET: api/EtapaItinerario
        public IEnumerable<EtapaItinerarioModel> Get()
        {
            return BLL.Get();
        }
    }
}
