using CVA.Portal.Producao.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Menu", "Apetit");

            //return View();
        }

        //public ActionResult Menu()
        //{
        //    if (System.Web.HttpContext.Current.Session["LEFT-MENU-CSS"] == null)
        //    {
        //        System.Web.HttpContext.Current.Session["LEFT-MENU-CSS"] = "nav-md";
        //    }
            
        //    return PartialView("Menu", CvaSession.Usuario.ViewList);
        //}

        //public JsonResult SetMenuClass(string cssClass)
        //{
        //    if (cssClass == "nav-md")
        //    {
        //        System.Web.HttpContext.Current.Session["LEFT-MENU-CSS"] = "nav-sm";
        //    }
        //    else
        //    {
        //        System.Web.HttpContext.Current.Session["LEFT-MENU-CSS"] = "nav-md";
        //    }

            
        //    return Json(cssClass, JsonRequestBehavior.AllowGet);
        //    //if (menuClass == null)
        //    //{
        //    //    menuClass = "nav-md";
        //    //}
        //}
    }
}