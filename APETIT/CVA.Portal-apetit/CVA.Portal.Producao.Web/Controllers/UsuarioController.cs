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
    [CvaAuthorize("Usuario")]
    public class UsuarioController : Controller
    {
        APICallUtil api = new APICallUtil();

        public async Task<ActionResult> Index()
        {
            return View(await api.GetListAsync<UsuarioModel>("Usuario"));
        }

        public async Task<ActionResult> Create()
        {
            UsuarioModel model = await api.GetAsync<UsuarioModel>("Usuario", "-1");

            ViewBag.Usuario = new SelectList(await api.GetListAsync<ColaboradorModel>("Colaborador"), "Codigo", "Nome");
            ViewBag.CodPerfil = new SelectList(await api.GetListAsync<PerfilModel>("Perfil"), "Code", "Descricao");
            return View(model);
        }

        // POST: Usuario/Create
        [HttpPost]
        public async Task<ActionResult> Create(UsuarioModel model)
        {
            try
            {
               string error = await api.PostAsync<UsuarioModel>("Usuario", model);
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

        // GET: Usuario/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            UsuarioModel model = await api.GetAsync<UsuarioModel>("Usuario", id);
            ViewBag.CodPerfil = new SelectList(await api.GetListAsync<PerfilModel>("Perfil"), "Code", "Descricao", model.CodPerfil);
            return View(model);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(UsuarioModel model)
        {
            try
            {
                string error = await api.PutAsync<UsuarioModel>("Usuario", model.Code, model);
                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
