using MODEL.Classes;
using System;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class LoginController : Controller
    {
        private CVAGestaoService.LoginClient _loginClient { get; set; }
        private CVAGestaoService.UserClient _userClient { get; set; }

        public LoginController()
        {
            this._loginClient = new CVAGestaoService.LoginClient();
            this._userClient =   new CVAGestaoService.UserClient();
        }

        public ActionResult Login()
        {
            ViewBag.LoginError = null;
            return View();
        }

        public ActionResult EfetuarLogin(UserModel userModel)
        {
            try
            {
                var result = _loginClient.LogIn(userModel.Email, userModel.Password);
                if (result != null)
                {
                    if (result.FirstAccess == 0)
                    {
                        System.Web.Security.FormsAuthentication.SetAuthCookie(result.Id.ToString(), false);
                        CreateUserSession(result);
                        return RedirectToAction("Index", "Portal");
                    }
                    else
                        return View("PrimeiroAcesso", result);
                }
                    
                ViewBag.LoginError = "Login e/ou senha incorretos";
                return View("Login");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult PrimeiroAcesso()
        {
            return View();
        }

        public ActionResult AlterarSenha(UserModel user)
        {
            user = _loginClient.FirstAccess(user);
            if(user != null)
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                CreateUserSession(user);
                return RedirectToAction("Index", "Portal");
            }
            else
            {
                ViewBag.LoginError = "Senhas não coincidem!";
                return View("Login");
            }
                
        }

        public ActionResult LogOut()
        {
            try
            {
                if(_loginClient.LogOff(((UserModel)Session["CVAUSR"]).Id).Error == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    DeleteUserSession();
                    return View("Login");
                }
                return RedirectToAction("Index", "Portal");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CreateUserSession(UserModel userModel)
        {
            try
            {
                System.Web.HttpContext.Current.Session["CVAUSR"] = userModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DeleteUserSession()
        {
            try
            {
                System.Web.HttpContext.Current.Session["CVAUSR"] = null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
