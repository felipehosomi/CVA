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
    public class ContratosController : ApiController
    {
        ContratosBLL BLLContratos;

        public ContratosController()
        {
            BLLContratos = new ContratosBLL();
        }


        [HttpGet]
        public List<ComboBoxModelHANA> GetContratos(string filial)
        {
            var retlist = BLLContratos.GetContrato(filial);
            return (retlist?.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }

        [HttpGet]
        public List<ComboBoxModelHANA> GetContratosNumber(string filialId)
        {
            var retlist = BLLContratos.GetContratoNumber(filialId);
            return (retlist?.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }
        
        [HttpGet]
        public List<ComboBoxModelHANA> GetContratoApontamento(string filialIdApontamento)
        {
            var retlist = BLLContratos.GetContratoApontamento(filialIdApontamento);
            return (retlist?.Count == 0 ? new List<ComboBoxModelHANA>() : retlist);
        }


    }
}
