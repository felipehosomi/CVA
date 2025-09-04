using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Estoque;
using CVA.Portal.Producao.Model.Producao;
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
    [CvaAuthorize("FichaProduto")]
    public class FichaProdutoController : Controller
    {
        APICallUtil api = new APICallUtil();

        // GET: FichaProduto
        public async Task<ActionResult> Index()
        {
            return View(await api.GetListAsync<FichaProdutoModel>("FichaProduto"));
        }

        // GET: FichaProduto/Create
        public async Task<ActionResult> Create()
        {
            await GetSelectLists(new FichaProdutoModel());
            return View();
        }

        public async Task<JsonResult> GetByOP(int nrOP)
        {
            List<FichaProdutoModel> list = await api.GetListAsync<FichaProdutoModel>("FichaProduto", "nrOP=" + nrOP);
            SelectList sls = new SelectList(list, "StageId", "CodEtapa");
            return Json(sls, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> GetSelectLists(FichaProdutoModel model)
        {
            List<ComboBoxModel> tipoList = new List<ComboBoxModel>();
            tipoList.Add(new ComboBoxModel() { Code = "I", Name = "Item" });
            tipoList.Add(new ComboBoxModel() { Code = "G", Name = "Grupo de Item" });
            ViewBag.Tipo = new SelectList(tipoList, "Code", "Name", model.Tipo);

            List<ModeloFichaModel> modeloFichaList = await api.GetListAsync<ModeloFichaModel>("ModeloFicha");
            ViewBag.CodModelo = new SelectList(modeloFichaList, "Code", "Descricao", model.CodModelo);

            List<EtapaItinerarioModel> etapaItinerarioList = await api.GetListAsync<EtapaItinerarioModel>("EtapaItinerario");
            ViewBag.CodEtapa = new SelectList(etapaItinerarioList, "Code", "Desc", model.CodEtapa);

            List<GrupoItemModel> grupoItemList = await api.GetListAsync<GrupoItemModel>("GrupoItem");
            ViewBag.CodGrupo = new SelectList(grupoItemList, "Code", "Name", model.CodGrupo);

            List<ItemModel> itemList = await api.GetListAsync<ItemModel>("Item");
            ViewBag.CodItem = new SelectList(itemList, "ItemCode", "Item", model.CodItem);

            return String.Empty;
        }

        // POST: FichaProduto/Create
        [HttpPost]
        public async Task<ActionResult> Create(FichaProdutoModel model)
        {
            try
            {
                string error = await api.PostAsync<FichaProdutoModel>("FichaProduto", model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await this.GetSelectLists(model);
                return View();
            }
        }

        // GET: FichaProduto/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            FichaProdutoModel model = await api.GetAsync<FichaProdutoModel>("FichaProduto", id);

            await GetSelectLists(model);
            return View(model);
        }

        // POST: FichaProduto/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(FichaProdutoModel model)
        {
            try
            {
                string error = await api.PutAsync<FichaProdutoModel>("FichaProduto", model.Code, model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await this.GetSelectLists(model);
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        #region Obrigatorio
        [AllowAnonymous]
        public async Task<JsonResult> Obrigatorio(int docEntryOP, string codItem, string codEtapa, double quantidade)
        {
            FichaProdutoModel fichaInspecaoModel = await api.GetAsync<FichaProdutoModel>("FichaProduto", $"docEntryOP={docEntryOP}&codItem={codItem}&codEtapa={codEtapa}&quantidade={quantidade}", "?");
            return Json(fichaInspecaoModel.ObrigatorioInt.ToString(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
