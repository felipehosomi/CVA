using CVA.Portal.Producao.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    [CvaAuthorize("Producao")]
    public class TSTController : Controller
    {
        // GET: ProducaoApetit
        public ActionResult Index()
        {
            return View();
        }
        
        

    }
}