using CVA.Portal.Producao.Model.Configuracoes;
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
    [CvaAuthorize("Perfil")]
    public class PerfilController : Controller
    {
        APICallUtil api = new APICallUtil();

        public async Task<ActionResult> Index()
        {
            return View(await api.GetListAsync<PerfilModel>("Perfil"));
        }

        public async Task<ActionResult> Create()
        {
            PerfilModel model = await api.GetAsync<PerfilModel>("Perfil", "-1");
            return View(model);
        }

        // POST: Perfil/Create
        [HttpPost]
        public async Task<ActionResult> Create(PerfilModel model)
        {
            try
            {
                string error = await api.PostAsync<PerfilModel>("Perfil", model);
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

        // GET: Perfil/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            //List<ViewModel> viewsList = await api.GetListAsync<ViewModel>("View");
            //ViewBag.Views = new SelectList(viewsList, "Code", "Descricao");

            PerfilModel model = await api.GetAsync<PerfilModel>("Perfil", id);
            //ViewBag.CodPerfil = new SelectList(await api.GetListAsync<PerfilModel>("Perfil"), "Code", "Descricao");
            return View(model);
        }

        // POST: Perfil/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(PerfilModel model)
        {
            try
            {
                string error = await api.PutAsync<PerfilModel>("Perfil", model.Code, model);
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
    }
}
