using CVA.Portal.Producao.Model.Configuracoes;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace CVA.Portal.Producao.Web.Controllers
{
    public class LoginController : Controller
    {
        APICallUtil api = new APICallUtil();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UsuarioModel model)
        {
            try
            {
                string message = await api.PostAsync<UsuarioModel>("Login", model);

                if (!String.IsNullOrEmpty(message))
                {
                    ViewBag.LoginError = message;
                    return View("Login");
                }
                else
                {
                    model = await api.GetAsync<UsuarioModel>("Usuario", model.Usuario);
                }
                FormsAuthentication.SetAuthCookie(model.Usuario, false);
                Session["CVAPARAM"] = await api.GetAsync<ParametrosModel>("Parametros", "0001");

                CreateUserSession(model);
                if (model.ViewList.Any(m => m.View == "Portal"))
                {
                    return RedirectToAction("Menu", "Apetit");
                }
                else
                {
                    ViewModel viewModel = model.ViewList.FirstOrDefault(m => !String.IsNullOrEmpty(m.Action));
                    if (viewModel != null)
                    {
                        return RedirectToAction(viewModel.Action, viewModel.Controller);
                    }
                    else
                    {
                        throw new Exception("Usuário sem permissão de acesso");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.LoginError = ex.Message;
                return View("Login");
            }
        }

        public ActionResult ChangePassword()
        {
            UsuarioModel model = new UsuarioModel();
            model.Usuario = CvaSession.Usuario.Usuario;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(UsuarioModel model)
        {
            UsuarioModel modelAtual = new UsuarioModel();
            modelAtual = await api.GetAsync<UsuarioModel>("Usuario", model.Usuario);
            if (modelAtual.Senha != model.Senha)
            {
                ModelState.AddModelError("Senha", "Senha atual inválida");
                return View();
            }
            if (model.NovaSenha != model.ConfirmarSenha)
            {
                ModelState.AddModelError("NovaSenha", "Senhas não coincidem");
                ModelState.AddModelError("ConfirmarSenha", "Senhas não coincidem");
                return View();
            }

            modelAtual.Senha = model.NovaSenha;
            string error = await api.PutAsync<UsuarioModel>("Usuario", modelAtual.Code, modelAtual);
            if (!String.IsNullOrEmpty(error))
            {
                ModelState.AddModelError("", error);
                return View();
            }

            if (CvaSession.ViewList.Any(m => m.Controller == "Producao"))
            {
                return RedirectToAction("Index", "Producao");
            }
            else
            {
                ViewModel viewModel = CvaSession.ViewList.FirstOrDefault();
                if (viewModel != null)
                {
                    return RedirectToAction(viewModel.Action, viewModel.Controller);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Login", "Login");
        }

        private void CreateUserSession(UsuarioModel model)
        {
            try
            {
                Session["CVAUSR"] = model;
                Session["CVAVIEW"] = model.ViewList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}