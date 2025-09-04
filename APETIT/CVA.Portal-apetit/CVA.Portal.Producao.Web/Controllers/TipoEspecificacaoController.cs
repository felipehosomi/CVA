using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Qualidade;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    [CvaAuthorize("TipoEspecificacao")]
    public class TipoEspecificacaoController : Controller
    {
        APICallUtil api = new APICallUtil();

        public SelectList GetTipo()
        {
            List<ComboBoxModel> list = new List<ComboBoxModel>();
            list.Add(new ComboBoxModel() { Code = "T", Name = "Texto" });
            list.Add(new ComboBoxModel() { Code = "N", Name = "Numérico" });
            list.Add(new ComboBoxModel() { Code = "D", Name = "Data" });

            return new SelectList(list, "Code", "Name");
        }

        public async Task<ActionResult> Index()
        {
            return View(await api.GetListAsync<TipoEspecificacaoModel>("TipoEspecificacao"));
        }

        public ActionResult Create()
        {
            ViewBag.Tipo = this.GetTipo();
            return View();
        }       

        // POST: TipoEspecificacao/Create
        [HttpPost]
        public async Task<ActionResult> Create(TipoEspecificacaoModel model)
        {
            try
            {
                string error = await api.PostAsync<TipoEspecificacaoModel>("TipoEspecificacao", model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: TipoEspecificacao/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            TipoEspecificacaoModel model = await api.GetAsync<TipoEspecificacaoModel>("TipoEspecificacao", id);
            ViewBag.Tipo = this.GetTipo();
            return View(model);
        }

        // POST: TipoEspecificacao/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(TipoEspecificacaoModel model)
        {
            try
            {
                string error = await api.PutAsync<TipoEspecificacaoModel>("TipoEspecificacao", model.Code, model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public async Task<JsonResult> Delete(string id)
        {
            string error = await api.DeleteAsync("TipoEspecificacao", id);
            return Json(error, JsonRequestBehavior.AllowGet);
        }
    }
}
