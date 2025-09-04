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
    [CvaAuthorize("ModeloFicha")]
    public class ModeloFichaController : Controller
    {
        APICallUtil api = new APICallUtil();

        public async Task<ActionResult> Index()
        {
            return View(await api.GetListAsync<ModeloFichaModel>("ModeloFicha"));
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Status = GetStatus();
            ModeloFichaModel model = new ModeloFichaModel();
            model.ItemList = new List<ModeloFichaItemModel>();
            model.ItemList.Add(new ModeloFichaItemModel() { ID = 1 });

            List<TipoEspecificacaoModel> especificacaoList = await api.GetListAsync<TipoEspecificacaoModel>("TipoEspecificacao");
            Session["CVAESP"] = especificacaoList;
            ViewBag.Especificacoes = new SelectList(especificacaoList, "Code", "Descricao");

            return View(model);
        }

        public ActionResult ModeloFichaItem(int linha)
        {
            ModeloFichaItemModel model = new ModeloFichaItemModel();
            model.ID = linha + 1;
            List<TipoEspecificacaoModel> especificacaoList = Session["CVAESP"] as List<TipoEspecificacaoModel>;
            ViewBag.Especificacoes = new SelectList(especificacaoList, "Code", "Descricao");

            return PartialView(model);
        }

        // POST: ModeloFicha/Create
        [HttpPost]
        public async Task<ActionResult> Create(ModeloFichaModel model)
        {
            try
            {
                string error = await api.PostAsync<ModeloFichaModel>("ModeloFicha", model);
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

        public JsonResult GetTipoCampoEspecificacao(string codEspec)
        {
            List<TipoEspecificacaoModel> especificacaoList = Session["CVAESP"] as List<TipoEspecificacaoModel>;
            return Json(especificacaoList.FirstOrDefault(m => m.Code == codEspec).Tipo, JsonRequestBehavior.AllowGet);
        }

        // GET: ModeloFicha/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Status = GetStatus();
            
            ModeloFichaModel model = await api.GetAsync<ModeloFichaModel>("ModeloFicha", id);

            List<TipoEspecificacaoModel> especificacaoList = await api.GetListAsync<TipoEspecificacaoModel>("TipoEspecificacao");
            foreach (var item in model.ItemList)
            {
                item.Especificacoes = new SelectList(especificacaoList, "Code", "Descricao", item.CodEspec);
            }
            Session["CVAESP"] = especificacaoList;

            return View(model);
        }

        // POST: ModeloFicha/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ModeloFichaModel model)
        {
            try
            {
                string error = await api.PutAsync<ModeloFichaModel>("ModeloFicha", model.Code, model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Status = GetStatus();
                List<TipoEspecificacaoModel> especificacaoList = await api.GetListAsync<TipoEspecificacaoModel>("TipoEspecificacao");
                foreach (var item in model.ItemList)
                {
                    item.Especificacoes = new SelectList(especificacaoList, "Code", "Descricao", item.CodEspec);
                }

                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public SelectList GetStatus()
        {
            List<ComboBoxModel> list = new List<ComboBoxModel>();
            list.Add(new ComboBoxModel() { Code = "0", Name = "Não Aprovado" });
            list.Add(new ComboBoxModel() { Code = "1", Name = "Aprovado" });

            return new SelectList(list, "Code", "Name");
        }
    }
}
