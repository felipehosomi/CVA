using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CVA.Portal.Producao.Model.PainelApontamentoModel;

namespace CVA.Portal.Producao.Web.Controllers
{
    public class ApontamentoPainelController : Controller
    {
        APICallUtil api = new APICallUtil();

       
        public async System.Threading.Tasks.Task<List<PainelContratoItens>> GetPainelAsync(string pnBplId, DateTime dateDe, DateTime dateAte)
        {
            var ret = await api.GetAsync<List<PainelContratoItens>>("Apontamento", $"GetPainel?pnBplId={pnBplId}&dateDe={dateDe}&dateAte={dateAte}");
            return ret;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetPainel(string pnBplId, DateTime dateDe, DateTime dateAte)
        {
            var retlist = await GetPainelAsync(pnBplId, dateDe, dateAte);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async System.Threading.Tasks.Task<SelectList> GetBPLIdAsync()
        {
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("Filial", $"GetFilial?userId={CvaSession.Usuario.Usuario}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                list.Add(new ComboBoxModel() { Code = string.Empty, Name = string.Empty });
                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CODE, Name = item.NAME });
            }

            return new SelectList(list, "Code", "Name");
        }


        [CvaAuthorize("ApontamentoPainel")]
        // GET: PainelApontamento
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var obj = new PainelApontamentoModel();

            if (TempData["modelPainelApontamento"] != null)
                obj = TempData["modelPainelApontamento"] as PainelApontamentoModel;

            if (obj.BPLIdList == null)
                obj.BPLIdList = await GetBPLIdAsync();

            if (obj.contratoDe == DateTime.MinValue)
                obj.contratoDe = DateTime.Now;

            if (obj.contratoAte == DateTime.MinValue)
                obj.contratoAte = DateTime.Now;
            
            SetTempAlert();

            return View(obj);
        }
               
        
        public void SetTempAlert()
        {
            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"].ToString();

            if (TempData["Success"] != null)
                ViewBag.Success = TempData["Success"].ToString();

            if (TempData["Alert"] != null)
                ViewBag.Alert = TempData["Alert"].ToString();
        }
    }
}
