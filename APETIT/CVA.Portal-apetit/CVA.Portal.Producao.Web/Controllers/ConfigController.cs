using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Configuracoes;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    public class ConfigController : Controller
    {
        APICallUtil api = new APICallUtil();

        public async Task<ActionResult> Parameters()
        {
            ParametrosModel model = await api.GetAsync<ParametrosModel>("Parametros", "0001");

            List<ComboBoxModel> listInspecaoMP = new List<ComboBoxModel>();
            listInspecaoMP.Add(new ComboBoxModel() { Code = "PDN", Name = "Recebimento de Mercadorias" });
            listInspecaoMP.Add(new ComboBoxModel() { Code = "PCH", Name = "Nota Fiscal Entrada" });

            ViewBag.InspecaoMP = new SelectList(listInspecaoMP, "Code", "Name", model.InspecaoMP);

            return View(model);
        }

        public JsonResult PermiteParcial()
        {
            ParametrosModel model = Session["CVAPARAM"] as ParametrosModel;
            if (model == null)
            {
                model = new ParametrosModel();
                model.PermiteParcialInt = 1;
            }

            return Json(model.PermiteParcialInt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Parameters(ParametrosModel model)
        {
            try
            {
                string error = await api.PutAsync<ParametrosModel>("Parametros", model.Code, model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                Session["CVAPARAM"] = model;

                return RedirectToAction("Index", "Producao");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}