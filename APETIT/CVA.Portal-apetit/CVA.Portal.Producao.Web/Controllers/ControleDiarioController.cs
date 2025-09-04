using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    [CvaAuthorize("Producao")]
    public class ControleDiarioController : Controller
    {
        APICallUtil api = new APICallUtil();

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetContrato(string BPLId)
        {
            var retlist = await GetContratoAsync(BPLId);
            return Json(retlist, JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<SelectList> GetContratoAsync(string BPLId)
        {
            var ret = await api.GetAsync<List<ComboBoxModelHANA>>("Contratos", $"GetContratoApontamento?filialIdApontamento={BPLId}");

            var list = new List<ComboBoxModel>();
            if (ret != null && ret.Count > 0)
            {
                list.Add(new ComboBoxModel() { Code = string.Empty, Name = string.Empty });

                foreach (var item in ret)
                    list.Add(new ComboBoxModel() { Code = item.CODE, Name = item.NAME });
            }

            return new SelectList(list, "Code", "Name");
        }

        public async System.Threading.Tasks.Task<ApontamentoGetInfoContratoModel> GetInfoAsync(string contratoId)
        {
            var ret = await api.GetAsync<ApontamentoGetInfoContratoModel>("Apontamento", $"GetInfoContrato?contratoIdInfoContrato={contratoId}");
            return ret;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> CallGetInfo(string contratoId)
        {
            var retlist = await GetInfoAsync(contratoId);
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

        // GET: 
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var obj = new ControleDiarioModel();

            if (TempData["modelControleDiario"] != null)
                obj = TempData["modelControleDiario"] as ControleDiarioModel;

            if (obj.BPLIdList == null)
                obj.BPLIdList = await GetBPLIdAsync();

            if (obj.dt == DateTime.MinValue)
                obj.dt = DateTime.Now;

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
