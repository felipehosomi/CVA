using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CVA.Portal.Producao.WebAPI.Controllers
{
    public class ProducaoController : ApiController
    {
        ProducaoBLL BLL;

        public ProducaoController()
        {
            BLL = new ProducaoBLL();
        }

        public IEnumerable<ProducaoModel> Get(string codUsuario, DateTime? dataDe, DateTime? dataAte, int? nrOP, int? nrPedido, string itemDesc, string etapa)
        {
            return BLL.GetOPsPendentes(codUsuario, dataDe, dataAte, nrOP, nrPedido, itemDesc, etapa);
        }

        public ProducaoModel Get(int nrOP, string codEtapa)
        {
            return BLL.GetDadosOP(nrOP, codEtapa);
        }

        public IEnumerable<ProducaoModel> GetListRecursosByUsuario(string codUsuario)
        {
            return BLL.GetListRecursosByUsuario(codUsuario);
        }
    }
}
