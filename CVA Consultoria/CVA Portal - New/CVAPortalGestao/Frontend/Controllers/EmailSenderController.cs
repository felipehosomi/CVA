using Microsoft.AspNet.SignalR.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class EmailSenderController : Controller
    {
        private CVAGestaoService.EmailSenderClient _emailSenderClient { get; set; }

        public EmailSenderController()
        {
            _emailSenderClient = new CVAGestaoService.EmailSenderClient();
        }

        public ActionResult EnviarSenhaEmail(string Email)
        {
            ViewBag.LoginError = null;

            var result = _emailSenderClient.SendEmail(Email);
            if (result.Success != null)
            {
                ViewBag.LoginError = result.Success.Message;
                return View("RecuperarSenha");
            }
            else
            {
                ViewBag.LoginError = result.Error.Message;
                return View("RecuperarSenha");
            }
        }


        public ActionResult RecuperarSenha()
        {
            ViewBag.LoginError = null;
            return View();
        }

        public ActionResult RetornaLogin()
        {
            return RedirectToAction("Login", "Login");
        }
    }
}