using CVA.Portal.Producao.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    
    public class ApetitController : Controller
    {

        [CvaAuthorize("Apetit")]
        // GET: ProducaoApetit
        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult SaidaMateriais()
        {
            return RedirectToAction("Index", "SaidaMateriais");
        }

        public ActionResult SaidaMateriaisRepport()
        {
            return RedirectToAction("ReportPosicaoEstoque", "SaidaMateriais");
        }

        
        public ActionResult ReposicaoInsumos()
        {
            return RedirectToAction("Index", "ReposicaoInsumos");
        }

        public ActionResult ApontamentoPainel()
        {
            return RedirectToAction("Index", "ApontamentoPainel");
        }

        public ActionResult Apontamento()
        {
            return RedirectToAction("Index", "Apontamento");
        }

        public ActionResult ApontamentoEnceramento()
        {
            return RedirectToAction("Index", "ApontamentoEncerramento");
        }

        public ActionResult CRD()
        {
            return RedirectToAction("Index", "ControleDiario");
        }

        public ActionResult Usuario()
        {
            return RedirectToAction("Index", "Usuario");
        }
        public ActionResult ServicoExtra()
        {
            return RedirectToAction("Index", "ServicoExtra");
        }

        public ActionResult OfertaCompra()
        {
            return RedirectToAction("Index", "OfertaCompra");
        }
    }
}