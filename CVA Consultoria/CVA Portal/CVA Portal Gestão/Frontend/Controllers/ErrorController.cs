using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVAGestaoLayout.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            ActionResult result;
            object model = Request.Url.PathAndQuery;

            if (!Request.IsAjaxRequest())
                result = View(model);
            else
                result = View("~/Views/Shared/AcessoNegado.cshtml");

            return result;
        }
    }
}