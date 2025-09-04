using CVA.Portal.Producao.Model.Configuracoes;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CVA.Portal.Producao.Web.Controllers
{
    public class LoginFornecedorController : Controller
    {
        APICallUtil api = new APICallUtil();

        // GET: LoginFornecedor
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UsuarioModel model)
        {
            try
            {
                string message = await api.PostAsync("LoginFornecedor", model);

                if (!String.IsNullOrEmpty(message))
                {
                    ViewBag.LoginError = message;
                    return View("Index");
                }
                else
                {
                    model = await api.GetAsync<UsuarioModel>("UsuarioFornecedor", $"usuario={model.Usuario}&senha={model.Senha}", "?");
                }
                FormsAuthentication.SetAuthCookie(model.Usuario, false);
                Session["CVAPARAM"] = await api.GetAsync<ParametrosModel>("Parametros", "0001");

                CreateUserSession(model);
                if (model.ViewList.Any(m => m.Controller == "Producao"))
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
                return View("Index");
            }
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

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Index", "LoginFornecedor");
        }
    }
}